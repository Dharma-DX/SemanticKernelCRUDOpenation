using CrudWithNLP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Newtonsoft.Json;
using System.ComponentModel;

namespace CrudWithNLP.Plugins
{
    public class IdentifyToDoObjectPlugin
    {
        public Kernel _kernel { get; set; }

       

        [KernelFunction]
        [Description("Detects the Task to be done from the user input")]
        [return: Description("A string text that is task to be done")]
        public async Task<string> IdentifyToDoTask([Description("User input to identify the Task to be done from")] string input)
        {
            string azureOpenAIDeploymentName = "";
            string azureOpenAIEndpoint = "";
            string azureOpenAIAPIKey = "";


            var builder = Kernel.CreateBuilder();

            builder.Plugins.AddFromPromptDirectory("./Plugins/CreateToDoPlugin");
            builder.Plugins.AddFromType<ToDoPlugin>();
            builder.Services.AddAzureOpenAIChatCompletion(azureOpenAIDeploymentName,
                azureOpenAIEndpoint
                , azureOpenAIAPIKey);

            var kernel = builder.Build();

            var result = await kernel.InvokePromptAsync($$"""
            Identify the task to be done in this given user input {input} 
            """, new() {
                { "input", input }
            });
            return result.ToString();
        }

        [KernelFunction]
        [Description("Detects the Status of the Task to be done from the user input")]
        [return: Description("A string value that shows the status of the task, either true or false")]
        public async Task<string> IdentifyToDoStatus([Description("User input to identify the Task status from")] string input)
        {

            string azureOpenAIDeploymentName = "";
            string azureOpenAIEndpoint = "";
            string azureOpenAIAPIKey = "";


            var builder = Kernel.CreateBuilder();

            builder.Plugins.AddFromPromptDirectory("./Plugins/CreateToDoPlugin");
            builder.Plugins.AddFromType<ToDoPlugin>();
            builder.Services.AddAzureOpenAIChatCompletion(azureOpenAIDeploymentName,
                azureOpenAIEndpoint
                , azureOpenAIAPIKey);

            var kernel = builder.Build();

            var result = await kernel.InvokePromptAsync($$"""
        Identify the status of the task to be done in this given user input {input}. Possible values are true and false only. Default value is false. Reply with just a "false" if you can not infer the status from the input. 
        """, new() {
            { "input", input }
        });
            return result.ToString();
        }

        [KernelFunction]
        [Description("Creates a ToDo Object from the given inputs. The ToDo Object created must be passed to appropriate methods to save them in the Database.")]
        [return: Description("A ToDo object ")]
        public async Task<ToDo> CreateAToDoObject(
            [Description("The title of the ToDo Task to be created")] string Name,
            [Description("The status of ToDo Task to be created, possible values are true and false")] string IsComplete)
        {
           ToDo _todo = new ToDo();

            _todo.Name = Name;
            bool.TryParse(IsComplete, out bool status); 
            _todo.IsComplete = status;
            return _todo;
        }
    }
}
