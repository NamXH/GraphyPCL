using System;
using Xamarin.Forms;

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
    }
}

