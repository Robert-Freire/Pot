namespace Pot.Data.SQLServer
{
    using System.Data.Entity;

    using global::Pot.Data.Model;

    using Microsoft.AspNet.Identity.EntityFramework;

    public class PotDbContext : IdentityDbContext<IdentityUser>
    {
        public PotDbContext()
            : base("name=Pot")
        {
        }

        public virtual DbSet<Expense> Expenses { get; set; }

        public virtual DbSet<Project> Projects { get; set; }

        public virtual DbSet<ProjectUser> ProjectUsers { get; set; }

        public virtual DbSet<User> AppUsers { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Expense>()
                .Property(e => e.Amount)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Expense>()
                .Property(e => e.Version)
                .IsFixedLength();

            modelBuilder.Entity<Project>()
                .Property(e => e.Version)
                .IsFixedLength();

            modelBuilder.Entity<Project>()
                .HasMany(e => e.Expenses)
                .WithRequired(e => e.Project)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Project>()
                .HasMany(e => e.ProjectUsers)
                .WithRequired(e => e.Project)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProjectUser>()
                .HasMany(e => e.Expenses)
                .WithRequired(e => e.ProjectUser)
                .HasForeignKey(e => new { e.UserId, e.ProjectId })
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<User>()
            //    .Property(e => e.Version)
            //    .IsFixedLength();

            modelBuilder.Entity<User>()
                .HasMany(e => e.Expenses)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ProjectUsers)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            // Change the name of the table to be Users instead of AspNetUsers
            //modelBuilder.Entity<IdentityUser>()
            //    .ToTable("User");
            //modelBuilder.Entity<User>()
            //    .ToTable("User");
        }
    }
}
