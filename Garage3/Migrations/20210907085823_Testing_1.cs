using Microsoft.EntityFrameworkCore.Migrations;

namespace Garage3.Migrations
{
    public partial class Testing_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicle_VehicleType_VehicleTypeId",
                table: "Vehicle");

            migrationBuilder.DropTable(
                name: "Parking");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Vehicle");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Spot");

            migrationBuilder.AlterColumn<int>(
                name: "VehicleTypeId",
                table: "Vehicle",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Capacity",
                table: "Spot",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Spot",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SpotVehicle",
                columns: table => new
                {
                    SpotsId = table.Column<int>(type: "int", nullable: false),
                    VehiclesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpotVehicle", x => new { x.SpotsId, x.VehiclesId });
                    table.ForeignKey(
                        name: "FK_SpotVehicle_Spot_SpotsId",
                        column: x => x.SpotsId,
                        principalTable: "Spot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpotVehicle_Vehicle_VehiclesId",
                        column: x => x.VehiclesId,
                        principalTable: "Vehicle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpotVehicle_VehiclesId",
                table: "SpotVehicle",
                column: "VehiclesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicle_VehicleType_VehicleTypeId",
                table: "Vehicle",
                column: "VehicleTypeId",
                principalTable: "VehicleType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicle_VehicleType_VehicleTypeId",
                table: "Vehicle");

            migrationBuilder.DropTable(
                name: "SpotVehicle");

            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Spot");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Spot");

            migrationBuilder.AlterColumn<int>(
                name: "VehicleTypeId",
                table: "Vehicle",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "Vehicle",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Spot",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Parking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpotId = table.Column<int>(type: "int", nullable: false),
                    VehicleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parking_Spot_SpotId",
                        column: x => x.SpotId,
                        principalTable: "Spot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Parking_Vehicle_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Parking_SpotId",
                table: "Parking",
                column: "SpotId");

            migrationBuilder.CreateIndex(
                name: "IX_Parking_VehicleId",
                table: "Parking",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicle_VehicleType_VehicleTypeId",
                table: "Vehicle",
                column: "VehicleTypeId",
                principalTable: "VehicleType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
