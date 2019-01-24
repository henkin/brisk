using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Brisk.Models;

namespace Brisk.Web.ApiModels
{
    public class TaskModel
    {
        [Display(Description = "example: 0e2ac84f-f723-4f24-878b-44e63e7ae580")]
        public Guid Id { get; set; }
        [Display(Description = "example: 0e2ac84f-f723-4f24-878b-44e63e7ae580")]
        public Guid TaskListId { get; set; }
        [Display(Description = "example: Clean the garage")]
        public string Name { get; set; }
        [Display(Description = "example: true")]
        public bool Completed { get; set; }

        public TaskModel()
        {
            Id = Guid.NewGuid();
        }

        public override string ToString()
        {
            return $"{Id}: {Name}, completed: {Completed}";
        }
    }
}