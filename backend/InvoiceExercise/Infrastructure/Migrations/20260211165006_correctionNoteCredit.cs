using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class correctionNoteCredit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CreditNotes",
                table: "CreditNotes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CreditNotes");

            migrationBuilder.AlterColumn<long>(
                name: "CreditNoteNumber",
                table: "CreditNotes",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreditNotes",
                table: "CreditNotes",
                column: "CreditNoteNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CreditNotes",
                table: "CreditNotes");

            migrationBuilder.AlterColumn<string>(
                name: "CreditNoteNumber",
                table: "CreditNotes",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CreditNotes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreditNotes",
                table: "CreditNotes",
                column: "Id");
        }
    }
}
