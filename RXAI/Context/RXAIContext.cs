using Microsoft.EntityFrameworkCore;
using RXAI.Entities;
using RXAI.Entities.RXAI.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace RXAI.Context
{
    public class RXAIContext : DbContext
    {
        public RXAIContext(DbContextOptions<RXAIContext> options) : base(options)
        {
        }
        public DbSet<Disease> Diseases { get; set; }
        public DbSet<ActiveIngredientBase> ActiveIngredientBases { get; set; }
        public DbSet<ActiveIngredientVariant> ActiveIngredientVariants { get; set; }
        public DbSet<TradeName> TradeNames { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Disease>()
                .HasKey(d => d.ICDCode);

            modelBuilder.Entity<ActiveIngredientBase>()
                .HasKey(a => a.DrugBankID);

            modelBuilder.Entity<ActiveIngredientBase>()
                .Property(a => a.IngredientName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<ActiveIngredientVariant>()
                .HasKey(a => new { a.DrugBankID, a.Strength, a.StrengthUnit });

            modelBuilder.Entity<ActiveIngredientVariant>()
                .HasOne(a => a.BaseIngredient)
                .WithMany(b => b.Variants)
                .HasForeignKey(a => a.DrugBankID);

            modelBuilder.Entity<ActiveIngredientVariant>()
                .HasOne(a => a.Disease)
                .WithMany(d => d.ActiveIngredientVariants)
                .HasForeignKey(a => a.ICDCode);

            modelBuilder.Entity<TradeName>()
                .HasKey(t => new { t.SKUCode });

            modelBuilder.Entity<TradeName>()
                .HasOne(t => t.ActiveIngredientVariant)
                .WithMany(a => a.Trades)
                .HasForeignKey(t => new { t.DrugBankID, t.Strength, t.StrengthUnit });

            modelBuilder.Entity<TradeName>()
                .Property(t => t.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Prescription>()
                .HasKey(p => p.PrescriptionID);

            modelBuilder.Entity<Prescription>()
                .Property(p => p.PrescriptionID)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Prescription>()
                .Property(p => p.Prescription_Description)
                .IsRequired();

            modelBuilder.Entity<Prescription>()
                .Property(p => p.Dose);

            modelBuilder.Entity<Prescription>()
                .Property(p => p.Form);

            modelBuilder.Entity<Prescription>()
                .Property(p => p.PrescriptionDate)
                .IsRequired();

            modelBuilder.Entity<Prescription>()
                .Property(p => p.Dispensedmedication);

            modelBuilder.Entity<Prescription>()
                .Property(p => p.DrugBankID)
                .HasMaxLength(20);

            modelBuilder.Entity<Prescription>()
                .HasOne(p => p.ActiveIngredientVariant)
                .WithMany(a => a.Prescriptions)
                .HasForeignKey(p => new { p.DrugBankID, p.Strength, p.StrengthUnit });

            modelBuilder.Entity<Prescription>()
                .HasOne(p => p.Patient)
                .WithMany(pat => pat.Prescriptions)
                .HasForeignKey(p => p.PhoneNumber);

            modelBuilder.Entity<Patient>()
                .HasKey(p => p.PhoneNumber);

            modelBuilder.Entity<Patient>()
                .Property(p => p.PhoneNumber)
                .HasMaxLength(20);

            modelBuilder.Entity<Patient>()
                .Property(p => p.PatientName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Patient>()
                .Property(p => p.RegistrationDate)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Prescriptions)
                .WithOne(pr => pr.Patient)
                .HasForeignKey(pr => pr.PhoneNumber);
        }
    }
}