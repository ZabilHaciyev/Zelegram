using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Zelegram.Messanging;
using Zelegram.Models;
using Zelegram.Services;
using Zelegram.Views;

namespace Zelegram.ViewModels
{

    [AddINotifyPropertyChangedInterface]
    public class MainWindowVM : ViewModelBase
    {
        private ObservableCollection<Chat> chats = new ObservableCollection<Chat>();
        
        public Chat SelectedChat { get; set; } = new Chat();
        public ObservableCollection<Chat> Chats {
            get
            {
                if (App.CurrentUser.Chats!=null)
                {
                    return new ObservableCollection<Chat>(App.CurrentUser.Chats.ToList());
                }
                return new ObservableCollection<Chat>();
            }
            set
            {
                chats = value;
            }
        }
        public Chat SelectedChatWithAllMessages { get; set; }
        public ObservableCollection<MessageUC> Messages { get; set; }
        public Messenger Messenger { get; set; }
        public RelayCommand<object> ChatSelectedCommand { get; set; }
        public RelayCommand<object> SendButtonClickedCommand { get; set; }
        public RelayCommand CreatChatButtonClicked { get; set; }
        public RelayCommand ChangePhotoCommand { get; set; }
        public ImageSource ImageSource { get; set; } 
        private string username;
        public string UserName
        {
            get
            {
                if (!string.IsNullOrEmpty(App.CurrentUser.UserName) )
                {
                    return App.CurrentUser.UserName;
                }
                return "";
            }
            set
            {
                username = value;
            }
        }
        public MainWindowVM(Messenger messenger)
        {
            //if (!string.IsNullOrEmpty(App.CurrentUser.UserName))
            //{
            //    byte[] bytes = Encoding.ASCII.GetBytes(App.CurrentUser.Photo);
            //    ImageSource = ConvertByteToBitmapImage.ConvertByteArrayToBitMapImage(bytes);
            //}
            Messenger = messenger;
            DispatcherTimer dt = new DispatcherTimer();
            dt.Tick += GetChats; 
            dt.Interval = new TimeSpan(0, 0, 3);
            dt.Start();
          
            Messages = new ObservableCollection<MessageUC>();
            ChatSelectedCommand = new RelayCommand<object>(ChatSelected);
            SendButtonClickedCommand = new RelayCommand<object>(SendButtonClicked);
            CreatChatButtonClicked = new RelayCommand(() => {
                Messenger.Send(new ViewMessanging()
                {
                    ViewModel = App.Container.GetInstance<CreateChatVM>()
                });
            });
            ChangePhotoCommand = new RelayCommand(ChangeUserPhoto);
        }

        private void ChangeUserPhoto()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter= "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            ofd.ShowDialog();
            var bytes=ConvertImageToByte.GetPhoto(ofd.FileName);
            ImageSource = ConvertByteToBitmapImage.ConvertByteArrayToBitMapImage(bytes);
        }

        private void GetChats(object sender, EventArgs e)
        {
            if (App.CurrentUser!=null)
            {
                if (!string.IsNullOrEmpty(App.CurrentUser.UserName))
                {
                    Task.Run(async () =>
                    {

                        var data = JsonConvert.SerializeObject(App.CurrentUser);
                        var str = await ObjectSender.GetResponse(Models.SendType.GetChats, data);
                        if (!string.IsNullOrEmpty(str))
                        {
                            App.CurrentUser.Chats = JsonConvert.DeserializeObject<List<Chat>>(str);
                            Chats = new ObservableCollection<Chat>(App.CurrentUser.Chats);
                        }

                    });
                }
            }
           
        }

        private void  ChatSelected(object obj)
        {
            var selectedChat = obj as Chat;
            if (selectedChat != null)
            {
                SelectedChat = selectedChat;

                Task.Run(async () =>
                {

                    var data = JsonConvert.SerializeObject(SelectedChat);
                    var str = await ObjectSender.GetResponse(Models.SendType.GetChats, data);
                    if (!string.IsNullOrEmpty(str))
                    {
                         SelectedChatWithAllMessages = JsonConvert.DeserializeObject<Chat>(str);
                    }
                });
                if (SelectedChatWithAllMessages!=null)
                {
                    getAllMessagesCurrentUser();
                    DispatcherTimer dt = new DispatcherTimer();
                    dt.Tick += dispatcherTimer_Tick;
                    dt.Interval = new TimeSpan(0, 0, 1);
                    dt.Start();


                }
            }
        }
        private void SendButtonClicked(object obj)
        {
            var messagetext = obj.ToString();
            if (messagetext != null)
            {
                var msg = new Message()
                {
                    DateTime = DateTime.Now,
                    Text = messagetext,
                    Type = MessageType.Text,
                    UserId = App.CurrentUser.Id

                };
                Messages.Add(new MessageUC() { MessageTxt = messagetext, MessageAllignment=HorizontalAlignment.Right,DateTimeTxt=msg.DateTime }); ;
                if (SelectedChat.Messages==null)
                {
                    SelectedChat.Messages = new List<Message>();
                }
                SelectedChat.Messages.Add(msg);
                Task.Run(async () =>
                {

                    var data = JsonConvert.SerializeObject(SelectedChat);
                    var str = await ObjectSender.GetResponse(Models.SendType.AddMessage, data);
                    if (!string.IsNullOrEmpty(str))
                    {
                        SelectedChat = JsonConvert.DeserializeObject<Chat>(str);
                    }
                });

            }
        }
        private void getAllMessagesCurrentUser()
        {
            var incomingMessages = new List<MessageUC>();
            var outgoingMessages = new List<MessageUC>();

            foreach (var item in SelectedChatWithAllMessages.Messages)
            {
                if (item.UserId==App.CurrentUser.Id)
                {
                    outgoingMessages.Add(new MessageUC() { MessageTxt = item.Text, MessageAllignment = HorizontalAlignment.Right, DateTimeTxt = item.DateTime });

                }
                else incomingMessages.Add(new MessageUC() { MessageTxt = item.Text, MessageAllignment = HorizontalAlignment.Left, DateTimeTxt = item.DateTime });
            }
            incomingMessages.RemoveAll(a => outgoingMessages.Any(b => a.MessageTxt == b.MessageTxt && a.DateTimeTxt == b.DateTimeTxt));
            var allmessages = incomingMessages.Union(outgoingMessages).OrderBy(x => x.DateTimeTxt);
            Messages = new ObservableCollection<MessageUC>(allmessages);
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
           Task.Run(async () =>
           {

               var data = JsonConvert.SerializeObject(SelectedChat);
               var str = await ObjectSender.GetResponse(Models.SendType.GetChats, data);
               if (!string.IsNullOrEmpty(str))
               {
                   SelectedChatWithAllMessages = JsonConvert.DeserializeObject<Chat>(str);
               }
           });
           getAllMessagesCurrentUser();

        }
    }
}
