using HR_Portal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace HR_Portal.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging(true);
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestType> RequestTypes { get; set; }
        public DbSet<RequestStatus> RequestStatuses { get; set; }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<TrainingDestination> TrainingDestinations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Department>()
                .HasOne(d => d.Director)
                .WithMany()
                .HasForeignKey(d => d.DirectorId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<User>()
                .HasOne(m => m.Manager)
                .WithMany()
                .HasForeignKey(m => m.ManagerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.User)
                .WithMany()
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(p => p.Position)
                .WithMany()
                .HasForeignKey(p => p.PositionId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Position>()
                .HasOne(d => d.Department)
                .WithMany()
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull);

                modelBuilder.Entity<Position>().HasData(SeedPosition());
                modelBuilder.Entity<User>().HasData(SeedUser());
                modelBuilder.Entity<Training>().HasData(SeedTraining());
                modelBuilder.Entity<Department>().HasData(SeedDepartment());
                modelBuilder.Entity<RequestType>().HasData(SeedRequestType());
                modelBuilder.Entity<RequestStatus>().HasData(SeedRequestStatus());



            base.OnModelCreating(modelBuilder);

        }

        public List<Department> SeedDepartment()
        {
            string json = System.IO.File.ReadAllText(@"wwwroot\lib\files\department1.txt");
            List<Department> departments = JsonConvert.DeserializeObject<List<Department>>(json);
            return departments;
        }
        public List<Position> SeedPosition()
        {
            string json = System.IO.File.ReadAllText(@"wwwroot\lib\files\position.txt");
            List<Position> positions = JsonConvert.DeserializeObject<List<Position>>(json);
            return positions;
        }
        public List<User> SeedUser()
        {
            string json = System.IO.File.ReadAllText(@"wwwroot\lib\files\user.txt");
            List<User> users = JsonConvert.DeserializeObject<List<User>>(json);
            
            return users;
        }
        public List<Training> SeedTraining()
        {
            string json = System.IO.File.ReadAllText(@"wwwroot\lib\files\training.txt");
            List<Training> trainings = JsonConvert.DeserializeObject<List<Training>>(json);
            return trainings;
        }
        public List<RequestType> SeedRequestType()
        {
            string json = System.IO.File.ReadAllText(@"wwwroot\lib\files\requestType.txt");
            List<RequestType> requestTypes = JsonConvert.DeserializeObject<List<RequestType>>(json);
            return requestTypes;
        }
        public List<RequestStatus> SeedRequestStatus()
        {
            string json = System.IO.File.ReadAllText(@"wwwroot\lib\files\requestStatus.txt");
            List<RequestStatus> requestStatuses = JsonConvert.DeserializeObject<List<RequestStatus>>(json);
            return requestStatuses;
        }

    }
}
