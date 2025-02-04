using CrudWithNLP.Plugins;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Planning.Handlebars;
using Microsoft.SemanticKernel;
using Newtonsoft.Json;

namespace CrudWithNLP.Controllers
{
    [Route("[controller]/[action]")]
    public class AIController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> NLPChat(string input)
        {
   
            string azureOpenAIAPIKey = "";
            string result = ""; 

            var builder = Kernel.CreateBuilder();

            //builder.Plugins.AddFromPromptDirectory("./Plugins/CreateToDoPlugin");
            builder.Plugins.AddFromType<ToDoPlugin>();
            builder.Plugins.AddFromType<IdentifyToDoObjectPlugin>(); 
            builder.AddOpenAIChatCompletion(
            "gpt-4o-mini",                  // OpenAI Model name
           azureOpenAIAPIKey);     // OpenAI API Key


            var kernel = builder.Build();

            var arguments = new KernelArguments();

            //7. Enable auto function calling
            OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
            };

#pragma warning disable SKEXP0003, SKEXP0011, SKEXP0052, SKEXP0060
            try
            {
                var planner = new HandlebarsPlanner(new HandlebarsPlannerOptions(allowLoops: true));

                arguments["input"] = input;

                var originalPlan = await planner.CreatePlanAsync(kernel, input);

                Console.WriteLine(originalPlan);

                result = await originalPlan.InvokeAsync(kernel, new KernelArguments(openAIPromptExecutionSettings));

                Console.WriteLine(originalPlan);
            }
            catch (JsonReaderException ex)
            {
                
            }
            catch (Exception ex)
            {
            }
            

            return Ok(result);

        }
    }
}
