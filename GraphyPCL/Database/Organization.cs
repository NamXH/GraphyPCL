﻿using System;
using SQLite.Net.Attributes;

namespace GraphyPCL
{
    public class Organization : IIdContainer
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}