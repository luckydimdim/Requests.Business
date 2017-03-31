using Cmas.BusinessLayers.Requests.Entities;
using Cmas.Infrastructure.Domain.Commands;

namespace Cmas.BusinessLayers.Requests.CommandsContexts
{
    public class UpdateRequestCommandContext : ICommandContext
    {
        public Request Request;
    }
}
