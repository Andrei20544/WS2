namespace WSHospital
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ModelDB : DbContext
    {
        public ModelDB()
            : base("name=ModelDB1")
        {
        }

        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<LabServices> LabServices { get; set; }
        public virtual DbSet<NumberAnalyze> NumberAnalyze { get; set; }
        public virtual DbSet<Orderr> Orderr { get; set; }
        public virtual DbSet<Patients> Patients { get; set; }
        public virtual DbSet<Rendering> Rendering { get; set; }
        public virtual DbSet<Rolee> Rolee { get; set; }
        public virtual DbSet<SetService> SetService { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>()
                .HasMany(e => e.Patients)
                .WithOptional(e => e.Company)
                .HasForeignKey(e => e.IDCompany);

            modelBuilder.Entity<LabServices>()
                .HasMany(e => e.NumberAnalyze)
                .WithOptional(e => e.LabServices)
                .HasForeignKey(e => e.IDService);

            modelBuilder.Entity<LabServices>()
                .HasMany(e => e.Orderr)
                .WithOptional(e => e.LabServices)
                .HasForeignKey(e => e.IDService);

            modelBuilder.Entity<LabServices>()
                .HasMany(e => e.Rendering)
                .WithOptional(e => e.LabServices)
                .HasForeignKey(e => e.IDService);

            modelBuilder.Entity<LabServices>()
                .HasMany(e => e.SetService)
                .WithOptional(e => e.LabServices)
                .HasForeignKey(e => e.IDService);

            modelBuilder.Entity<Patients>()
                .HasMany(e => e.NumberAnalyze)
                .WithOptional(e => e.Patients)
                .HasForeignKey(e => e.IDPatient);

            modelBuilder.Entity<Patients>()
                .HasMany(e => e.Orderr)
                .WithOptional(e => e.Patients)
                .HasForeignKey(e => e.IDPatient);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.Rendering)
                .WithOptional(e => e.Users)
                .HasForeignKey(e => e.UserID);
        }
    }
}
