using Microsoft.EntityFrameworkCore;
using ShopBridge.Interfaces;
using ShopBridge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly DataContext _context;

        public InventoryRepository(DataContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<IList<Inventory>> GetInventories()
        {
            try
            {
                var inventories = await _context.inventories.ToListAsync();

                return inventories == null ? null : inventories;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Inventory> GetInventory(int id)
        {
            try
            {
                var inventory = await _context.inventories.Where(i => i.Id == id).FirstOrDefaultAsync();

                return inventory == null ? null : inventory;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public async Task<bool> SaveAll()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}
