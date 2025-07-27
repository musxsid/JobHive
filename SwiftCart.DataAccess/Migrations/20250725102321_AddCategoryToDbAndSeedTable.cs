using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JobHive.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryToDbAndSeedTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCategory", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "JobCategory",
                columns: new[] { "Id", "DisplayOrder", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Computer Science" },
                    { 2, 2, "Information Technology (IT)" },
                    { 3, 3, "Software Development" },
                    { 4, 4, "Data Science & Analytics" },
                    { 5, 5, "Artificial Intelligence (AI)" },
                    { 6, 6, "Cybersecurity" },
                    { 7, 7, "Cloud Computing" },
                    { 8, 8, "Web Development" },
                    { 9, 9, "Mobile App Development" },
                    { 10, 10, "DevOps & SysAdmin" },
                    { 11, 11, "Networking" },
                    { 12, 12, "Database Administration" },
                    { 13, 13, "UI/UX Design" },
                    { 14, 14, "Quality Assurance (QA)" },
                    { 15, 15, "Game Development" },
                    { 16, 16, "Embedded Systems" },
                    { 17, 17, "Blockchain" },
                    { 18, 18, "Finance & Banking" },
                    { 19, 19, "Healthcare" },
                    { 20, 20, "Education & Training" },
                    { 21, 21, "Marketing & Advertising" },
                    { 22, 22, "Sales & Business Development" },
                    { 23, 23, "Customer Support" },
                    { 24, 24, "Human Resources (HR)" },
                    { 25, 25, "Accounting & Auditing" },
                    { 26, 26, "Legal" },
                    { 27, 27, "Engineering (Non-IT)" },
                    { 28, 28, "Manufacturing" },
                    { 29, 29, "Retail" },
                    { 30, 30, "Hospitality & Tourism" },
                    { 31, 31, "Media & Entertainment" },
                    { 32, 32, "Writing & Content Creation" },
                    { 33, 33, "Architecture & Design" },
                    { 34, 34, "Logistics & Supply Chain" },
                    { 35, 35, "Research & Development" },
                    { 36, 36, "Government & Public Sector" },
                    { 37, 37, "Non-Profit & Social Services" },
                    { 38, 38, "Environmental & Sustainability" },
                    { 39, 39, "Agriculture & Forestry" },
                    { 40, 40, "Other" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobCategory");
        }
    }
}
