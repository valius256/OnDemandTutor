using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnDemandTutor.Models.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConsultationRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsultationRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Media",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubjectType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateBy = table.Column<int>(type: "int", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AvatarImageId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Dob = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Role = table.Column<int>(type: "int", nullable: true),
                    RecordStatus = table.Column<bool>(type: "bit", nullable: true),
                    Balance = table.Column<decimal>(type: "money", nullable: true),
                    Rating = table.Column<double>(type: "float", nullable: true),
                    DegreeImageId = table.Column<int>(type: "int", nullable: true),
                    IdCardImageID = table.Column<int>(type: "int", nullable: true),
                    ScheduleDesciption = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Media_DegreeImageId",
                        column: x => x.DegreeImageId,
                        principalTable: "Media",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_Media_IdCardImageID",
                        column: x => x.IdCardImageID,
                        principalTable: "Media",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateBy = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdateBy = table.Column<int>(type: "int", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Blogs_Users_CreateBy",
                        column: x => x.CreateBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Blogs_Users_UpdateBy",
                        column: x => x.UpdateBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    TutorId = table.Column<int>(type: "int", nullable: true),
                    NumberOfStudent = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    TeachAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreateBy = table.Column<int>(type: "int", nullable: false),
                    TutorRating = table.Column<double>(type: "float", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Salary = table.Column<decimal>(type: "money", nullable: true),
                    PriceRatio = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Classes_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Classes_Users_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Classes_Users_TutorId",
                        column: x => x.TutorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FAQs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateBy = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAQs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FAQs_Users_CreateBy",
                        column: x => x.CreateBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverId = table.Column<int>(type: "int", nullable: true),
                    RefUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ViewStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TutorDegrees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TutorId = table.Column<int>(type: "int", nullable: true),
                    DegreeImgID = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TutorDegrees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TutorDegrees_Media_DegreeImgID",
                        column: x => x.DegreeImgID,
                        principalTable: "Media",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TutorDegrees_Users_TutorId",
                        column: x => x.TutorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TutorTeachTimeSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DayOfWeek = table.Column<DateOnly>(type: "date", nullable: true),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    TutorId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TutorTeachTimeSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TutorTeachTimeSchedules_Users_TutorId",
                        column: x => x.TutorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TutorVideos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TutorId = table.Column<int>(type: "int", nullable: true),
                    VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TutorVideos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TutorVideos_Users_TutorId",
                        column: x => x.TutorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Invitations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    TutorId = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invitations_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invitations_Users_TutorId",
                        column: x => x.TutorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Slots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    ActualEndtime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Slots_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Amount = table.Column<decimal>(type: "money", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReferenceId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    SlotId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Slots_SlotId",
                        column: x => x.SlotId,
                        principalTable: "Slots",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transactions_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transactions_Users_ReferenceId",
                        column: x => x.ReferenceId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_CreateBy",
                table: "Blogs",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_UpdateBy",
                table: "Blogs",
                column: "UpdateBy");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_StudentId",
                table: "Classes",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_SubjectId",
                table: "Classes",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_TutorId",
                table: "Classes",
                column: "TutorId");

            migrationBuilder.CreateIndex(
                name: "IX_FAQs_CreateBy",
                table: "FAQs",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_ClassId",
                table: "Invitations",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_TutorId",
                table: "Invitations",
                column: "TutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ReceiverId",
                table: "Notifications",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Slots_ClassId",
                table: "Slots",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CreatedBy",
                table: "Transactions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ReferenceId",
                table: "Transactions",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SlotId",
                table: "Transactions",
                column: "SlotId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorDegrees_DegreeImgID",
                table: "TutorDegrees",
                column: "DegreeImgID");

            migrationBuilder.CreateIndex(
                name: "IX_TutorDegrees_TutorId",
                table: "TutorDegrees",
                column: "TutorId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorTeachTimeSchedules_TutorId",
                table: "TutorTeachTimeSchedules",
                column: "TutorId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorVideos_TutorId",
                table: "TutorVideos",
                column: "TutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DegreeImageId",
                table: "Users",
                column: "DegreeImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IdCardImageID",
                table: "Users",
                column: "IdCardImageID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.DropTable(
                name: "ConsultationRequests");

            migrationBuilder.DropTable(
                name: "FAQs");

            migrationBuilder.DropTable(
                name: "Invitations");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "TutorDegrees");

            migrationBuilder.DropTable(
                name: "TutorTeachTimeSchedules");

            migrationBuilder.DropTable(
                name: "TutorVideos");

            migrationBuilder.DropTable(
                name: "Slots");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Media");
        }
    }
}
