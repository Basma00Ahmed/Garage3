using Garage3.Models.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Garage3.Models.ViewModels.ReceiptVM
{
    public class ReceiptViewModel
    {
        //Fields
        private decimal totalTime;
        private decimal hourlyRate;
        private decimal totalCost;

        //Properties
        public int Id { get; set; }

        [Display(Name = "Registration No")]
        public string RegNo { get; set; }

        public String VehicleType { get; set; }

        public double VehicleSize { get; set; }

        [Display(Name = "Customer")]
        public string CustomerName { get; set; }

        [Display(Name = "From")]
        public DateTime ArrivalTime { get; set; }

        [Display(Name = "To")]
        public DateTime EndTime { get; set; }

        [Display(Name = "Total Time")]
        public decimal TotalTime
        {
            get
            {
                totalTime = (decimal)Math.Round((EndTime - ArrivalTime).TotalHours, 2);
                return totalTime;
            }
        }

        [Display(Name = "Hourly Rate")]
        public decimal HourlyRate
        {
            get
            {
                hourlyRate = (decimal)VehicleSize * 10; // 10kr/hour for one parking place
                return hourlyRate;
            }
        }

        [Display(Name = "Total Cost")]
        public decimal TotalCost
        {
            get
            {
                totalCost = Math.Round(TotalTime * HourlyRate, 2);
                return totalCost;
            }
        }
    }
}