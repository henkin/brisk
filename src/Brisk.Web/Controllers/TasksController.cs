using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Brisk.Models;
using Brisk.Web.ApiModels;

namespace Brisk.Web.Controllers
{
    // https://app.swaggerhub.com/apis/aweiker/ToDo/1.0.0
    [Route("tasks")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IRepository<TodoTask> _taskRepository;

        public TasksController(IRepository<TodoTask> taskRepository)
        {
            _taskRepository = taskRepository;
        }
        
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<TaskListModel>> Get()
        {
            try
            {
                var tasks = _taskRepository.GetAll();
                return new JsonResult(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(404);
            }               
        }

        // GET /tasks
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult<string> Get(Guid id)
        {
            try
            {
                var task = _taskRepository.GetById(id);
                if (task == null)
                    return StatusCode(400);
                
                return new JsonResult(task);
            }
            catch (Exception ex)
            {
                return StatusCode(404);
            } 
        }

        // POST /tasks
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public ActionResult Post([FromBody] TaskModel todoTask)
        {
            try
            {
                var task = todoTask.ToTodoTask();
                var todoTasks = _taskRepository.Find(l => l.Name == todoTask.Name).ToList();
                if (todoTasks.Any())
                    return StatusCode(409);
                
                _taskRepository.Create(task);
                return CreatedAtAction("Get", new {id = todoTask.Id}, todoTask);
            }
            catch (Exception ex)
            {
                return StatusCode(400);
            } 
        }

        [HttpPut("{id}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public ActionResult Put(Guid id, [FromBody] TaskModel taskModel)
        {
            try
            {
                var todoTask = taskModel.ToTodoTask();
                _taskRepository.Update(todoTask);
                return CreatedAtAction("Get", new {id = taskModel.Id}, todoTask);
            }
            catch (Exception ex)
            {
                return StatusCode(400);
            } 
        }
    }
}
