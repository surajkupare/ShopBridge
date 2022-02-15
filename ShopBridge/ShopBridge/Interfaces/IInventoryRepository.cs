using ShopBridge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge.Interfaces
{
    public interface IInventoryRepository
    {
        Task<IList<Inventory>> GetInventories();

        Task<Inventory> GetInventory(int id);

        Task<bool> SaveAll();

        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
    }
}
