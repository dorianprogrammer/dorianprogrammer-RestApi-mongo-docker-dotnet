using Catalog.SI.Models;
using Catalog.SI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ItemsController> logger;
        
        public ItemsController(IInMemoryItemsRepository repository, ILogger<ItemsController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        [HttpGet("GetItems")]
        public async Task<IEnumerable<ItemDto>> GetItemsAsync(string nameToMatch = null)
        {
            var items = (await repository.GetItemsAsync()).Select(item => item.AsDto());

            if (!string.IsNullOrWhiteSpace(nameToMatch))
            {
                items = items.Where(item => item.Name.Contains(nameToMatch, StringComparison.OrdinalIgnoreCase));
            }

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

            return item.AsDto();
        }

        [HttpPost("CreateItem")]
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto)
        {
            Item item = new()
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Description=itemDto.Description,
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

            existingItem.Name = itemDto.Name;
            existingItem.Price = itemDto.Price;

            await repository.UpdateItemAsync(existingItem);

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
