using System;
using System.Threading.Tasks;
using Cmas.BusinessLayers.Requests.CommandsContexts;
using Cmas.Infrastructure.Domain.Commands;
using Cmas.Infrastructure.Domain.Criteria;
using Cmas.Infrastructure.Domain.Queries;
using Cmas.BusinessLayers.Requests.Entities;
using Cmas.BusinessLayers.Requests.Criteria;
using System.Collections.Generic;

namespace Cmas.BusinessLayers.Requests
{
    public class RequestsBusinessLayer
    {
        private readonly ICommandBuilder _commandBuilder;
        private readonly IQueryBuilder _queryBuilder;

        public RequestsBusinessLayer(ICommandBuilder commandBuilder, IQueryBuilder queryBuilder)
        {
            _commandBuilder = commandBuilder;
            _queryBuilder = queryBuilder;
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

            request.CallOffOrderIds = callOffOrderIds;
            request.ContractId = contractId;
            request.CreatedAt = DateTime.Now;
            request.UpdatedAt = DateTime.Now;
            request.Status = RequestStatus.NotPublished;

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
            request.UpdatedAt = DateTime.Now;
             
            var context = new UpdateRequestCommandContext
            {
                Request = request
            };

            context = await _commandBuilder.Execute(context);

            return context.Request.Id;
        }

    }
}
