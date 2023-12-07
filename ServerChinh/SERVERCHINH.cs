using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using Server;
namespace ServerChinh
{
    public partial class SERVERCHINH : Form
    {
        List<phong> listport;
        List<Server.Server> a;
        List<Thread> b;
        public SERVERCHINH()
        {
            InitializeComponent();
            listport = new List<phong>();
            a = new List<Server.Server>();
            b = new List<Thread>();
            CheckForIllegalCrossThreadCalls = false;
            Connect();
        }
        class phong
        {
            public int maphong;
            public int songuoi;
            public int dangchoi;
            public phong(int maphong, int songuoi)
            {
                this.maphong = maphong;
                this.songuoi = songuoi;
                this.dangchoi = 0;
            }
        }
        bool ktport(int stt)
        {
            foreach (var i in a)
            {
                if (i.getport() == stt)
                {
                    return true;
                }
            }
            return false;
        }
        int indexktport(int stt)
        {
            for (int i = 0; i < a.Count(); i++)
            {
                if (a[i].getport() == stt)
                {
                    return i;
                }
            }
            return -1;
        }

        int laystt(Socket s)
        {
            foreach (var i in clients)
            {
                if (s == i.Item1)
                {
                    return i.Item2;
                }
            }
            return -1;
        }
        int laychiso(int stt)
        {
            for (int i = 0; i < clients.Count(); i++)
            {
                if (stt == clients[i].Item2)
                {
                    return i;
                }
            }
            return -1;
        }

        bool ktstt(int stt)
        {
            foreach (var i in clients)
            {
                if (i.Item2 == stt)
                {
                    return true;
                }
            }
            return false;
        }
        #region Server
        IPEndPoint IP;
        Socket server;
        List<(Socket, int)> clients;
        void Connect()
        {
            clients = new List<(Socket, int)>();
            IP = new IPEndPoint(IPAddress.Any, 8888);
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
                            int stt = 1;
                            while (true)
                            {
                                if (ktstt(stt))
                                {
                                    stt++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            client.Send(serialize("STT-" + stt.ToString()));
                            clients.Add((client, stt));

                            Thread recieve = new Thread(Recieve);
                            recieve.IsBackground = true;
                            recieve.Start(client);
                        }
                    }
                    catch
                    {
                        IP = new IPEndPoint(IPAddress.Any, 8888);
                        server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                    }
                });
            listen.IsBackground = true;
            listen.Start();
        }
        public int songuoi()
        {
            return clients.Count();
        }
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
                    string[] s = str.Split('-');
                    //
                    if (s[1].CompareTo("Choingay") == 0)
                    {
                        choingayserver(int.Parse(s[0]));
                    }
                    //
                    else if (s[1].CompareTo("Taoban") == 0)
                    {
                        Taoban(int.Parse(s[0]));
                    }
                    else if (s[1].CompareTo("Chonban") == 0)
                    {
                        dsban(int.Parse(s[0]));
                    }
                    //
                    else if (s[1].CompareTo("Ketnoi") == 0)
                    {
                        string[] k = s[0].Split(',');
                        xinphepketnoi(int.Parse(k[1]), int.Parse(k[0]));
                    }
                }

            }
            catch
            {
                if (laystt(client) == -1)
                {
                    MessageBox.Show("Khong ton tai");
                }
                else
                {
                    int a = 0;
                    clients.Remove((client, a));
                    client.Close();
                }
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
        void Taoban(int client)
        {

            int stt = 1;
            while (true)
            {
                if (ktport(stt))
                {
                    stt++;
                }
                else
                {
                    Server.Server form = new Server.Server(stt);
                    a.Add(form);
                    phong phong = new phong(stt, 1);
                    listport.Add(phong);
                    Thread formThread = new Thread(() =>
                    {
                        a[a.Count() - 1].ShowDialog();
                    });
                    b.Add(formThread);
                    b[b.Count() - 1].Start();

                    break;
                }
            }
            string s = "Taoban-" + stt.ToString();
            clients[laychiso(client)].Item1.Send(serialize(s));
        }
        void dsban(int client)
        {
            string s = "modanhsach-";
            //Cập nhật trạng thái các phòng
            for (int i = 0; i < listport.Count(); i++)
            {
                    listport[i].songuoi = a[i].songuoi();
                    listport[i].dangchoi = a[i].dangchoi;
                    listport[i].maphong = a[i].getport();
            }
        //gửi thông tin các phòng
            for (int i = 0; i < listport.Count(); i++)
            {
                s = s + listport[i].maphong + "," + listport[i].songuoi +","+listport[i].dangchoi+ "-";
            }

            clients[laychiso(client)].Item1.Send(serialize(s));
        }
        void xinphepketnoi(int stt, int client)
        { 
            if (a[indexktport(stt)].songuoi() < 4)
            {
                clients[laychiso(client)].Item1.Send(serialize("Chophep-" + a[indexktport(stt)].getport().ToString()));
                listport[stt - 1].songuoi++;
            }
            else
            {
                clients[laychiso(client)].Item1.Send(serialize("Phòng đã đầy"));
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = clients.Count().ToString();
        }
        void choingayserver(int client)
        {
            int port = -1;
            for (int i = 0; i < listport.Count(); i++)
            {
                if (a[i].songuoi() < 4 && a[i].dangchoi==0)
                {
                    port = a[i].getport();
                    listport[i].songuoi++;
                    break;
                }
            }
            if (port == -1)
            {
                Taoban(client);
            }
            else
            {
                string s = "choingay-" + port.ToString();
                clients[laychiso(client)].Item1.Send(serialize(s));
            }
        }
        // Cập nhật tình trạng các phòng mỗi 2 giây

    }
    #endregion
}