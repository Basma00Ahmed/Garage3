using Garage_3._0.Data;
using Garage_3._0.Models;
using Garage_3._0.Models.ViewModels.Vehicle;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Garage_3._0.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly AppDbContext _context;

        public VehiclesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = _context.Vehicles.Include(v => v.VehicleType);
            return View(await model.ToListAsync());
        }
        
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
                return RedirectToAction(nameof(CheckIn), new { id = regNo }) ;
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

            var model = await _context.Vehicles
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
            return await _context.Vehicles.AnyAsync(v => v.RegNo == regNo);
        } 

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
