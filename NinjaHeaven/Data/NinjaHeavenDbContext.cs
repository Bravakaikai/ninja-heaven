using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NinjaHeaven.Models;
using NinjaHeaven.Services;

public class NinjaHeavenDbContext : DbContext, IDataProtectionKeyContext
    {
        public NinjaHeavenDbContext(DbContextOptions<NinjaHeavenDbContext> options)
            : base(options)
        {
        }

        public DbSet<NinjaHeaven.Models.User> User { get; set; }
        public DbSet<NinjaHeaven.Models.Equipment> Equipment { get; set; }
        public DbSet<NinjaHeaven.Models.UserEquipment> UserEquipment { get; set; }

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity("NinjaHeaven.Models.UserEquipment", b =>
            {
                b.HasKey("EquipmentId", "UserId");
            });
    }
}
