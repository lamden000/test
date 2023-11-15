using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Media;
using static System.Windows.Forms.LinkLabel;
using System;

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
        //
        int ReadyP = 0;//Số người đã sẵn sàng
        int SkipP = 0; //Số người đã bỏ lượt
        int turn = 0;//Lượt đi
        int[] exitedplayer = new int[4]; //Lưu lại lượi của người đã thoát
        int n = 0;
        int maxj; // Số người chơi trong game lúc phát bài
        void Connect()
        {
            clients = new List<Socket>();
            IP = new IPEndPoint(IPAddress.Any, 9999);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            server.Bind(IP);
            string str;
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

                         //Gửi số lượng người chơi hiện tại
                         str = "PlayerNumber-" + clients.Count().ToString();
                         Thread.Sleep(100);
                         foreach (Socket socket in clients) { socket.Send(serialize(str)); };
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
        //
        //Nhận dữ liệu từ người chơi
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
                        if (ReadyP >= clients.Count() && clients.Count() > 1)
                        {
                            PhatBai();
                            ReadyP = 0;
                            n = 0;
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
                            str = "GameEnd-player " + turn.ToString() + " won";
                        }
                        // Bỏ lượt
                        else if (strings[0].CompareTo("Skip") == 0)
                        {
                            SetTurn();
                            if (strings[1].CompareTo("0") == 0)
                                SkipP++;
                            if (SkipP != clients.Count - 1)
                            {
                                str = "Skip-" + turn.ToString();
                            }
                            else
                            {
                                str = "Skip-0-" + turn.ToString();
                                SkipP = 0;
                            }
                        }
                        //Yêu cầu khởi tạo lại lượt
                        else if (strings[0].CompareTo("SetTurn") == 0)
                        {
                            turn = int.Parse(strings[1]);
                        }
                        //
                        // Phản hồi cho người chơi
                        data = serialize(str);
                        foreach (Socket socket in clients)
                            socket.Send(data);
                    }
                }

            }
            catch
            {
                //Sử Lý khi có người thoát game

                exitedplayer[n] = clients.IndexOf(client) + 1;
                n++;
                client.Close();
                clients.Remove(client);              
                //Chia lại lượt                            
                SetTurn();
                string str = "SetTurn-" + turn.ToString();
                foreach (Socket socket in clients)
                    socket.Send(serialize(str));
                //Gửi số lượng người chơi còn lại trong bàn                                                       
                str = "PlayerNumber-" + clients.Count().ToString();
                foreach (Socket socket in clients)
                {
                    socket.Send(serialize(str));
                }
                //Thông báo và cấp quyền bắt đầu lại game cho người chơi
                str = "ReturnSanSangButton-Có Người Chơi Đã Thoát Hãy Sàng Nếu Bạn Muốn Chơi(Trò chơi sẽ bắt đầu lại nếu tất cả người chơi ấn sẵn sàng)";
                foreach (Socket socket in clients)
                    socket.Send(serialize(str));
            
                ReadyP = 0;

            }
        }
        //
        //Serialize & Deserialize
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
                str = str + clients.Count.ToString() + "-" + (j + 1).ToString() + "-0";
                clients[j].Send(serialize(str));
            }
            //Gửi lượt = lượt của người chơi có lá bài nhỏ nhất
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
            for (int i = 0; i < 4; i++)
            {
                if (turn == exitedplayer[i])
                {
                    turn++;
                }
                if (turn > maxj)
                    turn = 1;
            }
        }
    }
    #endregion
}