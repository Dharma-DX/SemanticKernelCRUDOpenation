using Microsoft.SemanticKernel;
using Newtonsoft.Json;
using System.ComponentModel;

namespace CrudWithNLP.Plugins
{
    public class ToDoPlugin
    {
        const string url = "https://localhost:7082/todoitems/";
        

        [KernelFunction, Description("Get all ToDo list items")]
        public static async Task<IEnumerable<ToDo>> GetToDos() 
        {
            IEnumerable<ToDo> ToDos = null; 
            using (var httpClient = new HttpClient())            
            try
            {
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync();
                    if (content != null)
                    {
                        ToDos = JsonConvert.DeserializeObject<IEnumerable<ToDo>>(content);
                    }
                //Console.WriteLine(result);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request exception: {e.Message}");
            }
            return ToDos;
        }

        //[KernelFunction, Description("Get Completed ToDo list items")]
        //public static IEnumerable<ToDo> GetCompletedToDos() 
        //{
        //    yield break;
        //}

        //[KernelFunction, Description("Get a specific ToDo item by ID")]
        //public static ToDo GetToDoById([Description("The id of the ToDo to fetch")] int id)
        //{
        //    return null;
        //}

        //[KernelFunction, Description("Creates a new ToDo Item")]
        //public static ToDo CreateNewToDo([Description("The ToDo item that needs to be created")] ToDo toDo)
        //{
        //    return null; 
        //}

        //[KernelFunction, Description("updates an existing ToDo Item")]
        //public static void UpdateToDo([Description("The id of the ToDo to Update")] int id)
        //{

        //}

        //[KernelFunction, Description("Deletes an existing ToDo Item")]
        //public static void DeleteToDo([Description("The id of the ToDo to Delete")] int id)
        //{

        //}
    }
}
