using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Brisk.Models;

namespace Brisk.Web.ApiModels
{
    public class TaskListModel 
    {
        [Display(Description = "example: 0e2ac84f-f723-4f24-878b-44e63e7ae580")]
        public Guid Id { get; set; }
        [Display(Description = "Must be unique")]
        public string Name { get; set; }
        [Display(Description = "Must be unique")]
        public string Description { get; set; }
        public List<TaskModel> Tasks { get; set; }

        public TaskListModel()
        {
            Id = Guid.NewGuid();
        }

        public override string ToString()
        {
            return $"{Id}: {Name}, {Description}, \n {string.Join("\n ", Tasks)}";
        }
    }
}