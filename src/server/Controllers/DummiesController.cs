using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Api.Models;
using Server.Api.Models.Database;
using Server.Api.Infrastructure;

namespace Server.Api.Controllers
{
    [ApiController]
    [Route("api/dummy")]
    [Produces("application/json")]
    public class DummiesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DummiesController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all dummies
        /// </summary>
        /// <returns>A list of all dummies</returns>
        [HttpGet(Name = nameof(GetAllItems))]
        public async Task<ActionResult<IEnumerable<Dummy>>> GetAllItems()
        {
            // Gets all data from database
            var data = await _context.Db_Dummy.ToListAsync();
            // No data found -> return
            if (data == null)
            {
                return NotFound();
            }

            // Create new list of <Dummy> and mapping data from database to evalitems
            List<Dummy> items = new();
            foreach (var item in data)
            {
                items.Add(new()
                {
                    Id = item.Id,
                    Name = item.Name
                });
            }

            // Return List of <Dummy>
            return items;
        }


        /// <summary>
        /// Gets a item by its ID
        /// </summary>
        /// <param name="id">The ID</param>
        /// <returns>The item</returns>
        [HttpGet("{id}", Name = nameof(GetItemById))]
        public async Task<ActionResult<Dummy>> GetItemById(int id)
        {
            // Gets unique data from database by id
            var data = await _context.Db_Dummy.FindAsync(id);
            // No data found -> return
            if (data == null)
            {
                return NotFound();
            }

            // Get item from data
            var item = await GetItemFromData(data);

            // Return <EvalItemAttrQuest>
            return item;
        }


        /// <summary>
        /// Creates a new dummy
        /// </summary>
        /// <param name="item">The initial properties</param>
        /// <returns>The newly created dummy</returns>
        [HttpPost(Name = nameof(CreateItem))]
        public async Task<ActionResult<Dummy>> CreateItem(Dummy item)
        {
            // Check input
            if (string.IsNullOrEmpty(item.Name))
            {
                return BadRequest();
            }

            // Set item in to data
            Db_Dummy data = new()
            {
                Name = item.Name
            };

            // Add data to context and save in database
            _context.Db_Dummy.Add(data);
            await _context.SaveChangesAsync();

            // Return <Dummy>
            return CreatedAtAction(nameof(GetItemById), new { id = data.Id }, await GetItemFromData(data));
        }


        /// <summary>
        /// Updates an existion dummy
        /// </summary>
        /// <param name="id">The ID</param>
        /// <param name="item">The updated question</param>
        [HttpPut("{id}", Name = nameof(UpdateItem))]
        public async Task<IActionResult> UpdateItem(int id, Dummy item)
        {
            if (id != item.Id
                || string.IsNullOrEmpty(item.Name))
            {
                return BadRequest();
            }

            // Get item from data
            var data = await _context.Db_Dummy
                .Where(e => e.Id == item.Id)
                .FirstOrDefaultAsync();
            if (data == null)
            {
                return BadRequest();
            }

            // Set item into data
            data.Id = item.Id;
            data.Name = item.Name;

            // Update data in context and save in database
            _context.Db_Dummy.Update(data);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction(nameof(GetItemById), new { id = data.Id }, await GetItemFromData(data));
        }

        /// <summary>
        /// Delete a dummy
        /// </summary>
        /// <param name="id">The ID</param>
        /// <returns></returns>
        [HttpDelete("{id}", Name = nameof(DeleteItem))]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var dummy = await _context.Db_Dummy.FindAsync(id);
            if (dummy == null)
            {
                return NotFound();
            }

            // Remove from context and safe in database
            _context.Db_Dummy.Remove(dummy);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ItemExists(int id)
        {
            return _context.Db_Dummy.Any(e => e.Id == id);
        }

        /// <summary>
        /// Gets item from database
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Item</returns>
        private async Task<Dummy> GetItemFromData(Db_Dummy data)
        {
            // Gets the item from database
            await _context.Db_Dummy
                .Where(e => e.Id == data.Id)
                .FirstOrDefaultAsync();

            // Mapping data from database to item
            Dummy item = new()
            {
                Id = data.Id,
                Name = data.Name
            };

            // Return item
            return item;
        }

}
}
