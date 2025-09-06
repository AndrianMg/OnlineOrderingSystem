using System;
using System.Collections.Generic;
using OnlineOrderingSystem.Models;

namespace OnlineOrderingSystem.Services
{
    /// <summary>
    /// Core service interface for the Online Ordering System's business operations.
    /// 
    /// This interface defines the contract for all ordering-related business operations,
    /// implementing the Interface Segregation Principle (ISP) by providing a focused
    /// set of methods specifically for order management functionality. It serves as the
    /// primary abstraction layer between the presentation layer and business logic.
    /// 
    /// Design Patterns Implemented:
    /// - Interface Segregation Principle: Focused, cohesive interface design
    /// - Dependency Injection: Enables loose coupling and testability
    /// - Strategy Pattern: Payment processing abstraction
    /// - Repository Pattern: Data access abstraction through service methods
    /// 
    /// Key Responsibilities:
    /// - Menu item retrieval and categorization
    /// - Order creation and management
    /// - Customer data access and validation
    /// - Payment processing and validation
    /// - Order history tracking and retrieval
    /// 
    /// Service Capabilities:
    /// - Comprehensive menu browsing with category filtering
    /// - Robust order placement with validation and error handling
    /// - Flexible payment processing supporting multiple payment methods
    /// - Complete order lifecycle management from creation to completion
    /// - Customer-centric operations with order history tracking
    /// 
    /// Implementation Notes:
    /// - All methods are designed to be stateless and thread-safe
    /// - Exception handling is delegated to the ExceptionHandler service
    /// - Data validation is performed at the service layer before persistence
    /// - Payment processing supports polymorphic payment method handling
    /// 
    /// Usage Example:
    /// <code>
    /// // Dependency injection setup
    /// services.AddScoped&lt;IOrderingService, OrderingService&gt;();
    /// 
    /// // Service usage
    /// var items = orderingService.GetAllItems();
    /// var order = orderingService.PlaceOrder(customerId, itemIds);
    /// var payment = orderingService.CreatePayment("Credit", order.TotalAmount);
    /// var success = orderingService.ProcessPayment(payment);
    /// </code>
    /// </summary>
    public interface IOrderingService
    {
        /// <summary>
        /// Retrieves all available menu items from the system.
        /// 
        /// This method provides access to the complete menu catalog, returning only items
        /// that are currently available for ordering. It serves as the primary method for
        /// menu browsing and is typically used by the presentation layer to populate
        /// menu displays and search functionality.
        /// 
        /// Business Rules:
        /// - Only returns items with Availability = true
        /// - Includes all item details (name, price, description, dietary tags, etc.)
        /// - Results are ordered by category and then by name
        /// - Performance optimized for frequent menu browsing operations
        /// 
        /// Usage Scenarios:
        /// - Initial menu loading in the user interface
        /// - Search functionality across all available items
        /// - Administrative menu management operations
        /// - API endpoints for external integrations
        /// </summary>
        /// <returns>A list of all available menu items. Returns empty list if no items are available.</returns>
        /// <exception cref="DataAccessException">Thrown when database access fails.</exception>
        /// <exception cref="ServiceUnavailableException">Thrown when the service is temporarily unavailable.</exception>
        List<Item> GetAllItems();

        /// <summary>
        /// Retrieves menu items filtered by a specific category.
        /// 
        /// This method provides category-based menu filtering, enabling users to browse
        /// items within specific food categories (e.g., "Main Courses", "Desserts", "Beverages").
        /// It implements efficient filtering at the service layer to optimize performance.
        /// 
        /// Supported Categories:
        /// - "Main Courses": Primary food items (pizzas, burgers, pasta, etc.)
        /// - "Starters": Appetizers and small plates
        /// - "Desserts": Sweet items and treats
        /// - "Beverages": Drinks and refreshments
        /// - "Specials": Featured or seasonal items
        /// 
        /// Business Rules:
        /// - Category matching is case-insensitive
        /// - Only returns available items within the specified category
        /// - Results are ordered by item name within the category
        /// - Returns empty list for invalid or non-existent categories
        /// </summary>
        /// <param name="category">The category name to filter by. Cannot be null or empty.</param>
        /// <returns>A list of available items in the specified category. Returns empty list if category not found.</returns>
        /// <exception cref="ArgumentException">Thrown when category is null or empty.</exception>
        /// <exception cref="DataAccessException">Thrown when database access fails.</exception>
        List<Item> GetItemsByCategory(string category);

        /// <summary>
        /// Creates and places a new order in the system.
        /// 
        /// This is the core business operation that processes customer orders from item
        /// selection through order creation. The method handles the complete order
        /// lifecycle including validation, pricing calculation, and persistence.
        /// 
        /// Order Processing Flow:
        /// 1. Validates customer exists and is active
        /// 2. Retrieves and validates all requested items
        /// 3. Creates order with calculated totals and taxes
        /// 4. Applies business rules and discounts
        /// 5. Persists order to database
        /// 6. Registers order for notification monitoring
        /// 7. Returns complete order details
        /// 
        /// Business Rules:
        /// - Customer must exist and be active
        /// - All item IDs must reference valid, available items
        /// - Minimum order amount validation (if applicable)
        /// - Automatic tax calculation based on location
        /// - Order status initialized to "Pending"
        /// - Payment status initialized to "Pending"
        /// 
        /// Validation Requirements:
        /// - Customer ID must be positive integer
        /// - Item IDs list cannot be null or empty
        /// - All items must be currently available
        /// - Items must have valid pricing information
        /// </summary>
        /// <param name="customerID">The unique identifier of the customer placing the order. Must be positive.</param>
        /// <param name="itemIDs">List of item identifiers to include in the order. Cannot be null or empty.</param>
        /// <returns>A complete Order object with all details populated and persisted.</returns>
        /// <exception cref="ArgumentException">Thrown when customerID is invalid or itemIDs is null/empty.</exception>
        /// <exception cref="InvalidOrderException">Thrown when order validation fails.</exception>
        /// <exception cref="InsufficientInventoryException">Thrown when requested items are unavailable.</exception>
        /// <exception cref="DataAccessException">Thrown when order persistence fails.</exception>
        Order PlaceOrder(int customerID, List<int> itemIDs);

        /// <summary>
        /// Retrieves customer information by unique identifier.
        /// 
        /// This method provides secure access to customer data for order processing,
        /// authentication, and customer service operations. It implements proper
        /// data access patterns and security considerations.
        /// 
        /// Security Considerations:
        /// - Sensitive data (passwords) are not included in returned object
        /// - Access logging for audit trail purposes
        /// - Input validation to prevent injection attacks
        /// - Proper error handling to prevent information disclosure
        /// 
        /// Data Returned:
        /// - Customer identification and contact information
        /// - Address and delivery preferences
        /// - Order history summary (without sensitive details)
        /// - Account status and preferences
        /// 
        /// Performance Notes:
        /// - Optimized database query with proper indexing
        /// - Caching considerations for frequently accessed customers
        /// - Lazy loading for related data to improve performance
        /// </summary>
        /// <param name="customerID">The unique identifier of the customer to retrieve. Must be positive.</param>
        /// <returns>Customer object with complete information, or null if customer not found.</returns>
        /// <exception cref="ArgumentException">Thrown when customerID is invalid.</exception>
        /// <exception cref="DataAccessException">Thrown when database access fails.</exception>
        Customer? GetCustomerById(int customerID);

        /// <summary>
        /// Processes a payment transaction for an order.
        /// 
        /// This method handles the complete payment processing workflow, including
        /// validation, processing, and status updates. It implements the Strategy
        /// Pattern to support multiple payment methods with consistent processing.
        /// 
        /// Payment Processing Flow:
        /// 1. Validates payment object and required fields
        /// 2. Performs payment method-specific validation
        /// 3. Processes payment through appropriate payment gateway
        /// 4. Updates payment status based on processing result
        /// 5. Logs transaction details for audit purposes
        /// 6. Returns processing success status
        /// 
        /// Supported Payment Methods:
        /// - Credit Card: Full validation including CVV and expiry
        /// - Debit Card: PIN validation and account verification
        /// - Cash: Amount verification and change calculation
        /// - Check: Bank validation and clearance processing
        /// - Digital Wallets: Third-party payment integration
        /// 
        /// Security Features:
        /// - PCI DSS compliance for card data handling
        /// - Encryption of sensitive payment information
        /// - Fraud detection and prevention measures
        /// - Secure transaction logging and monitoring
        /// 
        /// Business Rules:
        /// - Payment amount must match order total
        /// - Payment method must be valid and supported
        /// - Customer must have valid payment information
        /// - Transaction must be completed within timeout period
        /// </summary>
        /// <param name="payment">The payment object containing all payment details. Cannot be null.</param>
        /// <returns>True if payment was successfully processed; false if payment failed.</returns>
        /// <exception cref="ArgumentNullException">Thrown when payment parameter is null.</exception>
        /// <exception cref="PaymentProcessingException">Thrown when payment processing fails.</exception>
        /// <exception cref="InvalidPaymentException">Thrown when payment validation fails.</exception>
        /// <exception cref="ServiceUnavailableException">Thrown when payment gateway is unavailable.</exception>
        bool ProcessPayment(Payment payment);

        /// <summary>
        /// Retrieves complete order history for a specific customer.
        /// 
        /// This method provides comprehensive order tracking and history functionality,
        /// enabling customers to view their past orders, track order status, and
        /// access order details for customer service purposes.
        /// 
        /// Data Returned:
        /// - Complete order details including items and quantities
        /// - Order status and payment information
        /// - Order dates and delivery information
        /// - Pricing and tax breakdown
        /// - Order notes and special instructions
        /// 
        /// Ordering and Filtering:
        /// - Orders returned in reverse chronological order (newest first)
        /// - Includes all order statuses (pending, completed, cancelled)
        /// - Optional filtering by date range or status
        /// - Pagination support for large order histories
        /// 
        /// Performance Considerations:
        /// - Optimized queries with proper database indexing
        /// - Lazy loading for order details to improve performance
        /// - Caching for frequently accessed customer data
        /// - Efficient data transfer with minimal network overhead
        /// 
        /// Privacy and Security:
        /// - Customer authentication required
        /// - Access limited to customer's own orders
        /// - Sensitive payment details excluded from results
        /// - Audit logging for data access tracking
        /// </summary>
        /// <param name="customerID">The unique identifier of the customer. Must be positive.</param>
        /// <returns>List of orders for the specified customer. Returns empty list if no orders found.</returns>
        /// <exception cref="ArgumentException">Thrown when customerID is invalid.</exception>
        /// <exception cref="DataAccessException">Thrown when database access fails.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown when access to customer data is denied.</exception>
        List<Order> GetOrderHistory(int customerID);

        /// <summary>
        /// Retrieves list of available payment methods supported by the system.
        /// 
        /// This method provides dynamic payment method configuration, allowing the
        /// system to adapt to different payment options based on business requirements,
        /// regional availability, and customer preferences.
        /// 
        /// Supported Payment Methods:
        /// - "Credit": Credit card payments (Visa, MasterCard, American Express)
        /// - "Debit": Debit card payments with PIN verification
        /// - "Cash": Cash on delivery or pickup payments
        /// - "Check": Check payments with bank verification
        /// - "PayPal": PayPal digital wallet integration
        /// - "Apple Pay": Apple Pay mobile payment integration
        /// - "Google Pay": Google Pay mobile payment integration
        /// 
        /// Configuration Features:
        /// - Dynamic payment method enablement/disablement
        /// - Regional payment method availability
        /// - Customer-specific payment method restrictions
        /// - Business hour-based payment method availability
        /// 
        /// Business Rules:
        /// - Payment methods are validated against business configuration
        /// - Regional restrictions may apply based on customer location
        /// - Some payment methods may have minimum/maximum amount limits
        /// - Payment method availability may change based on order type
        /// </summary>
        /// <returns>List of available payment method names. Returns empty list if no methods available.</returns>
        /// <exception cref="ServiceUnavailableException">Thrown when payment configuration service is unavailable.</exception>
        List<string> GetAvailablePaymentMethods();

        /// <summary>
        /// Creates a payment object based on the specified payment method and amount.
        /// 
        /// This method implements the Factory Pattern to create appropriate payment
        /// objects based on the payment method type. It handles the polymorphic
        /// creation of different payment types while maintaining consistent interface.
        /// 
        /// Payment Object Creation:
        /// - Validates payment method against available options
        /// - Creates appropriate payment type (Credit, Cash, Check, etc.)
        /// - Initializes payment with provided amount and default values
        /// - Sets up payment-specific validation rules
        /// - Configures payment processing parameters
        /// 
        /// Payment Method Mapping:
        /// - "Credit" → Credit payment object with card validation
        /// - "Debit" → Debit payment object with PIN requirements
        /// - "Cash" → Cash payment object with change calculation
        /// - "Check" → Check payment object with bank verification
        /// - "PayPal" → PayPal payment object with API integration
        /// 
        /// Validation and Security:
        /// - Payment method validation against supported options
        /// - Amount validation (positive, within limits)
        /// - Security parameter initialization
        /// - Default value assignment for required fields
        /// 
        /// Business Rules:
        /// - Payment method must be supported and available
        /// - Amount must be positive and within acceptable limits
        /// - Payment object is created in "Pending" status
        /// - Default values are set for payment-specific fields
        /// </summary>
        /// <param name="paymentMethod">The payment method type. Must be a supported payment method.</param>
        /// <param name="amount">The payment amount. Must be positive and within acceptable limits.</param>
        /// <returns>A configured payment object ready for processing.</returns>
        /// <exception cref="ArgumentException">Thrown when paymentMethod is invalid or amount is negative.</exception>
        /// <exception cref="UnsupportedPaymentMethodException">Thrown when payment method is not supported.</exception>
        /// <exception cref="InvalidAmountException">Thrown when amount is outside acceptable limits.</exception>
        Payment CreatePayment(string paymentMethod, double amount);
    }
} 