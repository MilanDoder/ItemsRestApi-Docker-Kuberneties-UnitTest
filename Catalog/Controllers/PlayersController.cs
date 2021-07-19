using Catalog.Dtos.Player;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Controllers
{

    //GET /players
    [ApiController]
    [Route("[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerRepository repository;

        public PlayersController(IPlayerRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<PlayerDto>> GetPlayersAsync()
        {
            var players = (await repository.GetPlayersAsync())
                        .Select(item => item.AsDto());
            return players;
        }
        //GET /player/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerDto>> GetPlayerAsync(Guid id)
        {
            var player = await repository.GetPlayerAsync(id);

            if (player is null)
            {
                return NotFound();
            }
            return player.AsDto();
        }

    }
}
