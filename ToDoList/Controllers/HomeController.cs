using Microsoft.AspNetCore.Mvc;
using System.Data.SQLite;
using System.Diagnostics;
using System.Net.NetworkInformation;
using ToDoList.Models;
using ToDoList.Models.ViewModels;

namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {
        string connectionString = DatabaseHelper.ConnectionString;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger) {
            _logger = logger;
        }

        public IActionResult Index() {
            var todoListviewModel = GetAllTodos();
            return View(todoListviewModel);
        }

        internal ToDoViewModel GetAllTodos() 
        {
            List<ToDo> todoList = new();
            
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                using (var tableCommand = con.CreateCommand())
                {
                    con.Open();
                    tableCommand.CommandText = "SELECT * FROM Todo";

                    using (var reader = tableCommand.ExecuteReader()) 
                    {
                        if (reader.HasRows)
                        {
                            while(reader.Read()) 
                            {
                                todoList.Add(new ToDo {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1)
                                });
                            }
                        }
                        else 
                        {
                            return new ToDoViewModel {
                                TodoList = todoList
                            };
                        }
                    }
                }
            }

            return new ToDoViewModel {
                TodoList = todoList
            };

        }

        public RedirectResult Insert(ToDo todo)
        {

            using (SQLiteConnection con = new SQLiteConnection(connectionString)) 
            {
                using (var tableCommand = con.CreateCommand()) 
                {
                    con.Open();
                    tableCommand.CommandText = $"INSERT INTO Todo (name) VALUES ('{todo.Name}')";

                    try
                    {
                        tableCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            return Redirect("");
        }
    }
}