using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Post
{
    public partial class Delivery : Form
    {
        public Delivery()
        {
            InitializeComponent();
            ShowDelivery();
            GetAgentId();
            GetParcel();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Studio\Documents\PostOfficeDB.mdf;Integrated Security=True;Connect Timeout=30");
        private void ShowDelivery()
        {
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("Select * from DeliveryTbl", Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            DeliveryDGV.DataSource = ds.Tables[0];
            Con.Close();

        }
        private void GetAgentName()
        {
            Con.Open();
            string Query = "select * from AgentTbl where AgNum=" + AgNumCB.SelectedValue.ToString() + "";
            SqlCommand cmd = new SqlCommand(Query, Con);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                AgNameTb.Text = dr["AgName"].ToString();
            }
            Con.Close();
        }

        private void GetAgentId()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("select * from AgentTbl", Con);
            SqlDataReader rdr;
            rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("AgNum", typeof(int));
            dt.Load(rdr);
            AgNumCB.ValueMember = "AgNum";
            AgNumCB.DataSource = dt;
            Con.Close();
        }

        private void GetParcel()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("select * from ParcelTbl", Con);
            SqlDataReader rdr;
            rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("PNum", typeof(int));
            dt.Load(rdr);
            ParNumCb.ValueMember = "PNum";
            ParNumCb.DataSource = dt;
            Con.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Reset()
        {
            FeesTb.Text = "";
            AgNameTb.Text = "";
            AgNumCB.SelectedIndex = -1;
            ParNumCb.SelectedIndex = -1;
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (ParNumCb.SelectedIndex == -1 || AgNameTb.Text == "" || FeesTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into DeliveryTbl(PrNum, DelDate, AgentNum,AgentName,DelFees) values(@PN,@DD,@ANum,@AName,@DelFees)", Con);
                    cmd.Parameters.AddWithValue("@PN", ParNumCb.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@DD", DelDate.Value.Date);
                    cmd.Parameters.AddWithValue("@ANum", AgNumCB.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@AName", AgNameTb.Text);
                    cmd.Parameters.AddWithValue("@DelFees", FeesTb.Text);
                    cmd.ExecuteNonQuery();
                    Con.Close();
                    MessageBox.Show("Agent Recorded"); 
                    ShowDelivery();
                    
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void AgNumCB_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetAgentName();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            MainMenu Obj = new MainMenu();
            Obj.Show();
            this.Hide();
        }

        private void DeliveryDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
