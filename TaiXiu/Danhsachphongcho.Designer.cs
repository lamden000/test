using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace TaiXiu
{
    partial class Danhsachphongcho
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            listView1 = new ListView();
            cChon = new ColumnHeader();
            cMaPhong = new ColumnHeader();
            cSoNguoi = new ColumnHeader();
            cDangChoi = new ColumnHeader();
            SuspendLayout();
            // 
            // listView1
            // 
            listView1.BackColor = Color.PaleTurquoise;
            listView1.BackgroundImageTiled = true;
            listView1.Columns.AddRange(new ColumnHeader[] { cChon, cMaPhong, cSoNguoi, cDangChoi });
            listView1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            listView1.ForeColor = SystemColors.Info;
            listView1.Location = new Point(0, 2);
            listView1.MultiSelect = false;
            listView1.Name = "listView1";
            listView1.Scrollable = false;
            listView1.Size = new Size(797, 449);
            listView1.TabIndex = 0;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Details;
            listView1.SelectedIndexChanged += listView1_SelectedIndexChanged;
            // 
            // cChon
            // 
            cChon.Text = "Chọn phòng";
            cChon.Width = 150;
            // 
            // cMaPhong
            // 
            cMaPhong.Text = "Mã Phòng";
            cMaPhong.Width = 200;
            // 
            // cSoNguoi
            // 
            cSoNguoi.Text = "Số Người";
            cSoNguoi.Width = 200;
            // 
            // cDangChoi
            // 
            cDangChoi.Text = "Tình trạng";
            cDangChoi.Width = 200;
            // 
            // Danhsachphongcho
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(800, 450);
            Controls.Add(listView1);
            Name = "Danhsachphongcho";
            Text = "Danh Sách Phòng";
            FormClosing += Danhsachphongcho_FormClosing;
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion

        private ListView listView1;
        private ColumnHeader cChon;
        private ColumnHeader cMaPhong;
        private ColumnHeader cSoPhong;
        private ColumnHeader cSoNguoi;
        private ColumnHeader cDangChoi;
    }
}