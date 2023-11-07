using Microsoft.AspNetCore.Mvc;
using AirplaneProject.Database.DbData;
using AirplaneProject.Infrastructure.Database;
using AirplaneProject.Application.Interactors;
using AirplaneProject.Application.Utilities;
using AirplaneProject.WebUI.Pages.Shared;

namespace AirplaneProject.Pages
{
    public class SpawnDataModel : AuthOnPage
    {
        private readonly SpawnDataDb _dataSpawner;

        public SpawnDataModel(ApplicationDbContext dbContext, UserInteractor userInteractor) : base(userInteractor)
        {
            _dataSpawner = new SpawnDataDb(dbContext);
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