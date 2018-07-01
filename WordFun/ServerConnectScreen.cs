using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace WordFun
{
    public partial class ServerConnectScreen : Form
    {
        private bool Connected = false; //Allows the Form to know if it is currently connected to a server
        private SocketHandler ClientSocketHandler = new SocketHandler();
        public ServerConnectScreen()
        {
            Program.NextMenuID = 99;
            InitializeComponent();
        }
        private void MenuButton_Click(object sender, EventArgs e)
        {
            ClientSocketHandler.Disconnect();
            ClientSocketHandler.Dispose();
            Program.NextMenuID = 0;
            this.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!Connected)
            {
                int Port = int.Parse(textBox2.Text);
                try
                {
                    IPAddress Address = IPAddress.Parse(textBox1.Text);
                    try
                    {
                        ClientSocketHandler.Connect(Address, Port);
                        Connected = true;
                        Random rnd = new Random();
                        string Name = textBox3.Text;
                        if (Name == "") { Name = "IMDUMB" + rnd.Next(1, 51); }
                        ClientSocketHandler.Send(Name);
                        ClientSocketHandler.ClientName = Name;
                        richTextBox1.Text = "Connected!";
                        button1.Text = "Enter\nLobby";
                        richTextBox1.Text = "Press the button again to enter the Lobby, Or Return to the main menu to Disconnect from the server.";
                    }
                    catch
                    {
                        Connected = false;
                        richTextBox1.Text = "Failed to Connect! Try Again.";
                        button1.Text = "Connect!";
                    }
                }
                catch
                {
                    richTextBox1.Text = "Invalid Ip Address.";
                }
            }
            else
            {
                Program.NextMenuID = 3;
                Program.SocketHandle = ClientSocketHandler;
                this.Close();
            }
        }

        private void ServerConnectScreen_Load(object sender, EventArgs e)
        {

        }
    }
}
