using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Garage3.Data;
using Garage3.Models.Entities;
using Garage3.Models.ViewModels.Vehicles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Garage3.Controllers
{
    public class ParkingController : Controller
    {
        private readonly Garage3Context db;
        private readonly IMapper _mapper;
        private readonly int GarageCapacity = 100;

        public ParkingController(Garage3Context context, IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }

       /* public async Task<IActionResult> Index()
        {
            return View(await db.Parking.ToListAsync());
        }

        public IActionResult ParkVehicle()
        {
            return View();
        }
        */
        public async Task<IActionResult> ParkVehicle(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await db.Vehicle.FindAsync(id);
            
            var vehicleType = await db.VehicleType.FindAsync(vehicle.VehicleTypeId);
            double size = vehicleType.Size;
            Spot spot;
            if (size < 1)
            {
                spot = db.Spot.FirstOrDefault(sp => sp.Capacity > size);
                if (spot != null)
                {
                    spot.Capacity = spot.Capacity - size;
                    spot.IsAvailable = false;
                    Parking parking = new Parking
                    {
                        SpotId = spot.Id,
                        VehicleId = vehicle.Id
                    };
                    db.Update(spot);
                    db.Add(parking);
                    await db.SaveChangesAsync();
                }
            }
            else if (size == 1)
            {
                spot = db.Spot.FirstOrDefault(sp => sp.IsAvailable == true);
                if (spot != null)
                {
                    spot.Capacity = 0;
                    spot.IsAvailable = false;
                    Parking parking = new Parking
                    {
                        SpotId = spot.Id,
                        VehicleId = vehicle.Id
                    };
                    db.Update(spot);
                    db.Add(parking);
                    await db.SaveChangesAsync();
                }
            }
            else if(size > 1)
            {
                List<Spot> spots = new List<Spot>();
                spot = db.Spot.FirstOrDefault(sp => sp.IsAvailable == true);
                if(spot != null)
                {
                    spot.Capacity = 0;
                    spot.IsAvailable = false;
                    size -= 1;
                    spots.Add(spot);
                }
                int count = 0;
                do
                {
                    spot = db.Spot.FirstOrDefault(s => s.Id == (spot.Id + 1));
                    if (spot != null && spot.IsAvailable == true)
                    {
                        spot.Capacity = 0;
                        spot.IsAvailable = false;
                        size -= 1;
                        spots.Add(spot);
                    }
                    else
                    {
                        size = vehicleType.Size;
                        spots.Clear();
                        spot.IsAvailable = true;
                    }
                    count++;
                } while (size > 0 && count <= GarageCapacity);
                if (spots.Count > 0)
                {
                    foreach(Spot s in spots)
                    {
                        Parking parking = new Parking
                        {
                            SpotId = s.Id,
                            VehicleId = vehicle.Id
                        };
                        db.Update(spot);
                        db.Add(parking);
                    }
                    await db.SaveChangesAsync();
                }
            }
            return View();
            //var parkedVehicle = await _mapper.ProjectTo<CheckInViewModel>(db.Parking).FirstOrDefaultAsync(p => p. == vehicle.RegNo);
            //return View(nameof(Index),parkedVehicle);
           // return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UnParkVehicle(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var vehicle = await db.Vehicle.FirstOrDefaultAsync(v => v.Id == id);
            if(vehicle == null)
            {
                return NoContent();
            }
            return View();
        }
        [HttpPost, ActionName("UnParkVehicle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnParkConfirmed(int id)
        {
            var vehicle = await db.Vehicle.FindAsync(id);
            
            return RedirectToAction(nameof(Index));
        }
    }
}
