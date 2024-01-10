using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAPA.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddStaffToServiceCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PerformerId",
                table: "Services");

            migrationBuilder.AddColumn<int>(
                name: "StaffId",
                table: "ServiceCarts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCarts_StaffId",
                table: "ServiceCarts",
                column: "StaffId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceCarts_Staff_StaffId",
                table: "ServiceCarts",
                column: "StaffId",
                principalTable: "Staff",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceCarts_Staff_StaffId",
                table: "ServiceCarts");

            migrationBuilder.DropIndex(
                name: "IX_ServiceCarts_StaffId",
                table: "ServiceCarts");

            migrationBuilder.DropColumn(
                name: "StaffId",
                table: "ServiceCarts");

            migrationBuilder.AddColumn<int>(
                name: "PerformerId",
                table: "Services",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
