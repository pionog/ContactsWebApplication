using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactsWebApplication.Migrations
{
    /// <inheritdoc />
    public partial class DBMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountDetails",
                columns: table => new
                {
                    AccountID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nchar(32)", nullable: false),
                    Surname = table.Column<string>(type: "nchar(32)", nullable: false),
                    Email = table.Column<string>(type: "nchar(64)", nullable: false),
                    Password = table.Column<string>(type: "nchar(128)", nullable: false),
                    Category = table.Column<string>(type: "nchar(8)", nullable: false),
                    SubCategory = table.Column<string>(type: "nchar(32)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nchar(9)", nullable: false),
                    BirthDate = table.Column<string>(type: "nchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountDetails", x => x.AccountID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountDetails");
        }
    }
}
