using System;
using Xamarin.Forms;
using System.Diagnostics;

namespace GraphyPCL
{
    public class App : Application
    {
        public App()
        {
            MainPage = new GraphyPCL.MainPage();
        }

        // Pre Xamarin.Forms 1.3
//        public static Page GetMainPage()
//        {
//            return new MainPage();
//        }

        protected override void OnResume()
        {
            if ((DatabaseManager.DbConnection != null) && (UserDataManager.UserData != null))
            {
                UserDataManager.UserData.AppOpenCount++;
                DatabaseManager.DbConnection.Update(UserDataManager.UserData);
            }
        }
    }
}

