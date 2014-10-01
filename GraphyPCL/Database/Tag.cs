﻿using System;
using SQLite.Net.Attributes;

namespace GraphyPCL
{
    public class Tag : IIdContainer
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Detail { get; set; }
    }
}