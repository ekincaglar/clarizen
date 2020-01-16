﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen
{
    public class GetAllResult
    {
        public object Data { get; set; }
        public List<error> Errors { get; set; }

        public GetAllResult()
        {
            this.Errors = new List<error>() { };
        }
    }
}