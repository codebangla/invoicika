using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class itemgroups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerInvoiceGroupLines",
                columns: table => new
                {
                    InvoiceGroupLineId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerInvoice_id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    SubTotalAmount = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    VatAmount = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerInvoiceGroupLines", x => x.InvoiceGroupLineId);
                    table.ForeignKey(
                        name: "FK_CustomerInvoiceGroupLines_CustomerInvoices_CustomerInvoice_~",
                        column: x => x.CustomerInvoice_id,
                        principalTable: "CustomerInvoices",
                        principalColumn: "CustomerInvoiceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemGroups",
                columns: table => new
                {
                    ItemGroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    User_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemGroups", x => x.ItemGroupId);
                    table.ForeignKey(
                        name: "FK_ItemGroups_Users_User_id",
                        column: x => x.User_id,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerInvoiceGroupItemLines",
                columns: table => new
                {
                    GroupItemLineId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerInvoiceGroupLine_id = table.Column<Guid>(type: "uuid", nullable: false),
                    Item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerInvoiceGroupItemLines", x => x.GroupItemLineId);
                    table.ForeignKey(
                        name: "FK_CustomerInvoiceGroupItemLines_CustomerInvoiceGroupLines_Cus~",
                        column: x => x.CustomerInvoiceGroupLine_id,
                        principalTable: "CustomerInvoiceGroupLines",
                        principalColumn: "InvoiceGroupLineId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerInvoiceGroupItemLines_Items_Item_id",
                        column: x => x.Item_id,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemGroupItems",
                columns: table => new
                {
                    ItemGroupItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemGroup_id = table.Column<Guid>(type: "uuid", nullable: false),
                    Item_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemGroupItems", x => x.ItemGroupItemId);
                    table.ForeignKey(
                        name: "FK_ItemGroupItems_ItemGroups_ItemGroup_id",
                        column: x => x.ItemGroup_id,
                        principalTable: "ItemGroups",
                        principalColumn: "ItemGroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemGroupItems_Items_Item_id",
                        column: x => x.Item_id,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInvoiceGroupItemLines_CustomerInvoiceGroupLine_id",
                table: "CustomerInvoiceGroupItemLines",
                column: "CustomerInvoiceGroupLine_id");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInvoiceGroupItemLines_Item_id",
                table: "CustomerInvoiceGroupItemLines",
                column: "Item_id");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInvoiceGroupLines_CustomerInvoice_id",
                table: "CustomerInvoiceGroupLines",
                column: "CustomerInvoice_id");

            migrationBuilder.CreateIndex(
                name: "IX_ItemGroupItems_Item_id",
                table: "ItemGroupItems",
                column: "Item_id");

            migrationBuilder.CreateIndex(
                name: "IX_ItemGroupItems_ItemGroup_id",
                table: "ItemGroupItems",
                column: "ItemGroup_id");

            migrationBuilder.CreateIndex(
                name: "IX_ItemGroups_User_id",
                table: "ItemGroups",
                column: "User_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerInvoiceGroupItemLines");

            migrationBuilder.DropTable(
                name: "ItemGroupItems");

            migrationBuilder.DropTable(
                name: "CustomerInvoiceGroupLines");

            migrationBuilder.DropTable(
                name: "ItemGroups");
        }
    }
}
