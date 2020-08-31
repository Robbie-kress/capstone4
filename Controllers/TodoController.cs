using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskList4CapstoneRevisted.Models;

namespace TaskList4CapstoneRevisted.Controllers
{
    public class TodoController : Controller
    {

        private readonly ToDoListDbContext _ToDoListDb;

        public TodoController(ToDoListDbContext ToDoListDb)
        {
            _ToDoListDb = ToDoListDb;
        }
        //CRUD FUNCTIONS

        //Read All the elements of the ToDoList Table
        public IActionResult Index()
        {
            var ToDoList = _ToDoListDb.ToDoList.ToList();
            return View(ToDoList);
        }

        [HttpGet]
        public IActionResult AddToDoList()
        {
            return View();
        }
        [HttpPost] //This method is receiving information from the view (in this case, a Super object)
        public IActionResult AddToDoList(ToDoList toDoList)
        {
            _ToDoListDb.ToDoList.Add(toDoList);
            _ToDoListDb.SaveChanges(); //Needed to save changed made to table.

            return RedirectToAction(nameof(Index)); //Returns the Index view with (updated) List
        }

        public IActionResult DeleteToDoList(int Id)
        {
            //find task we want to delete from the database
            //the Find() will bring back an entire task 

            var foundTask = _ToDoListDb.ToDoList.Find(Id);
            if (foundTask != null)
            {
                //remove the task from database
                _ToDoListDb.ToDoList.Remove(foundTask);
                _ToDoListDb.SaveChanges();
            }
            return RedirectToAction(nameof(Index)); //Returns the Index view with (updated) List
        }

        public IActionResult SeeToDoListByUser(string UserId)
        {
            List<ToDoList> taskDescriptions = _ToDoListDb.ToDoList.Where(x => x.TaskDescription == UserId).ToList();
            ToDoList User = _ToDoListDb.ToDoList.Find(UserId);

            return View(taskDescriptions);
        }

        public IActionResult UpdateToDoList(int id)
        {
            //find the whole super we're looking to update
            ToDoList taskDescription = _ToDoListDb.ToDoList.Find(id);
            if (taskDescription == null)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(taskDescription);
            }
        }
        public IActionResult SaveChanges(ToDoList updatedToDoList)
        {
            ToDoList toDoList = _ToDoListDb.ToDoList.Find(updatedToDoList.ToDoListId);



            //merging the databaseSuper with the updateSuper one property at a time
            //This is to prevent losing information in the databaseSuper that might not
            //but in the updatedSuper
            toDoList.ToDoListId = updatedToDoList.ToDoListId;
            toDoList.TaskDescription = updatedToDoList.TaskDescription;
            toDoList.DueDate = updatedToDoList.DueDate;
            toDoList.Complete = updatedToDoList.Complete;
            toDoList.UserId = updatedToDoList.UserId;
            
            //creates a log of changes for this entry. A way to tracking our work
            _ToDoListDb.Entry(toDoList).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _ToDoListDb.Update(toDoList);
            _ToDoListDb.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
