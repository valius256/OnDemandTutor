using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OnDemandTutor.Models.Migrations
{
    /// <inheritdoc />
    public partial class Initial_add : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailTemplate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Params = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RecordStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FireBaseid = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvatarImageUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Dob = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Balance = table.Column<decimal>(type: "money", nullable: true),
                    TutorFeePerHour = table.Column<decimal>(type: "money", nullable: true),
                    Rating = table.Column<double>(type: "float", nullable: true),
                    IdCardImageUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ScheduleDesciption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Sex = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    DeaActiveReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TutorVerified = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateById = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateById = table.Column<int>(type: "int", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RecordStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Blogs_Users_CreateById",
                        column: x => x.CreateById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Blogs_Users_UpdateById",
                        column: x => x.UpdateById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConsultationRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HandleById = table.Column<int>(type: "int", nullable: false),
                    ConsultationContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ReasonFailed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RecordStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsultationRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConsultationRequests_Users_HandleById",
                        column: x => x.HandleById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Faqs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateById = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RecordStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faqs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Faqs_Users_CreateById",
                        column: x => x.CreateById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverId = table.Column<int>(type: "int", nullable: true),
                    RefUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsViewed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RecordStatus = table.Column<int>(type: "int", nullable: false)
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
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SubjectType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreateById = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RecordStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subjects_Users_CreateById",
                        column: x => x.CreateById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TutorVideos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TutorId = table.Column<int>(type: "int", nullable: true),
                    VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RecordStatus = table.Column<int>(type: "int", nullable: false)
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
                name: "Classes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TutorId = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    StudentName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SlotId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RecordStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Classes_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
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
                name: "TutorDegrees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TutorId = table.Column<int>(type: "int", nullable: true),
                    DegreeImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    DegreeNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IssuranceDate = table.Column<DateOnly>(type: "date", nullable: false),
                    TutorSubjectStatus = table.Column<int>(type: "int", nullable: false),
                    RejectReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RecordStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TutorDegrees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TutorDegrees_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TutorDegrees_Users_TutorId",
                        column: x => x.TutorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TutorSubjects",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RecordStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TutorSubjects", x => new { x.SubjectId, x.UserId });
                    table.ForeignKey(
                        name: "FK_TutorSubjects_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TutorSubjects_Users_UserId",
                        column: x => x.UserId,
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
                    StartTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreateById = table.Column<int>(type: "int", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    TeachAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ClassId = table.Column<int>(type: "int", nullable: true),
                    SubjectId = table.Column<int>(type: "integer", nullable: true),
                    IsOnline = table.Column<bool>(type: "bit", nullable: false),
                    NumberOfStudents = table.Column<int>(type: "int", nullable: false),
                    SlotStatus = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ActualEndTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsFinished = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RecordStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Slots_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Slots_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Slots_Users_CreateById",
                        column: x => x.CreateById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentClasses",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    RatingForTutorId = table.Column<int>(type: "int", nullable: false),
                    TutorId = table.Column<int>(type: "int", nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RecordStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentClasses", x => new { x.StudentId, x.ClassId });
                    table.ForeignKey(
                        name: "FK_StudentClasses_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentClasses_Users_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentClasses_Users_TutorId",
                        column: x => x.TutorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SlotStudents",
                columns: table => new
                {
                    SlotId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    Feedback = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SlotStudents", x => new { x.SlotId, x.UserId });
                    table.ForeignKey(
                        name: "FK_SlotStudents_Slots_SlotId",
                        column: x => x.SlotId,
                        principalTable: "Slots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SlotStudents_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Amount = table.Column<decimal>(type: "money", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SlotId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RecordStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Slots_SlotId",
                        column: x => x.SlotId,
                        principalTable: "Slots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "EmailTemplate",
                columns: new[] { "Id", "Body", "CreatedDate", "DeletedDate", "Description", "Name", "Params", "RecordStatus", "Status", "Subject", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "Welcome to OnDemandTutor! Dear [Name], thank you for joining us.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "This email is sent to welcome new users.", "Welcome_Email", "[Name]", 0, true, "Welcome to OnDemandTutor!", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Hello [Name], this is a reminder for your upcoming class on {Date}.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "This email is sent as a reminder for scheduled classes.", "Reminder_Email", "[Name], [Date]", 0, true, "Reminder for Your Class", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Dear [Name], your payment of [Amount] has been confirmed.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "This email confirms the successful payment for services.", "Payment_Confirmation", "[Name], [Amount]", 0, true, "Payment Confirmation", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "Hi [Name], we would love to hear your feedback about our services.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "This email requests feedback from users about their experience.", "Feedback_Request", "[Name]", 0, true, "Feedback Request", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "Dear [Name], please click the link to activate your account: [ActivationLink].", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "This email contains instructions to activate user accounts.", "Account_Activation", "[Name], [ActivationLink]", 0, true, "Account Activation", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, "\r\n                                           <!DOCTYPE html>\r\n                        <html lang=\"en\">\r\n                        <head>\r\n                            <meta charset=\"UTF-8\">\r\n                            <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n                            <title>Tutor Registration Approval</title>\r\n                            <style>\r\n                                body {\r\n                                    font-family: Arial, sans-serif;\r\n                                    line-height: 1.6;\r\n                                }\r\n                                .container {\r\n                                    margin: 0 auto;\r\n                                    padding: 20px;\r\n                                    max-width: 600px;\r\n                                    border: 1px solid #ddd;\r\n                                    border-radius: 5px;\r\n                                    background-color: #f9f9f9;\r\n                                }\r\n                                .header, .footer {\r\n                                    text-align: center;\r\n                                }\r\n                                .content {\r\n                                    margin-top: 20px;\r\n                                }\r\n                                .content p {\r\n                                    margin: 10px 0;\r\n                                }\r\n                            </style>\r\n                        </head>\r\n                        <body>\r\n                            <div class=\"container\">\r\n                                <div class=\"header\">\r\n                                    <h2>Tutor Registration Approval</h2>\r\n                                </div>\r\n                                <div class=\"content\">\r\n                                    <p>Dear [TutorName],</p>\r\n                                    <p>We are pleased to inform you that your registration as a tutor has been reviewed.</p>\r\n                                    <p>[ApprovalStatus]</p>\r\n                                    <p>If your registration has been approved, you can start using our platform to offer your tutoring services. If your registration has been rejected, please find the reason below:</p>\r\n                                    <p>[RejectionReason]</p>\r\n                                    <p>Thank you for your interest in joining our tutoring platform. If you have any questions, feel free to contact our support team.</p>\r\n                                </div>\r\n                                <div class=\"footer\">\r\n                                    <p>Best regards,</p>\r\n                                    <p>The On Demand Tutor Platform Team</p>\r\n                                </div>\r\n                            </div>\r\n                        </body>\r\n                        </html>\r\n\r\n    ", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Email template for notifying tutors about their registration approval status.", "TutorRegistrationApproval", "[TutorName], [ApprovalStatus], [RejectionReason]", 0, true, "Your Tutor Registration Approval Status", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "AvatarImageUrl", "Balance", "DeaActiveReason", "Dob", "Email", "FireBaseid", "FirstName", "IdCardImageUrl", "IsActive", "LastName", "Password", "Phone", "Rating", "Role", "ScheduleDesciption", "Sex", "TutorFeePerHour" },
                values: new object[] { 1, "123 Main St", "http://example.com/avatar1.png", 100.0m, null, new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "john.doe@example.com", "firebaseid1", "John", "http://example.com/id1.png", true, "Doe", "password123", "1234567890", 4.5, 3, "Available on weekends", 1, 50.0m });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "AvatarImageUrl", "Balance", "DeaActiveReason", "Dob", "Email", "FireBaseid", "FirstName", "IdCardImageUrl", "IsActive", "LastName", "Password", "Phone", "Rating", "Role", "ScheduleDesciption", "TutorFeePerHour" },
                values: new object[,]
                {
                    { 2, "456 Elm St", "http://example.com/avatar2.png", 200.0m, null, new DateTime(1985, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "jane.smith@example.com", "firebaseid2", "Jane", "http://example.com/id2.png", true, "Smith", "password123", "0987654321", 4.7000000000000002, 1, "Available on weekdays", 60.0m },
                    { 3, "789 Pine St", "http://example.com/avatar3.png", 300.0m, null, new DateTime(1995, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "alice.johnson@example.com", "firebaseid3", "Alice", "http://example.com/id3.png", true, "Johnson", "password123", "1122334455", 4.7999999999999998, 0, "Available in the evenings", 70.0m }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "AvatarImageUrl", "Balance", "DeaActiveReason", "Dob", "Email", "FireBaseid", "FirstName", "IdCardImageUrl", "IsActive", "LastName", "Password", "Phone", "Rating", "Role", "ScheduleDesciption", "Sex", "TutorFeePerHour" },
                values: new object[] { 4, "101 Maple St", "http://example.com/avatar4.png", 400.0m, null, new DateTime(1975, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "bob.williams@example.com", "firebaseid4", "Bob", "http://example.com/id4.png", true, "Williams", "password123", "2233445566", 4.9000000000000004, 1, "Available on weekends", 1, 80.0m });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "AvatarImageUrl", "Balance", "DeaActiveReason", "Dob", "Email", "FireBaseid", "FirstName", "IdCardImageUrl", "LastName", "Password", "Phone", "Rating", "Role", "ScheduleDesciption", "Sex", "TutorFeePerHour" },
                values: new object[] { 5, "202 Oak St", "http://example.com/avatar5.png", 500.0m, null, new DateTime(1980, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "charlie.brown@example.com", "firebaseid5", "Charlie", "http://example.com/id5.png", "Brown", "password123", "3344556677", 5.0, 0, "Available all day", 1, 90.0m });

            migrationBuilder.InsertData(
                table: "Blogs",
                columns: new[] { "Id", "Content", "CreateAt", "CreateById", "CreatedDate", "DeletedDate", "RecordStatus", "Title", "UpdateAt", "UpdateById", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "Content of the first blog.", new DateTime(2024, 7, 1, 9, 2, 42, 215, DateTimeKind.Local).AddTicks(7390), 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "First Blog", null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Content of the second blog.", new DateTime(2024, 7, 1, 9, 2, 42, 215, DateTimeKind.Local).AddTicks(7390), 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Second Blog", null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Content of the third blog.", new DateTime(2024, 7, 1, 9, 2, 42, 215, DateTimeKind.Local).AddTicks(7390), 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Third Blog", null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "Content of the fourth blog.", new DateTime(2024, 7, 1, 9, 2, 42, 215, DateTimeKind.Local).AddTicks(7390), 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Fourth Blog", null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "Content of the fifth blog.", new DateTime(2024, 7, 1, 9, 2, 42, 215, DateTimeKind.Local).AddTicks(7390), 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Fifth Blog", null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "ConsultationRequests",
                columns: new[] { "Id", "ConsultationContent", "CreatedDate", "DeletedDate", "HandleById", "Name", "Phone", "ReasonFailed", "RecordStatus", "RequestDate", "Status", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "Consultation content for request 1.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "John Doe", "1234567890", null, 0, new DateOnly(1, 1, 2), 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Consultation content for request 2.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Jane Smith", "9876543210", null, 0, new DateOnly(1, 1, 3), 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Consultation content for request 3.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Michael Brown", "5551234567", null, 0, new DateOnly(1, 1, 4), 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "Consultation content for request 4.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Emily Johnson", "4449876543", "Unavailable at requested time.", 0, new DateOnly(1, 1, 5), 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "Consultation content for request 5.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "David Lee", "7775551234", null, 0, new DateOnly(1, 1, 6), 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Faqs",
                columns: new[] { "Id", "Answer", "CreateAt", "CreateById", "CreatedDate", "DeletedDate", "Question", "RecordStatus", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "Lorem Ipsum is simply dummy text of the printing and typesetting industry.", new DateTime(2024, 7, 1, 9, 2, 42, 215, DateTimeKind.Local).AddTicks(7390), 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "What is Lorem Ipsum?", 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout.", new DateTime(2024, 6, 30, 9, 2, 42, 215, DateTimeKind.Local).AddTicks(7390), 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Why do we use it?", 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Contrary to popular belief, Lorem Ipsum is not simply random text.", new DateTime(2024, 6, 29, 9, 2, 42, 215, DateTimeKind.Local).AddTicks(7390), 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Where does it come from?", 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form.", new DateTime(2024, 6, 28, 9, 2, 42, 215, DateTimeKind.Local).AddTicks(7390), 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Where can I get some?", 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "The standard Lorem Ipsum passage, used since the 1500s, is reproduced below for those interested.", new DateTime(2024, 6, 27, 9, 2, 42, 215, DateTimeKind.Local).AddTicks(7390), 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "What is the standard Lorem Ipsum passage?", 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "Id", "Content", "CreatedDate", "DeletedDate", "IsViewed", "ReceiverId", "RecordStatus", "RefImageUrl", "RefUrl", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "New message received", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, 0, null, "/messages/1", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Meeting reminder", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 2, 0, null, "/events/5", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Payment received", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1, 0, null, "/payments/123", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "Id", "Content", "CreatedDate", "DeletedDate", "ReceiverId", "RecordStatus", "RefImageUrl", "RefUrl", "UpdatedDate" },
                values: new object[,]
                {
                    { 4, "New article published", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 0, "/images/articles/45.jpg", "/articles/45", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "Account updated", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, null, "/account/settings", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "Id", "CreateAt", "CreateById", "CreatedDate", "DeletedDate", "Description", "Name", "RecordStatus", "Status", "SubjectType", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 7, 1, 9, 2, 42, 215, DateTimeKind.Local).AddTicks(7390), 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Basic mathematics subject", "Mathematics", 0, 0, "Science", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(2024, 6, 30, 9, 2, 42, 215, DateTimeKind.Local).AddTicks(7390), 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Language arts subject", "English", 0, 0, "Language", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, new DateTime(2024, 6, 29, 9, 2, 42, 215, DateTimeKind.Local).AddTicks(7390), 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Study of matter and energy", "Physics", 0, 0, "Science", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, new DateTime(2024, 6, 28, 9, 2, 42, 215, DateTimeKind.Local).AddTicks(7390), 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Study of past events", "History", 0, 0, "Social Science", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, new DateTime(2024, 6, 27, 9, 2, 42, 215, DateTimeKind.Local).AddTicks(7390), 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Study of computers and computational systems", "Computer Science", 0, 0, "Science", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "TutorVideos",
                columns: new[] { "Id", "CreatedDate", "DeletedDate", "Description", "RecordStatus", "TutorId", "UpdatedDate", "VideoUrl" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Introduction to Mathematics", 0, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://example.com/video1.mp4" },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "English Literature Analysis", 0, 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://example.com/video2.mp4" },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Physics Fundamentals", 0, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://example.com/video3.mp4" },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Historical Events Overview", 0, 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://example.com/video4.mp4" },
                    { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Introduction to Programming", 0, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://example.com/video5.mp4" }
                });

            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "Id", "CreatedDate", "DeletedDate", "Name", "RecordStatus", "SlotId", "StudentName", "SubjectId", "TutorId", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mathematics 101", 0, 1, "John Doe", 1, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Literature 201", 0, 2, "Jane Smith", 2, 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Physics Lab", 0, 3, "Alice Johnson", 3, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "History Class", 0, 4, "Michael Brown", 4, 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chemistry 301", 0, 5, "Emily Davis", 5, 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Slots",
                columns: new[] { "Id", "ActualEndTime", "ClassId", "CreateById", "CreatedDate", "DeletedDate", "EndTime", "IsFinished", "IsOnline", "NumberOfStudents", "RecordStatus", "StartTime", "SubjectId", "TeachAddress", "UpdatedDate" },
                values: new object[,]
                {
                    { 4, new DateTime(2024, 7, 1, 0, 0, 0, 0, DateTimeKind.Local), null, 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 1, 17, 2, 42, 215, DateTimeKind.Local).AddTicks(7633), false, true, 2, 0, new DateTime(2024, 7, 1, 16, 2, 42, 215, DateTimeKind.Local).AddTicks(7632), 4, "101 Pine St", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, new DateTime(2024, 7, 1, 0, 0, 0, 0, DateTimeKind.Local), null, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 1, 19, 2, 42, 215, DateTimeKind.Local).AddTicks(7637), false, true, 4, 0, new DateTime(2024, 7, 1, 18, 2, 42, 215, DateTimeKind.Local).AddTicks(7636), 5, "111 Cedar St", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "TutorDegrees",
                columns: new[] { "Id", "CreatedDate", "DegreeImgUrl", "DegreeNumber", "DeletedDate", "Description", "IssuranceDate", "RecordStatus", "RejectReason", "SubjectId", "TutorId", "TutorSubjectStatus", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://example.com/degree1.jpg", "12345", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bachelor's in Mathematics", new DateOnly(2023, 5, 15), 0, null, 1, 1, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://example.com/degree2.jpg", "54321", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Master's in English Literature", new DateOnly(2022, 9, 30), 0, null, 2, 2, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://example.com/degree3.jpg", "98765", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "PhD in Physics", new DateOnly(2024, 2, 10), 0, null, 3, 1, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://example.com/degree4.jpg", "24680", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bachelor's in History", new DateOnly(2021, 12, 5), 0, null, 4, 2, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://example.com/degree5.jpg", "13579", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Master's in Computer Science", new DateOnly(2023, 8, 20), 0, null, 5, 1, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "TutorSubjects",
                columns: new[] { "SubjectId", "UserId", "CreatedDate", "DeletedDate", "Id", "RecordStatus", "Status", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 0, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 1, 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 0, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 0, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 0, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 0, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Slots",
                columns: new[] { "Id", "ActualEndTime", "ClassId", "CreateById", "CreatedDate", "DeletedDate", "EndTime", "IsFinished", "IsOnline", "NumberOfStudents", "RecordStatus", "StartTime", "SubjectId", "TeachAddress", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 7, 1, 0, 0, 0, 0, DateTimeKind.Local), 1, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 1, 11, 2, 42, 215, DateTimeKind.Local).AddTicks(7615), false, false, 5, 0, new DateTime(2024, 7, 1, 10, 2, 42, 215, DateTimeKind.Local).AddTicks(7614), 1, "123 Main St", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(2024, 7, 1, 13, 32, 42, 215, DateTimeKind.Local).AddTicks(7626), 2, 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 1, 13, 2, 42, 215, DateTimeKind.Local).AddTicks(7624), false, true, 3, 0, new DateTime(2024, 7, 1, 12, 2, 42, 215, DateTimeKind.Local).AddTicks(7623), 2, "456 Elm St", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, new DateTime(2024, 7, 1, 0, 0, 0, 0, DateTimeKind.Local), 1, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 7, 1, 15, 2, 42, 215, DateTimeKind.Local).AddTicks(7628), false, false, 7, 0, new DateTime(2024, 7, 1, 14, 2, 42, 215, DateTimeKind.Local).AddTicks(7628), 3, "789 Oak St", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "StudentClasses",
                columns: new[] { "ClassId", "StudentId", "CreatedDate", "DeletedDate", "Id", "Rating", "RatingForTutorId", "RecordStatus", "TutorId", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 5, 0, 0, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 1, 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 2, 0, 0, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 3, 0, 0, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 4, 0, 0, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 4, 0, 0, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "Amount", "CreatedById", "CreatedDate", "DeletedDate", "Notes", "PaymentMethod", "RecordStatus", "SlotId", "Status", "TransactionCode", "UpdatedDate" },
                values: new object[,]
                {
                    { 4, 120.00m, 2, new DateTime(2024, 6, 28, 9, 2, 42, 215, DateTimeKind.Local).AddTicks(7714), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Payment for online class", "Credit Card", 0, 4, 1, "TRX004", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 90.00m, 1, new DateTime(2024, 6, 27, 9, 2, 42, 215, DateTimeKind.Local).AddTicks(7717), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Payment for tutoring session", "PayPal", 0, 5, 1, "TRX005", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "SlotStudents",
                columns: new[] { "SlotId", "UserId", "Feedback", "PaymentStatus" },
                values: new object[,]
                {
                    { 1, 1, "Qua dinh luon em oi", 0 },
                    { 1, 2, "Qua dinh luon em oi 1", 0 },
                    { 2, 3, "Qua dinh luon em oi 2", 0 },
                    { 2, 4, "Qua dinh luon em oi 3", 0 },
                    { 3, 1, "Qua dinh luon em oi 4", 0 }
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "Amount", "CreatedById", "CreatedDate", "DeletedDate", "Notes", "PaymentMethod", "RecordStatus", "SlotId", "Status", "TransactionCode", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, 100.00m, 1, new DateTime(2024, 7, 1, 9, 2, 42, 215, DateTimeKind.Local).AddTicks(7707), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Payment for tutoring session", "Credit Card", 0, 1, 1, "TRX001", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 50.00m, 2, new DateTime(2024, 6, 30, 9, 2, 42, 215, DateTimeKind.Local).AddTicks(7710), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Payment for online class", "PayPal", 0, 2, 1, "TRX002", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 75.00m, 1, new DateTime(2024, 6, 29, 9, 2, 42, 215, DateTimeKind.Local).AddTicks(7712), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Payment for tutoring session", "Bank Transfer", 0, 3, 1, "TRX003", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_CreateById",
                table: "Blogs",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_UpdateById",
                table: "Blogs",
                column: "UpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_SubjectId",
                table: "Classes",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_TutorId",
                table: "Classes",
                column: "TutorId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultationRequests_HandleById",
                table: "ConsultationRequests",
                column: "HandleById");

            migrationBuilder.CreateIndex(
                name: "IX_Faqs_CreateById",
                table: "Faqs",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ReceiverId",
                table: "Notifications",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Slots_ClassId",
                table: "Slots",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Slots_CreateById",
                table: "Slots",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_Slots_SubjectId",
                table: "Slots",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SlotStudents_UserId",
                table: "SlotStudents",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentClasses_ClassId",
                table: "StudentClasses",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentClasses_TutorId",
                table: "StudentClasses",
                column: "TutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_CreateById",
                table: "Subjects",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CreatedById",
                table: "Transactions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SlotId",
                table: "Transactions",
                column: "SlotId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorDegrees_SubjectId",
                table: "TutorDegrees",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorDegrees_TutorId",
                table: "TutorDegrees",
                column: "TutorId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorSubjects_UserId",
                table: "TutorSubjects",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorVideos_TutorId",
                table: "TutorVideos",
                column: "TutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_FireBaseid",
                table: "Users",
                column: "FireBaseid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.DropTable(
                name: "ConsultationRequests");

            migrationBuilder.DropTable(
                name: "EmailTemplate");

            migrationBuilder.DropTable(
                name: "Faqs");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "SlotStudents");

            migrationBuilder.DropTable(
                name: "StudentClasses");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "TutorDegrees");

            migrationBuilder.DropTable(
                name: "TutorSubjects");

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
        }
    }
}
