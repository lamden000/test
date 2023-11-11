using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Media;

namespace Server
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Connect();
        }
        #region Server
        IPEndPoint IP;
        Socket server;
        List<Socket> clients;
        int ReadyP = 0;
        int SkipP = 0;
        int turn;
        void Connect()
        {
            clients = new List<Socket>();
            IP = new IPEndPoint(IPAddress.Any, 9999);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            server.Bind(IP);
            Thread listen = new Thread
             (() =>
             {
                 try
                 {
                     while (true)
                     {
                         server.Listen(4);
                         Socket client = server.Accept();
                         clients.Add(client);

                         Thread recieve = new Thread(Recieve);
                         recieve.IsBackground = true;
                         recieve.Start(client);
                     }
                 }
                 catch
                 {
                     IP = new IPEndPoint(IPAddress.Any, 9999);
                     server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                 }
             });
            listen.IsBackground = true;
            listen.Start();
        }
        void Close()
        { }
        void Recieve(object obj)
        {
            Socket client = obj as Socket;
            try
            {
                while (true)
                {
                    byte[] data = new byte[1024 * 5000];
                    client.Receive(data);
                    string str = (string)deserialize(data);
                    string[] strings = str.Split("-");
                    //
                    if (strings[0].CompareTo("Ready") == 0)
                    {
                        ReadyP++;
                        if (ReadyP >= clients.Count() && clients.Count()>1)
                        {
                            PhatBai();
                            ReadyP = 0;
                        }
                    }
                    //
                    //
                    else if (strings[0].CompareTo("ThongTinBai") == 0)
                    {
                        if (turn <clients.Count)
                            turn++;
                        else turn = 1;
                        str = str + "-" + turn.ToString();
                        data = serialize(str);
                        foreach (Socket socket in clients)
                            socket.Send(data);
                        SkipP = 0;
                    }

                    //
                    else if (strings[0].CompareTo("Win") == 0)
                    {
                        str = "GameEnd-player " + turn.ToString() + " won";
                        data = serialize(str);
                        foreach (Socket socket in clients)
                            socket.Send(data);
                    }
                    //
                    else if (strings[0].CompareTo("Skip") == 0)
                    {
                        turn++;
                        if (turn > clients.Count)
                            turn = 1;
                        str ="Skip" + "-" + turn.ToString();
                        data = serialize(str);
                        foreach (Socket socket in clients)
                            socket.Send(data);
                        SkipP++;
                        if (SkipP == clients.Count - 1)                  
                        {
                            str = "Skip-0-" + turn.ToString();
                            data = serialize(str);
                            foreach(Socket socket in clients) 
                                socket.Send(data);
                            SkipP = 0;
                        }
                    }
                }
            }
            catch
            {
                clients.Remove(client);
                client.Close();
            }
        }


        byte[] serialize(object o)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, o);
            return ms.ToArray();
        }
        object deserialize(byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            BinaryFormatter bf = new BinaryFormatter();
            return bf.Deserialize(ms);
        }
        void PhatBai()
        {
            int[] cards = new int[53];
            int a;
            string str;
            for (int i = 0; i < 53; i++)
                cards[i] = 1;
            Random random = new Random();
            turn = random.Next(1, clients.Count);
            for (int j = 0; j < clients.Count; j++)
            {
                str = "PhatBai-";
                for (int i = 0; i < 13; i++)
                {
                    a = random.Next(0, 52);
                    if (cards[a] == 1)
                    {
                        str = str + a.ToString() + ",";
                        cards[a] = 0;
                    }
                    else i--;
                }                
                str = str + clients.Count.ToString() + "-" + (j + 1).ToString() + "-" + "0" + "-" + turn.ToString();
                clients[j].Send(serialize(str));
            }
        }
    }
    #endregion
}