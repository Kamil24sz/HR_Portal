using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR_Portal.Migrations
{
    public partial class start : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestStatuses",
                columns: table => new
                {
                    StatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestStatuses", x => x.StatusId);
                });

            migrationBuilder.CreateTable(
                name: "RequestTypes",
                columns: table => new
                {
                    RequestTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdditionalDetails = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestTypes", x => x.RequestTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Trainings",
                columns: table => new
                {
                    TrainingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainingName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrainingType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainings", x => x.TrainingId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
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
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DirectorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.DepartmentId);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    PositionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PositionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    BaseSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.PositionId);
                    table.ForeignKey(
                        name: "FK_Positions_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ManagerId = table.Column<int>(type: "int", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "Date", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocumentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PositionId = table.Column<int>(type: "int", nullable: true),
                    VacationDays = table.Column<int>(type: "int", nullable: false),
                    BankAccount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SalaryBonus = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "PositionId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Users_Users_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestTypeId = table.Column<int>(type: "int", nullable: false),
                    RequestorId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_Requests_RequestStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "RequestStatuses",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Requests_RequestTypes_RequestTypeId",
                        column: x => x.RequestTypeId,
                        principalTable: "RequestTypes",
                        principalColumn: "RequestTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Requests_Users_RequestorId",
                        column: x => x.RequestorId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingDestinations",
                columns: table => new
                {
                    DestinationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainingId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Progress = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingDestinations", x => x.DestinationId);
                    table.ForeignKey(
                        name: "FK_TrainingDestinations_Trainings_TrainingId",
                        column: x => x.TrainingId,
                        principalTable: "Trainings",
                        principalColumn: "TrainingId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainingDestinations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "DepartmentId", "DepartmentName", "DirectorId" },
                values: new object[,]
                {
                    { 1, "Human Resources", null },
                    { 2, "IT Support", null },
                    { 3, "Accounting and Finance", null }
                });

            migrationBuilder.InsertData(
                table: "RequestStatuses",
                columns: new[] { "StatusId", "StatusName" },
                values: new object[,]
                {
                    { 1, "Pending Manager Approval" },
                    { 2, "Pending HR Confirmation" },
                    { 3, "Rejected" },
                    { 4, "Approved" }
                });

            migrationBuilder.InsertData(
                table: "RequestTypes",
                columns: new[] { "RequestTypeId", "AdditionalDetails", "RequestName" },
                values: new object[,]
                {
                    { 1, "Select dates when you want to take days off.", "Take Day Off" },
                    { 2, "Select dates for sick Leave. Remember to send medical document to HR Person.", "Report Sickness" },
                    { 3, "Select dates when you want to have vacation.", "Create Vacation Plan" },
                    { 4, "Select delegation date, costs which are on you, plece where you been.", "Report Delegation" },
                    { 5, "Provide data, that you would like to change.", "Change Personal Data" }
                });

            migrationBuilder.InsertData(
                table: "Trainings",
                columns: new[] { "TrainingId", "Cost", "Description", "TrainingName", "TrainingType" },
                values: new object[,]
                {
                    { 1, 0m, "Safety training for new workers", "Workplace Safety", "Internal" },
                    { 2, 500m, "These skills are useful for both new and old employees, and they play a vital role in building a respectful, collaborative, and efficient culture within a company.", "Soft Skills Development", "External" },
                    { 3, 0m, "Technical skills training is a basic component of employee education. It is a primary way for you to develop the skills you need in your role.", "Technical skills development", "Internal" },
                    { 4, 0m, " Quality training refers to familiarizing employees with the means of preventing, detecting, and eliminating non-quality items.", "Quality training", "Internal" },
                    { 5, 500m, "Leadership and management training courses are specialized programs designed to help you learn new leadership techniques and refine old skills to run your team, including assertive communication, motivation methods, and coaching.", "Leadership Training", "External" },
                    { 6, 0m, "Safety training for new workers", "Workplace Safety", "Internal" },
                    { 7, 700m, "Learn what's possible with Word, Excel, and PowerPoint.", "Microsoft 365 Training", "External" },
                    { 8, 400m, "Sales programs can help you and your team better prepare and execute your sales plan. When signing up for a sales training program, you can expect a range of resources designed to help participants learn the strategies that turn prospects into customers", "Sales Training", "External" },
                    { 9, 600m, "Learning another language provides many other benefits including greater academic achievement, greater cognitive development, and more positive attitudes towards other languages and cultures. You can choose french, deutch, spanish and russian", "Language Training", "External" },
                    { 10, 0m, "Diversity training is a program designed to facilitate positive intergroup interaction, reduce prejudice and discrimination, and generally teach individuals who are different from others how to work together effectively.", "Diversity Training", "Internal" }
                });

            migrationBuilder.InsertData(
                table: "Positions",
                columns: new[] { "PositionId", "BaseSalary", "DepartmentId", "PositionName" },
                values: new object[,]
                {
                    { 1, 3500m, 1, "HR Manager" },
                    { 2, 3900m, 1, "Business Partner" },
                    { 3, 4000m, 2, "Team Leader" },
                    { 4, 3700m, 2, "IT Support Specialist" },
                    { 5, 3400m, 3, "Main Accountant" },
                    { 6, 3500m, 3, "Finnancial Controller" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Address", "BankAccount", "CreationDate", "DateOfBirth", "DocumentId", "FirstName", "Gender", "LastName", "ManagerId", "PositionId", "SalaryBonus", "VacationDays" },
                values: new object[,]
                {
                    { 0, "Address line 1  \r\n Address line 2 ", "00000000000000000000", new DateTime(2022, 1, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2000, 10, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "0000000000", "Kamil", "Male", "Szczypczyk", 0, 4, 0m, 24 },
                    { 2, "Warszawska 31 \r\n 31-651 Krakow ", "12345678900987654321", new DateTime(2010, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1990, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "1237896541", "Pawel", "Male", "Piotrowski", 2, 3, 700m, 22 },
                    { 4, "Skawinska 18/6 \r\n 29-119 Krakow ", "67838164462801977", new DateTime(2018, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1989, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "5558880098", "Karolina", "Female", "Siekiera", 4, 1, 150m, 21 },
                    { 6, "Modrzewiowa 8/1  \r\n 30-221 Krakow ", "2213464679643220", new DateTime(2014, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1991, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "3517792018", "Anna", "Female", "Wesola", 6, 5, 600m, 24 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Address", "BankAccount", "CreationDate", "DateOfBirth", "DocumentId", "FirstName", "Gender", "LastName", "ManagerId", "PositionId", "SalaryBonus", "VacationDays" },
                values: new object[,]
                {
                    { 1, "Krakowska 13  \r\n 30-156 Krakow ", "99876543211234567890", new DateTime(2017, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1987, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "11223344550", "Piotr", "Male", "Pawlowski", 2, 4, 300m, 24 },
                    { 3, "Aleja Jana Pawla II 99 \r\n 31-887 Krakow ", "1300982287450662", new DateTime(2015, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1982, 11, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "6669998766", "Magda", "Female", "Konieczna", 4, 2, 700m, 26 },
                    { 5, "Sloneczna 12/3  \r\n 30-156 Krakow ", "9887718233092343", new DateTime(2017, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1985, 7, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "1234765126", "Filip", "Male", "Nowak", 2, 4, 200m, 23 },
                    { 7, "Wielicka 234  \r\n 31-186 Krakow ", "964678826543300", new DateTime(2017, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1990, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "654327876", "Kamila", "Female", "Niedzielska", 6, 6, 300m, 24 },
                    { 8, "Jerzmanowskiego 28/22  \r\n 30-386 Krakow ", "74783654442894652", new DateTime(2018, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1989, 8, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "98765442212", "Emil", "Male", "Balon", 6, 6, 200m, 22 },
                    { 14, "Szlachetna 2  \r\n 30-563 Krakow ", "45562899846541133344", new DateTime(2017, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1990, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "6543322674", "Julian", "Male", "Krolewski", 2, 4, 200m, 20 },
                    { 15, "Zywiecka 11  \r\n 30-157 Krakow ", "992996466190013440", new DateTime(2018, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1993, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "456289753", "Wiktoria", "Female", "Winnicka", 2, 4, 200m, 18 },
                    { 17, "Szwedzka 10  \r\n 30-156 Krakow ", "4552672788864544422", new DateTime(2017, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1992, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "5462678927", "Marian", "Male", "Kolarski", 4, 2, 200m, 23 },
                    { 20, "Poznanska 13  \r\n 30-432 Krakow ", "6677829937654111", new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2000, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "3552789265", "Marcel", "Male", "Litewski", 4, 2, 100m, 17 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Address", "BankAccount", "CreationDate", "DateOfBirth", "DocumentId", "FirstName", "Gender", "LastName", "ManagerId", "PositionId", "SalaryBonus", "VacationDays" },
                values: new object[,]
                {
                    { 9, "Kozia 3  \r\n 30-216 Krakow ", "435682998567737542", new DateTime(2020, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2000, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "6543677873", "Wiktor", "Male", "Gniewny", 3, 2, 100m, 18 },
                    { 10, "Kurza 18  \r\n 31-642 Krakow ", "545267289986552213", new DateTime(2019, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1994, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "645332788", "Angelina", "Female", "Kowalska", 5, 4, 100m, 19 },
                    { 11, "Rzozowska 4/8  \r\n 30-678 Krakow ", "5562891093544427911", new DateTime(2018, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1992, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "342578655", "Hubert", "Male", "Dziurski", 8, 6, 250m, 23 },
                    { 12, "Rzeszowska 8 \r\n 30-334 Krakow ", "9455452678927665532", new DateTime(2021, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1997, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "765432124", "Kamil", "Male", "Rutkowski", 1, 4, 100m, 18 },
                    { 13, "Ciekawska 3 \r\n 30-082 Krakow ", "25544377815455499223", new DateTime(2017, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1997, 7, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "6543213335", "Tomasz", "Male", "Lajdak", 7, 6, 100m, 20 },
                    { 16, "Sienna 7  \r\n 30-886 Krakow ", "4552728910107363533", new DateTime(2020, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1999, 12, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "1453678254", "Klaudia", "Female", "Grodziecka", 5, 4, 100m, 17 },
                    { 18, "Teligi 16/58  \r\n 30-836 Krakow ", "934427891653222110", new DateTime(2016, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1994, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "6785432219", "Maciej", "Male", "Krakowski", 3, 3, 200m, 24 },
                    { 19, "Studencka 9/11  \r\n 30-550 Krakow ", "251788992644448280", new DateTime(2019, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1999, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "5432224576", "Marlena", "Female", "Milewska", 8, 6, 100m, 22 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserId",
                table: "AspNetUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_DirectorId",
                table: "Departments",
                column: "DirectorId");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_DepartmentId",
                table: "Positions",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_RequestorId",
                table: "Requests",
                column: "RequestorId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_RequestTypeId",
                table: "Requests",
                column: "RequestTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_StatusId",
                table: "Requests",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingDestinations_TrainingId",
                table: "TrainingDestinations",
                column: "TrainingId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingDestinations_UserId",
                table: "TrainingDestinations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ManagerId",
                table: "Users",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PositionId",
                table: "Users",
                column: "PositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Users_UserId",
                table: "AspNetUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Users_DirectorId",
                table: "Departments",
                column: "DirectorId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Users_DirectorId",
                table: "Departments");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "TrainingDestinations");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "RequestStatuses");

            migrationBuilder.DropTable(
                name: "RequestTypes");

            migrationBuilder.DropTable(
                name: "Trainings");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
