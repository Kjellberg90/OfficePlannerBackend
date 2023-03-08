using DAL.Models;
using DAL.SQLModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class SkyDbContext : DbContext
    {
        public DbSet<SQLRoom> Rooms { get; set; }
        public DbSet<SQLGroup> Groups { get; set; }
        public DbSet<SQLBooking> Bookings { get; set; }
        public DbSet<SQLSchedule> Schedules { get; set; }
        public DbSet<SQLSingleBooking> SingleBookings { get; set; }
        public DbSet<SQLUser> Users { get; set; }
        public DbSet<SQLSingleRoomBooking> SingleRoomBookings { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./ProjectSKY_DB.sqlite");
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<SQLBooking>()
            //    .HasOne<SQLSchedule>()
            //    .WithMany(s => s.Bookings)
            //    .HasForeignKey(b => b.ScheduleId);

            //modelBuilder.Entity<SQLSingleBooking>()
            //    .HasNoKey();

            modelBuilder.Entity<SQLRoom>().HasData(
                new SQLRoom { Id = 1, Name = "Collaboration", Seats = 6 },
                new SQLRoom { Id = 2, Name = "Commitment", Seats = 6 },
                new SQLRoom { Id = 3, Name = "Innovation", Seats = 8 },
                new SQLRoom { Id = 4, Name = "Inspired", Seats = 8 },
                new SQLRoom { Id = 5, Name = "United", Seats = 6 },
                new SQLRoom { Id = 6, Name = "Inspired A", Seats = 4 },
                new SQLRoom { Id = 7, Name = "Inspired B", Seats = 4 }
                );

            modelBuilder.Entity<SQLGroup>().HasData(
                new SQLGroup { Id = 1, Name = "DQT", GroupSize = 6, Department = "A", },
                new SQLGroup { Id = 2, Name = "Phoenix", GroupSize = 4, Department = "A", },
                new SQLGroup { Id = 3, Name = "Bazinga", GroupSize = 4, Department = "A", },
                new SQLGroup { Id = 4, Name = "Consys", GroupSize = 4, Department = "A", },
                new SQLGroup { Id = 5, Name = "PAST", GroupSize = 6, Department = "B", },
                new SQLGroup { Id = 6, Name = "Heimdall", GroupSize = 6, Department = "B", },
                new SQLGroup { Id = 7, Name = "Battery", GroupSize = 5, Department = "B", },
                new SQLGroup { Id = 8, Name = "ConCore", GroupSize = 7, Department = "C", },
                new SQLGroup { Id = 9, Name = "Everest", GroupSize = 6, Department = "C", },
                new SQLGroup { Id = 10, Name = "Portal", GroupSize = 2, Department = "C", }
                );

            modelBuilder.Entity<SQLSchedule>().HasData(
                new SQLSchedule { Id = 1, Name = "Default", WeekInterval = 3 }
                );

        }
    }
}
