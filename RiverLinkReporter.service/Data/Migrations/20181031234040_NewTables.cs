using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RiverLinkReporter.service.Data.Migrations
{
    public partial class NewTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.CreateTable(
                name: "Transponders",
                columns: table => new
                {
                    Transponder_Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<Guid>(nullable: false),
                    TransponderNumber = table.Column<string>(nullable: true),
                    TransponderType = table.Column<string>(nullable: false),
                    PlateNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transponders", x => x.Transponder_Id);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionNumber = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<Guid>(nullable: false),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    PostedDate = table.Column<DateTime>(nullable: false),
                    TransactionStatus = table.Column<string>(nullable: true),
                    Plaza = table.Column<string>(nullable: true),
                    Journal_Id = table.Column<int>(nullable: false),
                    Transponder = table.Column<string>(nullable: true),
                    TransponderNumber = table.Column<int>(nullable: false),
                    TransactionType = table.Column<string>(nullable: true),
                    Amount = table.Column<double>(nullable: false),
                    TransactionDescription = table.Column<string>(maxLength: 50, nullable: true),
                    Lane = table.Column<int>(nullable: false),
                    PlateNumber = table.Column<string>(maxLength: 20, nullable: true),
                    VehicleClass_Id = table.Column<short>(nullable: false),
                    Transponder_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionNumber);
                    table.ForeignKey(
                        name: "FK_Transactions_Transponders_Transponder_Id",
                        column: x => x.Transponder_Id,
                        principalTable: "Transponders",
                        principalColumn: "Transponder_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Vehicle_Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<Guid>(nullable: false),
                    PlateNumber = table.Column<string>(maxLength: 20, nullable: true),
                    Make = table.Column<string>(maxLength: 32, nullable: true),
                    Model = table.Column<string>(maxLength: 32, nullable: true),
                    Year = table.Column<int>(nullable: false),
                    VehicleState = table.Column<string>(maxLength: 2, nullable: true),
                    VehicleStatus = table.Column<string>(nullable: true),
                    VehicleClass = table.Column<string>(nullable: true),
                    Transponder = table.Column<string>(nullable: true),
                    TransponderType = table.Column<string>(nullable: true),
                    Transponder_Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Vehicle_Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_Transponders_Transponder_Id",
                        column: x => x.Transponder_Id,
                        principalTable: "Transponders",
                        principalColumn: "Transponder_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Transponder_Id",
                table: "Transactions",
                column: "Transponder_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_Transponder_Id",
                table: "Vehicles",
                column: "Transponder_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Transponders");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
