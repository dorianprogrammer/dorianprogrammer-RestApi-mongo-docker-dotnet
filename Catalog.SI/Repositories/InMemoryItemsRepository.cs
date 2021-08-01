using Catalog.SI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.SI.Repositories
{
    public class InMemoryItemsRepository : IInMemoryItemsRepository
    {
        private readonly List<Item> listOfItems = new()
        {
            new Item { Id = Guid.NewGuid(), Name = "Potion", Price = 9, CreatedDate = DateTimeOffset.UtcNow },
            new Item { Id = Guid.NewGuid(), Name = "Iron Sword", Price = 20, CreatedDate = DateTimeOffset.UtcNow },
            new Item { Id = Guid.NewGuid(), Name = "Bronze shield", Price = 18, CreatedDate = DateTimeOffset.UtcNow }
        };

        public async Task CreateItemAsync(Item item)
        {
            listOfItems.Add(item);
            await Task.CompletedTask;
        }

        public async Task DeleteItemAsync(Guid id)
        {
            var index = listOfItems.FindIndex(existingItem => existingItem.Id == id);
            listOfItems.RemoveAt(index);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            return await Task.FromResult(listOfItems);
        }

        public async Task<Item> GetTheItemAsync(Guid id)
        {

            var item = listOfItems.Where(item => item.Id == id).SingleOrDefault();
            return await Task.FromResult(item);
        }

        public async Task UpdateItemAsync(Item item)
        {
            var index = listOfItems.FindIndex(existingItem => existingItem.Id == item.Id);
            listOfItems[index] = item;
            await Task.CompletedTask;
        }
    }
}
