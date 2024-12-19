#### Test API’s performance by simulating real-world traffic with Postman


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

<img src="" >

<img src="" >
