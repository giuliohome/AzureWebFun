#load "secret.csx" // const string CosmosSecret = ...
#r "Newtonsoft.Json"

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;

public class TodoItem {
    public string id;
    public string name;
}

public static async Task<IActionResult> Run(HttpRequest req, ILogger log)
{
    log.LogInformation("C# HTTP trigger function processed a request.");

    string name = req.Query["name"];

    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
    dynamic data = JsonConvert.DeserializeObject(requestBody);
    name = name ?? data?.name;
    
    string appver = "v2.2";
    string responseMessage = string.IsNullOrEmpty(name)
                ? $"This HTTP triggered function {appver} executed successfully. Pass a name in the query string or in the request body for a personalized response."
                        : $"Hello, {name}. This HTTP triggered function {appver} executed successfully.";

    try {
        using (CosmosClient cosmosClient = new CosmosClient(CosmosUrl, CosmosSecret))
        {
            Container container = cosmosClient.GetContainer("ToDoList", "Items");
            // Read item from container
            TodoItem todoItemResponse = await container.ReadItemAsync<TodoItem>("key-01", PartitionKey.None);
            responseMessage += $" Cosmos DB item name: {todoItemResponse.name}";
        }
    }
    catch(Exception exc)  {
            responseMessage += $" Exception msg: {exc.Message}";
    }
    
    return new OkObjectResult(responseMessage);
}