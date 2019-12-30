using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore;
using DatingApp.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace DatingApp.Data
{
    public class AppDbContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, 
    UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public AppDbContext(){}
        // public DbSet<Value> Values { get; set; }   
        // public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }         
        public DbSet<Like> Likes { get; set; }         
        public DbSet<Message> Messages { get; set; }         
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           
           //fix mysql error for key is too long 767byte
            modelBuilder.Entity<User>().Property(u => u.Id).HasMaxLength(127);
            modelBuilder.Entity<User>().Property(u => u.NormalizedEmail).HasMaxLength(127);
            modelBuilder.Entity<User>().Property(u => u.NormalizedUserName).HasMaxLength(127);
            modelBuilder.Entity<User>().Property(u => u.Email).HasMaxLength(127);
            modelBuilder.Entity<User>().Property(u => u.UserName).HasMaxLength(127);
           
            modelBuilder.Entity<Photo>().HasQueryFilter(p => p.IsApproved);

            modelBuilder.Entity<Role>().Property(u => u.NormalizedName).HasMaxLength(127);
            modelBuilder.Entity<Role>().Property(u => u.Name).HasMaxLength(127);

            modelBuilder.Entity<UserRole>(u => {
                u.HasKey(ur => new { ur.UserId, ur.RoleId });

                u.HasOne(ur =>  ur.User).WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

                u.HasOne(ur =>  ur.Role).WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
            });

        modelBuilder.Entity<IdentityUserLogin<int>>(entity => 
        { 
            entity.HasKey(m => m.UserId);
            entity.Property(m => m.LoginProvider).HasMaxLength(127); 
            entity.Property(m => m.ProviderKey).HasMaxLength(127); 
        }); 

        modelBuilder.Entity<IdentityUserToken<int>>(entity => 
        { 
            entity.Property(m => m.UserId).HasMaxLength(127); 
            entity.Property(m => m.LoginProvider).HasMaxLength(127); 
            entity.Property(m => m.Name).HasMaxLength(127); 
        }); 

            modelBuilder.Entity<Like>() 
                .HasKey(k => new {k.LikerId, k.LikeeId});
            modelBuilder.Entity<Like>()
                .HasOne(u => u.Likee)
                .WithMany(u => u.Likers)
                .HasForeignKey(u => u.LikeeId)
                .OnDelete(DeleteBehavior.Restrict);
             
             modelBuilder.Entity<Like>()
                .HasOne(u => u.Liker)
                .WithMany(u => u.Likees)
                .HasForeignKey(u => u.LikerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany(m => m.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);
                            
            modelBuilder.Entity<Message>()
                .HasOne(u => u.Recipient)
                .WithMany(m => m.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict);
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