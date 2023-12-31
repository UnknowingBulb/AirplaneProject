﻿using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using AirplaneProject.Infrastructure.Authorization;
using AirplaneProject.Domain.Entities;
using AirplaneProject.Application.Interactors;

namespace AirplaneProject.WebUI.Pages.Shared
{
    public abstract class AuthOnPage : PageModel
    {
        private readonly UserInteractor _userInteractor;
        private string? _authToken;
        private User? _activeUser;

        public AuthOnPage(UserInteractor userInteractor)
        {
            _userInteractor = userInteractor;
        }

        [BindProperty]
        public string? AuthLogin { get; set; }
        [BindProperty]
        [DataType(DataType.Password)]
        public string? AuthPassword { get; set; }

        /// <summary>
        /// Пользователь авторизован
        /// </summary>
        public bool IsAuthorized
        {
            get
            {
                return ActiveUser != null;
            }
        }

        /// <summary>
        /// Получить ФИО текущего пользователя
        /// </summary>
        public string? ActiveUserName
        {
            get
            {
                return ActiveUser?.Name;
            }
        }

        /// <summary>
        /// Текущий пользователь
        /// </summary>
        public User? ActiveUser
        {
            get
            {
                if (_authToken == null)
                {
                    _authToken = Request?.Headers.Authorization.ToString().Replace("Bearer ", "");
                }
                if (_activeUser == null)
                {
                    var userResult = _userInteractor.GetAsync(_authToken).Result;
                    if (userResult.IsSuccess)
                    {
                        _activeUser = userResult.Value;
                    }
                }
                return _activeUser;
            }
        }

        /// <summary>
        /// Логин
        /// </summary>
        public async Task<IActionResult> OnPostLoginAsync()
        {
            if (AuthLogin.IsNullOrEmpty() || AuthPassword.IsNullOrEmpty())
            {
                ModelState.AddModelError("LoginError", "Заполните поля");
                return Page();
            }

            var userResult = await _userInteractor.GetAsync(AuthLogin, AuthPassword);

            if (userResult.IsFailed)
            {
                ModelState.AddModelError("LoginError", userResult.Errors[0].Message);
                return Page();
            }

            _activeUser = userResult.Value;

            var userRole = _activeUser.IsEmployee ? RoleTypes.Employee : RoleTypes.Customer;
            _authToken = JwtToken.GenerateToken(_activeUser.Id, userRole);
            Response.Cookies.Append("authToken", _authToken);

            return RedirectToPage("./Index");
        }

        /// <summary>
        /// Разлогин
        /// </summary>
        public IActionResult OnPostLogout()
        {
            Response.Cookies.Delete("authToken");
            _authToken = null;
            _activeUser = null;

            return RedirectToPage("./Index");
        }
    }
}