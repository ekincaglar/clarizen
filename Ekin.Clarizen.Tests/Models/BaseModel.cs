﻿using System;

namespace Ekin.Clarizen.Tests.Models
{
    public abstract class BaseModel
    {
        public DateTime? DueDate { get; set; } = null;
        public string Id { get; set; }

        public string Name { get; set; }

        public string Parent { get; set; }
        public DateTime? StartDate { get; set; } = null;
    }
}