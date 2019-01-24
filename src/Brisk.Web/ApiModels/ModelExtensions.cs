using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Brisk.Models;

namespace Brisk.Web.ApiModels
{
    public static class ModelExtensions
    {
        static ModelExtensions()
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<TaskListModel, TodoTaskList>();
                cfg.CreateMap<TodoTaskList, TaskListModel>();
                cfg.CreateMap<TaskModel, TodoTask>();
                cfg.CreateMap<TodoTask, TaskModel>();
            });
        }

        public static TodoTaskList ToTodoTaskList(this TaskListModel model)
        {
            return Mapper.Map<TodoTaskList>(model);
        }

        public static TodoTask ToTodoTask(this TaskModel model)
        {
            return Mapper.Map<TodoTask>(model);
        }

        public static TodoTask ToTodoTask(this TaskModel model, Guid listId)
        {
            var task = model.ToTodoTask();
            task.TaskListId = listId;
            return task;
        }

        public static TaskListModel FromTodoTaskList(this TodoTaskList list, IEnumerable<TodoTask> todoTasks)
        {
            var model = Mapper.Map<TaskListModel>(list);
            model.Tasks = todoTasks.FromTodoTasks();
            return model;
        }

        public static List<TaskModel> FromTodoTasks(this IEnumerable<TodoTask> todoTasks)
        {
            return todoTasks.Select(Mapper.Map<TaskModel>).ToList();
        }
    }
}