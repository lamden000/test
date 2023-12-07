using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Media;
using static System.Windows.Forms.LinkLabel;
using System;

namespace Server
{
    public partial class Server : Form
    {
        public string maphong;
        public int port;
        public Server(int prt = 0)
        {
            InitializeComponent();
            port = prt;
            CheckForIllegalCrossThreadCalls = false;
            Connect();
        }
        #region maphong
        public void Maphong(string s)
        {
            maphong = s;
        }
        public int getport()
        {
            return port;
        }
        #endregion
        #region Server
        IPEndPoint IP;
        Socket server;
        List<Socket> clients;
        //
        int ReadyP = 0;//Số người đã sẵn sàng
        int SkipP = 0; //Số người đã bỏ lượt
        int turn = 0;//Lượt đi
        int[] exitedplayer = new int[4]; //Lưu lại lượi của người đã thoát
        int n = 0;
        int[] skippedPlayer = new int[4];// lưu lại người đã bỏ lượt
        int maxj; // Số người chơi trong game lúc phát bài
        public int dangchoi = 0;//biến để bết phòng đã chơi chưa
        void Connect()
        {
            clients = new List<Socket>();
            IP = new IPEndPoint(IPAddress.Any, port);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            server.Bind(IP);
            string str;
            Thread listen = new Thread
             (() =>
             {
                 try
                 {
                     while (true && clients.Count() <= 4)
                     {
                         server.Listen(4);
                         Socket client = server.Accept();
                         clients.Add(client);

                         Thread recieve = new Thread(Recieve);
                         recieve.IsBackground = true;
                         recieve.Start(client);

                         //Gửi số lượng người chơi hiện tại
                         str = "PlayerNumber-" + clients.Count().ToString();
                         Thread.Sleep(100);
                         SendDataToClients(str);
                         str = "ReadyP-" + ReadyP;
                         Thread.Sleep(100);
                         SendDataToClients(str);
                     }
                 }
                 catch
                 {
                     IP = new IPEndPoint(IPAddress.Any, port);
                     server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                 }
             });
            listen.IsBackground = true;
            listen.Start();
        }
        //
        //Nhận dữ liệu từ người chơi
        public int songuoi()
        {
            return clients.Count();
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

                    //Sẵn sàng
                    if (strings[0].CompareTo("Ready") == 0)
                    {
                        ReadyP++;
                        str = "ReadyP-" + ReadyP;
                        SendDataToClients(str);

                        if (ReadyP >= clients.Count() && clients.Count() > 1)
                        {
                            PhatBai();
                            ReadyP = 0;
                            Reset("skippedPlayer");
                            Reset("exitedplayer");
                            dangchoi = 1;
                        }                   
                    }
                    else
                    {
                        //Thông tin bài được đánh
                        if (strings[0].CompareTo("ThongTinBai") == 0)
                        {
                            SetTurn();
                            str = str + "-" + turn.ToString();                         
                        }

                        // Có người thắng
                        else if (strings[0].CompareTo("Win") == 0)
                        {
                            dangchoi = 0;
                            str = "ReadyP-" + ReadyP;
                            SendDataToClients(str);
                            str = "GameEnd-Người chơi- " + turn.ToString() + "- thắng";                           
                        }
                        // Bỏ lượt
                        else if (strings[0].CompareTo("Skip") == 0)
                        {                                                     
                            skippedPlayer[SkipP] = turn;
                            SkipP++;
                            SetTurn();
                            if (SkipP != clients.Count - 1)
                            {
                                str = "Skip-" + turn.ToString();
                            }
                            else
                            {
                                str = "Skip-0-" + turn.ToString();
                                Reset("skippedPlayer");
                            }
                        }
                        //Yêu cầu khởi tạo lại lượt
                        else if (strings[0].CompareTo("SetTurn") == 0)
                        {                          
                            turn = int.Parse(strings[1]);
                        }
                        //
                        // Phản hồi cho người chơi
                        SendDataToClients(str);
                    }
                }
            }
            catch
            {
                 try
                  {
                    //Sử Lý khi có người thoát game
                    //
                    int indexofclient = clients.IndexOf(client);
                    client.Close();
                    clients.Remove(client);
                    //Khi đang chơi
                    if (dangchoi == 1)
                    {
                        string str;                       
                        exitedplayer[n] = indexofclient + 1;
                        n++;               
                        //Chia lại lượt
                        if (indexofclient + 1 == turn)
                        {
                            SetTurn();
                            str = "SetTurn-" + turn.ToString();
                            SendDataToClients(str);
                        }
                     
                        //Nếu còn một người chơi người đó sẽ thắng
                        Thread.Sleep(100);
                        if (clients.Count == 1)
                        {
                            Reset("skippedPlayer");
                            Reset("exitedPlayer");
                            str = "GameEnd-Bạn Thắng";
                            SendDataToClients(str);
                            dangchoi = 0;
                        }                   
                        for (int i = 0; i < 4; i++)
                            if (indexofclient + 1 == skippedPlayer[i])
                                SkipP--;
                    }
                    //sử lý chung
                    //Thông báo và cấp quyền bắt đầu lại game cho người chơi
                    ReadyP = 0;
                    string s = "ReturnSanSangButton-Có Người Chơi Đã Thoát Hãy Sàng Nếu Bạn Muốn Chơi(Trò chơi sẽ bắt đầu lại nếu tất cả người chơi ấn sẵn sàng)-"+indexofclient;
                    Thread.Sleep(100);
                    SendDataToClients(s);

                    //gửi số người chơi còn lại trong bàn
                    s = "PlayerNumber-" + clients.Count().ToString();
                   SendDataToClients(s);
                }
                catch {  }
            }
        }
        //
        //Serialize & Deserialize
        void SendDataToClients(string data)
        {
            byte[] serializedData = serialize(data);
            foreach (Socket socket in clients)
            {
                socket.Send(serializedData);
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
        //
        //Hàm phát bài
        void PhatBai()
        {
            int[] cards = new int[53];
            int a;
            string str;
            Random random = new Random();
            int min = 53;//Lá bài nhỏ nhất
            //
            //Làm mới bộ bài
            for (int i = 0; i < 53; i++)
                cards[i] = 1;
            //Phát bài
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
                        //tìm lá bài nhỏ nhất và người nhận nó
                        if (a < min)
                        {
                            min = a;
                            turn = j + 1;
                        }
                    }
                    else i--;
                }
                str = str +"-"+ clients.Count.ToString() + "-" + (j + 1).ToString() + "-0";
                clients[j].Send(serialize(str));
            }
            //Gửi lượt = lượt của người chơi có lá bài nhỏ nhất
             Thread.Sleep(100);
            foreach (Socket client in clients)
            {
                str = "SetTurn-" + turn.ToString();
                client.Send(serialize(str));
            }
            //Số người chơi lúc mới phát bài xong
            maxj = clients.Count;
        }
        //
        //Hàm tạo lượt
        void SetTurn()
        {
            turn++;
            for (int i = 0; i < maxj; i++)
            {
                while (turn > maxj || skippedPlayer.Contains(turn) || exitedplayer.Contains(turn))
                {
                    turn++;
                    if (turn > maxj)
                        turn = 1;
                }
            }
        }


        void Reset(string str)
        { 
            switch (str) 
            {
                case ("skippedPlayer"):
                    for (int i = 0; i < 4; i++)
                        skippedPlayer[i] = 0;
                    SkipP = 0;
                    break;
                case ("exitedPlayer"):
                    for (int i = 0; i < 4; i++)
                        exitedplayer[i] = 0;
                    n= 0;
                    break;
            }
        }
    }
    #endregion
}