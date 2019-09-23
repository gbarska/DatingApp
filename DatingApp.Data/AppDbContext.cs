using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore;
using DatingApp.Domain.Models;


namespace DatingApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public AppDbContext(){}
        public DbSet<Value> Values { get; set; }   
        public DbSet<User> Users { get; set; }   

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {                
        }
        public override int SaveChanges()
        {
          return base.SaveChanges();  
        }
       
    }
    public class ContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder().Build();
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            var connString = "Server=localhost;Port=3306;Database=datingapp;Uid=gbarska;Pwd=password;";
            builder.UseMySql(connString);
            return new AppDbContext(builder.Options);
        }
    }

}