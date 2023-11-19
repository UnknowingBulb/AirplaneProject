using AirplaneProject.Application.Interfaces.DbData;
using FluentResults;

namespace AirplaneProject.Application.Interactors
{
    public class SpawnDataInteractor
    {
        private readonly ISpawnDataDb _spawnData;
        public SpawnDataInteractor(ISpawnDataDb spawnData)
        {
            _spawnData = spawnData;
        }


        /// <summary>
        /// Добавить даннных в БД
        /// </summary>
        public async Task<Result> AddDataAsync()
        {
            var isDbNotEmpty = await _spawnData.IsNotEmptyAsync();
            if (isDbNotEmpty)
                return Result.Fail("БД не пуста, операция прервана");

            var users = await _spawnData.AddUsersAsync();
            var passenger = await _spawnData.AddPassengerAsync(users[0]);
            var flights = await _spawnData.AddFlightsAsync();

            await _spawnData.AddOrdersAsync(users[0].Id, flights[0].Id, passenger.Id);
            await _spawnData.AddOrdersAsync(users[0].Id, flights[1].Id, passenger.Id);

            return Result.Ok();
        }
    }
}
