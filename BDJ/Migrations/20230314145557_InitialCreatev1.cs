using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BDJ.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreatev1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TicketId",
                table: "Bookings",
                column: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Ticket_TicketId",
                table: "Bookings",
                column: "TicketId",
                principalTable: "Ticket",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Ticket_TicketId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_TicketId",
                table: "Bookings");
        }
    }
}
