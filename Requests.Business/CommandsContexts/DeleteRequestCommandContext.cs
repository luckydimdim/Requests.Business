using Cmas.Infrastructure.Domain.Commands;

namespace Cmas.BusinessLayers.Requests.CommandsContexts
{
    public class DeleteRequestCommandContext : ICommandContext
    {
        /// <summary>
        /// ID заявки
        /// </summary>
        public string Id;

    }
}
