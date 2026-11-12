using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Refugio.API.Migrations
{
    /// <inheritdoc />
    public partial class InicialRefugio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NET_PONTO",
                columns: table => new
                {
                    ID_PONTO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODIGO_PUBLICO = table.Column<string>(type: "NVARCHAR2(7)", maxLength: 7, nullable: false),
                    NOME = table.Column<string>(type: "NVARCHAR2(110)", maxLength: 110, nullable: false),
                    BAIRRO = table.Column<string>(type: "NVARCHAR2(80)", maxLength: 80, nullable: false),
                    HORARIO_TEXTO = table.Column<string>(type: "NVARCHAR2(80)", maxLength: 80, nullable: false),
                    STATUS = table.Column<string>(type: "NVARCHAR2(8)", maxLength: 8, nullable: false, defaultValue: "ATIVO")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NET_PONTO", x => x.ID_PONTO);
                });

            migrationBuilder.CreateTable(
                name: "NET_RECURSO",
                columns: table => new
                {
                    ID_RECURSO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_PONTO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TIPO = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    DISPONIVEL = table.Column<string>(type: "CHAR(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NET_RECURSO", x => x.ID_RECURSO);
                    table.ForeignKey(
                        name: "FK_NET_RECURSO_NET_PONTO_ID_PONTO",
                        column: x => x.ID_PONTO,
                        principalTable: "NET_PONTO",
                        principalColumn: "ID_PONTO",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NET_PONTO_CODIGO_PUBLICO",
                table: "NET_PONTO",
                column: "CODIGO_PUBLICO",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NET_RECURSO_ID_PONTO_TIPO",
                table: "NET_RECURSO",
                columns: new[] { "ID_PONTO", "TIPO" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NET_RECURSO");

            migrationBuilder.DropTable(
                name: "NET_PONTO");
        }
    }
}
