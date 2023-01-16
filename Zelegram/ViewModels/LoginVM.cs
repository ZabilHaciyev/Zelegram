using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Zelegram.Messanging;
using Zelegram.Models;
using Zelegram.Services;

namespace Zelegram.ViewModels
{
    public class LoginVM:ViewModelBase
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public Messenger Messenger { get; set; }

        public RelayCommand<object> SignInButtonClickedCommand { get; set; }
        public RelayCommand SignUpButtonClickedCommand { get; set; }


        public LoginVM(Messenger messenger)
        {
            Messenger = messenger;
            SignInButtonClickedCommand = new RelayCommand<object>(SignInButtonClicked);
            SignUpButtonClickedCommand = new RelayCommand(() => {
                Messenger.Send(new ViewMessanging() { 
                    ViewModel = App.Container.GetInstance<RegisterVM>() 
                });
            });
        }
        public void SignInButtonClicked(object parameter)
        {
            Task.Run(async () =>
            {
                var passwordBox = parameter as PasswordBox;
                Password = passwordBox.Password;
                var user = new User()
                {
                    UserName = this.UserName,
                    Password = this.Password
                };
                var data = JsonConvert.SerializeObject(user);
                var str = await ObjectSender.GetResponse(Models.SendType.Finduser, data);
                if (!string.IsNullOrEmpty(str))
                {
                    App.CurrentUser=JsonConvert.DeserializeObject<User>(str);
                    if (App.CurrentUser!=null)
                    {
                    Messenger.Send(new ViewMessanging()
                    {
                        ViewModel = App.Container.GetInstance<MainWindowVM>()
                    });

                    }
                }
            });


         
        }


    }
}
