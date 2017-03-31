using System;
using System.Collections.Generic;

namespace Cmas.BusinessLayers.Requests.Entities
{
    /// <summary>
    /// Заявка на проверку
    /// </summary>
    public class Request
    {
        /// <summary>
        /// Уникальный внутренний идентификатор
        /// </summary>
        public string Id;

        /// <summary>
        /// Номер ревизии
        /// </summary>
        public string RevId;

        /// <summary>
        /// Идентификатор договора
        /// </summary>
        public string ContractId;

        /// <summary>
        /// Дата и время создания
        /// </summary>
        public DateTime CreatedAt;

        /// <summary>
        /// Дата и время обновления
        /// </summary>
        public DateTime UpdatedAt;
         
        /// <summary>
        /// Выбранные НЗ по заявке
        /// </summary>
        public IList<string> CallOffOrderIds;

        public RequestStatus RequestStatus;

        public Request()
        {
            CallOffOrderIds = new List<string>();
            RequestStatus = RequestStatus.NotPublished;
        }

    }
}
