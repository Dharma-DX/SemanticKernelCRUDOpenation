using CrudWithNLP.Models;
using Microsoft.SemanticKernel;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Text;

namespace CrudWithNLP.Plugins
{
    public class ToDoPlugin
    {
        const string url = "https://localhost:7082/todo";
        

        [KernelFunction, Description("Get all ToDo list items")]
        public static async Task<IEnumerable<ToDo>> GetToDos() 
        {
            Console.WriteLine($"{url}/GetAll");
            IEnumerable<ToDo> ToDos = null; 
            using (var httpClient = new HttpClient())            
            try
            {
                var response = await httpClient.GetAsync($"{url}/GetAll");
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

        [KernelFunction, Description("Get Completed ToDo list items")]
        public static async Task<IEnumerable<ToDo>> GetCompletedToDos()
        {
            IEnumerable<ToDo> ToDos = null;
            using (var httpClient = new HttpClient())
                try
                {  Console.WriteLine($"{url}/GetAllCompleteStatus");
                    var response = await httpClient.GetAsync($"{url}/GetAllCompleteStatus");
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

        [KernelFunction, Description("Get a specific ToDo item by ID")]
        public static async Task<ToDo> GetToDoById([Description("The id of the ToDo to fetch")] int id)
        {
            var ToDo = new ToDo();
            using (var httpClient = new HttpClient())
                try
                {Console.WriteLine($"{url}/GetOne");
                    var response = await httpClient.GetAsync($"{url}/GetOne/{id}");
                    response.EnsureSuccessStatusCode();
                    string content = await response.Content.ReadAsStringAsync();
                    if (content != null)
                    {
                        ToDo = JsonConvert.DeserializeObject<ToDo>(content);
                    }
                    //Console.WriteLine(result);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Request exception: {e.Message}");
                }
            return ToDo;
        }

        [KernelFunction, Description("Creates a new ToDo Item")]
        public static async Task<string> CreateNewToDo([Description("ToDo Object to create a new ToDo Task with")] ToDo _todo)
        {
            //var model = JsonConvert.DeserializeObject<ToDo>(input);
           Console.WriteLine($"{url}/Create");
            string ToDos = string.Empty;
            using (var httpClient = new HttpClient())
                try
                {
                    var json = JsonConvert.SerializeObject(_todo); // or JsonSerializer.Serialize if using System.Text.Json

                    var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json"); // use MediaTypeNames.Application.Json in Core 3.0+ and Standard 2.1+

                    var response = await httpClient.PostAsync($"{url}/Create", stringContent);
                    response.EnsureSuccessStatusCode();
                    string content = await response.Content.ReadAsStringAsync();
                    if (content != null)
                    {
                        ToDos = content;
                    }
                    //Console.WriteLine(result);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Request exception: {e.Message}");
                    throw;
                }

            return null;
        }

        //[KernelFunction, Description("updates an existing ToDo Item")]
        //public static void UpdateToDo([Description("The id of the ToDo to Update")] int id)
        //{

        //}

        [KernelFunction, Description("Deletes an existing ToDo Item")]
        public static async Task<string> DeleteToDo([Description("The id of the ToDo to Delete")] int id)
        {
            var result = string.Empty;
            using (var httpClient = new HttpClient())
                try
                {  Console.WriteLine($"{url}/Remove");
                    var response = await httpClient.DeleteAsync($"{url}/Remove/{id}");
                    response.EnsureSuccessStatusCode();
                    string content = await response.Content.ReadAsStringAsync();
                    if (content != null)
                    {
                        result = content;
                    }
                    //Console.WriteLine(result);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Request exception: {e.Message}");
                }
            return result;
        }
    }
}
