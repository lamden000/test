using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Resources.Extensions;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using TaiXiu.Properties;

namespace TaiXiu
{
    #region class
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Connect();
        }

        class Card
        {
            int value;
            int typeid;
            Image image;
            public Card(int V, int T, Image I)
            {
                value = V;
                typeid = T;
                image = I;
            }
            public Card()
            {
                value = 0;
                typeid = 0;
                image = Image.FromFile("b1fv.png");
            }
            public Image getimg()
            {
                return image;
            }
            public int getValue() { return value; }
            public int getTypeid() { return typeid; }
        }
        #endregion
        #region 52 cards definition
        Card c2C = new Card(16, 2, Image.FromFile("2C.png"));
        Card c2S = new Card(16, 1, Image.FromFile("2S.png"));
        Card c2D = new Card(16, 3, Image.FromFile("2D.png"));
        Card c2H = new Card(16, 4, Image.FromFile("2H.png"));
        Card c3C = new Card(3, 2, Image.FromFile("3C.png"));
        Card c3S = new Card(3, 1, Image.FromFile("3S.png"));
        Card c3D = new Card(3, 3, Image.FromFile("3D.png"));
        Card c3H = new Card(3, 4, Image.FromFile("3H.png"));
        Card c4C = new Card(4, 2, Image.FromFile("4C.png"));
        Card c4S = new Card(4, 1, Image.FromFile("4S.png"));
        Card c4D = new Card(4, 3, Image.FromFile("4D.png"));
        Card c4H = new Card(4, 4, Image.FromFile("4H.png"));
        Card c5C = new Card(5, 2, Image.FromFile("5C.png"));
        Card c5S = new Card(5, 1, Image.FromFile("5S.png"));
        Card c5D = new Card(5, 3, Image.FromFile("5D.png"));
        Card c5H = new Card(5, 4, Image.FromFile("5H.png"));
        Card c6C = new Card(6, 2, Image.FromFile("6C.png"));
        Card c6S = new Card(6, 1, Image.FromFile("6S.png"));
        Card c6D = new Card(6, 3, Image.FromFile("6D.png"));
        Card c6H = new Card(6, 4, Image.FromFile("6H.png"));
        Card c7C = new Card(7, 2, Image.FromFile("7C.png"));
        Card c7S = new Card(7, 1, Image.FromFile("7S.png"));
        Card c7D = new Card(7, 3, Image.FromFile("7D.png"));
        Card c7H = new Card(7, 4, Image.FromFile("7H.png"));
        Card c8C = new Card(8, 2, Image.FromFile("8C.png"));
        Card c8S = new Card(8, 1, Image.FromFile("8S.png"));
        Card c8D = new Card(8, 3, Image.FromFile("8D.png"));
        Card c8H = new Card(8, 4, Image.FromFile("8H.png"));
        Card c9C = new Card(9, 2, Image.FromFile("9C.png"));
        Card c9S = new Card(9, 1, Image.FromFile("9S.png"));
        Card c9D = new Card(9, 3, Image.FromFile("9D.png"));
        Card c9H = new Card(9, 4, Image.FromFile("9H.png"));
        Card c10C = new Card(10, 2, Image.FromFile("10C.png"));
        Card c10S = new Card(10, 1, Image.FromFile("10S.png"));
        Card c10D = new Card(10, 3, Image.FromFile("10D.png"));
        Card c10H = new Card(10, 4, Image.FromFile("10H.png"));
        Card cJC = new Card(11, 2, Image.FromFile("JC.png"));
        Card cJS = new Card(11, 1, Image.FromFile("JS.png"));
        Card cJD = new Card(11, 3, Image.FromFile("JD.png"));
        Card cJH = new Card(11, 4, Image.FromFile("JH.png"));
        Card cQC = new Card(12, 2, Image.FromFile("QC.png"));
        Card cQS = new Card(12, 1, Image.FromFile("QS.png"));
        Card cQD = new Card(12, 3, Image.FromFile("QD.png"));
        Card cQH = new Card(12, 4, Image.FromFile("QH.png"));
        Card cKC = new Card(13, 2, Image.FromFile("KC.png"));
        Card cKS = new Card(13, 1, Image.FromFile("KS.png"));
        Card cKD = new Card(13, 3, Image.FromFile("KD.png"));
        Card cKH = new Card(13, 4, Image.FromFile("KH.png"));
        Card cAC = new Card(14, 2, Image.FromFile("AC.png"));
        Card cAS = new Card(14, 1, Image.FromFile("AS.png"));
        Card cAD = new Card(14, 3, Image.FromFile("AD.png"));
        Card cAH = new Card(14, 4, Image.FromFile("AH.png"));
        #endregion
        #region global variable;
        int[] isSelected = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        Card[] player = new Card[13];
        int playern = new int();
        int selectedn = new int();
        int skipped=0;
        //Bai tren ban
        string Ttype; //Loai bai
        int Tvalue0;// gia tri la bai lon nhat 
        int Tvalue1;// kieu cua la bai lon nhat
        int cardsn;// so bai tren ban
        int turn;//luot
        int yourturn;//luot cua minh
        #endregion
        #region My functions
        void Select(Card[] Cards, ref PictureBox a, int boxi)
        {
            if (isSelected[boxi - 1] == 0)
            {
                isSelected[boxi - 1] = 1;
                selectedn++;
                a.Top -= 10;
            }
            else if (isSelected[boxi - 1] == 1)
            {
                isSelected[boxi - 1] = 0;
                selectedn--;
                a.Top += 10;
            }
        }
        string KTLoaiBai(Card[] Player)
        {
            if (selectedn == 1)
                return "Le";
            int j = 0;
            int[] value = new int[selectedn];
            int temp;
            for (int i = 0; i < 13; i++)
            {
                if (isSelected[i] == 1)
                {
                    value[j] = player[i].getValue();
                    j++;
                }
            }

            int equal = 1;
            for (int i = 0; i < selectedn - 1; i++)
            {
                if (value[i] != value[i + 1])
                {
                    equal = 0;
                }
            }
            if (equal == 1)
            {
                if (selectedn == 3)
                    return "Tam";
                if (selectedn == 4)
                    return "Tu Quy";
                if (selectedn == 2)
                    return "Doi";
            }

            if (selectedn > 2)
            {
                int laSanh = 1;
                for (int i = 0; i < selectedn - 1; i++)
                {
                    if (value[i] != value[i + 1] - 1)
                        laSanh = 0;
                }
                if (laSanh == 1)
                {
                    return "Sanh";
                }
            }

            if (selectedn > 5)
            {
                int isdoithong = 0;
                for (int i = 0; i < selectedn - 1; i++)
                {
                    if (value[i] == value[i + 1])
                    {
                        isdoithong = 1;
                        if (i < selectedn - 2)
                        {
                            if (value[i] + 1 != value[i + 2])
                            {
                                isdoithong = 0;
                                break;
                            }
                        }
                        i++;
                    }
                    else
                    {
                        isdoithong = 0;
                        break;
                    }
                }
                if (isdoithong == 1)
                {
                    if (selectedn == 6)
                        return "Doi Thong_3";
                    else if (selectedn == 8)
                        return "Doi Thong_4";
                    else if (selectedn == 10)
                        return "Doi Thong_5";
                }
            }

            return "Khong Danh Duoc";
        }

        int Chophepdanh()
        {
            if (turn != yourturn)
                return 0;
            if (selectedn == 0)
                return 0;
            if (KTLoaiBai(player).CompareTo("Khong Danh Duoc") == 0)
                return 0;
            if (Ttype == "0")
                return 1;
            int[] values = new int[selectedn];
            int[] types = new int[selectedn];
            int j = 0;
            for (int i = 0; i < 13; i++)
            {
                if (isSelected[i] == 1)
                {
                    values[j] = player[i].getValue();
                    types[j] = player[i].getTypeid();
                    j++;
                }
            }
            j--;
            //Tu quy
            if (KTLoaiBai(player).CompareTo("Tu Quy") == 0)
            {
                if (Ttype.CompareTo("Le") == 0 && Tvalue0 == 16)
                    return 1;
                if (values[0] > Tvalue0 && Ttype.CompareTo("Tu Quy") == 0)
                    return 1;
                return 0;
            }
            //doi thong
            if (KTLoaiBai(player).Contains("Doi Thong"))
            {
                if (types[j] < types[j - 1])
                    types[j] = types[j - 1];
                if (Ttype.CompareTo("Le") == 0 && Tvalue0 == 16)
                    return 1;
                if (Ttype.Contains("Doi Thong"))
                {
                    int a = int.Parse(Ttype.Split("_")[1]);
                    int b = int.Parse(KTLoaiBai(player).Split("_")[1]);
                    if (a > b) return 0;
                    else if (b > a) return 1;
                    if (Tvalue0 == values[j] && Tvalue1 < types[j])
                        return 1;
                    if (Tvalue0 < values[j])
                        return 1;
                }
                return 0;
            }
            //          
            if (KTLoaiBai(player).CompareTo(Ttype) != 0)
                return 0;
            if (cardsn != selectedn)
                return 0;
            // Le          
            if (Ttype.CompareTo("Le") == 0)
            {
                if (types[0] > Tvalue1 && values[0] == Tvalue0)
                    return 1;
                if (Tvalue0 < values[0])
                    return 1;
                return 0;
            }
            // Doi
            if (Ttype.CompareTo("Doi") == 0)
            {
                if (types[0] < types[1])
                    types[0] = types[1];
                if (types[0] > Tvalue1 && values[0] == Tvalue0)
                    return 1;
                if (Tvalue0 < values[0])
                    return 1;
                return 0;
            }
            //Tam
            if (Ttype.CompareTo("Tam") == 0)
            {
                if (values[0] > Tvalue0)
                    return 1;
                return 0;
            }
            // Sanh
            if (Ttype.CompareTo("Sanh") == 0)
            {
                if (values[j] > Tvalue0)
                    return 1;
                else if (values[j] == Tvalue0 && types[j] > Tvalue1)
                    return 1;
                return 0;
            }
            return 0;
        }
        void xepBai(Card[] Player)
        {
            Card temp = new Card();
            for (int i = 0; i < 13 - 1; i++)
            {
                for (int j = i + 1; j < 13; j++)
                    if (Player[i].getValue() > player[j].getValue())
                    {
                        temp = player[i];
                        Player[i] = Player[j];
                        Player[j] = temp;
                    }
            }

        }
        void BoLuot()
        {
            if (turn == yourturn && Ttype.CompareTo("0") != 0)
            {
                string str = "Skip-" + skipped.ToString();
                skipped = 1;
                client.Send(serialize(str));
                checkBox4.Hide();
            }
        }
        #endregion
        #region event handle

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Hide();
            pictureBox2.Hide();
            pictureBox3.Hide();
            pictureBox4.Hide();
            pictureBox5.Hide();
            pictureBox6.Hide();
            pictureBox7.Hide();
            pictureBox8.Hide();
            pictureBox9.Hide();
            pictureBox10.Hide();
            pictureBox11.Hide();
            pictureBox12.Hide();
            pictureBox13.Hide();
            player2.Image = Image.FromFile("b1fv.png");
            player3.Image = Image.FromFile("b1fv.png");
            player4.Image = Image.FromFile("b1fv.png");
            player2.Hide();
            player3.Hide();
            player4.Hide();
            button1.Hide();
            checkBox1.Hide();
            checkBox2.Hide();
            checkBox3.Hide();
            checkBox4.Hide();
            label1.Hide();
            button4.Hide();
            PictureBox[] pictureBoxes = { pic1, pic2, pic3, pic4, pic5, pic6, pic7, pic8, pic9, pic10, pic11, pic12, pic13 };
            for (int i = 0; i < 13; i++)
                pictureBoxes[i].Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Select(player, ref pictureBox1, 1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            client.Send(serialize("Ready"));
            button2.Hide();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Select(player, ref pictureBox4, 4);
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Select(player, ref pictureBox5, 5);
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            Select(player, ref pictureBox13, 13);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Select(player, ref pictureBox2, 2);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Select(player, ref pictureBox3, 3);
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Select(player, ref pictureBox6, 6);
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Select(player, ref pictureBox7, 7);
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Select(player, ref pictureBox8, 8);
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            Select(player, ref pictureBox9, 9);
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            Select(player, ref pictureBox10, 10);
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            Select(player, ref pictureBox11, 11);
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            Select(player, ref pictureBox12, 12);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            if (Chophepdanh() == 1)
            {
                PictureBox[] pictureBoxes = { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10, pictureBox11, pictureBox12, pictureBox13 };
                string str = "ThongTinBai-" + KTLoaiBai(player) + "-";
                for (int i = 0; i < 13; i++)
                {
                    if (isSelected[i] == 1)
                        pictureBoxes[i].Hide();
                    if (isSelected[i] == 1)
                    {
                        isSelected[i] = -1;
                        str = str + player[i].getValue().ToString() + "_" + player[i].getTypeid().ToString() + ",";
                    }
                }
                playern -= selectedn;
                checkBox4.Hide();
                selectedn = 0;
                if (playern != 0)
                    client.Send(serialize(str));
                else if (playern == 0)
                {
                    str = "Win";
                    client.Send(serialize(str));
                }
            }
            else
            {
                return;
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            BoLuot();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            this.Hide();
            form.ShowDialog();
            this.Close();
        }
        #endregion
        #region client
        IPEndPoint IP;
        Socket client;
        Thread listen;
        void Connect()
        {
            IP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999);
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
        void Close()
        {
            client.Close();
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
                    string[] strings = str.Split('-');
                    Card[] deck = { c3S, c3C, c3D, c3H, c4S, c4C, c4D, c4H, c5S, c5C, c5D, c5H, c6S, c6C, c6D, c6H, c7S, c7C, c7D, c7H, c8S, c8C, c8D, c8H, c9S, c9C, c9D, c9H, c10S, c10C, c10D, c10H, cJS, cJC, cJD, cJH, cQS, cQC, cQD, cQH, cQS, cKC, cKD, cKH, cAS, cAC, cAD, cAH, c2S, c2C, c2D, c2H };
                    //                  
                    //
                    if (strings[0].CompareTo("PhatBai") == 0)
                    {
                        playern = 13;
                        PictureBox[] pictureBoxes2 = { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10, pictureBox11, pictureBox12, pictureBox13, player2, player3, player4 };
                        PictureBox[] pictureBoxes = { pic1, pic2, pic3, pic4, pic5, pic6, pic7, pic8, pic9, pic10, pic11, pic12, pic13 };
                        string[] strings2 = strings[1].Split(",");
                        //
                        for (int i = 0; i < 13; i++)
                            pictureBoxes[i].Hide();
                        //
                        for (int i = 0; i < 13; i++)
                        {
                            player[i] = deck[int.Parse(strings2[i])];
                        }
                        xepBai(player);
                        int x = 146;
                        for (int i = 0; i < 13 + int.Parse(strings2[13]) - 1; i++)
                        {
                            if (i < 13)
                            {
                                pictureBoxes2[i].Image = player[i].getimg();
                                pictureBoxes2[i].Location = new Point(x, 353);
                                x += 24;
                            }
                            pictureBoxes2[i].Show();
                        }
                        yourturn = int.Parse(strings[2]);
                        Ttype = strings[3];
                        turn = int.Parse(strings[4]);
                        button1.Show();
                        label1.Text = yourturn.ToString();
                        label1.Show();
                        button4.Show();
                    }
                    //
                    else if (strings[0].CompareTo("ThongTinBai") == 0)
                    {
                        string[] strings2 = strings[2].Split(',');
                        string[] strings3 = new string[2];
                        Ttype = strings[1];
                        turn = int.Parse(strings.Last());
                        cardsn = strings2.Length - 1;
                        int[] values = new int[cardsn];
                        int[] types = new int[cardsn];
                        PictureBox[] pictureBoxes = { pic1, pic2, pic3, pic4, pic5, pic6, pic7, pic8, pic9, pic10, pic11, pic12, pic13 };
                        //
                        for (int i = 0; i < 13; i++)
                        {
                            isSelected[i] = 0;
                            pictureBoxes[i].Hide();
                        }
                        //
                        for (int i = 0; i < cardsn; i++)
                        {
                            strings3 = strings2[i].Split("_");
                            values[i] = int.Parse(strings3[0]);
                            types[i] = int.Parse(strings3[1]);
                        }
                        // lay anh
                        int x = 266;
                        int a;
                        for (int i = 0; i < cardsn; i++)
                        {
                            x += 24;
                            if (values[cardsn - 1] != 16)
                            {
                                a = (values[i] - 3) * 4 + types[i] - 1;
                                pictureBoxes[i].Image = deck[a].getimg();
                            }
                            else
                            {
                                a = (15 - 3) * 4 + types[i] - 1;
                                pictureBoxes[i].Image = deck[a].getimg();
                            }
                            pictureBoxes[i].Show();
                        }
                        //lay value0 va value1
                        Tvalue0 = values[cardsn - 1];
                        if (Ttype.CompareTo("Le") == 0)
                        {
                            Tvalue1 = types[0];
                        }
                        else if (Ttype.CompareTo("Doi") == 0)
                        {
                            if (types[1] > types[0])
                                Tvalue1 = types[1];
                            else Tvalue1 = types[0];
                        }
                        else if (Ttype.Contains("Doi Thong"))
                        {
                            if (types[cardsn - 1] > types[cardsn - 2])
                                Tvalue1 = types[cardsn - 1];
                            else Tvalue1 = types[cardsn - 2];
                        }
                        else if (Ttype.CompareTo("Sanh") == 0)
                        {
                            Tvalue1 = types[cardsn - 1];
                        }
                    }
                    //ket thuc
                    else if (strings[0].CompareTo("GameEnd") == 0)
                    {
                        button2.Show();
                        yourturn = -1;
                        MessageBox.Show(strings[1]);
                        label1.Hide();
                        button4.Hide();
                        button1.Hide();
                    }
                    //bo luot
                    else if (strings[0].CompareTo("Skip") == 0)
                    {
                        if (strings.Length > 2)
                        {
                            Ttype = strings[1];
                            if (skipped == 0)
                            {
                                string s = "SetTurn-" + yourturn.ToString();
                                turn = yourturn;
                                client.Send(serialize(s));
                            }
                            else
                            {
                                skipped = 0;
                            }
                        }
                        else
                        {
                            turn = int.Parse(strings[1]);
                        }
                    }
                    //                   
                    else if (strings[0].CompareTo("SetTurn") == 0)
                    {
                        turn = int.Parse(strings[1]);
                    }

                    if (strings[0].CompareTo("ReturnSanSangButton") == 0)
                    {
                        button2.Show();
                    }


                    else
                    {
                        if (turn == yourturn)
                            checkBox4.Show();
                        else checkBox4.Hide();
                        if (skipped == 1)
                            BoLuot();
                    }
                }
                catch
                {
                    return;
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
        #endregion      
    }
}
 


