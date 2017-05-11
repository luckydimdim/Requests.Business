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
        ///  Пустая заявка
        /// </summary>
        Empty = 1,

        /// <summary>
        ///  В процессе составления (было редактирование)
        /// </summary>
        Creating = 2,

        /// <summary>
        ///  Редактирование завершено
        /// </summary>
        Created = 3,

        /// <summary>
        /// На проверке
        /// </summary>
        Approving = 4,

        /// <summary>
        /// На исправлении
        /// </summary>
        Correcting = 5,

        /// <summary>
        /// Исправленно
        /// </summary>
        Corrected = 6,

        /// <summary>
        /// Проверена, согласована
        /// </summary>
        Approved = 7,

    }
}
