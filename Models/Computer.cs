﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFun.Models
{
    public class Computer
    {
        public int ComputerId { get; set; }
        public string Motherboard { get; set; } = "";
        public int? CPUCores { get; set; } = 0;
        public bool HasWifi { get; set; }
        public bool HasLTE { get; set; }
        public DateTime ReleaseDate { get; set; }
        public decimal Price { get; set; }
        public string VideoCard { get; set; } = "";

    }
}
