using Microsoft.AspNetCore.Mvc;
using AirplaneProject.Application.Interactors;
using AirplaneProject.Application.Utilities;
using AirplaneProject.WebUI.Pages.Shared;

namespace AirplaneProject.WebUI.Pages
{
    public class SpawnDataModel : AuthOnPage
    {
        private readonly SpawnDataInteractor _dataSpawner;

        public SpawnDataModel(SpawnDataInteractor dataSpawner, UserInteractor userInteractor) : base(userInteractor)
        {
            _dataSpawner = dataSpawner;
        }
        public string Message { get; set; } = string.Empty;

        public IActionResult OnGet()
        {
            return Page();
        }

        /// <summary>
        /// Добавить данные в БД
        /// </summary>
        public async Task OnPostSpawnAsync()
        {
            var spawnResult = await _dataSpawner.AddDataAsync();
            if (spawnResult.IsFailed)
            {
                Message = spawnResult.GetResultErrorMessages();
            }
            else
            {
                Message = "Данные успешно добавлены";
            }
        }
    }
}