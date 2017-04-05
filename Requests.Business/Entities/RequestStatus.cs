namespace Cmas.BusinessLayers.Requests.Entities
{
    /// <summary>
    /// Статус заявки на проверку
    /// </summary>
    public enum RequestStatus
    {
        /// <summary>
        ///  Нет никакого статуса
        /// </summary>
        None = 0,

        /// <summary>
        ///  В процессе составления
        /// </summary>
        Creation = 1,

        /// <summary>
        /// На проверке
        /// </summary>
        Validation = 2,

        /// <summary>
        /// На исправлении
        /// </summary>
        Correction = 3,

        /// <summary>
        /// Проверена, согласована
        /// </summary>
        Done = 4,

    }
}
