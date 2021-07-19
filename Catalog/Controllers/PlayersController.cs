using Catalog.Dtos.Player;
using Catalog.Entities;
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

        //Post  /items
        [HttpPost]
        public async Task<ActionResult<PlayerDto>> CreateItemAsync(CreatePlayerDto playerDto)
        {
            Player player = new()
            {
                Id = Guid.NewGuid(),
                FirstName = playerDto.FirstName,
                LastName = playerDto.LastName,
                Email = playerDto.Email,
                Password = Player.EncodePassword(playerDto.Password)
            };

            await repository.CreatePlayerAsync(player);
            return CreatedAtAction(nameof(GetPlayerAsync), new { id = player.Id }, player.AsDto());
        }

        // PUT /items/id
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePlayerAsync(Guid id, UpdatePlayerDto itemDto)
        {
            var existingItem = await repository.GetPlayerAsync(id);

            if (existingItem is null)
            {
                return NotFound();
            }

            Player updateItem = new ()
            {
                FirstName = existingItem.FirstName,
                LastName = existingItem.LastName,
                Email = existingItem.Email

            };

            await repository.UpdatePlayerAsync(updateItem);

            return NoContent();
        }

        //Delete /items/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            var existingPlayer = await repository.GetPlayerAsync(id);

            if (existingPlayer is null)
            {
                return NotFound();
            }

            await repository.DeletePlayerAsync(id);

            return NoContent();

        }

    }
}
