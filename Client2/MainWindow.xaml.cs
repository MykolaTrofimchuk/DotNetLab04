using Client2.ServiceChat2;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ChatLibrary;

namespace Client2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, Client2.ServiceChat2.IChatServiceCallback
    {
        bool IsConnected = false;
        ServiceChat2.ChatServiceClient client;
        int ID;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        void ConnectUser()
        {
            if (!IsConnected)
            {
                client = new ChatServiceClient(new System.ServiceModel.InstanceContext(this));
                ID = client.Connect(tbUserName.Text);
                tbUserName.IsEnabled = false;
                bConnectDiscon.Content = "Disconect";
                IsConnected = true;

                UpdateOnlineUsers();
            }
        }

        void DisconectUser()
        {
            if (IsConnected)
            {
                client.Disconnect(ID);
                client = null;

                tbUserName.IsEnabled = true;
                bConnectDiscon.Content = "Connect";
                IsConnected = false;

                lbUsersOnline.Items.Clear();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (IsConnected)
            {
                DisconectUser();
            }
            else
            {
                ConnectUser();
            }
        }

        public void SendMessageToClient(string message)
        {
            lbChatWindow.Items.Add(message);
            lbChatWindow.ScrollIntoView(lbChatWindow.Items[lbChatWindow.Items.Count - 1]);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DisconectUser();
        }

        private void tbMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (client != null)
                {
                    client.SendMessage(tbMessage.Text, ID);
                    tbMessage.Text = string.Empty;
                }
            }

        }

        public void UpdateOnlineUsers()
        {
            lbUsersOnline.Items.Clear();
            var onlineUsers = client.GetOnlineUsers();
            foreach (var user in onlineUsers)
            {
                lbUsersOnline.Items.Add(user);
            }
        }
    }
}
