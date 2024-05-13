using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseManagerAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeachingRequest_AspNetUsers_TeacherId",
                table: "TeachingRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_TeachingRequest_Courses_CourseID",
                table: "TeachingRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeachingRequest",
                table: "TeachingRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Request",
                table: "Request");

            migrationBuilder.RenameTable(
                name: "TeachingRequest",
                newName: "TeachingRequests");

            migrationBuilder.RenameTable(
                name: "Request",
                newName: "Message");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "TeachingRequests",
                newName: "TeacherName");

            migrationBuilder.RenameIndex(
                name: "IX_TeachingRequest_TeacherId",
                table: "TeachingRequests",
                newName: "IX_TeachingRequests_TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_TeachingRequest_CourseID",
                table: "TeachingRequests",
                newName: "IX_TeachingRequests_CourseID");

            migrationBuilder.AlterColumn<string>(
                name: "TeacherId",
                table: "TeachingRequests",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "RequestStatus",
                table: "TeachingRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeachingRequests",
                table: "TeachingRequests",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Message",
                table: "Message",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Enrollment",
                columns: table => new
                {
                    EnrollmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    EnrollmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollment", x => x.EnrollmentID);
                    table.ForeignKey(
                        name: "FK_Enrollment_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enrollment_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_CourseId",
                table: "Enrollment",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_UserId",
                table: "Enrollment",
                column: "UserId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeachingRequests_AspNetUsers_TeacherId",
                table: "TeachingRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_TeachingRequests_Courses_CourseID",
                table: "TeachingRequests");

            migrationBuilder.DropTable(
                name: "Enrollment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeachingRequests",
                table: "TeachingRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Message",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "RequestStatus",
                table: "TeachingRequests");

            migrationBuilder.RenameTable(
                name: "TeachingRequests",
                newName: "TeachingRequest");

            migrationBuilder.RenameTable(
                name: "Message",
                newName: "Request");

            migrationBuilder.RenameColumn(
                name: "TeacherName",
                table: "TeachingRequest",
                newName: "Status");

            migrationBuilder.RenameIndex(
                name: "IX_TeachingRequests_TeacherId",
                table: "TeachingRequest",
                newName: "IX_TeachingRequest_TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_TeachingRequests_CourseID",
                table: "TeachingRequest",
                newName: "IX_TeachingRequest_CourseID");

            migrationBuilder.AlterColumn<string>(
                name: "TeacherId",
                table: "TeachingRequest",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeachingRequest",
                table: "TeachingRequest",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Request",
                table: "Request",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TeachingRequest_AspNetUsers_TeacherId",
                table: "TeachingRequest",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeachingRequest_Courses_CourseID",
                table: "TeachingRequest",
                column: "CourseID",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
