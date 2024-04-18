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
    public partial class Parcels : Form
    {
        public Parcels()
        {
            InitializeComponent();
            ShowParcels();
            GetSendId();
            AgentLbl.Text = Login.AName;
            DateLbl.Text = DateTime.Today.Day + "-" + DateTime.Today.Month + "-" + DateTime.Today.Year;
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Studio\Documents\PostOfficeDB.mdf;Integrated Security=True;Connect Timeout=30");
        private void ShowParcels()
        {
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("Select * from ParcelTbl", Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            ParcelDGV.DataSource = ds.Tables[0];
            Con.Close();

        }

        private void GetSenderName()
        {
            Con.Open();
            string Query = "select * from CustomerTbl where CustNum=" + SendIdCb.SelectedValue.ToString() + "";
            SqlCommand cmd = new SqlCommand(Query, Con);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                SNameTB.Text = dr["custname"].ToString();
            }
            Con.Close();
        }

        private void GetSendId()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("select * from CustomerTbl", Con);
            SqlDataReader rdr;
            rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("CustNum", typeof(int));
            dt.Load(rdr);
            SendIdCb.ValueMember = "CustNum";
            SendIdCb.DataSource = dt;
            Con.Close();
        }
        private void Savebtn_Click(object sender, EventArgs e)
        {
            if (PSAddressTb.Text == "" || SNameTB.Text == "" || RecNameTb.Text == "" || RecPhoneTb.Text == "" || PSourceCb.SelectedIndex == -1 || PacTypeCb.SelectedIndex == -1 || PacSTb.Text == "" || PacWTb.Text == "" || PacTypeCb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into ParcelTbl(PDate,PSource,PSName, PRName, PSAdd, PSPhone, PRPhone, PackW,PackSize,Ptype,PAgent,PStatus) values(@PD, @PS, @PSN, @PRN, @PSA, @PSP, @PRP, @PW,@PaS,@Pt,@PAg,@PSt)", Con);
                    cmd.Parameters.AddWithValue("@PD", PDate.Value.Date);
                    cmd.Parameters.AddWithValue("@PS", PSourceCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@PSN", SNameTB.Text);
                    cmd.Parameters.AddWithValue("@PRN", RecNameTb.Text);
                    cmd.Parameters.AddWithValue("@PSA", PSAddressTb.Text);
                    cmd.Parameters.AddWithValue("@PSP", SPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@PRP", RecPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@PW", PacWTb.Text);
                    cmd.Parameters.AddWithValue("@PaS", PacSTb.Text);
                    cmd.Parameters.AddWithValue("@Pt", PacTypeCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@PAg", AgentLbl.Text);
                    cmd.Parameters.AddWithValue("@PSt", StatusCb.SelectedItem.ToString());
                    cmd.ExecuteNonQuery();
                    Con.Close();
                    MessageBox.Show("Parcel Recorded");
                    ShowParcels();
                }
                catch (Exception Ex)
                {
                    Con.Close();
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void SendIdCb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetSenderName();
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (PSAddressTb.Text == "" || SNameTB.Text == "" || RecNameTb.Text == "" || RecPhoneTb.Text == "" || PSourceCb.SelectedIndex == -1 || PacTypeCb.SelectedIndex == -1 || PacSTb.Text == "" || PacWTb.Text == "" || PacTypeCb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("update ParcelTbl set PDate=@PD, PSource=@PS, PSName=@PSN, PRName=@PRN, PSAdd=@PSA, PSPhone=@PSP, PRPhone=@PRP, PStatus=@PSt, PackW=@PW, PackSize=@PaS, Ptype=@Pt, PAgent=@PAg where PNum=@Pkey", Con);
                    cmd.Parameters.AddWithValue("@PD", PDate.Value.Date);
                    cmd.Parameters.AddWithValue("@PS", PSourceCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@PSN", SNameTB.Text);
                    cmd.Parameters.AddWithValue("@PRN", RecNameTb.Text);
                    cmd.Parameters.AddWithValue("@PSA", PSAddressTb.Text);
                    cmd.Parameters.AddWithValue("@PSP", SPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@PRP", RecPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@PW", PacWTb.Text);
                    cmd.Parameters.AddWithValue("@PaS", PacSTb.Text);
                    cmd.Parameters.AddWithValue("@Pt", PacTypeCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@PAg", AgentLbl.Text);
                    cmd.Parameters.AddWithValue("@PSt", StatusCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@PKey", Key);
                    cmd.ExecuteNonQuery();
                    Con.Close();
                    MessageBox.Show("Parcel Updated");
                    ShowParcels();
                }
                catch (Exception Ex)
                {
                    Con.Close();
                    MessageBox.Show(Ex.Message);
                }
            }
        }
        int Key = 0;
        private void ParcelDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            PDate.Text = ParcelDGV.SelectedRows[0].Cells[1].Value.ToString();
            PSourceCb.SelectedItem = ParcelDGV.SelectedRows[0].Cells[2].Value.ToString();
            SNameTB.Text = ParcelDGV.SelectedRows[0].Cells[3].Value.ToString();
            RecNameTb.Text = ParcelDGV.SelectedRows[0].Cells[4].Value.ToString();
            PSAddressTb.Text = ParcelDGV.SelectedRows[0].Cells[5].Value.ToString();
            SPhoneTb.Text = ParcelDGV.SelectedRows[0].Cells[6].Value.ToString();
            RecPhoneTb.Text = ParcelDGV.SelectedRows[0].Cells[7].Value.ToString();
            PacWTb.Text = ParcelDGV.SelectedRows[0].Cells[8].Value.ToString();
            PacSTb.Text = ParcelDGV.SelectedRows[0].Cells[9].Value.ToString();
            PacTypeCb.SelectedItem = ParcelDGV.SelectedRows[0].Cells[10].Value.ToString();
            StatusCb.SelectedItem = ParcelDGV.SelectedRows[0].Cells[12].Value.ToString();

            if (SNameTB.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(ParcelDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void Deletebtn_Click(object sender, EventArgs e)
        {
            if (Key == 0)
            {
                MessageBox.Show("Selecet a Transfer");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete from ParcelTbl where PNum = @PKey", Con);
                    cmd.Parameters.AddWithValue("@PKey", Key);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Parcel Deleted");
                    Con.Close();
                    ShowParcels();
                    // Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            MainMenu Obj = new MainMenu();
            Obj.Show();
            this.Hide();
        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void SearchBtn_Click(object sender, EventArgs e)
        {
            if (SearchNum.Text != "")
            {
                try
                {
                    Con.Open();
                    string query = "SELECT * FROM ParcelTbl WHERE PSPhone LIKE '%" + SearchNum.Text + "%' OR PRPhone LIKE '%" + SearchNum.Text + "%'";
                    SqlDataAdapter sda = new SqlDataAdapter(query, Con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    ParcelDGV.DataSource = dt;
                    Con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please enter a phone number to search.");
            }
        }
    }
}
