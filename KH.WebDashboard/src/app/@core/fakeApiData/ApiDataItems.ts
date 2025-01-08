// SampleDataItems.ts

export const apiDataItems = [
    {
        id: 1,
        orderId: "ORD1001",
        customerName: "Alice Johnson",
        productCategory: "Electronics",
        productName: "Smartphone",
        quantity: 2,
        originalPrice: 549.99,
        priceAfterDiscount: 499.99,
        orderDate: "2024-10-15T10:45:00",
        status: "Pending",
        shippingAddress: `<div style="font-size: 14px; color: #333; background-color: #e3f2fd; padding: 10px;">
                            123 Main St<br>
                            Springfield, IL
                          </div>`,
        paymentMethod: "Credit Card",
        notes: "Handle with care"
    },
    {
        id: 2,
        orderId: "ORD1002",
        customerName: "John Smith",
        productCategory: "Home Appliances",
        productName: "Air Conditioner",
        quantity: 1,
        originalPrice: 329.99,
        priceAfterDiscount: 299.99,
        orderDate: "2024-10-16T13:30:00",
        status: "Shipped",
        shippingAddress: `<div style="font-size: 14px; color: #333; background-color: #e3f2fd; padding: 10px;">
                            456 Oak St<br>
                            Chicago, IL
                          </div>`,
        paymentMethod: "PayPal",
        notes: "Deliver between 9 AM and 5 PM"
    },
    {
        id: 3,
        orderId: "ORD1003",
        customerName: "Maria Garcia",
        productCategory: "Fashion",
        productName: "Leather Jacket",
        quantity: 1,
        originalPrice: 180.0,
        priceAfterDiscount: 150.0,
        orderDate: "2024-10-17T09:15:00",
        status: "Delivered",
        shippingAddress: `<div style="font-size: 14px; color: #333; background-color: #e3f2fd; padding: 10px;">
                            789 Pine St<br>
                            Austin, TX
                          </div>`,
        paymentMethod: "Cash on Delivery",
        notes: "Gift wrap the item"
    },
    {
        id: 4,
        orderId: "ORD1004",
        customerName: "James Wilson",
        productCategory: "Beauty & Personal Care",
        productName: "Hair Dryer",
        quantity: 1,
        originalPrice: 59.99,
        priceAfterDiscount: 45.99,
        orderDate: "2024-10-18T15:20:00",
        status: "Cancelled",
        shippingAddress: `<div style="font-size: 14px; color: #333; background-color: #e3f2fd; padding: 10px;">
                            321 Maple Ave<br>
                            Denver, CO
                          </div>`,
        paymentMethod: "Credit Card",
        notes: "Customer requested cancellation"
    },
    {
        id: 5,
        orderId: "ORD1005",
        customerName: "Linda Martinez",
        productCategory: "Books",
        productName: "The Great Gatsby",
        quantity: 3,
        originalPrice: 12.99,
        priceAfterDiscount: 10.99,
        orderDate: "2024-10-19T18:45:00",
        status: "Delivered",
        shippingAddress: `<div style="font-size: 14px; color: #333; background-color: #e3f2fd; padding: 10px;">
                            654 Cedar St<br>
                            Miami, FL
                          </div>`,
        paymentMethod: "Credit Card",
        notes: "Leave package at front door"
    },
    {
        id: 6,
        orderId: "ORD1006",
        customerName: "Robert Brown",
        productCategory: "Sports & Outdoors",
        productName: "Yoga Mat",
        quantity: 1,
        originalPrice: 25.0,
        priceAfterDiscount: 20.0,
        orderDate: "2024-10-20T11:00:00",
        status: "Shipped",
        shippingAddress: `<div style="font-size: 14px; color: #333; background-color: #e3f2fd; padding: 10px;">
                            987 Walnut St<br>
                            Seattle, WA
                          </div>`,
        paymentMethod: "PayPal",
        notes: "Deliver on weekend"
    }
];
