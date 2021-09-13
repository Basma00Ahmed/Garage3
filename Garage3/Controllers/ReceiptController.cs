using Garage3.Data;
using Garage3.Models.Entities;
using Garage3.Models.ViewModels.ReceiptVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage3.Controllers
{
    public class ReceiptController : Controller
    {
        private readonly Garage3Context _context;

        public ReceiptController(Garage3Context context)
        {
            _context = context;
        }

        //[Route("Receipt/Index/{vehicle}")]
        [HttpGet]
        public async Task<IActionResult> Index(Vehicle vehicle)
        {
            /*var vehicle = await _context.Vehicle
                                .Include(v => v.Member)
                                .Include(v => v.VehicleType)
                                .FirstOrDefaultAsync(v => v.Id == id);*/

            var tempVehicle = (Vehicle)TempData["tempVehicle"];

            if (vehicle == null)
            {
                return Content("Vehicle was null");
            }

            var receipt = new ReceiptViewModel
            {
                Id = vehicle.Id,
                RegNo = vehicle.RegNo,
                CustomerName = $"{vehicle.Member.FirstName} {vehicle.Member.LastName}",
                VehicleType = vehicle.VehicleType.Type,
                VehicleSize = vehicle.VehicleType.Size,
                ArrivalTime = vehicle.ArrivalTime,
                EndTime = DateTime.Now,
            };

            return View(receipt);
        }
    }
}
