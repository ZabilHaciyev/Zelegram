using GalaSoft.MvvmLight.Messaging;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using Zelegram.Models;
using Zelegram.Services;
using Zelegram.ViewModels;

namespace Zelegram
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Container Container { get; set; }
        public static  HttpClient Client = new HttpClient();
        public static User CurrentUser = new User();
        public App()
        {
            Container = new Container();
            Container.RegisterSingleton<Messenger>();
            Container.RegisterSingleton<BaseVM>();
            Container.Register<LoginVM>();
            Container.Register<RegisterVM>();
            Container.Register<CreateChatVM>();
            
            Container.Register<MainWindowVM>();
            Container.Register<KeyChekVM>();

        }
    }
}
