using SimpleTCP;
using System;
using System.Text;
using System.Windows;
using System.Net;
using System.Net.Sockets;

namespace Hus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SimpleTcpServer server;
        public MainWindow()
        {
            InitializeComponent();
            server = new SimpleTcpServer();
            server.Delimiter = 0x13; //enter
            server.StringEncoder = Encoding.UTF8;
            server.DataReceived += Server_DataRecived;

        }

        private void Server_DataRecived(object sender, SimpleTCP.Message e)
        {
            this.textStatus.Dispatcher.Invoke(() =>
            {
                textStatus.AppendText(e.MessageString);
                e.ReplyLine(string.Format("you said: {0}", e.MessageString));
            });
        }

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            textStatus.AppendText("server starting...\n");
            IPAddress ip;
            if(IPAddress.TryParse(textHost.Text, out ip))  // long.Parse(textHost.Text)
            { }
            else
            {
                textStatus.AppendText("Error TryParse IP address: " + ip + "\n");
            }
            Int32 myPort = Convert.ToInt32(textPort.Text);
            server.Start(ip, myPort);
            buttonStart.IsEnabled = false;
        }

        private void buttonStop_Click(object sender, RoutedEventArgs e)
        {
            textStatus.AppendText("server stopping...\n");
            if (server.IsStarted)
            {
                server.Stop();
                buttonStart.IsEnabled = true;
            }
        }
    }
}
