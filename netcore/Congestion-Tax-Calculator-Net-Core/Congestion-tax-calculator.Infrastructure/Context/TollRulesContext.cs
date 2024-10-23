using Congestion_tax_calculator.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Congestion_tax_calculator.Infrastructure.Context
{
    public class TollRulesContext : DbContext
    {


        public DbSet<CityTollRule> CityTollRules { get; set; }

        public DbSet<City> Cities { get; set; }


        public TollRulesContext(DbContextOptions<TollRulesContext> options) : base(options)
        {

        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //var connectionString = Environment.GetEnvironmentVariable("EFCORETOOLSDB");
            //if (string.IsNullOrEmpty(connectionString))
            //    throw new InvalidOperationException("Sql server connection string configuration required teeeeeeet");

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer("Data Source=.;Initial Catalog=TollFeeInfo;Integrated Security=True;Encrypt=False");
                    
            }

            base.OnConfiguring(optionsBuilder);
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<City>().HasData(
                
                new City {ID=1,Name="Gothenburg" , Description=""}
                               

            );


            modelBuilder.Entity<CityTollRule>().HasData(
               
                new CityTollRule { Id=1,   CityId = 1, From = "06:00", To = "06:29", Amount = 8 },
                new CityTollRule { Id = 2, CityId = 1, From = "06:30", To = "06:59", Amount = 13 },
                new CityTollRule { Id = 4, CityId = 1, From = "07:00", To = "07:59", Amount = 18 },
                new CityTollRule { Id = 5, CityId = 1, From = "08:00", To = "08:29", Amount = 13 },
                new CityTollRule { Id = 6, CityId = 1, From = "08:30", To = "14:59", Amount = 8 },
                new CityTollRule { Id = 7, CityId = 1, From = "15:00", To = "15:29", Amount =13 },
                new CityTollRule { Id = 8, CityId = 1, From = "15:30", To = "16:59", Amount = 18 },
                new CityTollRule { Id = 9, CityId = 1, From = "17:00", To = "17:59", Amount = 13 },
                new CityTollRule { Id = 10, CityId = 1, From = "18:00", To = "18:29", Amount = 8 },
                new CityTollRule { Id = 11, CityId = 1, From = "18:30", To = "05:59", Amount = 0 }


                );

             
            //// configures one-to-many relationship
            //modelBuilder.Entity<Student>()
            //    .HasRequired<Grade>(s => s.CurrentGrade)
            //    .WithMany(g => g.Students)
            //    .HasForeignKey<int>(s => s.CurrentGradeId);
        }
    }
}
