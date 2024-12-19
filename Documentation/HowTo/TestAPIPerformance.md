# API Performance + Concurrency Testing with Postman

## Common Questions in API Performance Testing
When it comes to API testing, you may have wondered:

1. How will my APIs perform in real-world situations?
2. How will response times change when multiple users send requests simultaneously?
3. Will users experience acceptable response times under load, or will they see errors?
4. How can performance bottlenecks that may become major production issues be identified?

---

## How to Use Postman for API Performance Testing
You can use Postman’s Collection Runner to set up a performance test by following these steps:

### Step 1: Select a Collection and Run
1. Choose a collection.
2. Optionally, select an environment.
3. Click **Run**.

![Step 1](https://voyager.postman.com/gif/june-2023-step-1-how-to-setup-a-run-in-postman.gif)

### Step 2: Configure Performance Settings
1. Navigate to the **Performance** tab in the Runner.
2. Specify load settings such as virtual users, duration, and load profile.
3. Click **Run**.

![Step 2](https://voyager.postman.com/gif/june-2023-step-2-how-to-setup-a-run-in-postman.gif)

### Step 3: Observe Metrics in Real-Time
Postman provides real-time metrics during performance tests:
- **Average Response Time**
- **Requests Per Second**
- **Error Rate**

![Step 3](https://voyager.postman.com/gif/june-2023-step-3-how-to-setup-a-run-in-postman.gif)

---

## Load Configuration for Real-World Traffic Simulation
To simulate real-world traffic, you can specify the following inputs:

- **Virtual Users (VUs):** Maximum number of parallel users.
- **Test Duration:** Duration of the test in minutes.
- **Load Profile:** Intensity of the load during the test. Postman supports steady and step-up load profiles.

![Real-World Traffic Simulation](https://github.com/user-attachments/assets/5d297b6a-cebd-4eb2-bc2e-3acb97f54ec3)

---

## Visualizing Metrics of a Performance Test
Postman provides real-time visualization of the following:

- **Average Response Time:** Mean response time for requests.
- **Requests Per Second (Throughput):** Number of requests served per second.
- **Error Rate:** Percentage of requests that return non-2XX responses or fail.

These metrics are aggregated and visualized over time, helping you identify performance trends and issues.

---

## Troubleshooting Errors
When performance tests indicate elevated error rates:
- Hover over error points to inspect the cause of errors.
- Use Postman’s error breakdown tab to analyze error details.

![Error Breakdown](https://voyager.postman.com/gif/june-2023-error-breakdown-postman.gif)

---

## Case Study: Testing Role Management API
### Observations
- **Role Listing Test:** Passed successfully.
- **Adding a New Role with 20 Users Simultaneously:** Failed due to primary key violations.

### Error Details
- **Inner Exception:** `Violation of PRIMARY KEY constraint 'PK_Roles'. Cannot insert duplicate key in object 'dbo.Roles'. The duplicate key value ...`

![Primary Key Violation](https://github.com/user-attachments/assets/855eefd5-61ec-4022-89c2-fb738bd9ba1d)

### Possible Solutions
1. **Use Auto-Increment Keys or GUIDs:** Simplest and most reliable solution.
2. **Custom ID Generation:** Create a custom ID format such as `YYYY-DD-OrderNum`.
3. **Add Concurrency Control:** Implement locks to prevent simultaneous requests from generating duplicate IDs.

---

## Concurrency Control Solutions
### Application-Level Concurrency Control
#### Using SemaphoreSlim
A `SemaphoreSlim` can restrict access to an endpoint, ensuring only one user can process it at a time.

```csharp
private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

[HttpPost("restricted-endpoint")]
public async Task<IActionResult> RestrictedEndpoint([FromBody] string data)
{
    if (!_semaphore.Wait(0)) // Check if the semaphore is available
    {
        return StatusCode(StatusCodes.Status429TooManyRequests, "Endpoint is in use. Try later.");
    }

    try
    {
        await Task.Delay(5000); // Simulate processing logic
        return Ok("Request processed successfully.");
    }
    finally
    {
        _semaphore.Release();
    }
}
```

![SemaphoreSlim Solution](https://github.com/user-attachments/assets/f7cec675-397d-44f4-9b9a-09b48cf94235)

#### Scalability Considerations
For distributed environments, use distributed locks like:
- **Redis Distributed Lock**
- **SQL-Based Locks**
- **Azure Storage Leases**

---

## Database-Level Concurrency Control
1. **Lock IDs in a Separate Table:** Move ID generation to a separate table with row-level locking.
2. **Use SQL Locks:** Execute queries with explicit locks (`ROWLOCK`, `UPDLOCK`).


![All Working](https://github.com/user-attachments/assets/2eff48e7-c6b0-4ddc-a6af-f25a7b3459ee)


all seem working 

![image](https://github.com/user-attachments/assets/2eff48e7-c6b0-4ddc-a6af-f25a7b3459ee)


 Lock Table/Row When Calculating IDs

![image](https://github.com/user-attachments/assets/4cede6f2-0d3c-4ccb-bfd2-eadd71a522a7)


### flow example
## Handling Concurrency with SQL Locks

This project demonstrates how to handle concurrency when multiple users attempt to perform simultaneous operations that require unique sequential IDs. Below is an example scenario using SQL locking hints (`UPDLOCK, ROWLOCK`) to ensure data consistency during role creation.

### Code Overview

The `AddAsync` method ensures sequential ID generation by:
1. Using SQL locking hints to prevent race conditions.
2. Wrapping the operations in a transaction for atomicity.

Example code snippet:
```csharp
var lastRoleId = await repository.ExecuteSqlSingleAsync<long>(
    "SELECT TOP 1 Id FROM Roles WITH (UPDLOCK, ROWLOCK) ORDER BY Id DESC",
    cancellationToken
);

var newId = lastRoleId + 1;
```

#### Scenario: Two Users Adding Roles at the Same Time
1. **User 1**:
   - Starts a transaction.
   - Locks the row with the highest `Id` using `UPDLOCK` and `ROWLOCK`.
   - Reads the `lastRoleId` and calculates a new ID.
   - Inserts the new role and commits the transaction.
   - Releases the lock.

2. **User 2**:
   - Starts a transaction but is **blocked** because User 1 holds the lock.
   - Once User 1's transaction is committed, User 2 proceeds.
   - Reads the updated `lastRoleId` and calculates a unique new ID.
   - Inserts the new role and commits the transaction.

#### Outcome
- User 1 receives `newId = lastRoleId + 1`.
- User 2 receives `newId = (User 1's newId) + 1`.

This ensures:
- **Sequential IDs**: No duplicate or skipped IDs.
- **Data Consistency**: Transactions are isolated, preventing conflicts.

### Challenges with High Concurrency
While this approach guarantees consistency, high concurrency can lead to:
- Increased contention for the locked row.
- Delays as transactions queue up, waiting for the lock to be released.

### Optimizing for Scalability
To improve performance in high-concurrency environments, consider the following alternatives:

1. **Database Sequence**:
   - Use a database sequence to generate unique IDs without locking rows.
   ```sql
   CREATE SEQUENCE RoleIdSequence START WITH 1 INCREMENT BY 1;
   ```
   - Fetch the next value using `NEXT VALUE FOR RoleIdSequence`.

2. **IDENTITY Column**:
   - Let the database handle ID generation with an auto-increment column.

3. **Reduce Lock Scope**:
   - Minimize the transaction duration to reduce contention.

---

## Best Practices for Concurrency Control
1. **Right Lock Scope:** Prefer row-level locks over table-level locks.
2. **Minimize Lock Duration:** Keep locked sections as short as possible.
3. **Monitor Deadlocks:** Avoid circular dependencies between transactions.

---

## Notes
- **Postman Free Plan:** Limited to 100 virtual users per test.
- **Realistic Traffic Simulation:** Use datasets for simulating diverse requests.
- **Documentation and Automation:** Postman supports generating API documentation and test automation.



## Monitoring with Postman
Postman allows monitoring of APIs with scheduling features. If monitoring fails, Postman can send email alerts. These capabilities enable:
- **Continuous Monitoring:** Detect issues before they escalate.
- **Visualization:** Use charts to track metrics.

![Monitoring Example](https://github.com/user-attachments/assets/834a44c7-5285-4266-a9f9-0cdac2bbce2a)

---
