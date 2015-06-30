using System;

namespace GraphyPCL
{
    public class UserDataViewModel
    {
        public UserData UserData { get; set; }

        public UserDataViewModel()
        {
            UserData = UserDataManager.UserData;
        }
    }
}