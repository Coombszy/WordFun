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
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static int NextMenuID = 0;
        public static SocketHandler SocketHandle;
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            NextMenu();
        }
        static void NextMenu()
        {
            switch (NextMenuID)
            {
                case 0:
                    MenuID0();
                    break;
                case 1:
                    MenuID1();
                    break;
                case 2:
                    MenuID2();
                    break;
                case 3:
                    MenuID3();
                    break;
                case 4:
                    MenuID4();
                    break;
                case 97:
                    MenuID97();
                    break;
                case 99:
                    MenuID99();
                    break;
                default:
                    MenuID0();
                    break;
            }

        }
        static void MenuID0()
        {
            SocketHandle = null;
            var Menu = new MenuScreen();
            Application.Run(Menu);
            NextMenu();
        }//Main Menu
        static void MenuID1()
        {
            var GameScreen = new GameScreen();
            GameScreen.CreateGameHandle();
            Application.Run(GameScreen);
            NextMenu();
        }//Debug Menu
        static void MenuID2()
        {
            var ServerScreen = new ServerConnectScreen();
            Application.Run(ServerScreen);
            NextMenu();
        }//Multiplayer Menu
        static void MenuID3()
        {
            var LobbyScreen = new LobbyScreen();
            LobbyScreen.ParseSocketHandler(SocketHandle);
            Application.Run(LobbyScreen);
            NextMenu();
        }//Lobby Menu
        static void MenuID4()
        {
            var MultiplayerGameScreen = new MultiplayerGameScreen();
            MultiplayerGameScreen.ParseSocketHandler(SocketHandle);
            Application.Run(MultiplayerGameScreen);
            NextMenu();
        }//MultiplayerGameMenu
        static void MenuID97()
        {
            SocketHandle.Disconnect();
            SocketHandle.Dispose();
            NextMenuID = 0;
        }//Disconnect
        static void MenuID99()
        {
            Application.Exit();
        }//Terminate Game
    }
}
