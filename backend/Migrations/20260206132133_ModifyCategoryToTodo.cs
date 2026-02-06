using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class ModifyCategoryToTodo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_Categories_CategoryId1",
                table: "Todos");

            migrationBuilder.DropIndex(
                name: "IX_Todos_CategoryId1",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "CategoryId1",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "TodoId",
                table: "Categories");

            migrationBuilder.UpdateData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: 1,
                column: "CategoryId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: 2,
                column: "CategoryId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Todos",
                keyColumn: "Id",
                keyValue: 3,
                column: "CategoryId",
                value: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Todos_CategoryId",
                table: "Todos",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_Categories_CategoryId",
                table: "Todos",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_Categories_CategoryId",
                table: "Todos");

            migrationBuilder.DropIndex(
                name: "IX_Todos_CategoryId",
                table: "Todos");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId1",
                table: "Todos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TodoId",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "TodoId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "TodoId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "TodoId",
                value: 0);

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
    }
}
