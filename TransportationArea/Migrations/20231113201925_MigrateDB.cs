using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TransportationArea.Migrations
{
    /// <inheritdoc />
    public partial class MigrateDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Number = table.Column<string>(type: "text", nullable: false),
                    Сapacity = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GridsOfArea",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Area1Id = table.Column<int>(type: "integer", nullable: false),
                    Area2Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GridsOfArea", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GridsOfArea_Areas_Area1Id",
                        column: x => x.Area1Id,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GridsOfArea_Areas_Area2Id",
                        column: x => x.Area2Id,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoadingPoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    AreaId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadingPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoadingPoints_Areas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SendSityId = table.Column<int>(type: "integer", nullable: false),
                    SendAddress = table.Column<string>(type: "text", nullable: false),
                    ReceivedSityId = table.Column<int>(type: "integer", nullable: false),
                    ReceivedAddress = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Mass = table.Column<double>(type: "double precision", nullable: false),
                    LoadingDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_LoadingPoints_ReceivedSityId",
                        column: x => x.ReceivedSityId,
                        principalTable: "LoadingPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_LoadingPoints_SendSityId",
                        column: x => x.SendSityId,
                        principalTable: "LoadingPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarRoutes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CarId = table.Column<int>(type: "integer", nullable: false),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    Start = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    End = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Mass = table.Column<double>(type: "double precision", nullable: false),
                    SendSityId = table.Column<int>(type: "integer", nullable: false),
                    ReceivedSityId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarRoutes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarRoutes_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarRoutes_LoadingPoints_ReceivedSityId",
                        column: x => x.ReceivedSityId,
                        principalTable: "LoadingPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarRoutes_LoadingPoints_SendSityId",
                        column: x => x.SendSityId,
                        principalTable: "LoadingPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarRoutes_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarRoutes_CarId",
                table: "CarRoutes",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_CarRoutes_OrderId",
                table: "CarRoutes",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_CarRoutes_ReceivedSityId",
                table: "CarRoutes",
                column: "ReceivedSityId");

            migrationBuilder.CreateIndex(
                name: "IX_CarRoutes_SendSityId",
                table: "CarRoutes",
                column: "SendSityId");

            migrationBuilder.CreateIndex(
                name: "IX_GridsOfArea_Area1Id",
                table: "GridsOfArea",
                column: "Area1Id");

            migrationBuilder.CreateIndex(
                name: "IX_GridsOfArea_Area2Id",
                table: "GridsOfArea",
                column: "Area2Id");

            migrationBuilder.CreateIndex(
                name: "IX_LoadingPoints_AreaId",
                table: "LoadingPoints",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ReceivedSityId",
                table: "Orders",
                column: "ReceivedSityId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SendSityId",
                table: "Orders",
                column: "SendSityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarRoutes");

            migrationBuilder.DropTable(
                name: "GridsOfArea");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "LoadingPoints");

            migrationBuilder.DropTable(
                name: "Areas");
        }
    }
}
