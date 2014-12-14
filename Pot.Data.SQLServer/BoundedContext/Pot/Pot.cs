namespace Pot.Data.SQLServer
{
    using System.Data.Entity;

    using global::Pot.Data.Model;

    public partial class Pot : DbContext
    {
        public Pot()
            : base("name=Pot")
        {
        }

        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectUser> ProjectUsers { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
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

            modelBuilder.Entity<User>()
                .Property(e => e.Version)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .HasMany(e => e.Expenses)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ProjectUsers)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);
        }
    }
}
