using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NinjaHeaven.Models;

    public class NinjaHeavenDbContext : DbContext
    {
        public NinjaHeavenDbContext(DbContextOptions<NinjaHeavenDbContext> options)
            : base(options)
        {
        }

        public DbSet<NinjaHeaven.Models.User> User { get; set; }
        public DbSet<NinjaHeaven.Models.Equipment> Equipment { get; set; }
        public DbSet<NinjaHeaven.Models.UserEquipment> UserEquipment { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity("NinjaHeaven.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .HasColumnType("TEXT");

                    b.Property<string>("Role")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("Wallet")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("NinjaHeaven.Models.Equipment", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("INTEGER");

                b.Property<DateTime>("CreatedDate")
                    .HasColumnType("TEXT");

                b.Property<string>("Description")
                    .HasColumnType("TEXT");

                b.Property<string>("ImgUrl")
                   .HasColumnType("TEXT");

                b.Property<string>("Name")
                    .HasColumnType("TEXT");

                b.Property<int>("Price")
                   .HasColumnType("INTEGER");

                b.Property<DateTime>("UpdatedDate")
                    .HasColumnType("TEXT");

                b.HasKey("Id");

                b.ToTable("Equipment");
            });

            modelBuilder.Entity("NinjaHeaven.Models.UserEquipment", b =>
            {
                b.Property<int>("Amount")
                        .HasColumnType("INTEGER");

                b.Property<DateTime>("CreatedDate")
                    .HasColumnType("TEXT");

                b.Property<int>("EquipmentId")
                   .HasColumnType("INTEGER");

                b.Property<int>("UserId")
                    .HasColumnType("INTEGER");

                b.Property<DateTime>("UpdatedDate")
                    .HasColumnType("TEXT");

                b.HasKey("EquipmentId", "UserId");

                b.ToTable("UserEquipment");
            });
    }
}
