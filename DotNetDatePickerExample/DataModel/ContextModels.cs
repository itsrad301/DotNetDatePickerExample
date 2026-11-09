using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateExample.DataModel
{
    /// <summary>
    /// Custom validation attribute to ensure EndDate is greater than StartDate
    /// </summary>
    public class EndDateGreaterThanStartDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Get the instance of the object being validated
            var localDate = validationContext.ObjectInstance as LocalDate;
            // allowing for nullable date and also if attribute
            // is incorrectly added to a different object            
            if (localDate == null)
                return ValidationResult.Success;

            if (localDate.EndDate <= localDate.StartDate)
            {
                return new ValidationResult(
                    "EndDate must be greater than StartDate.",
                    new[] { nameof(LocalDate.EndDate) });
            }
            return ValidationResult.Success;
        }
    }

    [EndDateGreaterThanStartDate] // Note the Attribute postfix is dropped like the Controller postfix
    // This class represents a local date range with a start and end date.
    // It is the local reference to be validated with the custom attribute EndDateGreaterThanStartDateAttribute
    public class LocalDate
    {
        [Key]
        public int ID { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yy}", ApplyFormatInEditMode = false)]
        [Required(ErrorMessage ="You forgot the start date!!")] // This override the standard required message
                                                                // as dates are required by default
                                                                // Try commenting it out and inspect in the 
                                                                // Browser console window F12 inspect the Date control
        public DateTime StartDate { get; set; }
        [DataType(DataType.DateTime)]
        [Column(TypeName = "datetime2")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yy HH:mm:ss}", ApplyFormatInEditMode = false)]
        public DateTime EndDate { get; set; }
    }

    public class DateContext : DbContext
    {
        public DbSet<LocalDate> Dates { get; set; }


        //public DateContext() : base()
        //{

        //}

        public DateContext(DbContextOptions<DateContext> options) : base(options)
        {
            // No migrations , just create the database if it doesn't exist
            Database.EnsureCreated();

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var myconnectionstring = "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = DateExample2023-DB";
            optionsBuilder.UseSqlServer(myconnectionstring)
              .LogTo(Console.WriteLine,
                     new[] { DbLoggerCategory.Database.Command.Name },
                     LogLevel.Information).
                        EnableSensitiveDataLogging(true);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // activated by Database.EnsureCreated() in the constructor
            modelBuilder.Entity<LocalDate>().HasData(new LocalDate[] {
                new LocalDate
                {
                    ID = 1,
                    StartDate = new DateTime(day: 25, month: 05, year: 2026),
                    EndDate = new DateTime(day: 26, month: 05, year: 2026, hour:13,minute:20,second:23),

                },
            });
        }
    }
}
