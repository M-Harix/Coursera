using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coursera.Migrations
{
    /// <inheritdoc />
    public partial class initialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "instructors",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    first_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    time_created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teachers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "students",
                columns: table => new
                {
                    pin = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    time_created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_students", x => x.pin);
                });

            migrationBuilder.CreateTable(
                name: "courses",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    instructor_id = table.Column<int>(type: "int", nullable: false),
                    total_time = table.Column<byte>(type: "tinyint", nullable: false),
                    credit = table.Column<byte>(type: "tinyint", nullable: false),
                    time_created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_courses", x => x.id);
                    table.ForeignKey(
                        name: "FK_courses_instructors",
                        column: x => x.instructor_id,
                        principalTable: "instructors",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "students_courses_xref",
                columns: table => new
                {
                    student_pin = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    course_id = table.Column<int>(type: "int", nullable: false),
                    completion_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_students_courses_xref", x => new { x.student_pin, x.course_id });
                    table.ForeignKey(
                        name: "FK_students_courses_xref_courses",
                        column: x => x.course_id,
                        principalTable: "courses",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_students_courses_xref_students",
                        column: x => x.student_pin,
                        principalTable: "students",
                        principalColumn: "pin");
                });

            migrationBuilder.CreateIndex(
                name: "IX_courses_instructor_id",
                table: "courses",
                column: "instructor_id");

            migrationBuilder.CreateIndex(
                name: "IX_students_courses_xref_course_id",
                table: "students_courses_xref",
                column: "course_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "students_courses_xref");

            migrationBuilder.DropTable(
                name: "courses");

            migrationBuilder.DropTable(
                name: "students");

            migrationBuilder.DropTable(
                name: "instructors");
        }
    }
}
