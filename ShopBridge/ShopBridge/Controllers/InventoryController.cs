using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShopBridge.DTOs;
using ShopBridge.Interfaces;
using ShopBridge.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopBridge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryRepository _repo;
        private readonly IMapper _mapper;

        public InventoryController(IInventoryRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetInventories()
        {
            var inventories = await _repo.GetInventories();

            if (inventories != null && inventories.Count > 0)
            {
                var inventoriesToReturn = _mapper.Map<IEnumerable<InventoryDto>>(inventories);
                return Ok(inventoriesToReturn);
            }
           
            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInventory(int id)
        {
            if (id != 0)
            {
                var inventory = await _repo.GetInventory(id);

                if (inventory != null)
                {
                    var inventoryToReturn = _mapper.Map<InventoryDto>(inventory);
                    return Ok(inventoryToReturn);
                }
            }
            
            return NotFound();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateInventory(int id, InventoryDto inventoryDto)
        {
            if (id == 0)
                return BadRequest();

            var inventoryFromRepo = await _repo.GetInventory(id);

            _mapper.Map(inventoryDto, inventoryFromRepo);

            if (await _repo.SaveAll())
                return NoContent();

            return BadRequest($"Updating Inventory with {id} failed.");
        }

        [HttpPost]
        public async Task<IActionResult> AddInventory(InventoryDto inventoryDto)
        {
            if (inventoryDto == null)
                return BadRequest("Failed to update");

            Inventory inventory = new Inventory();
            _mapper.Map(inventoryDto, inventory);

            _repo.Add(inventory);

            if (await _repo.SaveAll())
                return Ok();

            return BadRequest("Failed to update");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventory(int id)
        {
            if (id == 0)
                return BadRequest("Failed to delete");

            var inventory = await _repo.GetInventory(id);

            if (inventory != null)
            {
                _repo.Delete(inventory);

                if (await _repo.SaveAll())
                    return Ok();
            }

            return BadRequest("Failed to delete");
        }
    }
}
