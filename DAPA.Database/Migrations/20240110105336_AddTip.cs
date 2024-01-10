using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAPA.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddTip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Tip",
                table: "Orders",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Name_Surname",
                table: "Clients",
                columns: new[] { "Name", "Surname" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Clients_Name_Surname",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Tip",
                table: "Orders");
        }
    }
}
