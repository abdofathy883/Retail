using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ProductImageModification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_ProductVarients_ProductVarientId",
                table: "ProductImages");

            migrationBuilder.DropIndex(
                name: "IX_ProductImages_ProductVarientId",
                table: "ProductImages");

            migrationBuilder.RenameColumn(
                name: "ProductVarientId",
                table: "ProductImages",
                newName: "ProductVariantId");

            migrationBuilder.AddColumn<int>(
                name: "VariantImageId",
                table: "ProductVarients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductVariantId",
                table: "ProductImages",
                column: "ProductVariantId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_ProductVarients_ProductVariantId",
                table: "ProductImages",
                column: "ProductVariantId",
                principalTable: "ProductVarients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_ProductVarients_ProductVariantId",
                table: "ProductImages");

            migrationBuilder.DropIndex(
                name: "IX_ProductImages_ProductVariantId",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "VariantImageId",
                table: "ProductVarients");

            migrationBuilder.RenameColumn(
                name: "ProductVariantId",
                table: "ProductImages",
                newName: "ProductVarientId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductVarientId",
                table: "ProductImages",
                column: "ProductVarientId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_ProductVarients_ProductVarientId",
                table: "ProductImages",
                column: "ProductVarientId",
                principalTable: "ProductVarients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
