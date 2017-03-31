using Cmas.BusinessLayers.Requests.Entities;
using Cmas.Infrastructure.Domain.Commands;

namespace Cmas.BusinessLayers.Requests.CommandsContexts
{
    public class CreateRequestCommandContext : ICommandContext
    {
        /// <summary>
        /// ID созданной сущности
        /// </summary>
        public string Id;

        public Request Request;
    }
}
