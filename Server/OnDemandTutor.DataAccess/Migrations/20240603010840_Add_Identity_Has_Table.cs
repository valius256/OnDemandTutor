using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnDemandTutor.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Add_Identity_Has_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "identity");
              
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "ConsultationRequest",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsultationRequest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Media",
                schema: "dbo",
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
                name: "RoleClaims",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subject",
                schema: "dbo",
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
                    table.PrimaryKey("PK_Subject", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                schema: "identity",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "identity",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                schema: "identity",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)     
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AvatarImageId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Dob = table.Column<DateTime>(type: "datetime", nullable: true),
                    Role = table.Column<int>(type: "int", nullable: true),
                    RecordStatus = table.Column<bool>(type: "bit", nullable: true),
                    Balance = table.Column<decimal>(type: "money", nullable: true),
                    Rating = table.Column<double>(type: "float", nullable: true),
                    DegreeImageId = table.Column<int>(type: "int", nullable: true),
                    IdCardImageID = table.Column<int>(type: "int", nullable: true),
                    ScheduleDesciption = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Media",
                        column: x => x.IdCardImageID,
                        principalSchema: "dbo",
                        principalTable: "Media",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_User_Media1",
                        column: x => x.DegreeImageId,
                        principalSchema: "dbo",
                        principalTable: "Media",
                        principalColumn: "Id");
                });

               migrationBuilder.CreateTable(
        name: "Blog",
        schema: "dbo",
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
            table.PrimaryKey("PK_Blog", x => x.Id);
            table.ForeignKey(
                name: "FK_Blog_User",
                column: x => x.CreateBy,
                principalSchema: "identity",
                principalTable: "User",
                principalColumn: "Id");
            table.ForeignKey(
                name: "FK_Blog_User1",
                column: x => x.UpdateBy,
                principalSchema: "identity",
                principalTable: "User",
                principalColumn: "Id");
        });

            migrationBuilder.CreateTable(
                name: "Class",
                schema: "dbo",
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
                    table.PrimaryKey("PK_Class", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Class_Subject",
                        column: x => x.SubjectId,
                        principalSchema: "dbo",
                        principalTable: "Subject",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Class_User",
                        column: x => x.TutorId,
                        principalSchema: "identity",
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Class_User1",
                        column: x => x.StudentId,
                        principalSchema: "identity",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FAQ",
                schema: "dbo",
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
                    table.PrimaryKey("PK_FAQ", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FAQ_User",
                        column: x => x.CreateBy,
                        principalSchema: "identity",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                schema: "dbo",
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
                    table.PrimaryKey("PK_Notification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notification_User",
                        column: x => x.ReceiverId,
                        principalSchema: "identity",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TutorDegree",
                schema: "dbo",
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
                    table.PrimaryKey("PK_TutorDegree", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TutorDegree_Media",
                        column: x => x.DegreeImgID,
                        principalSchema: "dbo",
                        principalTable: "Media",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TutorDegree_User",
                        column: x => x.TutorId,
                        principalSchema: "identity",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TutorTeachTimeSchedule",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DayOfWeek = table.Column<DateOnly>(type: "date", nullable: true),
                    StartTime = table.Column<TimeOnly>(type: "time(0)", precision: 0, nullable: true),
                    EndTime = table.Column<TimeOnly>(type: "time(0)", precision: 0, nullable: true),
                    TutorId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TutorFreeTimeSchedule_User",
                        column: x => x.TutorId,
                        principalSchema: "identity",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TutorVideo",
                schema: "dbo",
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
                    table.PrimaryKey("PK_TutorVIdeo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TutorVIdeo_User",
                        column: x => x.TutorId,
                        principalSchema: "identity",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClassRequest",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    TutorId = table.Column<int>(type: "int", nullable: false),
                    ApproverId = table.Column<int>(type: "int", nullable: true),
                    RequestDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassRequest_Class",
                        column: x => x.ClassId,
                        principalSchema: "dbo",
                        principalTable: "Class",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClassRequest_User",
                        column: x => x.TutorId,
                        principalSchema: "identity",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Invitation",
                schema: "dbo",
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
                    table.PrimaryKey("PK_Invitation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invitation_Class",
                        column: x => x.ClassId,
                        principalSchema: "dbo",
                        principalTable: "Class",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Invitation_User",
                        column: x => x.TutorId,
                        principalSchema: "identity",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Slot",
                schema: "dbo",
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
                    table.PrimaryKey("PK_Lesson", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lesson_Class",
                        column: x => x.ClassId,
                        principalSchema: "dbo",
                        principalTable: "Class",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                schema: "dbo",
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
                    table.PrimaryKey("PK_Transaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transaction_Slot",
                        column: x => x.SlotId,
                        principalSchema: "dbo",
                        principalTable: "Slot",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transaction_User",
                        column: x => x.ReferenceId,
                        principalSchema: "identity",
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transaction_User1",
                        column: x => x.CreatedBy,
                        principalSchema: "identity",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blog_CreateBy",
                schema: "dbo",
                table: "Blog",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_Blog_UpdateBy",
                schema: "dbo",
                table: "Blog",
                column: "UpdateBy");

            migrationBuilder.CreateIndex(
                name: "IX_Class_StudentId",
                schema: "dbo",
                table: "Class",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Class_SubjectId",
                schema: "dbo",
                table: "Class",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Class_TutorId",
                schema: "dbo",
                table: "Class",
                column: "TutorId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassRequest_ClassId",
                schema: "dbo",
                table: "ClassRequest",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassRequest_TutorId",
                schema: "dbo",
                table: "ClassRequest",
                column: "TutorId");

            migrationBuilder.CreateIndex(
                name: "IX_FAQ_CreateBy",
                schema: "dbo",
                table: "FAQ",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_Invitation_ClassId",
                schema: "dbo",
                table: "Invitation",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitation_TutorId",
                schema: "dbo",
                table: "Invitation",
                column: "TutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_ReceiverId",
                schema: "dbo",
                table: "Notification",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Slot_ClassId",
                schema: "dbo",
                table: "Slot",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_CreatedBy",
                schema: "dbo",
                table: "Transaction",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_ReferenceId",
                schema: "dbo",
                table: "Transaction",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_SlotId",
                schema: "dbo",
                table: "Transaction",
                column: "SlotId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorDegree_DegreeImgID",
                schema: "dbo",
                table: "TutorDegree",
                column: "DegreeImgID");

            migrationBuilder.CreateIndex(
                name: "IX_TutorDegree_TutorId",
                schema: "dbo",
                table: "TutorDegree",
                column: "TutorId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorTeachTimeSchedule_TutorId",
                schema: "dbo",
                table: "TutorTeachTimeSchedule",
                column: "TutorId");

            migrationBuilder.CreateIndex(
                name: "IX_TutorVideo_TutorId",
                schema: "dbo",
                table: "TutorVideo",
                column: "TutorId");

         migrationBuilder.CreateIndex(
                name: "IX_User_IdCardImageID",
                schema: "identity",
                table: "User",
                column: "IdCardImageID");

            migrationBuilder.CreateIndex(
                name: "IX_User_DegreeImageId",
                schema: "identity",
                table: "User",
                column: "DegreeImageId");


      
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        { 
                            migrationBuilder.DropTable(
                    name: "Class",
                    schema: "dbo");

                migrationBuilder.DropTable(
                    name: "Blog",
                    schema: "dbo");

                migrationBuilder.DropTable(
                    name: "UserTokens",
                    schema: "identity");

                migrationBuilder.DropTable(
                    name: "UserRoles",
                    schema: "identity");

                migrationBuilder.DropTable(
                    name: "UserLogins",
                    schema: "identity");

                migrationBuilder.DropTable(
                    name: "UserClaims",
                    schema: "identity");

                migrationBuilder.DropTable(
                    name: "Roles",
                    schema: "identity");

                migrationBuilder.DropTable(
                    name: "RoleClaims",
                    schema: "identity");

                migrationBuilder.DropTable(
                    name: "User",
                    schema: "identity");

                migrationBuilder.DropTable(
                    name: "Subject",
                    schema: "dbo");

                migrationBuilder.DropTable(
                    name: "Media",
                    schema: "dbo");

                // Drop schemas
                migrationBuilder.DropSchema(
                    name: "identity");

                migrationBuilder.DropSchema(
                    name: "dbo");
            
            
            }
    }
}
