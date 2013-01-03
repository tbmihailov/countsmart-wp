using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using CountSmart.Messages;
using GalaSoft.MvvmLight.Messaging;

namespace CountSmart
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void appBarButtonClear_Click(object sender, EventArgs e)
        {
            var clearMessage = new ClearCounterValueMessage();
            Messenger.Default.Send(clearMessage);
        }

        private void menuItemSendAsEmail_Click(object sender, EventArgs e)
        {
            var sendAsEmailMessage = new SendCounterValueAsEmailMessage();
            Messenger.Default.Send(sendAsEmailMessage);
        }

        private void menuItemSendAsSms_Click(object sender, EventArgs e)
        {
            var sendAsSmsMessage = new SendCounterValueAsSmsMessage();
            Messenger.Default.Send(sendAsSmsMessage);
        }
    }
}