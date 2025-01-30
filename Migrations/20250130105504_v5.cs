using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartCafeOrderingSystem_Api_V2.Migrations
{
    /// <inheritdoc />
    public partial class v5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_MenuItems_MenuItemItemID",
                table: "OrderItems");

            migrationBuilder.AddColumn<string>(
                name: "GatewayResponse",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionID",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MenuItemItemID",
                table: "OrderItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_MenuItems_MenuItemItemID",
                table: "OrderItems",
                column: "MenuItemItemID",
                principalTable: "MenuItems",
                principalColumn: "ItemID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_MenuItems_MenuItemItemID",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "GatewayResponse",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "TransactionID",
                table: "Payments");

            migrationBuilder.AlterColumn<int>(
                name: "MenuItemItemID",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_MenuItems_MenuItemItemID",
                table: "OrderItems",
                column: "MenuItemItemID",
                principalTable: "MenuItems",
                principalColumn: "ItemID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
