using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineOrderingSystem.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    CartID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.CartID);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    PreferredPaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerID);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Available = table.Column<bool>(type: "bit", nullable: false),
                    PrepTime = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    DietaryTags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemID = table.Column<int>(type: "int", nullable: false),
                    Availability = table.Column<bool>(type: "bit", nullable: false),
                    PreparationTime = table.Column<int>(type: "int", nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Ingredients = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReviewCount = table.Column<int>(type: "int", nullable: false),
                    IsPopular = table.Column<bool>(type: "bit", nullable: false),
                    IsChefSpecial = table.Column<bool>(type: "bit", nullable: false),
                    AllergenInfo = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Calories = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentDetails = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TransactionID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PaymentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AmountTendered = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    ChangeDue = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    ChequeNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsCleared = table.Column<bool>(type: "bit", nullable: true),
                    CardNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CardHolderName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CVV = table.Column<int>(type: "int", nullable: true),
                    Authorized = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentID);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    RestaurantID = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    OrderStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DeliveryAddress = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    EstimatedDeliveryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualDeliveryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveryInstructions = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SpecialRequests = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DeliveryFee = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TaxAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDelivery = table.Column<bool>(type: "bit", nullable: false),
                    OrderNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    CartID = table.Column<int>(type: "int", nullable: false),
                    ItemID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => new { x.CartID, x.ItemID });
                    table.ForeignKey(
                        name: "FK_CartItems_Carts_CartID",
                        column: x => x.CartID,
                        principalTable: "Carts",
                        principalColumn: "CartID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_Items_ItemID",
                        column: x => x.ItemID,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomizationOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    AdditionalCost = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    Choices = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxSelections = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomizationOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomizationOptions_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    OrderDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    ItemID = table.Column<int>(type: "int", nullable: false),
                    ItemName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SpecialInstructions = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Customizations = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomizationCost = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    DietaryNotes = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IsSpicy = table.Column<bool>(type: "bit", nullable: false),
                    AllergenInfo = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.OrderDetailID);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatusUpdates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatusUpdates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderStatusUpdates_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ItemID",
                table: "CartItems",
                column: "ItemID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomizationOptions_ItemId",
                table: "CustomizationOptions",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderID",
                table: "OrderDetails",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerID",
                table: "Orders",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatusUpdates_OrderID",
                table: "OrderStatusUpdates",
                column: "OrderID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "CustomizationOptions");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "OrderStatusUpdates");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
