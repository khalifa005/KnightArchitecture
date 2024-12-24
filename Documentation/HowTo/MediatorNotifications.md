# Using MediatR for Command and Notification Handling in .NET

This repository demonstrates how to use **MediatR** to implement the **CQRS pattern** for handling commands and notifications in a .NET application. The example shows how to create a user and send notifications (SMS and email) after the user is successfully created.

## Features
- Implements **CQRS** using MediatR.
- Decouples business logic from notification logic.
- Demonstrates the use of domain events and event handlers.

---

## Table of Contents
- [Overview](#overview)
- [Technologies Used](#technologies-used)
- [Setup and Installation](#setup-and-installation)
- [Implementation Details](#implementation-details)
  - [Command: CreateUserCommand](#1-command-createusercommand)
  - [Command Handler](#2-command-handler)
  - [Domain Event: UserCreatedEvent](#3-domain-event-usercreatedevent)
  - [Event Handlers](#4-event-handlers)
- [How to Run](#how-to-run)
- [Extending the Example](#extending-the-example)
- [License](#license)

---

## Overview

The project demonstrates how to:
1. Use **MediatR** for in-process messaging.
2. Handle commands (e.g., `CreateUserCommand`) to perform actions such as creating a user.
3. Use notifications (e.g., `UserCreatedEvent`) to notify other parts of the system (e.g., send SMS and email).

---

## Technologies Used

- .NET 8
- MediatR
- Dependency Injection
- Clean Architecture Principles

---

## Setup and Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/mediatr-cqrs-example.git
   cd mediatr-cqrs-example
   ```

2. Install dependencies:
   ```bash
   dotnet restore
   ```

3. Run the application:
   ```bash
   dotnet run
   ```

---

## Implementation Details

### 1. Command: `CreateUserCommand`

The `CreateUserCommand` encapsulates the data required to create a user.

```csharp
public class CreateUserCommand : IRequest<Guid> // Returns the created user's ID
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}
```

---

### 2. Command Handler

The `CreateUserCommandHandler` is responsible for processing the command. It creates a user and publishes a domain event (`UserCreatedEvent`) after the user is created.

```csharp
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _userRepository; // For saving user to DB
    private readonly IMediator _mediator; // To publish events

    public CreateUserCommandHandler(IUserRepository userRepository, IMediator mediator)
    {
        _userRepository = userRepository;
        _mediator = mediator;
    }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Create the user and save to DB
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Username = request.Username,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber
        };
        
        await _userRepository.AddAsync(user);

        // Publish a domain event after the user is created
        var userCreatedEvent = new UserCreatedEvent(user.Id, user.Email, user.PhoneNumber);
        await _mediator.Publish(userCreatedEvent);

        return userId;
    }
}
```

---

### 3. Domain Event: `UserCreatedEvent`

The `UserCreatedEvent` is a notification that is published when a user is created.

```csharp
public class UserCreatedEvent : INotification
{
    public Guid UserId { get; }
    public string Email { get; }
    public string PhoneNumber { get; }

    public UserCreatedEvent(Guid userId, string email, string phoneNumber)
    {
        UserId = userId;
        Email = email;
        PhoneNumber = phoneNumber;
    }
}
```

---

### 4. Event Handlers

**Handler for Sending Email:**
```csharp
public class SendEmailHandler : INotificationHandler<UserCreatedEvent>
{
    private readonly IEmailService _emailService;

    public SendEmailHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        var subject = "Welcome to the platform!";
        var body = $"Hello, your account has been created. Your user ID is {notification.UserId}.";
        await _emailService.SendEmailAsync(notification.Email, subject, body);
    }
}
```

**Handler for Sending SMS:**
```csharp
public class SendSmsHandler : INotificationHandler<UserCreatedEvent>
{
    private readonly ISmsService _smsService;

    public SendSmsHandler(ISmsService smsService)
    {
        _smsService = smsService;
    }

    public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        var message = $"Welcome! Your account has been created. Your user ID is {notification.UserId}.";
        await _smsService.SendSmsAsync(notification.PhoneNumber, message);
    }
}
```

---

## How to Run

1. Trigger the `CreateUserCommand` in your application:
   ```csharp
   var command = new CreateUserCommand
   {
       Username = "JohnDoe",
       Email = "johndoe@example.com",
       PhoneNumber = "1234567890"
   };

   var userId = await _mediator.Send(command);
   ```

2. Observe that:
   - The user is created and saved to the database.
   - An email notification is sent.
   - An SMS notification is sent.

---

## Extending the Example

1. **Add Push Notifications:**
   - Create a new handler for `UserCreatedEvent` to send push notifications.

2. **Background Processing:**
   - Offload email/SMS sending to a background worker using tools like **Hangfire** or **Azure Functions**.

3. **Error Handling:**
   - Implement retries or a failure-handling mechanism for notifications.

4. **Advanced CQRS:**
   - Separate the write and read models for more complex scenarios.

---

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
