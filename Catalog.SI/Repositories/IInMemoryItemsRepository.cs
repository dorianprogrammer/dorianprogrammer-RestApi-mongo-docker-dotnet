using Catalog.SI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.SI.Repositories
{
    public interface IInMemoryItemsRepository
    {
        Task<IEnumerable<Item>> GetItemsAsync();
        Task<Item> GetTheItemAsync(Guid id);
        Task CreateItemAsync(Item item);
        Task UpdateItemAsync(Item item);
        Task DeleteItemAsync(Guid id);
    }
}