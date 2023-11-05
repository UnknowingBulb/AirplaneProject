using System.ComponentModel.DataAnnotations;

namespace AirplaneProject.Objects
{
    /// <summary>
    /// Пользователь сайта
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// ФИО пользователя
        /// </summary>
        [Required]
        [Display(Name = "ФИО")]
        public string Name { get; set; }
        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// Номер телефона
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "{0} должен длиннее {2} символов", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
        /// <summary>
        /// Номер телефона
        /// </summary>
        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Является ли сотрудником
        /// </summary>
        public bool IsEmployee { get; set; } = false;
    }
}
