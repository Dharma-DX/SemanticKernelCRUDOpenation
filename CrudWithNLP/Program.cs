using CrudWithNLP;
using CrudWithNLP.Plugins;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Planning.Handlebars;



//// Run the web host in a separate task
//var webHostTask = Task.Run(() => { });  
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ToDoDb>(opt => opt.UseInMemoryDatabase("ToDoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();
var todoItems = app.MapGroup("/todoitems");

//get all
todoItems.MapGet("/", async (ToDoDb db) => { 
    var result = await db.Todos.ToListAsync();  
    return Results.Json(result);  
});

//get complete 
todoItems.MapGet("/complete", async (ToDoDb db) => 
{ 
    var result = await db.Todos.Where(x=>x.IsComplete).ToListAsync();
    return Results.Json(result);
});

//get by id
todoItems.MapGet("/{id}", async (int id, ToDoDb db) => 
    await db.Todos.FindAsync(id) is ToDo todo ? Results.Ok(todo) : Results.NotFound());

//create new 
todoItems.MapPost("/", async (ToDo todo, ToDoDb db) => { 
    db.Todos.Add(todo);
    await db.SaveChangesAsync();

    return Results.Created($"/todoitems/{todo.Id}", todo); 
});


//update 
todoItems.MapPut("/{id}", async (int id, ToDo todo, ToDoDb db) => 
{
    var existingtodo = await db.Todos.FindAsync(id);
    if (existingtodo is null) return Results.NotFound(); 

    existingtodo.Name= todo.Name;
    existingtodo.IsComplete= todo.IsComplete;

    await db.SaveChangesAsync();

    return Results.NoContent();
    
});


//delete 
todoItems.MapDelete("/{id}", async (int id, ToDoDb db) => 
{
    if (await db.Todos.FindAsync(id) is ToDo todo)
    {
        db.Todos.Remove(todo); 
        await db.SaveChangesAsync();
        return Results.NoContent(); 
    }
    return Results.NotFound();
});

app.MapPost("/llm", async (LlmInput inputObj) => 
{
    const string azureOpenAIDeploymentName = "";
    const string azureOpenAIEndpoint = "";
    const string azureOpenAIAPIKey = "";
    const string azureOPenAIModelId = "";

    //Create the Semantic Kernel, Kernel
    var KernelBuilder = Kernel.CreateBuilder()
                                .AddAzureOpenAIChatCompletion(azureOPenAIModelId, azureOpenAIEndpoint, azureOpenAIAPIKey);

    KernelBuilder.Plugins.AddFromType<ToDoPlugin>();

    Kernel kernel = KernelBuilder.Build();
    Console.Write("Kernel Created");

    var arguments = new KernelArguments();

    //7. Enable auto function calling
    OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
    {
        ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
    };

    //8. Create planner 
#pragma warning disable SKEXP0003, SKEXP0011, SKEXP0052, SKEXP0060
    var planner = new HandlebarsPlanner();

    Func<string, Task> Chat = async (string input) =>
    {
        arguments["input"] = input;

        var originalPlan = await planner.CreatePlanAsync(kernel, input);

        Console.WriteLine("Original plan:\n");
        Console.WriteLine(originalPlan);


        //7. Execute the Original plan 
        var originalPlanResult = await originalPlan.InvokeAsync(kernel, new KernelArguments(openAIPromptExecutionSettings));

        Console.WriteLine("Original Plan results:\n");
        Console.WriteLine(originalPlanResult.ToString());
    };

    await Chat(inputObj.Input);
}); 


app.Run();

////7. Start with a basic chat    
//Console.WriteLine("Enter Query");
//var readUserInput = Console.ReadLine();




//while (true)
//{
    //7. Start with a basic chat    
    //var readUserInput = Console.ReadLine();

    //Func<string, Task> Chat = async (string input) =>
    //{
    //    arguments["input"] = input;

    //    var originalPlan = await planner.CreatePlanAsync(kernel, input);

    //    Console.WriteLine("Original plan:\n");
    //    Console.WriteLine(originalPlan);


    //    //7. Execute the Original plan 
    //    var originalPlanResult = await originalPlan.InvokeAsync(kernel, new KernelArguments(openAIPromptExecutionSettings));

    //    Console.WriteLine("Original Plan results:\n");
    //    Console.WriteLine(originalPlanResult.ToString());
    //};

    //await Chat(readUserInput);
//}




