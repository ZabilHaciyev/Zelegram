using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Zelegram.Messanging;
using Zelegram.Models;
using Zelegram.Services;
using Zelegram.Views;

namespace Zelegram.ViewModels
{
    [AddINotifyPropertyChangedInterface]

    public class RegisterVM:BaseVM
    {
        public Messenger Messenger { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Key { get; set; }
        public RelayCommand<object> SignUpButonClickedCommand { get; set; }
        public RelayCommand SignInButonClickedCommand { get; set; }

        

        public RegisterVM(Messenger messenger)
        {
            Messenger = messenger;
            Random random = new Random();
            Key = random.Next(0, 100000).ToString();
            SignUpButonClickedCommand = new RelayCommand<object>(SignUpButonClicked);
            SignInButonClickedCommand = new RelayCommand(() =>{
                 Messenger.Send(new ViewMessanging(){
                     ViewModel = App.Container.GetInstance<LoginVM>()
                 });
             });
        }
        //void sendEmailNottification()
        //{
        //    MailAddress from = new MailAddress("zrello69@gmail.com", "Zelegram Company");
        //    MailAddress to = new MailAddress(Email);
        //    MailMessage m = new MailMessage(from, to);
        //    m.Subject = "Welcoome";
        //    m.Body = $"<h2>Congrats , Dear {Name} , now you are the member of Zelegram Company </h2><p>Your Key is : {Key}</p>";
        //    m.IsBodyHtml = true;
        //    SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
        //    smtp.Credentials = new NetworkCredential("zrello62@gmail.com", "Zrello");
        //    smtp.EnableSsl = true;
        //    smtp.Send(m);
        //}
        public void SignUpButonClicked(object parameter)
        {
            if (!(string.IsNullOrEmpty(UserName) && string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(Password)))
            {

                Task.Run(async () =>
                {
                    var passwordBox = parameter as PasswordBox;
                    Password = passwordBox.Password;
                    string ImagesPath = System.IO.Directory.GetParent(System.IO.Directory.GetParent(System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).ToString()).ToString()).ToString();
                    ImagesPath += "\\Images\\DefaultUser.jpg";
                    var photo=ConvertImageToByte.GetPhoto(ImagesPath);
                    var user = new User()
                    {
                        UserName = this.UserName,
                        Password = this.Password,
                        Mail = this.Email,
                        Chats = new List<Chat>(),
                        Photo = photo
                    };
                    var data = JsonConvert.SerializeObject(user);
                    var str = await ObjectSender.GetResponse(Models.SendType.CreateUser, data);
                    if (!string.IsNullOrEmpty(str))
                    {
                        MessageBox.Show("User Created Successfully");
                        App.CurrentUser = JsonConvert.DeserializeObject<User>(str);
                        Messenger.Send(new ViewMessanging()
                        {
                            ViewModel = App.Container.GetInstance<MainWindowVM>()
                        });

                        
                    }
                });

               
            }
            else
            {
                // MessageBox.Show("Please Fill all the fields");
                

            }
        }
    }
}
