using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TransportationArea.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderRoute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SendAddress",
                table: "Orders",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ReceivedAddress",
                table: "Orders",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "orderRouteId",
                table: "Orders",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderRouteId",
                table: "Areas",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrderRoutes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderRoutes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_orderRouteId",
                table: "Orders",
                column: "orderRouteId");

            migrationBuilder.CreateIndex(
                name: "IX_Areas_OrderRouteId",
                table: "Areas",
                column: "OrderRouteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Areas_OrderRoutes_OrderRouteId",
                table: "Areas",
                column: "OrderRouteId",
                principalTable: "OrderRoutes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_OrderRoutes_orderRouteId",
                table: "Orders",
                column: "orderRouteId",
                principalTable: "OrderRoutes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Areas_OrderRoutes_OrderRouteId",
                table: "Areas");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_OrderRoutes_orderRouteId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "OrderRoutes");

            migrationBuilder.DropIndex(
                name: "IX_Orders_orderRouteId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Areas_OrderRouteId",
                table: "Areas");

            migrationBuilder.DropColumn(
                name: "orderRouteId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderRouteId",
                table: "Areas");

            migrationBuilder.AlterColumn<string>(
                name: "SendAddress",
                table: "Orders",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "ReceivedAddress",
                table: "Orders",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }
    }
}
