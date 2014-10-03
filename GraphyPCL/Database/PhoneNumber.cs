﻿using System;
using SQLite.Net.Attributes;

namespace GraphyPCL
{
    public class PhoneNumber : IIdContainer, IContactIdRelated
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        public string Type { get; set; }

        public string Number { get; set; }

        public Guid ContactId { get; set; }
    }
}