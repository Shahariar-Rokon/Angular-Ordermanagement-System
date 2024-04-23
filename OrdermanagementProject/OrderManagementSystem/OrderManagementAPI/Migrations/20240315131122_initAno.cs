using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class initAno : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyMenus_CustomerHeaders_CustomerHeaderId",
                table: "DailyMenus");

            migrationBuilder.DropTable(
                name: "DailyMenuCustomerRecords");

            migrationBuilder.DropTable(
                name: "CustomerHeaders");

            migrationBuilder.DropIndex(
                name: "IX_DailyMenus_CustomerHeaderId",
                table: "DailyMenus");

            migrationBuilder.DropColumn(
                name: "CustomerHeaderId",
                table: "DailyMenus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerHeaderId",
                table: "DailyMenus",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CustomerHeaders",
                columns: table => new
                {
                    CustomerHeaderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCheckedOut = table.Column<bool>(type: "bit", nullable: false),
                    TableNo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerHeaders", x => x.CustomerHeaderId);
                });

            migrationBuilder.CreateTable(
                name: "DailyMenuCustomerRecords",
                columns: table => new
                {
                    DailyMenuCustomerRecordId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerHeaderId = table.Column<int>(type: "int", nullable: false),
                    DailyMenuId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyMenuCustomerRecords", x => x.DailyMenuCustomerRecordId);
                    table.ForeignKey(
                        name: "FK_DailyMenuCustomerRecords_CustomerHeaders_CustomerHeaderId",
                        column: x => x.CustomerHeaderId,
                        principalTable: "CustomerHeaders",
                        principalColumn: "CustomerHeaderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DailyMenuCustomerRecords_DailyMenus_DailyMenuId",
                        column: x => x.DailyMenuId,
                        principalTable: "DailyMenus",
                        principalColumn: "DailyMenuId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyMenus_CustomerHeaderId",
                table: "DailyMenus",
                column: "CustomerHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyMenuCustomerRecords_CustomerHeaderId",
                table: "DailyMenuCustomerRecords",
                column: "CustomerHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyMenuCustomerRecords_DailyMenuId",
                table: "DailyMenuCustomerRecords",
                column: "DailyMenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyMenus_CustomerHeaders_CustomerHeaderId",
                table: "DailyMenus",
                column: "CustomerHeaderId",
                principalTable: "CustomerHeaders",
                principalColumn: "CustomerHeaderId");
        }
    }
}
