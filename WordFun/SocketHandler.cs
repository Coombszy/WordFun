using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace WordFun
{
    public class SocketHandler
    {
        public TcpClient ClientSocket;
        public NetworkStream DataStream;
        public LobbyScreen LobbyScreenReference;
        public MultiplayerGameScreen MultiScreenReference;
        public bool InLobby = false;
        public string ClientName;
        public int MyID;
        public SocketHandler()
        {
            ClientSocket = new System.Net.Sockets.TcpClient();
        }
        public void Dispose()
        {
            ClientSocket.Close();
            ClientSocket = null;
            Console.WriteLine("CLIENTSOCKETCLOSED");
        }
        public void EnterLobby()
        {
            MyID = int.Parse(Listen());
            Console.WriteLine("MyID=" + MyID);
            InLobby = true;
            Thread LobbyThread = new Thread(LobbyChat);
            LobbyThread.Start();
        }
        private void LobbyChat()
        {
            Send("/INLOBBY");
            string DataReceived;
            while (InLobby)
            {
                try
                {
                    DataReceived = Listen();
                    if (!(DataReceived == ""))
                    {
                        if (!(DataReceived == "/GAMESTART"))
                        {
                            Console.WriteLine("I Received= " + DataReceived);
                            LobbyScreenReference.AddToTextList(DataReceived);
                        }
                        else
                        {
                            InGame();
                            break;
                        }
                    }
                }
                catch {}
            }
            
        }
        public void InGame()
        {
            LobbyScreenReference.EnterGame();
        }
        public void OpenSocket()
        {
            ClientSocket = new System.Net.Sockets.TcpClient();
        }
        public void Connect(IPAddress Ip, int Port)
        {
            ClientSocket.Connect(Ip, Port);
        }
        public void Disconnect()
        {
            Console.WriteLine("Disconnect now!");
            InLobby = false;
            Send("/DISCONNECT");
        }
        public void Send(string ToSend)
        {
            try
            {
                NetworkStream SendStream = ClientSocket.GetStream();
                string data = ToSend;
                Console.WriteLine("ISent=" + data);
                byte[] outStream = System.Text.Encoding.ASCII.GetBytes(data + "$");
                SendStream.Write(outStream, 0, outStream.Length);
                SendStream.Flush();
            }
            catch { }
        }
        public string Listen()
        {
            byte[] BytesReceived = new byte[1280];
            string DataReceived = null;
            while (true)
            {
                try
                {
                    DataStream = ClientSocket.GetStream();
                    if (DataStream.DataAvailable)
                    {
                        DataStream.Read(BytesReceived, 0, BytesReceived.Length);
                        DataReceived = System.Text.Encoding.ASCII.GetString(BytesReceived);
                        DataReceived = DataReceived.Substring(0, DataReceived.IndexOf("$"));
                    }
                    else { DataReceived = ""; }
                    DataStream.Flush();
                    break;
                }
                catch { break; }
            }
            if(DataReceived == "/AREYOUTHERE")
            {
                Console.WriteLine("IGotAREYOUTHEREMessage");
                Send("/IMHERE");
                DataReceived = "";
            }
            return DataReceived;
        }
    }
}
