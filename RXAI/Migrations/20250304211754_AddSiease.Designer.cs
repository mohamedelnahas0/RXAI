﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RXAI.Context;

#nullable disable

namespace RXAI.Migrations
{
    [DbContext(typeof(RXAIContext))]
    [Migration("20250304211754_AddSiease")]
    partial class AddSiease
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("RXAI.Entities.ActiveIngredientBase", b =>
                {
                    b.Property<string>("DrugBankID")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("ICDCode")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("IngredientName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("DrugBankID");

                    b.HasIndex("ICDCode");

                    b.ToTable("ActiveIngredientBases");
                });

            modelBuilder.Entity("RXAI.Entities.ActiveIngredientVariant", b =>
                {
                    b.Property<string>("DrugBankID")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasColumnOrder(0);

                    b.Property<string>("Strength")
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)")
                        .HasColumnOrder(1);

                    b.Property<string>("StrengthUnit")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasColumnOrder(2);

                    b.HasKey("DrugBankID", "Strength", "StrengthUnit");

                    b.ToTable("ActiveIngredientVariants");
                });

            modelBuilder.Entity("RXAI.Entities.Disease", b =>
                {
                    b.Property<string>("ICDCode")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("DiseaseName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("ICDCode");

                    b.ToTable("Diseases");
                });

            modelBuilder.Entity("RXAI.Entities.Patient", b =>
                {
                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("PatientName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("RegistrationDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.HasKey("PhoneNumber");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("RXAI.Entities.RXAI.Entities.Prescription", b =>
                {
                    b.Property<int>("PrescriptionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PrescriptionID"));

                    b.Property<string>("Dispensedmedication")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Dose")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DrugBankID")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Form")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IngredientName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("PrescriptionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Prescription_Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("QuantityDispensed")
                        .HasColumnType("int");

                    b.Property<string>("Strength")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.Property<string>("StrengthUnit")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("PrescriptionID");

                    b.HasIndex("PhoneNumber");

                    b.HasIndex("DrugBankID", "Strength", "StrengthUnit");

                    b.ToTable("Prescriptions");
                });

            modelBuilder.Entity("RXAI.Entities.TradeName", b =>
                {
                    b.Property<string>("SKUCode")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("DrugBankID")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("IngredientName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ManufactureCountry")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PharmaceuticalForm")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal?>("Price")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("QuantityStock")
                        .HasColumnType("int");

                    b.Property<string>("Strength")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.Property<string>("StrengthUnit")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("SKUCode");

                    b.HasIndex("DrugBankID", "Strength", "StrengthUnit");

                    b.ToTable("TradeNames");
                });

            modelBuilder.Entity("RXAI.Entities.ActiveIngredientBase", b =>
                {
                    b.HasOne("RXAI.Entities.Disease", "Disease")
                        .WithMany("ActiveIngredientBase")
                        .HasForeignKey("ICDCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Disease");
                });

            modelBuilder.Entity("RXAI.Entities.ActiveIngredientVariant", b =>
                {
                    b.HasOne("RXAI.Entities.ActiveIngredientBase", "BaseIngredient")
                        .WithMany("Variants")
                        .HasForeignKey("DrugBankID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BaseIngredient");
                });

            modelBuilder.Entity("RXAI.Entities.RXAI.Entities.Prescription", b =>
                {
                    b.HasOne("RXAI.Entities.Patient", "Patient")
                        .WithMany("Prescriptions")
                        .HasForeignKey("PhoneNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RXAI.Entities.ActiveIngredientVariant", "ActiveIngredientVariant")
                        .WithMany("Prescriptions")
                        .HasForeignKey("DrugBankID", "Strength", "StrengthUnit")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ActiveIngredientVariant");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("RXAI.Entities.TradeName", b =>
                {
                    b.HasOne("RXAI.Entities.ActiveIngredientVariant", "ActiveIngredientVariant")
                        .WithMany("Trades")
                        .HasForeignKey("DrugBankID", "Strength", "StrengthUnit")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ActiveIngredientVariant");
                });

            modelBuilder.Entity("RXAI.Entities.ActiveIngredientBase", b =>
                {
                    b.Navigation("Variants");
                });

            modelBuilder.Entity("RXAI.Entities.ActiveIngredientVariant", b =>
                {
                    b.Navigation("Prescriptions");

                    b.Navigation("Trades");
                });

            modelBuilder.Entity("RXAI.Entities.Disease", b =>
                {
                    b.Navigation("ActiveIngredientBase");
                });

            modelBuilder.Entity("RXAI.Entities.Patient", b =>
                {
                    b.Navigation("Prescriptions");
                });
#pragma warning restore 612, 618
        }
    }
}
