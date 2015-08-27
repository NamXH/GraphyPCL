using System;
using SQLite.Net.Attributes;

namespace GraphyPCL
{
    public class UserData
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        public int TagSearchCount { get; set; }

        public int TagUsedInSearchCount { get; set; }

        public int RelationshipSearchCount { get; set; }

        public int RelationshipUsedInSearchCount { get; set; }

        public int AllSearchCount { get; set; }

        public int RelationshipNavigationCount { get; set; }

        public int AppOpenCount { get; set; }
    }
}