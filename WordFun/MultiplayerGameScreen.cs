using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WordFun
{
    public partial class MultiplayerGameScreen : Form
    {
        public SocketHandler ClientSocketHandler;
        private MultiplayerHandler MultiplayerHandle;
        public MultiplayerGameScreen()
        {
            Program.NextMenuID = 97;
            InitializeComponent();
        }
        public void ParseSocketHandler(SocketHandler SocketHandle)
        {
            ClientSocketHandler = SocketHandle;
            ClientSocketHandler.MultiScreenReference = this;
            Program.SocketHandle = ClientSocketHandler;
        }
        public void CreateGameHandle()
        {
            MultiplayerHandle = new MultiplayerHandler(this);
        }

        private void MultiplayerGameScreen_Load(object sender, EventArgs e)
        {

        }
    }
}
