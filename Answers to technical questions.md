##### Question 1. How long did you spend on the coding assignment? What would you add to your solution if you had more time? If you didn't spend much time on the coding assignment, then use this as an opportunity to explain what you would add.

Answer 1. I spent approximately 32 to 35 hours. I spent roughly 6 hours reading the APIs documentation and getting familiar with both CoinMarketCap and ExchangeRatesApi payloads and testing their different endpoint to understand their API response. I spent around 6 hours working on project infrastructure to design it in a way that can be testable and extendible.
If I get more time I would focus on the following features addition to this project.

1. Target at least 80 - 85 percent code coverage. The picture shows the current code coverage percentage.

![code_Coverage](https://user-images.githubusercontent.com/10161791/163448624-74a75898-7478-40de-a931-0d66edb5acc2.PNG)

2. Encode appsettings files using Azure key-vault service.
4. Run IHostedService in the background on an interval basis to get the data from Apis then add it to the Cache instead of invoking external api endpoints everytime.
5. Integrate Healthchecks UI, to be able to monitor the uptime of the external dependencies, and to be informed almost immediately on the health of the dependencies.

![image](https://user-images.githubusercontent.com/10161791/163449385-82aafa0d-1e8f-4706-82e9-f05c91b87616.png)

6. Decorate Caching mechanism to speed up the response time.
7. Implement Jason Web Token (JWT) authentication mechanism.


##### Question 2: What was the most useful feature that was added to the latest version of your language of choice? Please include a snippet of code that shows how you've used it.

Answer 2: One of the latest features that Microsoft has introduced in .net core is IHostedService or backgroundService.
it runs in the same application instance and uses the application's lifetime.
```cs
 public Task StartAsync(CancellationToken cancellationToken)
        {

            _logger.LogInformation("{typeName} is starting up in [{serviceConfigurationEnvironment}] environment.", GetType().Name, _serviceConfiguration.Environment);
            _logger.LogInformation("{typeName} v{serviceConfigurationServiceInfoVersion} is starting up in [{serviceConfigurationEnvironment}] environment...",
                GetType().Name, _serviceConfiguration.ServiceInfo.Version, _serviceConfiguration.Environment);

            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    try
                    {
                        _adapter.ToList().ForEach(c => c.Process());
                        await Task.Delay(TimeSpan.FromMilliseconds(_serviceConfiguration.ApplicationSettings.PollFrequencyMs));
                        if (cancellationToken.IsCancellationRequested)
                            break;
                    }
                    catch (Exception e)
                    {
                        _logger.LogError("Error in loop call OnStopped", e);
                        throw;
                    }
                }
                //cleanup
            }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);

            return Task.CompletedTask;
        }
```


##### Question 3 How would you track down a performance issue in production? Have you ever had to do this?
Answer 3: To track down the performance issue in the production the follwoing checklist would be followed on the first attempt.
##### UI Checks in the production environment:
1. Check the network tab in the browser to check if any cdn link is used and that's blocking the css to load.
2. Check if the issue is in a single browser or accross all.
3. Check whether assets are compressed and minified.
4. Check the response payload returned from backend, if it's huge and heavy to process.
5. Check the UI application's logs if any issue is reported. Front end in microservice architecture is deployed separately with its own set of log files.

##### Backend Checks in the production environment::
1. Check application logs if any external service is slowly responding.
2. Check the external dependencies are up and running.
3. Check the database whether it is up and running it is best to perform a simple select statement.

##### Database Checks in the production environment:
1. Check the execution plan if any of the stored procedures or statements are taking too long to return result.
2. Check table has clustered and non-clustered indexes.
3. Check if the sql statement running is very long with alot of joins.

My experience on the performance issue in the production:
After limiting the number of records returned from the DB, it has significantly reduced the load time of the page.


##### Question 4 What was the latest technical book you have read or tech conference you have been to? What did you learn?

I have not specifically read any book recently, but I do read Microsoft documentation regularly to keep up with the enhancements that are happening in the .NET stack. 
I recently read Microsoft documentation on upgrading ASP .Net 4.7 to DotNet 6. It is a big change specially if the service is big with all the dependencies.


##### Question 5 What do you think about this technical assessment?

It’s a wonderful approach to filter out the best candidates.


##### Question 6. Please, describe yourself using JSON.

Answer 6:  


{
"firstname": "Sohail",
"lastname": "Shaghasi",
"age": 30,
"nationality": "Afghanistan",
"livesin": "Malaysia",
"passions": [
"Software development",
"Teaching others"
],
"interests": [
"Swimming",
"Weight lifting"
],
"funfact": 
"able to speak 4 different languages."
}
