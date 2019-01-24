using System;
using System.Reflection;

namespace Brisk.Models
{
    public abstract class Entity
    {
        public Guid Id { get; set; }

        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        
    }
}