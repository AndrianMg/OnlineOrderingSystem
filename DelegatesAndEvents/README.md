# OrderEventHandler - Delegates, Events & Lambda Expressions

This directory contains a comprehensive implementation of delegates, events, and lambda expressions for the Tasty Eats Online Ordering System.

## Overview

The OrderEventHandler demonstrates advanced C# programming concepts including:
- **Custom Delegates**: Type-safe function pointers for order and payment events
- **Events**: Event-driven programming for order processing
- **Lambda Expressions**: Anonymous functions for event handling and data processing
- **LINQ Integration**: Functional programming with lambda expressions
- **Event Arguments**: Custom event data classes

## Files

### 1. OrderEventHandler.cs
The main implementation file containing:

#### Custom Delegates
```csharp
public delegate void OrderEventHandler(Order order, string eventType);
public delegate void PaymentEventHandler(Payment payment, bool success);
```

#### Event Arguments Classes
- `OrderEventArgs`: Contains order data and event information
- `PaymentEventArgs`: Contains payment data and processing results

#### OrderProcessingService Class
- Event-driven order processing
- Lambda expressions for event handling
- LINQ operations with lambda expressions
- Functional programming examples

#### OrderEventLogger Class
- Logging functionality for order and payment events
- Demonstrates event handling patterns

#### OrderEventHandlerIntegration Class
- Practical integration examples
- Complete order processing workflow
- Payment processing with events
- Order analytics with lambda expressions

### 2. OrderEventHandlerDemo.cs
Demonstration program showing practical usage:

#### Demo Methods
- `RunDemo()`: Complete demonstration of all features
- `DemoBasicOrderProcessing()`: Order creation and processing
- `DemoPaymentProcessing()`: Payment processing with events
- `DemoOrderAnalytics()`: Data analysis with lambda expressions
- `DemoFunctionalProgramming()`: Functional programming examples
- `DemoAdvancedDelegates()`: Advanced delegate usage
- `DemoLINQWithLambda()`: LINQ operations with lambda expressions

## Key Features

### 1. Event-Driven Architecture
```csharp
// Events using custom delegates
public event OrderEventHandler? OrderCreated;
public event OrderEventHandler? OrderStatusChanged;
public event OrderEventHandler? OrderCompleted;

// Events using standard EventHandler pattern
public event EventHandler<OrderEventArgs>? OrderProcessed;
public event EventHandler<PaymentEventArgs>? PaymentProcessed;
```

### 2. Lambda Expressions for Event Handling
```csharp
// Lambda expression for order created event
OrderCreated += (order, eventType) =>
{
    Console.WriteLine($"Order {order.OrderID} {eventType} at {DateTime.Now}");
};

// Lambda expression for payment processed event
PaymentProcessed += (sender, e) =>
{
    var status = e.Success ? "SUCCESS" : "FAILED";
    Console.WriteLine($"Payment {status}: {e.Message}");
};
```

### 3. LINQ with Lambda Expressions
```csharp
// Filter orders by status
public List<Order> GetOrdersByStatus(List<Order> orders, string status)
{
    return orders.Where(order => order.OrderStatus == status)
                .OrderBy(order => order.OrderDate)
                .ToList();
}

// Order statistics with lambda expressions
public (int totalOrders, double totalRevenue, double averageOrderValue) GetOrderStatistics(List<Order> orders)
{
    var totalOrders = orders.Count;
    var totalRevenue = orders.Sum(order => order.TotalAmount);
    var averageOrderValue = orders.Any() ? 
        orders.Average(order => order.TotalAmount) : 0;

    return (totalOrders, totalRevenue, averageOrderValue);
}
```

### 4. Functional Programming Examples
```csharp
// High-value orders using lambda expressions
public List<Order> GetHighValueOrders(List<Order> orders, double threshold = 50.0)
{
    return orders.Where(order => order.TotalAmount > threshold)
                .OrderByDescending(order => order.TotalAmount)
                .ToList();
}
```

## Usage Examples

### Basic Order Processing
```csharp
var integration = new OrderEventHandlerIntegration();
var items = new List<Item> { /* items */ };
var order = integration.ProcessCompleteOrder(1, items);
```

### Payment Processing
```csharp
var success = integration.ProcessPaymentWithEvents("Credit", 25.50);
```

### Order Analytics
```csharp
var orders = /* list of orders */;
integration.DemonstrateOrderAnalytics(orders);
```

## Integration with Main Application

The OrderEventHandler is integrated with the main application through:

1. **MainForm Integration**: Added "Event Demo" button to demonstrate functionality
2. **Thread-Safe Execution**: Demo runs in separate thread to avoid UI blocking
3. **Console Output**: Detailed logging and demonstration output

## Running the Demo

1. Start the application
2. Navigate to the main form
3. Click the "🎯 Event Demo" button
4. Check the console for detailed output

## Benefits

### 1. Loose Coupling
- Events allow components to communicate without direct dependencies
- Order processing is decoupled from UI and logging

### 2. Extensibility
- Easy to add new event handlers without modifying existing code
- New order processing steps can be added through events

### 3. Functional Programming
- Lambda expressions enable concise, readable code
- LINQ operations provide powerful data processing capabilities

### 4. Type Safety
- Custom delegates provide compile-time type checking
- Event arguments ensure proper data passing

### 5. Maintainability
- Clear separation of concerns
- Event-driven architecture is easy to understand and modify

## Advanced Features

### 1. Custom Event Arguments
```csharp
public class OrderEventArgs : EventArgs
{
    public Order Order { get; set; }
    public string EventType { get; set; }
    public DateTime Timestamp { get; set; }
}
```

### 2. Multiple Event Types
- Order creation, status changes, and completion
- Payment processing with success/failure handling
- Custom event types for specific business logic

### 3. Thread-Safe Event Handling
- Events are raised safely with null checking
- Thread-safe event subscription and unsubscription

### 4. Comprehensive Logging
- All events are logged with timestamps
- Detailed event information for debugging

## Best Practices Demonstrated

1. **Event Naming**: Clear, descriptive event names
2. **Null Checking**: Safe event invocation with null-conditional operators
3. **Exception Handling**: Proper error handling in event handlers
4. **Documentation**: Comprehensive XML documentation
5. **Separation of Concerns**: Clear separation between event raising and handling
6. **Type Safety**: Strong typing with custom delegates and event arguments

## Future Enhancements

1. **Async Event Handling**: Support for async event handlers
2. **Event Filtering**: Advanced event filtering capabilities
3. **Event Persistence**: Database storage of event history
4. **Real-time Notifications**: WebSocket integration for real-time updates
5. **Event Analytics**: Advanced analytics on event patterns

This implementation provides a solid foundation for event-driven programming in the Tasty Eats Online Ordering System, demonstrating modern C# programming techniques and best practices. 