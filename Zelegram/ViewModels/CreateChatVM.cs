using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Zelegram.Messanging;
using Zelegram.Models;
using Zelegram.Services;

namespace Zelegram.ViewModels
{
    public class CreateChatVM:ViewModelBase
    {
        public RelayCommand BackButtonClicked { get; set; }
        public RelayCommand<object> SearchButtonClicked { get; set; }
        public RelayCommand<object> CreateChatButtonClicked { get; set; }
        public Messenger Messenger { get; set; }
        public List<User> SearchedUsers { get; set; }
        public User SelectedUser { get; set; }
        public string Errormessage { get; set; }
        public CreateChatVM(Messenger messanger)
        {
            Messenger = messanger;
            BackButtonClicked = new RelayCommand(()=> {
                Messenger.Send(new ViewMessanging()
                {
                    ViewModel = App.Container.GetInstance<MainWindowVM>()
                });
            });
            SearchButtonClicked = new RelayCommand<object>(SearchUsers);
            CreateChatButtonClicked = new RelayCommand<object>(CreateChat);
        }
        private void SearchUsers(object param)
        {
            var username = param as string;
            Task.Run(async () =>
            {
                var data = JsonConvert.SerializeObject(username);
                var str = await ObjectSender.GetResponse(Models.SendType.GetUsersByName, data);
                if (!string.IsNullOrEmpty(str))
                {
                    SearchedUsers = JsonConvert.DeserializeObject<List<User>>(str);
                }
            });
        }
        private void CreateChat(object param)
        {
            var chatname = param as string;
            if (!string.IsNullOrEmpty(chatname))
            {
                if (SelectedUser != null)
                {
                    var users = new List<User>();
                    users.Add(App.CurrentUser);
                    users.Add(SelectedUser);
                    var chat = new ChatForSending()
                    {
                        Name = chatname,
                        Users = users
                    };
                    Task.Run(async () =>
                    {
                        var data = JsonConvert.SerializeObject(chat);
                        var str = await ObjectSender.GetResponse(Models.SendType.CreateChat, data);
                        if (!string.IsNullOrEmpty(str))
                        {
                            App.CurrentUser.Chats = JsonConvert.DeserializeObject<List<Chat>>(str);

                        }
                    });
                    Messenger.Send(new ViewMessanging()
                    {
                        ViewModel = App.Container.GetInstance<MainWindowVM>()
                    });
                }
                else Errormessage = "Please ,Select user to contact with him (her) ";
            }
            else Errormessage = "Please ,Enter the name of your new chat  ";

        }
    }
}
