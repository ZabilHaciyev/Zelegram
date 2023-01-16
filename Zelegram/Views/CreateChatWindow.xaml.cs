using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Zelegram.Models;

namespace Zelegram.Views
{
    /// <summary>
    /// Interaction logic for CreateChatWindow.xaml
    /// </summary>
    public partial class CreateChatWindow : Window
    {
        public User CurrentUser { get; set; }
        public CreateChatWindow(User currentUser)
        {
            CurrentUser = currentUser;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var users = new List<User>();
            


            //using (var db = new ZelegramDb())
            //{
            //    var _currentUser = db.Users.FirstOrDefault(x => x.UserName == CurrentUser.UserName);
            //    var _addedUser = db.Users.FirstOrDefault(x => x.UserName == AddedUserName.Text);

            //    users.Add(_currentUser);
            //    users.Add(_addedUser);

            //    db.Chats.Add(new Models.Chat()
            //    {
            //        Users = new List<Models.User>(users)

            //    }); ;

            //    db.SaveChanges();
            //}
            this.Close();
        }
    }
}
