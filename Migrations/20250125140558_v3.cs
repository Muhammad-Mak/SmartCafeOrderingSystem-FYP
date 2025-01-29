using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartCafeOrderingSystem_Api_V2.Migrations
{
    /// <inheritdoc />
    public partial class v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_MenuItems_ItemID",
                table: "OrderItems");

            migrationBuilder.DropTable(
                name: "InventoryLogs");

            migrationBuilder.DropTable(
                name: "Inventories");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_ItemID",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "MenuItems");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Orders",
                newName: "OrderType");

            migrationBuilder.RenameColumn(
                name: "TotalRevenue",
                table: "Analytics",
                newName: "TotalSales");

            migrationBuilder.AlterColumn<int>(
                name: "Role",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Score",
                table: "Recommendations",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "MenuItemItemID",
                table: "Recommendations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TableNumber",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MenuItemItemID",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "MenuItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "ImageURL",
                table: "MenuItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MenuItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<int>(
                name: "CategoryID",
                table: "MenuItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "MenuItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsPopular",
                table: "MenuItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "MenuCategories",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuCategories", x => x.CategoryID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recommendations_MenuItemItemID",
                table: "Recommendations",
                column: "MenuItemItemID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_MenuItemItemID",
                table: "OrderItems",
                column: "MenuItemItemID");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_CategoryID",
                table: "MenuItems",
                column: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItems_MenuCategories_CategoryID",
                table: "MenuItems",
                column: "CategoryID",
                principalTable: "MenuCategories",
                principalColumn: "CategoryID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_MenuItems_MenuItemItemID",
                table: "OrderItems",
                column: "MenuItemItemID",
                principalTable: "MenuItems",
                principalColumn: "ItemID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recommendations_MenuItems_MenuItemItemID",
                table: "Recommendations",
                column: "MenuItemItemID",
                principalTable: "MenuItems",
                principalColumn: "ItemID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItems_MenuCategories_CategoryID",
                table: "MenuItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_MenuItems_MenuItemItemID",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Recommendations_MenuItems_MenuItemItemID",
                table: "Recommendations");

            migrationBuilder.DropTable(
                name: "MenuCategories");

            migrationBuilder.DropIndex(
                name: "IX_Recommendations_MenuItemItemID",
                table: "Recommendations");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_MenuItemItemID",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_MenuItems_CategoryID",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "MenuItemItemID",
                table: "Recommendations");

            migrationBuilder.DropColumn(
                name: "TableNumber",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "MenuItemItemID",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "IsPopular",
                table: "MenuItems");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Users",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "OrderType",
                table: "Orders",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "TotalSales",
                table: "Analytics",
                newName: "TotalRevenue");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Score",
                table: "Recommendations",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "MenuItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ImageURL",
                table: "MenuItems",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MenuItems",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "MenuItems",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    InventoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemID = table.Column<int>(type: "int", nullable: false),
                    QuantityInStock = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => x.InventoryID);
                    table.ForeignKey(
                        name: "FK_Inventories_MenuItems_ItemID",
                        column: x => x.ItemID,
                        principalTable: "MenuItems",
                        principalColumn: "ItemID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventoryLogs",
                columns: table => new
                {
                    LogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InventoryID = table.Column<int>(type: "int", nullable: false),
                    ChangeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QuantityChanged = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryLogs", x => x.LogID);
                    table.ForeignKey(
                        name: "FK_InventoryLogs_Inventories_InventoryID",
                        column: x => x.InventoryID,
                        principalTable: "Inventories",
                        principalColumn: "InventoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ItemID",
                table: "OrderItems",
                column: "ItemID");

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_ItemID",
                table: "Inventories",
                column: "ItemID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryLogs_InventoryID",
                table: "InventoryLogs",
                column: "InventoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_MenuItems_ItemID",
                table: "OrderItems",
                column: "ItemID",
                principalTable: "MenuItems",
                principalColumn: "ItemID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
