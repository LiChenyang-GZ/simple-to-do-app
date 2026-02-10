using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryToTodo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Todos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId1",
                table: "Todos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TodoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Color", "Description", "Name", "TodoId" },
                values: new object[,]
                {
                    { 1, "#60A5FA", "Personal tasks", "Personal", 0 },
                    { 2, "#F97316", "Work related", "Work", 0 },
                    { 3, "#34D399", "Learning and study", "Study", 0 }
                });

            migrationBuilder.UpdateData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CategoryId", "CategoryId1" },
                values: new object[] { 0, null });

            migrationBuilder.UpdateData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CategoryId", "CategoryId1" },
                values: new object[] { 0, null });

            migrationBuilder.UpdateData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CategoryId", "CategoryId1" },
                values: new object[] { 0, null });

            migrationBuilder.CreateIndex(
                name: "IX_Todos_CategoryId1",
                table: "Todos",
                column: "CategoryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_Categories_CategoryId1",
                table: "Todos",
                column: "CategoryId1",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_Categories_CategoryId1",
                table: "Todos");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Todos_CategoryId1",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "CategoryId1",
                table: "Todos");
        }
    }
}
