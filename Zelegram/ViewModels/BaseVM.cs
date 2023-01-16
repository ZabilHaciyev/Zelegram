using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Zelegram.Messanging;
using Zelegram.Models;

namespace Zelegram.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class BaseVM : ViewModelBase
    {

        public ViewModelBase CurrentVM { get; set; }

        public BaseVM()
        {
            CurrentVM = App.Container.GetInstance<LoginVM>();
            var messenger = App.Container.GetInstance<Messenger>();
            messenger.Register<ViewMessanging>(this, message =>
            {
                CurrentVM = message.ViewModel;
            });
        }

    }
}
