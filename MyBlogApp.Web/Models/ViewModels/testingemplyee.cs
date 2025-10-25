using System;
using System.ComponentModel.DataAnnotations;

namespace MyApp.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        // The date employee joined the company
        public DateTime DateOfJoining { get; set; }

        public string Position { get; set; }

        // computed read-only property for years of experience
        public int YearsOfExperience
        {
            get
            {
                var now = DateTime.UtcNow.Date;
                var years = now.Year - DateOfJoining.Date.Year;
                if (DateOfJoining.Date > now.AddYears(-years)) years--;
                return Math.Max(0, years);
            }
        }
    }
}