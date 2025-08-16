# Database Integration Guide
## Orders and Payments

This guide explains how the Online Ordering System now saves orders and payments to the database instead of using hardcoded data.

---

## Overview

The system has been updated to provide **full database persistence** for:
- ✅ **Orders** - Complete order details, items, and status history
- ✅ **Payments** - Payment information with transaction tracking
- ✅ **Order History** - Real-time order history from database
- ✅ **Payment History** - Complete payment records and statistics

---

## What Was Changed

### Before (Hardcoded Data)
- Orders were simulated but not saved
- Payment processing was fake
- Order history was static demo data
- No persistent storage

### After (Database Integration)
- Orders are saved to SQL Server database
- Payments are stored with transaction IDs
- Order history is retrieved from database
- Complete audit trail maintained

---

## New Database Tables

### 1. **PaymentEntity Table**
Stores all payment information:
```sql
CREATE TABLE PaymentEntities (
    PaymentID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT FOREIGN KEY REFERENCES Orders(OrderID),
    CustomerID INT FOREIGN KEY REFERENCES Customers(CustomerID),
    Amount DECIMAL(10,2),
    PaymentMethod NVARCHAR(50),
    PaymentStatus NVARCHAR(50),
    PaymentDate DATETIME2,
    PaymentDetails NVARCHAR(500),
    TransactionID NVARCHAR(100),
    CardNumber NVARCHAR(20),
    CardHolderName NVARCHAR(100),
    ExpiryDate DATETIME2,
    CVV INT,
    ChequeNumber NVARCHAR(50),
    BankName NVARCHAR(100),
    AmountTendered DECIMAL(10,2)
)
```

### 2. **Enhanced Orders Table**
Orders now include:
- Complete item details
- Status history tracking
- Payment integration
- Delivery information

### 3. **OrderStatusUpdates Table**
Tracks order status changes:
```sql
CREATE TABLE OrderStatusUpdates (
    Id INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT FOREIGN KEY REFERENCES Orders(OrderID),
    Status NVARCHAR(50),
    Message NVARCHAR(500),
    Timestamp DATETIME2
)
```

---

## How It Works

### 1. **Order Creation Process**
```csharp
// 1. Create order from cart
var order = new Order();
order.CreateOrder(customerId, cart);

// 2. Set delivery details
order.DeliveryAddress = address;
order.IsDelivery = true;
order.DeliveryFee = 2.99;

// 3. Create payment
var payment = new Cash { AmountTendered = total + 5, TotalAmount = total };

// 4. Save to database (order + payment)
var orderDataAccess = new OrderDataAccess();
var savedOrder = orderDataAccess.CreateOrderWithPayment(order, payment);
```

### 2. **Payment Processing**
```csharp
// Payment is automatically saved when order is created
var paymentEntity = PaymentEntity.FromPayment(payment, order.OrderID, customerId);
context.Payments.Add(paymentEntity);
context.SaveChanges();
```

### 3. **Order History Retrieval**
```csharp
// Get complete order history with payments
var orderDataAccess = new OrderDataAccess();
var orderHistory = orderDataAccess.GetOrderHistoryWithPayments(customerId);

foreach (var orderWithPayment in orderHistory)
{
    var order = orderWithPayment.Order;
    var payment = orderWithPayment.Payment;
    
    Console.WriteLine($"Order #{order.OrderID}: £{order.TotalAmount:F2}");
    Console.WriteLine($"Payment: {payment?.PaymentMethod} - {payment?.TransactionID}");
}
```

---

## New Classes and Methods

### 1. **PaymentEntity Class**
- Concrete database entity for payments
- Stores all payment method details
- Links to orders and customers
- Generates unique transaction IDs

### 2. **PaymentDataAccess Class**
```csharp
public class PaymentDataAccess
{
    public PaymentEntity CreatePayment(PaymentEntity payment);
    public List<PaymentEntity> GetPaymentsByOrderId(int orderId);
    public List<PaymentEntity> GetPaymentsByCustomerId(int customerId);
    public bool UpdatePaymentStatus(int paymentId, string newStatus);
    public (int totalPayments, double totalAmount, double averagePayment) GetPaymentStatistics(int customerId);
}
```

### 3. **Enhanced OrderDataAccess Class**
```csharp
public class OrderDataAccess
{
    public Order CreateOrderWithPayment(Order order, Payment payment);
    public List<OrderWithPayment> GetOrderHistoryWithPayments(int customerId);
    public (int totalOrders, double totalSpent, double averageOrderValue) GetOrderStatistics(int customerId);
}
```

---

## Usage Examples

### 1. **Checkout Process**
```csharp
// In CheckoutForm
private void BtnPlaceOrder_Click(object sender, EventArgs e)
{
    // Create order and payment
    var order = new Order();
    order.CreateOrder(customerId, cart);
    
    var payment = CreatePayment(paymentMethod, total);
    
    // Save to database
    var orderDataAccess = new OrderDataAccess();
    var savedOrder = orderDataAccess.CreateOrderWithPayment(order, payment);
    
    if (savedOrder != null)
    {
        MessageBox.Show($"Order #{savedOrder.OrderID} placed successfully!");
    }
}
```

### 2. **Viewing Order History**
```csharp
// In OrderHistoryForm
private void LoadOrderHistory()
{
    var orderDataAccess = new OrderDataAccess();
    var orderHistory = orderDataAccess.GetOrderHistoryWithPayments(customerId);
    
    foreach (var orderWithPayment in orderHistory)
    {
        var order = orderWithPayment.Order;
        var payment = orderWithPayment.Payment;
        
        // Display order information
        lstOrderHistory.Items.Add($"Order #{order.OrderID} - £{order.TotalAmount:F2}");
    }
}
```

### 3. **Payment Tracking**
```csharp
// Get payment statistics
var paymentDataAccess = new PaymentDataAccess();
var stats = paymentDataAccess.GetPaymentStatistics(customerId);

Console.WriteLine($"Total Payments: {stats.totalPayments}");
Console.WriteLine($"Total Amount: £{stats.totalAmount:F2}");
Console.WriteLine($"Average Payment: £{stats.averagePayment:F2}");
```

---

## Testing the Integration

### 1. **Run Database Tests**
```csharp
// Test complete database functionality
DatabaseDemo.RunDatabaseTest();

// Test specific payment integration
TestMenuDatabase.TestOrderAndPaymentDatabase();
```

### 2. **Expected Test Output**
```
=== Testing Order and Payment Database Integration ===
Testing with customer: John Doe and item: Margherita Pizza
✓ Order saved to database with ID: 1001
✓ Order retrieved from database: Order #1001 - 2 items - £24.99 - Pending
✓ Payment retrieved from database: 1 payment(s)
  - Payment ID: 1001
  - Amount: £24.99
  - Method: Cash
  - Status: Completed
  - Transaction ID: A1B2C3D4E5F6G7H8
✓ Order history retrieved: 3 orders
✓ Order statistics: 3 orders, £74.97 total spent
```

---

## Benefits of Database Integration

### 1. **Data Persistence**
- Orders survive application restarts
- Payment history is maintained
- Complete audit trail available

### 2. **Real-time Information**
- Order status updates immediately
- Payment confirmations are instant
- History is always current

### 3. **Business Intelligence**
- Order statistics and analytics
- Payment method preferences
- Customer spending patterns

### 4. **Scalability**
- Multiple users can place orders
- Concurrent payment processing
- Database handles large volumes

---

## Migration Notes

### 1. **Database Schema**
- New tables are created automatically
- Existing data is preserved
- No data loss during upgrade

### 2. **Code Changes Required**
- Update form constructors to pass customerId
- Use new database methods instead of hardcoded data
- Handle database exceptions appropriately

### 3. **Testing**
- Run database tests to verify functionality
- Test order placement and retrieval
- Verify payment processing works

---

## Troubleshooting

### 1. **Common Issues**
- **Connection errors**: Check SQL Server connection string
- **Missing tables**: Ensure migrations are run
- **Data not saving**: Verify database permissions

### 2. **Debug Steps**
- Check database connection in `OrderingDbContext`
- Verify table creation with SQL Server Management Studio
- Run test methods to isolate issues

### 3. **Performance Considerations**
- Large order histories may need pagination
- Consider indexing on frequently queried fields
- Monitor database performance with large datasets

---

## Future Enhancements

### 1. **Planned Features**
- Payment gateway integration (Stripe, PayPal)
- Email notifications for order updates
- Advanced reporting and analytics
- Customer loyalty program integration

### 2. **Architecture Improvements**
- Async/await for database operations
- Caching for frequently accessed data
- Microservices architecture
- API endpoints for mobile apps

---

## Conclusion

The database integration transforms the Online Ordering System from a demo application to a **production-ready system** that:

- ✅ **Saves real data** instead of simulating operations
- ✅ **Maintains complete history** of all orders and payments
- ✅ **Provides business insights** through data analytics
- ✅ **Scales to handle multiple users** and high order volumes
- ✅ **Ensures data integrity** through proper database constraints

This foundation enables the system to be used in real-world scenarios while maintaining the clean, professional architecture that demonstrates advanced programming concepts.

---

**Next Steps**: 
1. Test the integration with the provided test methods
2. Update any forms that need customerId parameters
3. Verify database operations work correctly
4. Consider implementing additional features like email notifications
