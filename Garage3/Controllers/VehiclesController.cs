using Garage3.Data;
using Garage3.Models;
using Garage3.Models.Entities;
using Garage3.Models.ViewModels.Vehicles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly Garage3Context _context;

        public VehiclesController(Garage3Context context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index(string RegNo, string drpVehicleType)
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

        public IActionResult Home()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Home(string regNo)
        {
            TempData["UserMessage"] = "";
            if (await VehicleExistsByRegNo(regNo.ToUpper()) == false)
            {
                // If a vehicle doesn't exist, then we need to create a member or connect a member to this vehicle.
                TempData["UserMessage"] = $"The vehicle with license plate number {regNo.ToUpper()} isn't registrated and need to be registred.";
                return RedirectToAction(nameof(Registration));
            }
            else
            {
                // It's probably just checkedout atm and needs to checkin again.
                if(await _context.Vehicle.AnyAsync(v => v.RegNo == regNo && v.IsCheckedOut == false))
                {
                    TempData["UserMessage"] = $"The vehicle with license plate number {regNo.ToUpper()} is already parked in the garage. Please try again.";
                    return View();
                }
                TempData["UserMessage"] = $"The vehicle with license plate number {regNo.ToUpper()} is already registrated and only need to check in.";
                return RedirectToAction(nameof(CheckIn), new { id = regNo }) ;
            }
        }

        public IActionResult Registration()
        {
            ViewData["VehicleType"] = new SelectList(_context.VehicleType, "Id", "Type");

            ViewData["MemberId"] = new SelectList(_context.Member, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration([Bind("Id,RegNo,Make,Model,Color,NoOfWheels,IsCheckedOut,ArrivalTime,VehicleTypeId,MemberId")] Vehicle vehicle)
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

        // CHECKIN VIEW NEEDS TO BE COMPLETE!!!
        [Route("Vehicles/CheckIn/{regNo}")] // Route need to be definied to work with a string as param at this point.
        public async Task<IActionResult> CheckIn(string regNo)
        {
            if (regNo == null)
            {
                return NotFound();
            }

            var model = await _context.Vehicle
                                      .FirstOrDefaultAsync(v => v.RegNo == regNo);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        public async Task<IActionResult> CheckOut(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var vehicle = await _context.Vehicle.FindAsync(id);
            vehicle.IsCheckedOut = true;

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
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Leave(int? id)
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

            var model = new LeaveViewModel
            {
                Id = vehicle.Id,
                RegNo = vehicle.RegNo,
                Type = vehicle.VehicleType.Type,
                Make = vehicle.Make,
                Model = vehicle.Model,
                ArrivalTime = vehicle.ArrivalTime,
                MemberFullName = $"{vehicle.Member.FirstName} {vehicle.Member.LastName}"
            };
            return View(model);
        }

        [HttpPost, ActionName("Leave")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LeaveConfirmed(int id)
        {
            var vehicle = await _context.Vehicle.FindAsync(id);
            
            _context.Vehicle.Remove(vehicle);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Receipt", vehicle); // Action, Controller, object
        }

        public async Task<IActionResult> Update(int? id)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, [Bind("Id,RegNo,Make,Model,Color,NoOfWheels,IsCheckedOut,ArrivalTime,VehicleTypeId,MemberId")] Vehicle vehicle)
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

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(int? id)
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

        private bool VehicleExists(int id)
        {
            return _context.Vehicle.Any(e => e.Id == id);
        }

        private async Task<bool> VehicleExistsByRegNo(string regNo)
        {
            return await _context.Vehicle.AnyAsync(v => v.RegNo == regNo);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
