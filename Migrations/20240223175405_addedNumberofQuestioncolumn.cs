using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EngineeringExamPreparation.Migrations
{
    public partial class addedNumberofQuestioncolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
                name: "NumberOfQuestion",
                table: "Tests",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfQuestion",
                table: "Tests");
        }
    }
}
