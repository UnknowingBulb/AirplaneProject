﻿namespace AiplaneProject.Models
{
    /// <summary>
    /// Пользователь клиента
    /// </summary>
    public class CustomerUser : User
    {
        /// <summary>
        /// Номер телефона
        /// </summary>
        public string PhoneNumber { get; set; }
    }
}