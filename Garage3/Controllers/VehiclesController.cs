using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Garage3.Data;
using Garage3.Models.Entities;

namespace Garage3.Views
{
    public class VehiclesController : Controller
    {
        private readonly Garage3Context _context;

        public VehiclesController(Garage3Context context)
        {
            _context = context;
        }
        

        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            var garage3Context = _context.Vehicle.Include(v => v.Member);
            return View(await garage3Context.ToListAsync());
        }
        // GET: Vehicles Filter
        public async Task<IActionResult> Filter(string RegNo,string drpVehicleType)
        {
            var VehicleTypesList = _context.VehicleType

                                      .Select(e => new SelectListItem
                                      {
                                          Text = e.Type,
                                          Value = e.Type
                                      })
                                      .Distinct()
                                      .AsEnumerable();

            ViewBag.VehicleTypes = VehicleTypesList;

            var garage3Context = _context.Vehicle
                .Include(v => v.Member)
                .Include(v => v.VehicleType)
                .Where(v => (string.IsNullOrWhiteSpace(RegNo) || v.RegNo.Contains(RegNo)) &&
               (string.IsNullOrWhiteSpace(drpVehicleType) || v.VehicleType.Type.StartsWith(drpVehicleType)));

            return View(await garage3Context.ToListAsync());
        }
        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .Include(v => v.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }
        // GET: Vehicles/Details/5
        public async Task<IActionResult> VehicleDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .Include(v => v.Member)
                .Include(v => v.VehicleType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // GET: Vehicles/Create
        public IActionResult Create()
        {

            ViewData["VehicleType"] = new SelectList(_context.VehicleType, "Id", "Type"); 

            ViewData["MemberId"] = new SelectList(_context.Member, "Id", "Id");
            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RegNo,Make,Model,Color,NoOfWheels,IsCheckedOut,ArrivalTime,VehicleTypeId,MemberId")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                bool regNoExists = _context.Vehicle.Any(v => v.RegNo == vehicle.RegNo);
                if (!regNoExists)
                {
                    vehicle.RegNo = vehicle.RegNo.ToUpper();
                    vehicle.Color = vehicle.Color.Substring(0, 1).ToUpper() + vehicle.Color.Substring(1);
                    vehicle.Make = vehicle.Make.Substring(0, 1).ToUpper() + vehicle.Make.Substring(1);
                    vehicle.Model = vehicle.Model.Substring(0, 1).ToUpper() + vehicle.Model.Substring(1);
                    vehicle.ArrivalTime = System.DateTime.Now;
                    vehicle.IsCheckedOut = false;
                    _context.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("RegNo", "A vehicle with this register Number is already in the garage.");
                }
            }
            ViewData["MemberId"] = new SelectList(_context.Member, "Id", "Id", vehicle.MemberId);
            return View(vehicle);
        }

        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
             ViewData["VehicleType"] = new SelectList(_context.VehicleType, "Id", "Type");
            ViewData["MemberId"] = new SelectList(_context.Member, "Id", "Id", vehicle.MemberId);
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RegNo,Make,Model,Color,NoOfWheels,IsCheckedOut,ArrivalTime,VehicleTypeId,MemberId")] Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberId"] = new SelectList(_context.Member, "Id", "Id", vehicle.MemberId);
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .Include(v => v.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicle = await _context.Vehicle.FindAsync(id);
            _context.Vehicle.Remove(vehicle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicle.Any(e => e.Id == id);
        }
    }
}
