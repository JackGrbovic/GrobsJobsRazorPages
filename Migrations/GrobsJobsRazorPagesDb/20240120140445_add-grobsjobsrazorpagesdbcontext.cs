using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrobsJobsRazorPages.Migrations.GrobsJobsRazorPagesDb
{
    /// <inheritdoc />
    public partial class addgrobsjobsrazorpagesdbcontext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    JobPosterId = table.Column<string>(type: "TEXT", nullable: false),
                    JobPosterNormalizedUserName = table.Column<string>(type: "TEXT", nullable: false),
                    DateTimePosted = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MessageTitle = table.Column<string>(type: "TEXT", nullable: false),
                    MessageBody = table.Column<string>(type: "TEXT", nullable: false),
                    MessageSender = table.Column<string>(type: "TEXT", nullable: false),
                    MessageRecipient = table.Column<string>(type: "TEXT", nullable: false),
                    MessageSenderUserName = table.Column<string>(type: "TEXT", nullable: false),
                    MessageRecipientUserName = table.Column<string>(type: "TEXT", nullable: false),
                    DateTimeSent = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "Messages");
        }
    }
}
