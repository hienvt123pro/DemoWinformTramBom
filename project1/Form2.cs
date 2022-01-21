using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using S7.Net;
using System.Data.SqlClient;
namespace project1
{
    public partial class Form2 : Form
    {
        Plc myPlc;
        public bool isCnt { get; set; }
        public bool alertTranbe { get; set; }
        public bool alert1 { get; set; }
        public bool alert2 { get; set; }
        public bool alertClo { get; set; }
       
        byte[] output = new byte[3];
        string sql;
        SqlConnection ketnoi;
        SqlCommand thuchien;
        SqlDataReader docdulieu;
        int i = 0;
        public Form2()
        {
            InitializeComponent();
        }

        private void btncnt_Click(object sender, EventArgs e)
        {
            myPlc = new Plc(CpuType.S71200, "169.254.248.19", 0, 1);
            myPlc.Open();
            if (myPlc.IsConnected)
            {
                MessageBox.Show("PLC connected");
                isCnt = true;
            }
            else
            {
                MessageBox.Show("PLC connected failed");
                isCnt = false;
            }
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (isCnt == true)
            {
                var valueconvert = ((uint)myPlc.Read("MD4")).ConvertToDouble(); // read gt muc nuoc
                gtmucnuoc.Text = valueconvert.ToString();
                progressBar1.Value = (int)valueconvert;

                output = myPlc.ReadBytes(DataType.Output, 5, 0, 2);   // read gia tri output cac may bom
                if (output[0].SelectBit(0) == true)
                {
                    Qpump0.DiscreteValue1 = false;
                    Qpump0.DiscreteValue2 = true;
                    progressP0.Value = 100;
                }
                else
                {
                    Qpump0.DiscreteValue1 = true;
                    Qpump0.DiscreteValue2 = false;
                    progressP0.Value = 0;
                }
                if (output[0].SelectBit(1) == true)
                {
                    Qpump1.DiscreteValue1 = false;
                    Qpump1.DiscreteValue2 = true;
                    progressP1.Value = 100;
                }
                else
                {
                    Qpump1.DiscreteValue1 = true;
                    Qpump1.DiscreteValue2 = false;
                    progressP1.Value = 0;
                }
                if (output[0].SelectBit(2) == true)
                {
                    Qpump2.DiscreteValue1 = false;
                    Qpump2.DiscreteValue2 = true;
                    progressP2.Value = 100;
                }
                else
                {
                    Qpump2.DiscreteValue1 = true;
                    Qpump2.DiscreteValue2 = false;
                    progressP2.Value = 0;
                }
                if (output[0].SelectBit(3) == true)  // den bao tran be
                {
                    lightTranbe.DiscreteValue1 = false;
                    lightTranbe.DiscreteValue2 = true;
                    alertTranbe = true;
                }
                else
                {
                    lightTranbe.DiscreteValue1 = true;
                    lightTranbe.DiscreteValue2 = false;
                    alertTranbe = false;
                }
                if (output[0].SelectBit(5) == true)
                {
                    Qauto.DiscreteValue1 = false;
                    Qauto.DiscreteValue2 = true;
                }
                else
                {
                    Qauto.DiscreteValue1 = true;
                    Qauto.DiscreteValue2 = false;
                }

                if (output[0].SelectBit(6) == true)  // den bao ap suat ong 1
                {
                    light1.DiscreteValue1 = false;
                    light1.DiscreteValue2 = true;
                    alert1 = true;
                }
                else
                {
                    light1.DiscreteValue1 = true;
                    light1.DiscreteValue2 = false;
                    alert2 = false;
                }
                if (output[0].SelectBit(7) == true)  // den bao ap suat ong 2
                {
                    light2.DiscreteValue1 = false;
                    light2.DiscreteValue2 = true;
                    alert2 = true;
                }
                else
                {
                    light2.DiscreteValue1 = true;
                    light2.DiscreteValue2 = false;
                    alert2 = false;
                }

                var valueApsuat1 = ((uint)myPlc.Read("MD20")).ConvertToDouble(); // read gt ap suat ong 1
                textGTAS1.Text = valueApsuat1.ToString();
                var valueApsuat2 = ((uint)myPlc.Read("MD30")).ConvertToDouble(); // read gt ap suat ong 2
                textGTAS2.Text = valueApsuat2.ToString();
                var valueClo = ((uint)myPlc.Read("MD40")).ConvertToDouble(); // read gt clo hien tai
                txtOutClo.Text = valueClo.ToString();

                if (output[1].SelectBit(0) == true)     // read gt may bom xa be
                {
                    standardControl1.DiscreteValue1 = false;
                    standardControl1.DiscreteValue2 = true;
                }
                else
                {
                    standardControl1.DiscreteValue1 = true;
                    standardControl1.DiscreteValue2 = false;
                }
                if (output[1].SelectBit(2) == true)     // read gt den bao vuot muc clo
                {
                    standardControl2.DiscreteValue1 = false;
                    standardControl2.DiscreteValue2 = true;
                    alertClo = true;
                }
                else
                {
                    standardControl2.DiscreteValue1 = true;
                    standardControl2.DiscreteValue2 = false;
                    alertClo = false;
                }
            }
        }
        private void btnonAuto_Click(object sender, EventArgs e)
        {
            myPlc.WriteBit(DataType.Memory, 0, 1, 1, 1);
        }

        private void btnoffAuto_Click(object sender, EventArgs e)
        {
            myPlc.WriteBit(DataType.Memory, 0, 1, 1, 0);
        }

        private void btnonP0_Click(object sender, EventArgs e)
        {
            myPlc.WriteBit(DataType.Memory, 5, 0, 0, 1);
            progressP0.Value = 100;
        }

        private void btnoffP0_Click(object sender, EventArgs e)
        {
            myPlc.WriteBit(DataType.Memory, 5, 0, 0, 0);
            progressP0.Value = 0;
        }

        private void btnonP1_Click(object sender, EventArgs e)
        {
            myPlc.WriteBit(DataType.Memory, 5, 0, 2, 1);
            progressP1.Value = 100;
        }

        private void btnoffP1_Click(object sender, EventArgs e)
        {
            myPlc.WriteBit(DataType.Memory, 5, 0, 2, 0);
            progressP1.Value = 0;
        }

        private void btnonP2_Click(object sender, EventArgs e)
        {
            myPlc.WriteBit(DataType.Memory, 5, 0, 3, 1);
            progressP2.Value = 100;
        }

        private void btnoffP2_Click(object sender, EventArgs e)
        {
            myPlc.WriteBit(DataType.Memory, 5, 0, 3, 0);
            progressP2.Value = 0;
        }

        private void btnonClo_Click(object sender, EventArgs e)   // mo van cham clo
        {
           if( alertClo == false)
           {
                if (comboBox1.SelectedItem != null)  
                {
                    myPlc.Write("MW44", double.Parse(txtInClo.Text));     // write gt nhap clo xuong plc
                    myPlc.WriteBit(DataType.Memory, 10, 10, 4, 1);        // write bit mo van clo   
                    var valueClo = ((uint)myPlc.Read("MD40")).ConvertToDouble(); // update gt clo
                    txtOutClo.Text = valueClo.ToString();                        //
                    listView1.Items.Clear();                                     // hien thi lich su
                    ketnoi.Open();
                    sql = @"insert into lichsuClo ([Thoi gian],[Nguoi cham],[So kg Clo],[Nong do hien tai]) values (N'" + DateTime.Now.ToString() + @"',N'" + comboBox1.Text + @"',N'" + txtInClo.Text + @"',N'" + txtOutClo.Text + @"')";
                    thuchien = new SqlCommand(sql, ketnoi);
                    thuchien.ExecuteNonQuery();
                    ketnoi.Close();
                    hienthi();
                }
                else
                {
                    MessageBox.Show("Yêu cầu nhập người dùng", "!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
           else
           {
                MessageBox.Show("Nồng độ Clo vượt mức cho phép", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
           }         
        }

        private void btnonPout_Click(object sender, EventArgs e)
        {
            myPlc.WriteBit(DataType.Memory, 10, 10, 1, 1);
        }

        private void btnoffPout_Click(object sender, EventArgs e)
        {
            myPlc.WriteBit(DataType.Memory, 10, 10, 1, 0);
        }

        private void Form2_Load(object sender, EventArgs e)                   //////////***********///////////
        {
            timer1.Start();
            timer2.Start();
            labeltime.Text = DateTime.Now.ToLongTimeString();
            labeldate.Text = DateTime.Now.ToLongDateString();
            ketnoi = new SqlConnection(@"Data Source=DESKTOP-98PT33E\WINCC;Initial Catalog=HETHONG;Integrated Security=True");
            hienthi();
        }

        public void hienthi()                         // ctr con hien thi lich su cham clo
        {
            listView1.Items.Clear();
            ketnoi.Open();
            sql = @"SELECT TOP (1000) [Thoi gian]
      ,[Nguoi cham]
      ,[So kg Clo]
      ,[Nong do hien tai]
  FROM [HETHONG].[dbo].[lichsuClo]";
            thuchien = new SqlCommand(sql, ketnoi);
            docdulieu = thuchien.ExecuteReader();
            i = 0;
            while(docdulieu.Read())
            {
                listView1.Items.Add(docdulieu[0].ToString());
                listView1.Items[i].SubItems.Add(docdulieu[1].ToString());
                listView1.Items[i].SubItems.Add(docdulieu[2].ToString());
                listView1.Items[i].SubItems.Add(docdulieu[3].ToString());
                i++;
            }
            ketnoi.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            labeltime.Text = DateTime.Now.ToLongTimeString();
            timer1.Start();
        }

        private void lightTranbe_MouseClick(object sender, MouseEventArgs e)
        {
            if (isCnt == true && alertTranbe == true)
            {
                MessageBox.Show("Bể đầy", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void light1_MouseClick(object sender, MouseEventArgs e)
        {
            if (isCnt == true && alert1 == true)
            {
                MessageBox.Show("Yêu cầu tắt động cơ 1", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void light2_MouseClick(object sender, MouseEventArgs e)
        {
            if (isCnt == true && alert2 == true)
            {
                MessageBox.Show("Yêu cầu tắt động cơ 2", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void standardControl2_MouseClick(object sender, MouseEventArgs e)
        {
            if (isCnt == true && alertClo == true)
            {
                MessageBox.Show("Nồng độ Clo vượt mức cho phép", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)            // nut xoa lich su clo trong sql
        {
            if (isCnt == true)
            {
                listView1.Items.Clear();
                ketnoi.Open();
                sql = @"Delete FROM lichsuClo";
                thuchien = new SqlCommand(sql, ketnoi);
                thuchien.ExecuteNonQuery();
                ketnoi.Close();
                hienthi();
            }
        }

     
    }
}
