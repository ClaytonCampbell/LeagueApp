﻿using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LeagueApp.Data
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<Team> Teams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder
            .Conventions
            .Remove<PluralizingTableNameConvention>();

            modelBuilder
                .Configurations
                .Add(new IdentityUserLoginConfiguration())
                .Add(new IdentityUserRoleConfiguration());

            // add code so you can delete a team or coach, but need front end to say cannot delete with ..
            modelBuilder.Entity<Player>()
                //.HasOptional<Team>(s => s.Team)
                .HasRequired<Team>(s => s.Team)
                .WithMany()
                .WillCascadeOnDelete(false);
            //Change to required for a team to have a coach
            modelBuilder.Entity<Coach>()
                .HasRequired<Team>(s => s.Team)
                //.HasOptionnal<Team>(s => s.Team)
                .WithMany()
                .WillCascadeOnDelete(false);
            // end

            //add code for team
            modelBuilder.Entity<Team>()
                .HasMany<Coach>(t => t.Coaches)
                //.WithOptional(e=> e.Team)
                .WithRequired(e => e.Team)
                .HasForeignKey<int>(s => s.TeamId);

            modelBuilder.Entity<Team>()
                .HasMany<Player>(t => t.Players)
                //.WithOptional(e=> e.Team)
                .WithRequired(e => e.Team)
                .HasForeignKey<int>(s => s.TeamId);

            //modelBuilder.Entity<Team>().HasData(
                
                
            //    )
        }
    }
    public class IdentityUserLoginConfiguration : EntityTypeConfiguration<IdentityUserLogin>
    {
        public IdentityUserLoginConfiguration()
        {
            HasKey(iul => iul.UserId);
        }
    }
    public class IdentityUserRoleConfiguration : EntityTypeConfiguration<IdentityUserRole>
    {
        public IdentityUserRoleConfiguration()
        {
            HasKey(iur => iur.UserId);
        }
    }
}