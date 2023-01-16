using System;
using System.Collections.Generic;
using System.Text;

namespace Zelegram.Models
{
    public  enum SendType
    {
        CreateUser, GetAllUsers, UpdateUser, DeleteUser,Finduser,
        CreateChat, UpdateChat,DeleteChat, GetChats, AddMessage, GetUsersByName,GetChatsForUser
    }
}
