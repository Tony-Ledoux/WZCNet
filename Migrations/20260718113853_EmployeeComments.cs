using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WZCNet.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmploymentHistories_Employees_EmployeeId",
                table: "EmploymentHistories");

            migrationBuilder.CreateTable(
                name: "EmployeeComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedByEmployeeId = table.Column<int>(type: "integer", nullable: false),
                    AuthorJobTitleSnapshot = table.Column<string>(type: "text", nullable: false),
                    CreatedForEmployeeId = table.Column<int>(type: "integer", nullable: false),
                    RecipientJobTitleSnapshot = table.Column<string>(type: "text", nullable: false),
                    CreatedDuringEmploymentId = table.Column<int>(type: "integer", nullable: false),
                    IsPrivate = table.Column<bool>(type: "boolean", nullable: false),
                    IsResolved = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeComments_Employees_CreatedByEmployeeId",
                        column: x => x.CreatedByEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeComments_Employees_CreatedForEmployeeId",
                        column: x => x.CreatedForEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeComments_CreatedByEmployeeId",
                table: "EmployeeComments",
                column: "CreatedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeComments_CreatedForEmployeeId",
                table: "EmployeeComments",
                column: "CreatedForEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmploymentHistories_Employees_EmployeeId",
                table: "EmploymentHistories",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmploymentHistories_Employees_EmployeeId",
                table: "EmploymentHistories");

            migrationBuilder.DropTable(
                name: "EmployeeComments");

            migrationBuilder.AddForeignKey(
                name: "FK_EmploymentHistories_Employees_EmployeeId",
                table: "EmploymentHistories",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
