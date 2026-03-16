using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Address = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "VATs",
                columns: table => new
                {
                    VatId = table.Column<Guid>(type: "uuid", nullable: false),
                    Percentage = table.Column<decimal>(type: "numeric(4,2)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VATs", x => x.VatId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    EmailAddress = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    PhotoUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Roles_Role_id",
                        column: x => x.Role_id,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerInvoices",
                columns: table => new
                {
                    CustomerInvoiceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    User_id = table.Column<Guid>(type: "uuid", nullable: false),
                    InvoiceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SubTotalAmount = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    VatAmount = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Vat_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerInvoices", x => x.CustomerInvoiceId);
                    table.ForeignKey(
                        name: "FK_CustomerInvoices_Customers_Customer_id",
                        column: x => x.Customer_id,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerInvoices_Users_User_id",
                        column: x => x.User_id,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerInvoices_VATs_Vat_id",
                        column: x => x.Vat_id,
                        principalTable: "VATs",
                        principalColumn: "VatId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    PurchasePrice = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    SalePrice = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    User_id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_Items_Users_User_id",
                        column: x => x.User_id,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerInvoiceLines",
                columns: table => new
                {
                    InvoiceLineId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerInvoice_id = table.Column<Guid>(type: "uuid", nullable: false),
                    Item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerInvoiceLines", x => x.InvoiceLineId);
                    table.ForeignKey(
                        name: "FK_CustomerInvoiceLines_CustomerInvoices_CustomerInvoice_id",
                        column: x => x.CustomerInvoice_id,
                        principalTable: "CustomerInvoices",
                        principalColumn: "CustomerInvoiceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerInvoiceLines_Items_Item_id",
                        column: x => x.Item_id,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInvoiceLines_CustomerInvoice_id",
                table: "CustomerInvoiceLines",
                column: "CustomerInvoice_id");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInvoiceLines_Item_id",
                table: "CustomerInvoiceLines",
                column: "Item_id");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInvoices_Customer_id",
                table: "CustomerInvoices",
                column: "Customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInvoices_User_id",
                table: "CustomerInvoices",
                column: "User_id");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInvoices_Vat_id",
                table: "CustomerInvoices",
                column: "Vat_id");

            migrationBuilder.CreateIndex(
                name: "IX_Items_User_id",
                table: "Items",
                column: "User_id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Role_id",
                table: "Users",
                column: "Role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerInvoiceLines");

            migrationBuilder.DropTable(
                name: "CustomerInvoices");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "VATs");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
