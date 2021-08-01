using Catalog.SI.DTOs;
using Catalog.SI.Models;
using Catalog.SI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Catalog.SI.Controllers
{
    [Route("items")]
    [ApiController]
    public class ItemsController : ControllerBase
    {

        private readonly IInMemoryItemsRepository repository;

        
        public ItemsController(IInMemoryItemsRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet("GetItems")]
        public async Task<IEnumerable<ItemDto>> GetItemsAsync()
        {
            var items = (await repository.GetItemsAsync()).Select(item => item.AsDto());
            return items;
        }

        [HttpGet("GetAnItem/{id}")]
        public async Task<ActionResult<ItemDto>> GetAnItemAsync(Guid id)
        {
            var item = await repository.GetTheItemAsync(id);

            if (item is null)
            {
                return NotFound();
            }

            return Ok(item.AsDto());
        }

        [HttpPost("CreateItem")]
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto)
        {
            Item item = new()
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await repository.CreateItemAsync(item);

            return CreatedAtAction(nameof(GetAnItemAsync), new { id = item.Id }, item.AsDto());
        }


        [HttpPut("UpdateItem/{id}")]
        public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDto itemDto)
        {
            var existingItem = await repository.GetTheItemAsync(id);

            if (existingItem is null)
            {
                return NotFound();
            }

            Item updatedItem = existingItem with
            {
                Name = itemDto.Name,
                Price = itemDto.Price
            };

            await repository.UpdateItemAsync(updatedItem);

            return NoContent();
        }

        [HttpDelete("DeleteItem/{id}")]
        public async Task<ActionResult> DeleteItemAsync(Guid id)
        {
            var existingItem = await repository.GetTheItemAsync(id);

            if (existingItem is null)
            {
                return NotFound();
            }

            await repository.DeleteItemAsync(id);

            return NoContent();
        }
    }
}
