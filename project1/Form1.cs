using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace project1
{
    public partial class Form1 : Form
    {
        public bool IsLogin { get; set; }
        SqlConnection ketnoi;
        public static string ID;
        public Form1()
        {
            InitializeComponent();
            IsLogin = false;
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            
            try
            {
                ketnoi = new SqlConnection(@"Data Source=DESKTOP-98PT33E\WINCC;Initial Catalog=HETHONG;Integrated Security=True");
                ketnoi.Open();
                string sql = "select Count(*) From [HETHONG].[dbo].[ID-user] Where Account=@acc And Pass=@ass";
                SqlCommand cmd = new SqlCommand(sql,ketnoi);
                cmd.Parameters.Add(new SqlParameter("@acc", textTK.Text));
                cmd.Parameters.Add(new SqlParameter("@ass", textMK.Text));
                int x = (int)cmd.ExecuteScalar();
                if ( x == 1)
                {
                    ID = getID(textTK.Text, textMK.Text);//                  
                    MessageBox.Show("ID acccount: " + ID,"Info...");
                    this.Close();
                    IsLogin = true;
                }
                else
                {
                    MessageBox.Show("Login failed");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Fail connected ");
            }
        }
        private void btnexit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public string getID (string a , string b)
        {
            string id = "";
            try
            {
                SqlCommand cmd1 = new SqlCommand("SELECT * FROM [HETHONG].[dbo].[ID-user] WHERE Account ='" + textTK.Text + "' and Pass='" + textMK.Text + "'", ketnoi);
                SqlDataAdapter dtaAdap = new SqlDataAdapter(cmd1);
                DataTable dtaTable = new DataTable();
                dtaAdap.Fill(dtaTable);
                if (dtaTable != null)
                {
                    foreach (DataRow dtarow in dtaTable.Rows)
                    {
                        id = dtarow["ID"].ToString();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi xảy ra khi truy vấn dữ liệu hoặc kết nối với server thất bại !");
            }
            finally
            {
                ketnoi.Close();
            }
            return id;
        }   
    }
}
