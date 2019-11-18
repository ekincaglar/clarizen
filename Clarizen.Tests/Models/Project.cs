using System;
using Ekin.Clarizen.Data;

namespace Clarizen.Tests.Models
{
    public class Project
    {
        public Project()
        {
            
        }

        public Project(bool createId)
        {
            if (createId)
            {
                Id= Guid.NewGuid();
            }
        }
        
        public Guid Id { get; set; }
        public DateTime? DueDate { get; set; } = null;
        public DateTime? StartDate { get; set; } = null;
        public string Description { get; set; }
        public string Name { get; set; }
        
    }
}
