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

namespace WordFun
{
    public partial class LobbyScreen : Form
    {
        private List<string> Messages = new List<string>();
        private SocketHandler ClientSocketHandler;
        public void ParseSocketHandler(SocketHandler SocketHandle)
        {
            Console.WriteLine("HandlerParseStart");
            ClientSocketHandler = SocketHandle;
            ClientSocketHandler.LobbyScreenReference = this;
            Program.SocketHandle = ClientSocketHandler;
            Console.WriteLine("HandlerParseComplete");
        }
        private void MenuButton_Click(object sender, EventArgs e)
        {
            ClientSocketHandler.Disconnect();
            ClientSocketHandler.Dispose();
            Program.NextMenuID = 97;
            this.Close();
        }
        public void AddToTextList(string Message)
        {
            if(InvokeRequired)
            {
                this.Invoke(new Action<string>(AddToTextList), new object[] { Message });
                return;
            }
            Messages.Add(Message);
            try 
            { 
                textBox2.Text = textBox2.Text + Environment.NewLine + " >> " + Message;
            }
            catch (Exception Error) { Console.WriteLine(Error); }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ClientSocketHandler.Send(ClientSocketHandler.ClientName+": "+textBox1.Text);
            textBox1.Text = "";
        }
        public LobbyScreen()
        {
            Program.NextMenuID = 97;
            InitializeComponent();
            this.AutoScroll = true;
        }
        private void LobbyScreen_Load(object sender, EventArgs e)
        {
            Console.WriteLine("Lobby Started");
            try
            {
                ClientSocketHandler.EnterLobby();
                richTextBox1.Text = "Name: "+ ClientSocketHandler.ClientName+" | Your ID:"+ ClientSocketHandler.MyID;
            }
            catch{ Console.WriteLine("EnterLobbyFailed!"); }
        }
        public void EnterGame()
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(EnterGame));
                return;
            }
            Program.NextMenuID = 4;
            this.Close();
        }
    }
}
