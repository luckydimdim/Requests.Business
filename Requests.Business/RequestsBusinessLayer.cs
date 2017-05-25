using System;
using System.Threading.Tasks;
using Cmas.BusinessLayers.Requests.CommandsContexts;
using Cmas.Infrastructure.Domain.Commands;
using Cmas.Infrastructure.Domain.Criteria;
using Cmas.Infrastructure.Domain.Queries;
using Cmas.BusinessLayers.Requests.Entities;
using Cmas.BusinessLayers.Requests.Criteria;
using System.Collections.Generic;
using System.Security.Claims;
using Cmas.Infrastructure.Security;
using Cmas.Infrastructure.ErrorHandler;

namespace Cmas.BusinessLayers.Requests
{
    public class RequestsBusinessLayer
    {
        private readonly ICommandBuilder _commandBuilder;
        private readonly IQueryBuilder _queryBuilder;
        private readonly ClaimsPrincipal _claimsPrincipal;

        public RequestsBusinessLayer(IServiceProvider serviceProvider, ClaimsPrincipal claimsPrincipal)
        {
            _claimsPrincipal = claimsPrincipal;
            _commandBuilder = (ICommandBuilder)serviceProvider.GetService(typeof(ICommandBuilder));
            _queryBuilder = (IQueryBuilder)serviceProvider.GetService(typeof(IQueryBuilder));
        }

        /// <summary>
        /// Создать заявку
        /// </summary>
        /// <param name="contractId">ID договора</param>
        /// <param name="callOffOrderIds">ID работ (наряд заказов)</param>
        /// <returns>ID созданной заявки</returns>
        public async Task<string> CreateRequest(string contractId, IList<string> callOffOrderIds)
        {
            var request = new Request();

            var counter = await _queryBuilder.For<Task<string>>().With(new GetCounter());

            if (counter == null)
            {
                throw new Exception("error while getting counter");
            }

            request.CallOffOrderIds = callOffOrderIds;
            request.Id = null;
            request.Counter = counter;
            request.ContractId = contractId;
            request.CreatedAt = DateTime.UtcNow;
            request.UpdatedAt = DateTime.UtcNow;

            request.Status = RequestStatus.Empty;
             
            var context = new CreateRequestCommandContext
            {
               Request = request
            }; 

            context = await _commandBuilder.Execute(context);

            return context.Id;
        }

        /// <summary>
        /// Получить заявку по ее ID
        /// </summary>
        public async Task<Request> GetRequest(string requestId)
        {
            return await _queryBuilder.For<Task<Request>>().With(new FindById(requestId));
        }

        /// <summary>
        /// Получить все заявки
        /// </summary>
        public async Task<IEnumerable<Request>> GetRequests()
        {
            return await _queryBuilder.For<Task<IEnumerable<Request>>>().With(new AllEntities());
        }

        /// <summary>
        /// Получить заявки по договору
        /// </summary>
        /// <param name="contractId">ID договора</param
        public async Task<IEnumerable<Request>> GetRequestsByContractId(string contractId)
        {
            return await _queryBuilder.For<Task<IEnumerable<Request>>>().With(new FindByContractId(contractId));
        }

        /// <summary>
        /// Удалить заявку
        /// </summary>
        /// <param name="requestId">ID заявки</param>
        public async Task<string> DeleteRequest(string requestId)
        {
            var context = new DeleteRequestCommandContext
            {
                Id = requestId
            };

            context = await _commandBuilder.Execute(context);

            return context.Id;
        }

        public async Task<string> UpdateRequest(Request request)
        {
            request.UpdatedAt = DateTime.UtcNow;
             
            var context = new UpdateRequestCommandContext
            {
                Request = request
            };

            context = await _commandBuilder.Execute(context);

            return context.Request.Id;
        }

        public async Task UpdateRequestStatusAsync(Request request, RequestStatus status)
        {
            if (status == RequestStatus.None)
                throw new ArgumentException("status");

            if (status == request.Status)
                return;

            switch (status)
            {
                case RequestStatus.Empty:
                    throw new GeneralServiceErrorException(
                        string.Format("cannot set '{0}' status from {1}", status, request.Status));
                case RequestStatus.Creating:
                    if (request.Status != RequestStatus.Empty && request.Status != RequestStatus.Created)
                        throw new GeneralServiceErrorException(
                            string.Format("cannot set '{0}' status from {1}", status, request.Status));
                    else
                        request.Status = status;
                    break;
                case RequestStatus.Created:
                    if (request.Status != RequestStatus.Creating && request.Status != RequestStatus.Empty)
                        throw new GeneralServiceErrorException(
                            string.Format("cannot set '{0}' status from {1}", status, request.Status));
                    else
                        request.Status = status;
                    break;
                case RequestStatus.Approving:
                    if (request.Status != RequestStatus.Corrected && request.Status != RequestStatus.Created)
                        throw new GeneralServiceErrorException(
                            string.Format("cannot set '{0}' status from {1}", status, request.Status));
                    else
                        request.Status = status;
                    break;
                case RequestStatus.Correcting:
                    if (request.Status != RequestStatus.Approving && request.Status != RequestStatus.Corrected)
                        throw new GeneralServiceErrorException(
                            string.Format("cannot set '{0}' status from {1}", status, request.Status));
                    else
                    {
                        if (request.Status == RequestStatus.Approving && !_claimsPrincipal.HasRoles(new[] { Role.Customer }))
                            throw new ForbiddenErrorException();

                        request.Status = status;
                    }
                    break;
                case RequestStatus.Corrected:
                    if (request.Status != RequestStatus.Correcting)
                        throw new GeneralServiceErrorException(
                            string.Format("cannot set '{0}' status from {1}", status, request.Status));
                    else
                    {
                        request.Status = status;
                    }
                    break;
                case RequestStatus.Approved:
                    if (request.Status != RequestStatus.Approving)
                        throw new GeneralServiceErrorException(
                            string.Format("cannot set '{0}' status from {1}", status, request.Status));
                    else
                    {
                        if (!_claimsPrincipal.HasRoles(new[] { Role.Customer }))
                            throw new ForbiddenErrorException();

                        request.Status = status;
                    }
                    break;

                default:
                    throw new GeneralServiceErrorException("unknown status");
            }

            await UpdateRequest(request);
        }

    }
}
