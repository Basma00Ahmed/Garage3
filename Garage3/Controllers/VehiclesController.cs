using AutoMapper;
using Garage3.Data;
using Garage3.Models;
using Garage3.Models.Entities;
using Garage3.Models.ViewModels.Vehicles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Garage3.Data;
using Garage3.Models.Entities;
using Garage3.Models.ViewModels.Vehicle;
using Garage3.Models;
using System.Diagnostics;

namespace Garage3.Views
{
    public class VehiclesController : Controller
    {
        private readonly Garage3Context _context;
        private readonly IMapper _mapper;

        public VehiclesController(Garage3Context context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }


        public async Task<IActionResult> Index(string RegNo, string drpVehicleType,string drpVehicleStatus)
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
                (string.IsNullOrWhiteSpace(drpVehicleType) || v.VehicleType.Type.StartsWith(drpVehicleType))&&
                (string.IsNullOrWhiteSpace(drpVehicleStatus) || v.IsCheckedOut == bool.Parse(drpVehicleStatus)));

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

        public async Task<IActionResult> Registration(RegistrationViewModel vehicle)
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
                    //var student = _mapper.Map<Vehicle>(vehicle);
                    _context.Add(_mapper.Map<Vehicle>(vehicle));
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
        public IActionResult VerifyPersonalNo(string personalNo)
        {
            bool PersonalNoNotExists = _context.Member.Any(m => m.PersonalNo == personalNo);
            if (!PersonalNoNotExists)
            {
                return Json($"A Member with personl number {personalNo} Not Has a membership.");
            }
            return Json(true);
        }

        public IActionResult VerifyMemberAge(string personalNo)
        {
            bool PersonalNoNotExists = _context.Member.Any(m => m.PersonalNo == personalNo);
            if (!PersonalNoNotExists)
            {
                return Json($"A Member with personl number {personalNo} Not Has a membership.");
            }
            return Json(true);
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
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        public IActionResult Home()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Home(string regNo)
        {
            TempData["UserMessage"] = "";
            if (await VehicleExist(regNo.ToUpper()) == false)
            {
                // If a vehicle doesn't exist, then we need to create a member or connect a member to this vehicle.
                TempData["UserMessage"] = $"The vehicle with license plate number {regNo.ToUpper()} isn't registrated and need to be registred.";
                return RedirectToAction(nameof(Registration));
            }
            else
            {
                // It's probably just checkedout atm and needs to checkin again.
                TempData["UserMessage"] = $"The vehicle with license plate number {regNo.ToUpper()} is already registrated and only need to check in.";
                return RedirectToAction(nameof(CheckIn), new { id = regNo });
            }
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            return RedirectToAction("Index");
        }

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

        public async Task<IActionResult> CheckOut()
        {
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Leave()
        {
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update()
        {
            return RedirectToAction("Index");
        }

        private async Task<bool> VehicleExist(string regNo)
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
  