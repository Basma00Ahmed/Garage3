using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Garage3.Data;
using Garage3.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Garage3.Controllers
{
    public class VehicleTypeController : Controller
    {
        private readonly Garage3Context db;

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type,Size")] VehicleType vehicleType)
        {
            if (ModelState.IsValid)
            {
                bool typeExists = db.VehicleType.Any(t => t.Type == vehicleType.Type);
                if (!typeExists)
                {
                    db.Add(vehicleType);
                    await db.SaveChangesAsync();
                    TempData["Message"] = $"{vehicleType.Type} has been succssfully registered";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("PersonalNo", "A member with this personal number alredy exists.");
                }
            }
            return View();
        }
    }
}
