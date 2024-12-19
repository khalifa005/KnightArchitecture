

When it comes to API testing, you may have wondered some or all of these questions:

How will my APIs perform in real-world situations?

How will the response times change when multiple users are sending requests at the same time?

Will my users see acceptable response times when my system is under load, or will they see errors?

How can I identify performance bottlenecks that may become major production issues?

How to use Postman for API performance testing
You can use Postman’s Collection Runner to set up a performance test in Postman by following these steps:

Step 1: Select a collection, select an environment (optional), and click Run:
<img src="https://voyager.postman.com/gif/june-2023-step-1-how-to-setup-a-run-in-postman.gif" >

from our template and case study we will observe
![image](https://github.com/user-attachments/assets/5d297b6a-cebd-4eb2-bc2e-3acb97f54ec3)


Step 2: Select the Performance tab under Runner, specify the load settings, and click Run:
<img src="https://voyager.postman.com/gif/june-2023-step-2-how-to-setup-a-run-in-postman.gif" >

Step 3: Observe the response times and error rate in real time:

<img src="https://voyager.postman.com/gif/june-2023-step-3-how-to-setup-a-run-in-postman.gif" >

How to configure the load to simulate real-world traffic
You can now use the Collection Runner to simulate real-world traffic. You will be able to specify the following inputs to simulate the load condition:

Virtual users (VUs): The maximum number of parallel users you want to simulate.

Test duration: The amount of time (in minutes) for which you want to run the test.

Load profile: The intensity of the load during the test’s duration. We currently support two load profiles:

Visualizing the metrics of a performance test
As soon as the performance test starts, you will be able to visualize and observe the performance of your APIs. Postman will show the following metrics in real time:

Average response time: This is the average of the response times received for the multiple parallel virtual users across the various requests.

Requests per second: The requests per second (throughput) metric helps you observe how many requests can be served by your API per second. Each virtual user is continuously hitting your endpoints, and depending on the response times, each virtual user can send multiple requests in a second. For example, setting up 10 virtual users to test a GET request that you expect to respond in ~200ms might produce 50 requests per second at best. However, the realistic number of request hits per second will depend on your API’s response time and other various factors, such as the use of pre-request or test scripts.

Error rate: This metric indicates the fraction of the requests that get a non-2XX response or face non-HTTP errors while sending the request.

Note that all of the above metrics are commutative across all your selected requests. Postman aggregates your metrics in short-term intervals. Subsequent metrics from consecutive time intervals are placed together, helping you visualize the changes to these metrics over time.

Troubleshooting errors in your performance test runs
When your performance tests indicate elevated error rates and you would like to know more, you can simply hover over the point of interest and see what’s causing the spike. This helps you identify the cause of the error and troubleshoot the problem further, as shown below:
<img src="https://voyager.postman.com/gif/june-2023-error-breakdown-postman.gif" >
<img src="https://voyager.postman.com/gif/june-2023-errors-tab-in-postman.gif" >


when i used it to test the role listing it pass but when i used it to test adding new role by 20 user at the same time 
keep in mind role table doesn't support auto incemental id so it fail with this error 
Inner Exception: Violation of PRIMARY KEY constraint 'PK_Roles'. Cannot insert duplicate key in object 'dbo.Roles'. The duplicate key value 
![image](https://github.com/user-attachments/assets/855eefd5-61ec-4022-89c2-fb738bd9ba1d)

The simplest and most reliable solution is to use an auto-increment primary key or GUIDs.
but we won't this solution maybe our case needs it to perform like we add th sequesnse id +1 or we format our own sequest like date + id (2025-20-01-OrderNum) 

solutions
 Add Concurrency Control at the Application Level
Use an async lock or a distributed lock mechanism (e.g., Redis or a database-based lock) to prevent simultaneous AddAsync requests from calculating the same Id.
implement concurrency control at the application level in a .NET 8 API to ensure only one user can access an endpoint at a time, you can use a synchronization mechanism such as a SemaphoreSlim or lock. Below is a step-by-step guide to achieve this:

notes 
postman allow 100 user limit for free plain 
all user will used the same machine ip 
and u can used dataset in oher plain to make the test simulate realword examples
and also there is away to generate api documentation - api test and more will post about them later on
visualize data in charts

we can also use it for monotring and if shcdule montioring fail it will send emails
![image](https://github.com/user-attachments/assets/834a44c7-5285-4266-a9f9-0cdac2bbce2a)



//code with concurency issues 
 public async Task<ApiResponse<string>> AddAsync(CreateRoleRequest request, CancellationToken cancellationToken)
 {
   ApiResponse<string>? res = new ApiResponse<string>((int)HttpStatusCode.OK);

   await _unitOfWork.BeginTransactionAsync(cancellationToken);

   try
   {

     var repository = _unitOfWork.Repository<Role>();

     // Use a query to lock the table and get the last ID
     var lastRole = await repository.GetQueryable()
         .OrderByDescending(r => r.Id)
         .AsTracking()
         .FirstOrDefaultAsync(cancellationToken);

     // If no roles exist, start with 1, otherwise increment the last ID
     var newId = (lastRole?.Id ?? 0) + 1;

   //  var lastRole = await repository.ExecuteSqlRawAsync(
   //"SELECT TOP 1 * FROM Roles WITH (ROWLOCK, UPDLOCK) ORDER BY Id DESC", cancellationToken);


     var entity = request.ToEntity();
     entity.Id = newId;

     await repository.AddAsync(entity, cancellationToken: cancellationToken);
     await _unitOfWork.CommitAsync(cancellationToken);

     await _unitOfWork.CommitTransactionAsync(cancellationToken);
     repository.RemoveCache();

     res.Data = entity.Id.ToString();
     return res;
   }
   catch (Exception ex)
   {
     await _unitOfWork.RollBackTransactionAsync(cancellationToken);
     return ex.HandleException(res, _env, _logger);
   }
 }


//usig SemaphoreSlim
Scalability:

this solutions work for a single application instance. If your API is deployed in a distributed environment (e.g., multiple instances), you need a distributed lock mechanism like Redis, SQL-based locks, or Azure Storage Leases.
 private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    [HttpPost("restricted-endpoint")]
    public async Task<IActionResult> RestrictedEndpoint([FromBody] string data)
    {
        if (!_semaphore.Wait(0)) // Immediately check if the semaphore is available
        {
            return StatusCode(StatusCodes.Status429TooManyRequests, "This endpoint is currently in use. Please try again later.");
        }

        try
        {
            // Simulate endpoint processing
            await Task.Delay(5000); // Replace with your actual processing logic
            return Ok("Request processed successfully.");
        }
        finally
        {
            _semaphore.Release();
        }

          [HttpPost("exclusive-access")]
    public async Task<IActionResult> ExclusiveAccess([FromBody] string data)
    {
        await _semaphore.WaitAsync(); // Wait until the semaphore is available

        try
        {
            // Simulate endpoint processing
            await Task.Delay(5000); // Replace with your actual processing logic
            return Ok($"Request processed successfully with data: {data}");
        }
        finally
        {
            _semaphore.Release(); // Release the semaphore for the next request
        }
    }


role aafter adinf app lock
![image](https://github.com/user-attachments/assets/028878ab-aa64-4759-99f0-56a13b7d218c)

as u can see it worked without anu issues 
![image](https://github.com/user-attachments/assets/f7cec675-397d-44f4-9b9a-09b48cf94235)


other app lock solution
Redis Distributed Lock
Background Service	//to set the request in the queue and process it once its time comes up

db solution 
move the table max id to a seperate table and lock it so it doesn't affect orignal table with data 

 Best Practices
Use the Right Lock Scope: Prefer row-level locks unless table-level locks are strictly necessary.
Minimize Lock Duration: Keep the locked section as short as possible. Only perform operations that require locking within the transaction.
Monitor Deadlocks: Ensure that no circular dependencies occur between transactions to avoid deadlocks.

 Lock Table/Row When Calculating IDs

![image](https://github.com/user-attachments/assets/4cede6f2-0d3c-4ccb-bfd2-eadd71a522a7)

all seem working 

![image](https://github.com/user-attachments/assets/2eff48e7-c6b0-4ddc-a6af-f25a7b3459ee)
