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
    public class LocalDate
    {
        [Key]
        public int ID { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yy}", ApplyFormatInEditMode = false)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.DateTime)]
        [Column(TypeName = "datetime2")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yy HH:mm:ss}", ApplyFormatInEditMode = false)]
        public DateTime EndDate{ get; set; }
    }

    public class DateContext : DbContext
    {
        public DbSet<LocalDate> Dates { get; set; }


        //public DateContext() : base()
        //{

        //}

        public DateContext(DbContextOptions<DateContext> options) : base(options)
        {
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
            modelBuilder.Entity<LocalDate>().HasData(new LocalDate[] {
                new LocalDate
                {
                    ID = 1,
                    StartDate = new DateTime(day: 25, month: 05, year: 2022),
                    EndDate = new DateTime(day: 26, month: 05, year: 2022, hour:13,minute:20,second:23),

                },
            });
        }
    }
}
