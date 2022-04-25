using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NinjaHeaven.Services;

namespace NinjaHeaven.Migrations
{
    public partial class SQLServer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Price = table.Column<int>(type: "int", nullable: false),
                    ImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Wallet = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserEquipment",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    EquipmentId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEquipment", x => new { x.EquipmentId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserEquipment_Equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserEquipment_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Equipment",
                columns: new[] { "Id", "CreatedDate", "Description", "ImgUrl", "Name", "Price", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 4, 22, 11, 52, 47, 698, DateTimeKind.Utc).AddTicks(3380), "傷害+1", "https://dotown.maeda-design-room.net/wp-content/uploads/2022/03/other_shuriken.png", "手裡劍", 5, new DateTime(2022, 4, 22, 11, 52, 47, 698, DateTimeKind.Utc).AddTicks(4050) },
                    { 2, new DateTime(2022, 4, 22, 11, 52, 47, 698, DateTimeKind.Utc).AddTicks(4670), "傷害+3", "https://dotown.maeda-design-room.net/wp-content/uploads/2022/01/other_hammer_01.png", "鐵鎚", 10, new DateTime(2022, 4, 22, 11, 52, 47, 698, DateTimeKind.Utc).AddTicks(4670) },
                    { 3, new DateTime(2022, 4, 22, 11, 52, 47, 698, DateTimeKind.Utc).AddTicks(4670), "傷害+5", "https://dotown.maeda-design-room.net/wp-content/uploads/2022/01/other_goldbar_01.png", "狼牙棒", 20, new DateTime(2022, 4, 22, 11, 52, 47, 698, DateTimeKind.Utc).AddTicks(4670) },
                    { 4, new DateTime(2022, 4, 22, 11, 52, 47, 698, DateTimeKind.Utc).AddTicks(4670), "傷害+50，速度+10", "https://dotown.maeda-design-room.net/wp-content/uploads/2022/01/vehicle_tank_01.png", "戰車", 300, new DateTime(2022, 4, 22, 11, 52, 47, 698, DateTimeKind.Utc).AddTicks(4680) },
                    { 5, new DateTime(2022, 4, 22, 11, 52, 47, 698, DateTimeKind.Utc).AddTicks(4680), "速度+50", "https://dotown.maeda-design-room.net/wp-content/uploads/2022/01/other_ufo_01.png", "UFO", 250, new DateTime(2022, 4, 22, 11, 52, 47, 698, DateTimeKind.Utc).AddTicks(4680) },
                    { 6, new DateTime(2022, 4, 22, 11, 52, 47, 698, DateTimeKind.Utc).AddTicks(4680), "體力+5", "https://dotown.maeda-design-room.net/wp-content/uploads/2022/01/food_rice_03.png", "白飯", 10, new DateTime(2022, 4, 22, 11, 52, 47, 698, DateTimeKind.Utc).AddTicks(4680) },
                    { 7, new DateTime(2022, 4, 22, 11, 52, 47, 698, DateTimeKind.Utc).AddTicks(4680), "體力+10", "https://dotown.maeda-design-room.net/wp-content/uploads/2022/01/other_babybottle_01.png", "牛奶", 20, new DateTime(2022, 4, 22, 11, 52, 47, 698, DateTimeKind.Utc).AddTicks(4680) },
                    { 8, new DateTime(2022, 4, 22, 11, 52, 47, 698, DateTimeKind.Utc).AddTicks(4680), "透視+5，魅力+5", "https://dotown.maeda-design-room.net/wp-content/uploads/2022/01/other_glasses_01.png", "透視鏡", 50, new DateTime(2022, 4, 22, 11, 52, 47, 698, DateTimeKind.Utc).AddTicks(4690) },
                    { 9, new DateTime(2022, 4, 22, 11, 52, 47, 698, DateTimeKind.Utc).AddTicks(4690), "親密度+5", "https://dotown.maeda-design-room.net/wp-content/uploads/2022/01/other_lettter_01.png", "信紙", 1, new DateTime(2022, 4, 22, 11, 52, 47, 698, DateTimeKind.Utc).AddTicks(4690) },
                    { 10, new DateTime(2022, 4, 22, 11, 52, 47, 698, DateTimeKind.Utc).AddTicks(4690), "全區喊話", "https://dotown.maeda-design-room.net/wp-content/uploads/2022/01/other_trumpet_01.png", "大聲公", 10, new DateTime(2022, 4, 22, 11, 52, 47, 698, DateTimeKind.Utc).AddTicks(4690) }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "CreatedDate", "Email", "Gender", "Name", "Password", "Role", "UpdatedDate", "Wallet" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 4, 22, 11, 52, 47, 691, DateTimeKind.Utc).AddTicks(4500), "kelly@gmail.com", "Female", "Kelly", EncryptionService.Encrypt("123456"), "Admin", new DateTime(2022, 4, 22, 11, 52, 47, 691, DateTimeKind.Utc).AddTicks(5320), 1000 },
                    { 2, new DateTime(2022, 4, 22, 11, 52, 47, 696, DateTimeKind.Utc).AddTicks(2950), "kevin@gmail.com", "Male", "Kevin", EncryptionService.Encrypt("123456"), "Player", new DateTime(2022, 4, 22, 11, 52, 47, 696, DateTimeKind.Utc).AddTicks(2950), 1000 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserEquipment_UserId",
                table: "UserEquipment",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserEquipment");

            migrationBuilder.DropTable(
                name: "Equipment");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
