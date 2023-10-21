using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
            public int getValue()
            { return value; }
        }
        #endregion
        #region 52 cards definition
        Card c2C = new Card(16, 1, Image.FromFile("2C.png"));
        Card c2S = new Card(16, 2, Image.FromFile("Resources\\2S.png"));
        Card c2D = new Card(16, 3, Image.FromFile("2D.png"));
        Card c2H = new Card(16, 4, Image.FromFile("2H.png"));
        Card c3C = new Card(3, 1, Image.FromFile("3C.png"));
        Card c3S = new Card(3, 2, Image.FromFile("3S.png"));
        Card c3D = new Card(3, 3, Image.FromFile("3D.png"));
        Card c3H = new Card(3, 4, Image.FromFile("3H.png"));
        Card c4C = new Card(4, 1, Image.FromFile("4C.png"));
        Card c4S = new Card(4, 2, Image.FromFile("4S.png"));
        Card c4D = new Card(4, 3, Image.FromFile("4D.png"));
        Card c4H = new Card(4, 4, Image.FromFile("4H.png"));
        Card c5C = new Card(5, 1, Image.FromFile("5C.png"));
        Card c5S = new Card(5, 2, Image.FromFile("5S.png"));
        Card c5D = new Card(5, 3, Image.FromFile("5D.png"));
        Card c5H = new Card(5, 4, Image.FromFile("5H.png"));
        Card c6C = new Card(6, 1, Image.FromFile("6C.png"));
        Card c6S = new Card(6, 2, Image.FromFile("6S.png"));
        Card c6D = new Card(6, 3, Image.FromFile("6D.png"));
        Card c6H = new Card(6, 4, Image.FromFile("6H.png"));
        Card c7C = new Card(7, 1, Image.FromFile("7C.png"));
        Card c7S = new Card(7, 2, Image.FromFile("7S.png"));
        Card c7D = new Card(7, 3, Image.FromFile("7D.png"));
        Card c7H = new Card(7, 4, Image.FromFile("7H.png"));
        Card c8C = new Card(8, 1, Image.FromFile("8C.png"));
        Card c8S = new Card(8, 2, Image.FromFile("8S.png"));
        Card c8D = new Card(8, 3, Image.FromFile("8D.png"));
        Card c8H = new Card(8, 4, Image.FromFile("8H.png"));
        Card c9C = new Card(9, 1, Image.FromFile("9C.png"));
        Card c9S = new Card(9, 2, Image.FromFile("9S.png"));
        Card c9D = new Card(9, 3, Image.FromFile("9D.png"));
        Card c9H = new Card(9, 4, Image.FromFile("9H.png"));
        Card c10C = new Card(10, 1, Image.FromFile("10C.png"));
        Card c10S = new Card(10, 2, Image.FromFile("10S.png"));
        Card c10D = new Card(10, 3, Image.FromFile("10D.png"));
        Card c10H = new Card(10, 4, Image.FromFile("10H.png"));
        Card cJC = new Card(11, 1, Image.FromFile("JC.png"));
        Card cJS = new Card(11, 2, Image.FromFile("JS.png"));
        Card cJD = new Card(11, 3, Image.FromFile("JD.png"));
        Card cJH = new Card(11, 4, Image.FromFile("JH.png"));
        Card cQC = new Card(12, 1, Image.FromFile("QC.png"));
        Card cQS = new Card(12, 2, Image.FromFile("QS.png"));
        Card cQD = new Card(12, 3, Image.FromFile("QD.png"));
        Card cQH = new Card(12, 4, Image.FromFile("QH.png"));
        Card cKC = new Card(13, 1, Image.FromFile("KC.png"));
        Card cKS = new Card(13, 2, Image.FromFile("KS.png"));
        Card cKD = new Card(13, 3, Image.FromFile("KD.png"));
        Card cKH = new Card(13, 4, Image.FromFile("KH.png"));
        Card cAC = new Card(14, 1, Image.FromFile("AC.png"));
        Card cAS = new Card(14, 2, Image.FromFile("AS.png"));
        Card cAD = new Card(14, 3, Image.FromFile("AD.png"));
        Card cAH = new Card(14, 4, Image.FromFile("AH.png"));
        #endregion
        #region global variable;
        int[] isSelected = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        Card[] player = new Card[13];
        Card[] player2 = new Card[13];
        int playern = new int();
        int selectedn = new int();
        string LoaiBai = new string("0");
        #endregion
        #region My functions
        void Select(Card[] Cards,ref PictureBox a, int boxi)
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
            Card[] Cards = new Card[selectedn];
            int j = 0;
            int[] value = new int[selectedn];
            int temp;
            for (int i = 0; i < 13; i++)
            {
                if (isSelected[i] == 1)
                {
                    Cards[j] = Player[i];
                    value[j] = Cards[j].getValue();
                    j++;
                }
            }
            if (selectedn == 3)
            {
                if (value[0] == value[1] && value[1] == value[2])
                    return "Tam";
            }
            if (selectedn == 4)
            {
                if (value[0] == value[1] && value[1] == value[2] && value[2] == value[3])
                    return "Tu Quy";
            }
            if (selectedn == 2)
            {
                if (Cards[0].getValue() == Cards[1].getValue())
                    return "Doi";
            }
            for (int i = 0; i < selectedn - 1; i++)
                for (int a = i + 1; a < selectedn; a++)
                {
                    if (value[a] < value[i])
                    {
                        temp = value[i];
                        value[i] = value[a];
                        value[a] = temp;
                    }
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
                        return "Doi Thong";
                    else if (selectedn == 8)
                        return "4 doi thong";
                    else if (selectedn == 10)
                        return "5 doi thong";
                    else if (selectedn == 12)
                        return "6 doi thong";
                }
            }
            return "Khong Danh Duoc";
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
        #endregion
        #region event handle
        private void button1_Click(object sender, EventArgs e)
        {

        }

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

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Select(player,ref pictureBox1, 1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            playern = 13;
            selectedn = 0;
            Card[] desk = { c2C, c2S, c2D, c2H, c3C, c3D, c3H, c3S, c4C, c4D, c4H, c4S, c5C, c5D, c5H, c5S, c6C, c6D, c6H, c6S, c7C, c7D, c7S, c7H, c8C, c8D, c8H, c8S, c9C, c9D, c9H, c9S, c10C, c10D, c10H, c10S, cJC, cJD, cJH, cJS, cQC, cQD, cQH, cQS, cKS, cKH, cKD, cKC, cAS, cAD, cAH, cAC };
            int[] deski = new int[52];
            Random rand = new Random();
            int random;
            for (int i = 0; i < 13; i++)
            {
                isSelected[i] = 0;
            }
            for (int i = 0; i < 52; i++)
            {
                deski[i] = 1;
            }
            //Chia Bai
            for (int i = 0; i < 13; i++)
            {
                random = rand.Next(0, 51);
                if (deski[random] != 0)
                {
                    player[i] = desk[random];
                    deski[random] = 0;
                }
                else i -= 1;
            }
            xepBai(player);
            #region picture
            pictureBox1.Location = new Point(146, 353);
            button2.Location = new Point(2, 12);
            pictureBox2.Location = new Point(168, 353);
            pictureBox3.Location = new Point(192, 354);
            pictureBox4.Location = new Point(218, 353);
            pictureBox5.Location = new Point(241, 354);
            pictureBox6.Location = new Point(266, 353);
            pictureBox7.Location = new Point(292, 353);
            pictureBox8.Location = new Point(315, 353);
            pictureBox9.Location = new Point(340, 353);
            pictureBox10.Location = new Point(366, 354);
            pictureBox11.Location = new Point(389, 354);
            pictureBox12.Location = new Point(414, 354);
            pictureBox13.Location = new Point(440, 354);
            pictureBox1.Image = player[0].getimg();
            pictureBox2.Image = player[1].getimg();
            pictureBox3.Image = player[2].getimg();
            pictureBox4.Image = player[3].getimg();
            pictureBox5.Image = player[4].getimg();
            pictureBox6.Image = player[5].getimg();
            pictureBox7.Image = player[6].getimg();
            pictureBox8.Image = player[7].getimg();
            pictureBox9.Image = player[8].getimg();
            pictureBox10.Image = player[9].getimg();
            pictureBox11.Image = player[10].getimg();
            pictureBox12.Image = player[11].getimg();
            pictureBox13.Image = player[12].getimg();
            pictureBox1.Show();
            pictureBox2.Show();
            pictureBox3.Show();
            pictureBox4.Show();
            pictureBox5.Show();
            pictureBox6.Show();
            pictureBox7.Show();
            pictureBox8.Show();
            pictureBox9.Show();
            pictureBox10.Show();
            pictureBox11.Show();
            pictureBox12.Show();
            pictureBox13.Show();
            #endregion          
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
            if (selectedn == 0)
            {
                return;
            }
            if (KTLoaiBai(player).CompareTo("Khong Danh Duoc") == 0)
            {
                MessageBox.Show(KTLoaiBai(player));
                return;
            }
            MessageBox.Show(KTLoaiBai(player));
            PictureBox[] pictureBoxes = { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10, pictureBox11, pictureBox12, pictureBox13 };
            int x = 266;
            int y = 160;
            for (int i = 0; i < 13; i++)
            {
                if (isSelected[i] == -1)
                    pictureBoxes[i].Hide();
                if (isSelected[i] == 1)
                {
                    isSelected[i] = -1;
                    pictureBoxes[i].Location = new Point(x, 160);
                    x += 24;
                }
            }
            playern -= selectedn;
            selectedn = 0;
            if (playern == 0)
            {
                MessageBox.Show("You Win");
            }
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {

        }
        private void button3_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            this.Hide();
            form.ShowDialog();
            this.Close();
        }
    }
    #endregion
}
