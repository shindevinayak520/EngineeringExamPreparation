using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EngineeringExamPreparation.Migrations
{
    public partial class selctedInTestChoicescolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Selected",
                table: "TestChoices",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Selected",
                table: "TestChoices");
        }
    }
}
