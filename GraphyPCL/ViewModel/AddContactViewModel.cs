using System;

namespace GraphyPCL
{
    public class AddContactViewModel
    {
        public Contact Contact { get; set; }

        public AddContactViewModel()
        {
            Contact = new Contact();
        }
    }
}

