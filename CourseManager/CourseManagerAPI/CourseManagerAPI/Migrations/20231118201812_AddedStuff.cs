using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseManagerAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedStuff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeachingRequests_AspNetUsers_TeacherId",
                table: "TeachingRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_TeachingRequests_Courses_CourseID",
                table: "TeachingRequests");

            migrationBuilder.RenameColumn(
                name: "CourseID",
                table: "TeachingRequests",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_TeachingRequests_CourseID",
                table: "TeachingRequests",
                newName: "IX_TeachingRequests_CourseId");

            migrationBuilder.AlterColumn<string>(
                name: "TeacherId",
                table: "TeachingRequests",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TeachingRequests_AspNetUsers_TeacherId",
                table: "TeachingRequests",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeachingRequests_Courses_CourseId",
                table: "TeachingRequests",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeachingRequests_AspNetUsers_TeacherId",
                table: "TeachingRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_TeachingRequests_Courses_CourseId",
                table: "TeachingRequests");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "TeachingRequests",
                newName: "CourseID");

            migrationBuilder.RenameIndex(
                name: "IX_TeachingRequests_CourseId",
                table: "TeachingRequests",
                newName: "IX_TeachingRequests_CourseID");

            migrationBuilder.AlterColumn<string>(
                name: "TeacherId",
                table: "TeachingRequests",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_TeachingRequests_AspNetUsers_TeacherId",
                table: "TeachingRequests",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TeachingRequests_Courses_CourseID",
                table: "TeachingRequests",
                column: "CourseID",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
