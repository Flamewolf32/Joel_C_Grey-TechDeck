using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TechDesk.Data;
using TechDesk.Models;

namespace TechDesk.Controllers
{
    public class DevicesController : Controller
    {
        private readonly AppDbContext _context;

        public DevicesController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("devices")]
        public async Task<IActionResult> Index(string searchName, bool availableOnly)
        {
            IQueryable<Device> query = _context.Devices;

            if (!string.IsNullOrEmpty(searchName))
            {
                query = query.Where(d => d.Name.Contains(searchName));
            }
            if (availableOnly)
            {
                query = query.Where(d => !d.IsOnLoan);
            }
            query = query.OrderBy(d => d.AssetTag);
            var devices = await query.ToListAsync();

            ViewBag.SearchName = searchName;
            ViewBag.AvailableOnly = availableOnly;

            ViewBag.DeviceCount = $"{devices.Count} device{(devices.Count == 1 ? "" : "s")}";

            return View(devices);



        }

        [HttpGet("devices/add")]
        public IActionResult Add() => View();

        [HttpPost("devices/add")]
        public async Task<IActionResult> Add(Device device)
        {
            if (await _context.Devices.AnyAsync(d => d.AssetTag == device.AssetTag))
            {
                ModelState.AddModelError("AssetTag", "A device with this Asset Tag already exists.");
            }

            if (ModelState.IsValid)
            {
                _context.Devices.Add(device);
                await _context.SaveChangesAsync();
                TempData["Message"] = $"{device.AssetTag} {device.Name} was added.";
                return RedirectToAction(nameof(Index));
            }
            return View(device);
        }
        [HttpGet("devices/edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var device = await _context.Devices.FindAsync(id);
            if (device == null) return NotFound();

            return View(device);
        }

        [HttpPost("devices/edit/{id:int}")]
        public async Task<IActionResult> Edit(int id, Device device)
        {
            if (id != device.Id) return NotFound();

            if (await _context.Devices.AnyAsync(d => d.AssetTag == device.AssetTag && d.Id != id))
            {
                ModelState.AddModelError("AssetTag", "Another device already has that asset tag.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(device);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = $"{device.AssetTag} {device.Name} was updated.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Devices.Any(e => e.Id == device.Id)) return NotFound();
                    throw;
                }
            }
            return View(device);
        }
        [HttpGet("devices/delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var device = await _context.Devices.FindAsync(id);
            if (device == null)
            {
                TempData["Message"] = "That device was already deleted.";
                return RedirectToAction(nameof(Index));
            }
            return View(device);
        }

        [HttpPost("devices/delete/{id:int}"), ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var device = await _context.Devices.FindAsync(id);
            if (device == null) 
            {
                TempData["Message"] = "That device was already deleted.";
                return RedirectToAction(nameof(Index));
            }

            string identifier = $"{device.AssetTag} {device.Name}";
            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"{identifier} was deleted.";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet("devices/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var device = await _context.Devices.FirstOrDefaultAsync(d => d.Id == id);

            if (device == null)
            {
                return NotFound(); 
            }

            return View(device);
        }

    }
}

