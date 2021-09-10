using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Garage3.Data;
using Garage3.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Garage3.Controllers
{
    public class VehicleTypeController : Controller
    {
        private readonly Garage3Context db;

        public VehicleTypeController(Garage3Context context)
        {
            db = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await db.VehicleType.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string size, VehicleType vehicleType)
        {

            if (ModelState.IsValid)
            {
                bool typeExists = db.VehicleType.Any(t => t.Type == vehicleType.Type);
                if (!typeExists)
                {
                    vehicleType.Size = Double.Parse(size);
                    db.Add(vehicleType);
                    await db.SaveChangesAsync();
                    TempData["Message"] = $"{vehicleType.Type} has been succssfully registered";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("vehicleType", "A Vehicle type with this name alredy exists.");
                }
            }
            return View(vehicleType);
        }

        public IActionResult VerifyVehicleType(string Type)
        {
            bool vehicleTypeExists = db.VehicleType.Any(v => v.Type == Type);
            if (vehicleTypeExists)
            {
                return Json($"{Type} already exists.");
            }
            return Json(true);
        }
    }
}