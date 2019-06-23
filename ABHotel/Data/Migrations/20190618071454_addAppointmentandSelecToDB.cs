using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ABHotel.Data.Migrations
{
    public partial class addAppointmentandSelecToDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AppointmentDate = table.Column<DateTime>(nullable: false),
                    CustomerName = table.Column<string>(nullable: true),
                    CustomerPhone = table.Column<string>(nullable: true),
                    CustomerEmail = table.Column<string>(nullable: true),
                    isConfirmed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RoomSelectedForAppointments",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AppointmentId = table.Column<int>(nullable: false),
                    RoomId = table.Column<int>(nullable: false),
               
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomSelectedForAppointments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RoomSelectedForAppointments_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomSelectedForAppointments_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomSelectedForAppointments_AppointmentId",
                table: "RoomSelectedForAppointments",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomSelectedForAppointments_RoomId",
                table: "RoomSelectedForAppointments",
                column: "RoomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomSelectedForAppointments");

            migrationBuilder.DropTable(
                name: "Appointments");
        }
    }
}
