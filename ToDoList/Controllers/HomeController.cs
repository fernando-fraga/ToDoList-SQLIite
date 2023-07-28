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

        public IActionResult Index() 
        {
            ToDoViewModel todoListviewModel = GetAllTodos();
            return View(todoListviewModel);
        }


        public JsonResult PopularForm (int id) 
        {
            var todo = GetById(id);
            return Json(todo);
        }

        internal ToDo GetById (int id) 
        {
            ToDo todo = new();

            using (SQLiteConnection con = new SQLiteConnection(connectionString)) 
            {
                using (var tableCommand = con.CreateCommand()) 
                {
                    con.Open();
                    tableCommand.CommandText = $"SELECT * FROM Todo Where Id = {id}";
                    
                    using (var reader = tableCommand.ExecuteReader()) 
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            todo.Id = reader.GetInt32(0);
                            todo.Name = reader.GetString(1);
                        }
                        else 
                        {
                            return todo;
                        }
                    };
                }
            }
            return todo;
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
                    };
                }
            }

            return new ToDoViewModel 
            {
                TodoList = todoList
            };

        }

        public IActionResult Insert(ToDo todo)
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

            return RedirectToAction ("Index");
        }


        public IActionResult Update (ToDo todo)
        {
            using (SQLiteConnection con = new SQLiteConnection(connectionString)) {
                using (var tableCommand = con.CreateCommand())
                {
                    con.Open();
                    tableCommand.CommandText = $"UPDATE Todo SET name = '{todo.Name}' where Id = {todo.Id}";

                    try {
                        tableCommand.ExecuteNonQuery();
                    } catch (Exception ex) {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return RedirectToAction("Index");


        }



        [HttpPost]
        public JsonResult Delete (int id)
        {
            using (SQLiteConnection con = new SQLiteConnection(connectionString))
            {
                using (var tableCommand = con.CreateCommand()) 
                {
                    con.Open();
                    tableCommand.CommandText = $"DELETE from Todo where Id = {id}";
                    tableCommand.ExecuteNonQuery();
                }
            }

            return Json(new { });
        }



    }
}