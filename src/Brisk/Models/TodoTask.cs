using System;
using Brisk.Models;

namespace Brisk.Models
{
    public class TodoTask : Entity
    {
        public Guid TaskListId { get; set; }
        public string Name { get; set; }
        public bool Completed { get; set; }
    }
}