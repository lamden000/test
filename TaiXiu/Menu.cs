using System.Net.Sockets;
using System.Net;
using System.Numerics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using Server;
using System.Net.NetworkInformation;
using static System.Windows.Forms.DataFormats;
using System.Diagnostics.Eventing.Reader;
using System.Diagnostics;

namespace TaiXiu
{
    public partial class Form1 : Form
    {
        int stt;
        string kich;
        List<phong> listphong;
        int phongdavao;
        Danhsachphongcho a;
        public delegate void TruyenDuLieu(List<phong> phongs);
        public TruyenDuLieu DSPhong;
        int DangXemDS=0;
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            listphong =new List<phong>();
            Connect();
        }
        bool kttontai(int port)
        {
            foreach(var i in listphong)
            {
                if(i.maphong == port)
                {
                    return true;
                }
            }
            return false;
        }
        void NhanMaPhong(int maphong)
        {
            client.Send(serialize(stt+","+ maphong + "-Ketnoi"));
        }
        #region button
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            kich = stt.ToString() + "-Choingay";
            this.Hide();
            client.Send(serialize(kich));
            
            
        }
        private void button3_Click(object sender, EventArgs e)
        {
            kich = stt.ToString() + "-Taoban";
            this.Hide();
            client.Send(serialize(kich));
        }
        private void button4_Click(object sender, EventArgs e)
        {
            kich = stt.ToString() + "-Chonban";
            client.Send(serialize(kich));
        }
        #endregion
        #region client
        IPEndPoint IP;
        Socket client;
        Thread listen;
        void Connect()
        {
            IP = new IPEndPoint(IPAddress.Parse("127.0.0.2"), 8888);
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            try
            {
                client.Connect(IP);
            }
            catch
            {
                MessageBox.Show("Khong the ket noi", "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            listen = new Thread(Recieve);
            listen.IsBackground = true;
            listen.Start();
        }

        //
        void Recieve()
        {
            while (true)
            {
                try
                {
                    byte[] data = new byte[1024 * 5000];
                    client.Receive(data);
                    string str = (string)deserialize(data);
                    string[] chuoi = str.Split('-');
                    if (chuoi[0]=="STT")
                    {
                        stt = int.Parse(chuoi[1]);
                    }
                    else if (chuoi[0]=="Taoban")
                    {
                        phong a = new phong();
                        a.maphong = int.Parse(chuoi[1]);
                        a.songuoi = 1;                     
                        Game f = new Game(int.Parse(chuoi[1]));
                        this.Hide();
                        f.ShowDialog();
                        this.Show();
                    }
                    else if (chuoi[0]== "Phòng đã đầy")
                    {
                        a.phongday();
                    }
                    else if (chuoi[0]== "Chophep")
                    {
                        a.Close();
                        Game f = new Game(int.Parse(chuoi[1]));
                        this.Hide();
                        f.ShowDialog();
                        this.Show();                       
                    }
                    //Mở danh sách phòng
                    else if (chuoi[0] == "modanhsach")
                    {
                        if (chuoi.Length > 2)
                        {
                            listphong.Clear();
                            for (int i = 1; i < chuoi.Count() - 1; i++)
                            {
                                string[] k = chuoi[i].Split(',');
                                phong h = new phong();
                                h.songuoi = int.Parse(k[1]);
                                h.maphong = int.Parse(k[0]);
                                h.dangchoi = int.Parse(k[2]);
                                listphong.Add(h);
                            }
                            if (DangXemDS==0)
                            {
                                a = new Danhsachphongcho(this);
                                this.Hide();
                                a.TruyenMaPhong = new Danhsachphongcho.Data(NhanMaPhong);
                                DSPhong(listphong);
                                DangXemDS = 1;
                                Thread threading = new Thread(() =>
                                {
                                    a.ShowDialog(); 
                                    if (a.done == -1)
                                        this.Show();
                                });
                                threading.Start();
                            }
                            if(DangXemDS==1)
                            {
                                DSPhong(listphong);
                                Thread.Sleep(2000);
                                if (a.done == 0)
                                    client.Send(serialize(stt.ToString() + "-Chonban"));
                                else
                                    DangXemDS = 0;                                                                                                                    
                            }
                        }
                        else 
                        {
                           MessageBox.Show("Chưa có người chơi nào tạo bàn, hãy ấn 'Tạo bàn' hoặc 'Chơi ngay' nếu bạn muốn chơi"); 
                        }
                    }
                    //
                    else if (chuoi[0] == "choingay")
                    {
                        Game f = new Game(int.Parse(chuoi[1]));
                        this.Hide();
                        f.ShowDialog();
                        this.Show();
                    }
                }
                catch { }
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
        #endregion

        public void UpdateDataFromForm2(int dataFromForm2)
        {
            phongdavao = dataFromForm2;
            string s = stt.ToString()+","+phongdavao.ToString() + "-Ketnoi";
            client.Send(serialize(s));
        }

    }
}