using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAPA.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscountReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Loyalties",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Start_date",
                table: "Loyalties",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "Discount",
                table: "Loyalties",
                newName: "DiscountId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Discounts",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Start_date",
                table: "Discounts",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "End_date",
                table: "Discounts",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "Applicable_Category",
                table: "Discounts",
                newName: "ApplicableCategory");

            migrationBuilder.CreateIndex(
                name: "IX_Loyalties_DiscountId",
                table: "Loyalties",
                column: "DiscountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Loyalties_Discounts_DiscountId",
                table: "Loyalties",
                column: "DiscountId",
                principalTable: "Discounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loyalties_Discounts_DiscountId",
                table: "Loyalties");

            migrationBuilder.DropIndex(
                name: "IX_Loyalties_DiscountId",
                table: "Loyalties");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Loyalties",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Loyalties",
                newName: "Start_date");

            migrationBuilder.RenameColumn(
                name: "DiscountId",
                table: "Loyalties",
                newName: "Discount");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Discounts",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Discounts",
                newName: "Start_date");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Discounts",
                newName: "End_date");

            migrationBuilder.RenameColumn(
                name: "ApplicableCategory",
                table: "Discounts",
                newName: "Applicable_Category");
        }
    }
}
