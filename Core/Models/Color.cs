﻿using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Color: IDeletable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ColorCode { get; set; }
        public bool IsDeleted { get; set; }
    }
}
