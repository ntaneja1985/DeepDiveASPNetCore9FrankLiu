using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salary = table.Column<double>(type: "float", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "Description", "Email", "Name" },
                values: new object[,]
                {
                    { 1, "Sales Department", null, "Sales" },
                    { 2, "Engineering Department", null, "Engineering" },
                    { 3, "Quanlity Assurance", null, "QA" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "DepartmentId", "Name", "Position", "Salary" },
                values: new object[,]
                {
                    { 1, 1, "John Doe", "Engineer", 60000.0 },
                    { 2, 1, "Jane Smith", "Manager", 75000.0 },
                    { 3, 1, "Sam Brown", "Technician", 50000.0 },
                    { 4, 2, "Alice Johnson", "Analyst", 55000.0 },
                    { 5, 2, "Bob Lee", "Developer", 65000.0 },
                    { 6, 2, "Carol Wang", "Designer", 70000.0 },
                    { 7, 3, "David Kim", "Support", 48000.0 },
                    { 8, 3, "Eve Rogers", "Consultant", 72000.0 },
                    { 9, 3, "Franklin Zhang", "Architect", 80000.0 },
                    { 10, 1, "Grace Liu", "Coordinator", 53000.0 },
                    { 11, 2, "Henry Thompson", "Specialist", 62000.0 },
                    { 12, 3, "Isabelle Nguyen", "Technician", 57000.0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employees",
                column: "DepartmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
