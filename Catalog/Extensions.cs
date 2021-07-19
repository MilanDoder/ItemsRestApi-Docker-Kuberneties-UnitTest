using Catalog.Dtos;
using Catalog.Dtos.Player;
using Catalog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog
{
    public static class Extensions
    {
        public static ItemDto AsDto(this Item item) 
        {
            return new ItemDto
            {
                Id = item.Id,
                CreatedDate = item.CreatedDate,
                Name = item.Name,
                Price = item.Price
            };
        }

        public static PlayerDto AsDto(this Player player)
        {
            return new PlayerDto
            {
                Id = player.Id,
                Email =player.Email,
                FirstName =player.FirstName,
                LastName =player.LastName,
                Password = Player.DecodePassword(player.Password)
            };
        }
    }
}
