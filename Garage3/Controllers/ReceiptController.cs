using Garage3.Data;
using Garage3.Models.Entities;
using Garage3.Models.ViewModels.ReceiptVM;
using Microsoft.AspNetCore.Mvc;
using System;
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

        [HttpGet]
        public async Task<IActionResult> Index(Vehicle vehicle)
        {
            var member = await _context.Member.FindAsync(vehicle.MemberId);
            var vehicleType = await _context.VehicleType.FindAsync(vehicle.VehicleTypeId);

            if (vehicle == null)
            {
                return Content("Vehicle was null");
            }

            ReceiptViewModel receipt = null;
            if (vehicle.IsCheckedOut == true)
            {
                receipt = new ReceiptViewModel
                {
                    Id = vehicle.Id,
                    RegNo = vehicle.RegNo,
                    CustomerName = $"{member.FirstName} {member.LastName}",
                    VehicleType = vehicleType.Type,
                    VehicleSize = vehicleType.Size,
                    ArrivalTime = vehicle.ArrivalTime,
                    EndTime = DateTime.Now,
                };
            }
            else
            {
                receipt = new ReceiptViewModel
                {
                    RegNo = "none"
                };
            }

            return View(receipt);
        }
    }
}