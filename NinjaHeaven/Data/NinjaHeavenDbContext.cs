﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NinjaHeaven.Models;
using NinjaHeaven.Services;

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
            modelBuilder.Entity("NinjaHeaven.Models.User")
                        .HasData(
                            new User
                            {
                                Id = 1,
                                Name = "Kelly",
                                Email = "kelly@gmail.com",
                                Password = EncryptionService.Encrypt("123456"),
                                Gender = "Female",
                                Role = "Admin",
                                Wallet = 1000,
                                CreatedDate = DateTime.UtcNow.AddHours(8),
                                UpdatedDate = DateTime.UtcNow.AddHours(8)
                            },
                            new User
                            {
                                Id = 2,
                                Name = "Kevin",
                                Email = "kevin@gmail.com",
                                Password = EncryptionService.Encrypt("123456"),
                                Gender = "Male",
                                Role = "Player",
                                Wallet = 1000,
                                CreatedDate = DateTime.UtcNow.AddHours(8),
                                UpdatedDate = DateTime.UtcNow.AddHours(8)
                            }
                        );

            modelBuilder.Entity("NinjaHeaven.Models.Equipment")
                        .HasData(
                            new Equipment
                            {
                                Id = 1,
                                Name = "手裡劍",
                                Description = "傷害+1",
                                Price = 5,
                                ImgUrl = "https://dotown.maeda-design-room.net/wp-content/uploads/2022/03/other_shuriken.png",
                                CreatedDate = DateTime.UtcNow.AddHours(8),
                                UpdatedDate = DateTime.UtcNow.AddHours(8)
                            },
                            new Equipment
                            {
                                Id = 2,
                                Name = "鐵鎚",
                                Description = "傷害+3",
                                Price = 10,
                                ImgUrl = "https://dotown.maeda-design-room.net/wp-content/uploads/2022/01/other_hammer_01.png",
                                CreatedDate = DateTime.UtcNow.AddHours(8),
                                UpdatedDate = DateTime.UtcNow.AddHours(8)
                            },
                            new Equipment
                            {
                                Id = 3,
                                Name = "狼牙棒",
                                Description = "傷害+5",
                                Price = 20,
                                ImgUrl = "https://dotown.maeda-design-room.net/wp-content/uploads/2022/01/other_goldbar_01.png",
                                CreatedDate = DateTime.UtcNow.AddHours(8),
                                UpdatedDate = DateTime.UtcNow.AddHours(8)
                            },
                            new Equipment
                            {
                                Id = 4,
                                Name = "戰車",
                                Description = "傷害+50，速度+10",
                                Price = 300,
                                ImgUrl = "https://dotown.maeda-design-room.net/wp-content/uploads/2022/01/vehicle_tank_01.png",
                                CreatedDate = DateTime.UtcNow.AddHours(8),
                                UpdatedDate = DateTime.UtcNow.AddHours(8)
                            },
                            new Equipment
                            {
                                Id = 5,
                                Name = "UFO",
                                Description = "速度+50",
                                Price = 250,
                                ImgUrl = "https://dotown.maeda-design-room.net/wp-content/uploads/2022/01/other_ufo_01.png",
                                CreatedDate = DateTime.UtcNow.AddHours(8),
                                UpdatedDate = DateTime.UtcNow.AddHours(8)
                            },
                            new Equipment
                            {
                                Id = 6,
                                Name = "白飯",
                                Description = "體力+5",
                                Price = 10,
                                ImgUrl = "https://dotown.maeda-design-room.net/wp-content/uploads/2022/01/food_rice_03.png",
                                CreatedDate = DateTime.UtcNow.AddHours(8),
                                UpdatedDate = DateTime.UtcNow.AddHours(8)
                            },
                            new Equipment
                            {
                                Id = 7,
                                Name = "牛奶",
                                Description = "體力+10",
                                Price = 20,
                                ImgUrl = "https://dotown.maeda-design-room.net/wp-content/uploads/2022/01/other_babybottle_01.png",
                                CreatedDate = DateTime.UtcNow.AddHours(8),
                                UpdatedDate = DateTime.UtcNow.AddHours(8)
                            },
                            new Equipment
                            {
                                Id = 8,
                                Name = "透視鏡",
                                Description = "透視+5，魅力+5",
                                Price = 50,
                                ImgUrl = "https://dotown.maeda-design-room.net/wp-content/uploads/2022/01/other_glasses_01.png",
                                CreatedDate = DateTime.UtcNow.AddHours(8),
                                UpdatedDate = DateTime.UtcNow.AddHours(8)
                            },
                            new Equipment
                            {
                                Id = 9,
                                Name = "信紙",
                                Description = "親密度+5",
                                Price = 1,
                                ImgUrl = "https://dotown.maeda-design-room.net/wp-content/uploads/2022/01/other_lettter_01.png",
                                CreatedDate = DateTime.UtcNow.AddHours(8),
                                UpdatedDate = DateTime.UtcNow.AddHours(8)
                            },
                            new Equipment
                            {
                                Id = 10,
                                Name = "大聲公",
                                Description = "全區喊話",
                                Price = 10,
                                ImgUrl = "https://dotown.maeda-design-room.net/wp-content/uploads/2022/01/other_trumpet_01.png",
                                CreatedDate = DateTime.UtcNow.AddHours(8),
                                UpdatedDate = DateTime.UtcNow.AddHours(8)
                            }
                        );

            modelBuilder.Entity("NinjaHeaven.Models.UserEquipment", b =>
            {
                b.HasKey("EquipmentId", "UserId");
            });
    }
}
