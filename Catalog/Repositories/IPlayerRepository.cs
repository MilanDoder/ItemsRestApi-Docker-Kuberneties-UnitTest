using Catalog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Repositories
{
    public interface IPlayerRepository
    {
        Task<Player> GetPlayerAsync(Guid id);
        Task<IEnumerable<Player>> GetPlayersAsync();

        Task CreatePlayerAsync(Player item);
        Task UpdatePlayerAsync(Player item);
        Task DeletePlayerAsync(Guid id);
    }
}
