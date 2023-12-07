using Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TaiXiu
{
    public partial class Danhsachphongcho : Form
    {
        public delegate void Data(int maphong);
        public Data TruyenMaPhong;
        public int done = 0;
        public int loaded = 0;
        List<phong> lastLoadedData = new List<phong>();
        public Danhsachphongcho(Form1 form)
        {
            InitializeComponent();
            form.DSPhong = new Form1.TruyenDuLieu(LoadDanhSach);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            loaded = 1;
        }

        private void Form1_Load(object sender, EventArgs e)
        { 
        }

        private void LoadDanhSach(List<phong> ListPhong)
        {           
            if (!ListPhong.SequenceEqual(lastLoadedData))
            {
                lastLoadedData = new List<phong>(ListPhong);
                listView1.Items.Clear();           
                string[] str = { "Chọn", "", "", "" };
                ListViewItem item;
                for (int i = 0; i < ListPhong.Count; i++)
                {
                    item = new ListViewItem(str);
                    item.SubItems[2].Text = ListPhong[i].songuoi.ToString();
                    item.SubItems[1].Text = ListPhong[i].maphong.ToString();
                    item.SubItems[3].Text = (ListPhong[i].dangchoi == 1) ? "Đang chơi" : "Chưa bắt đầu";
                    listView1.Items.Add(item);
                }
            }           
        }
        public void phongday()
        {
            MessageBox.Show("Phòng đã đầy");
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                if (listView1.SelectedItems[0].SubItems[2].Text.CompareTo("4") == 0)
                {
                    MessageBox.Show("Phòng đã đầy");
                    return;
                }
                if (listView1.SelectedItems[0].SubItems[3].Text.CompareTo("Đang chơi") == 0)
                {
                    MessageBox.Show("Bạn không được vào phòng đang chơi");
                    return;
                }
                TruyenMaPhong(int.Parse(listView1.SelectedItems[0].SubItems[1].Text));
                done = 1;
                this.Close();
            }
        }

        private void Danhsachphongcho_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(done!=1)
              done = -1;
        }
    }
}
