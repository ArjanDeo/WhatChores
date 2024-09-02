using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Characters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_Characters",
                columns: table => new
                {
                    CharacterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CharacterData = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Characters", x => x.CharacterId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_ClassData",
                columns: table => new
                {
                    ClassId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClassColor = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_ClassData", x => x.ClassId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_MythicPlusValues",
                columns: table => new
                {
                    KeyLevel = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_MythicPlusValues", x => x.KeyLevel);
                });

            migrationBuilder.CreateTable(
                name: "tbl_USRealms",
                columns: table => new
                {
                    RealmID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RealmName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_USRealms", x => x.RealmID);
                });

            migrationBuilder.CreateTable(
                name: "tbl_VaultRaidBosses",
                columns: table => new
                {
                    Boss = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_VaultRaidBosses", x => x.Boss);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_Characters");

            migrationBuilder.DropTable(
                name: "tbl_ClassData");

            migrationBuilder.DropTable(
                name: "tbl_MythicPlusValues");

            migrationBuilder.DropTable(
                name: "tbl_USRealms");

            migrationBuilder.DropTable(
                name: "tbl_VaultRaidBosses");
        }
    }
}
