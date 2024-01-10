using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAPA.Database.Migrations
{
    /// <inheritdoc />
    public partial class StaffIdNullableServiceCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceCarts_Staff_StaffId",
                table: "ServiceCarts");

            migrationBuilder.AlterColumn<int>(
                name: "StaffId",
                table: "ServiceCarts",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceCarts_Staff_StaffId",
                table: "ServiceCarts",
                column: "StaffId",
                principalTable: "Staff",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceCarts_Staff_StaffId",
                table: "ServiceCarts");

            migrationBuilder.AlterColumn<int>(
                name: "StaffId",
                table: "ServiceCarts",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceCarts_Staff_StaffId",
                table: "ServiceCarts",
                column: "StaffId",
                principalTable: "Staff",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
