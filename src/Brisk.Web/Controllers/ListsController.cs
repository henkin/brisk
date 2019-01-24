using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using NSwag;
using NSwag.Annotations;
using Brisk.Models;
using Brisk.Web.ApiModels;

namespace Brisk.Web.Controllers
{
    // https://app.swaggerhub.com/apis/aweiker/ToDo/1.0.0
    [Route("lists")]
    [ApiController]
    public class ListsController : ControllerBase
    {
        private readonly IRepository<TodoTaskList> _listRepository;
        private readonly IRepository<TodoTask> _taskRepository;

        public ListsController(IRepository<TodoTaskList> listRepository, IRepository<TodoTask> taskRepository)
        {
            _listRepository = listRepository;
            _taskRepository = taskRepository;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<IEnumerable<TaskListModel>> Get(string search, int? skip, int? limit)
        {
            try
            {
                var taskLists = _listRepository.GetAll().ToList();

                // in real, production code, this filtering would be pushed down the the persistence layer queries.
                if (!string.IsNullOrEmpty(search))
                {
                    taskLists = taskLists.Where(l => l.Name == search).ToList();
                }

                if (skip.HasValue) taskLists = taskLists.Skip(skip.Value).ToList();
                if (limit.HasValue) taskLists = taskLists.Take(limit.Value).ToList();

                var tasks = _taskRepository.GetAll().ToList();

                var listsWithTasks = taskLists.Select(list =>
                    list.FromTodoTaskList(tasks.Where(t => t.TaskListId == list.Id))
                ).ToList();
                return new JsonResult(listsWithTasks);
            }
            catch (Exception ex)
            {
                return StatusCode(400, "bad input parameter");
            }
        }

        // GET /lists/5
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult<TaskListModel> Get(Guid id)
        {
            try
            {
                var taskList = _listRepository.GetById(id);
                if (taskList == null) return StatusCode(404, "list not found"); // Not found
                var tasks = _taskRepository.Find(t => t.TaskListId == id).ToList();

                return new JsonResult(taskList.FromTodoTaskList(tasks));
            }
            catch (Exception ex)
            {
                return StatusCode(400, "invalid id supplied");
            }
        }

        // POST /lists
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public ActionResult Post([FromBody] TaskListModel taskList)
        {
            try
            {
                // existing item
                if (_listRepository.Find(l => l.Name == taskList.Name).Any())
                    return StatusCode(409);

                var list = taskList.ToTodoTaskList();
                _listRepository.Create(list);
                var tasks = taskList.Tasks.Select(t => t.ToTodoTask(list.Id)).ToList();
                tasks.ForEach(t => _taskRepository.Create(t));
                taskList.Tasks = tasks.FromTodoTasks();
                return CreatedAtAction("Get", new {id = taskList.Id}, taskList);
            }
            catch (Exception ex)
            {
                return StatusCode(409);
            }
        }
    }
}