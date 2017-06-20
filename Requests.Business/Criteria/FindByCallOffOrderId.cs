using Cmas.Infrastructure.Domain.Criteria;

namespace Cmas.BusinessLayers.Requests.Criteria
{
    public class FindByCallOffOrderId : ICriterion
    {
        public FindByCallOffOrderId(string callOffOrderId = null)
        {
            this.CallOffOrderId = callOffOrderId;
        }

        public string CallOffOrderId { get; set; }
    }
}
