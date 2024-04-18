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
    public partial class MoneyTrans : Form
    {
        public MoneyTrans()
        {
            InitializeComponent();
            ShowTransfers();
            AgentNameLbl.Text = Login.AName;
            DateLbl.Text = DateTime.Today.Day + "-" + DateTime.Today.Month + "-" + DateTime.Today.Year;
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Studio\Documents\PostOfficeDB.mdf;Integrated Security=True;Connect Timeout=30");
        private void ShowTransfers()
        {
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("Select * from MoneyTrTbl", Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            TransferDGV.DataSource = ds.Tables[0];
            Con.Close();

        }
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (SNameTb.Text == "" || RNameTb.Text == "" || SPhoneTb.Text == "" || RPhoneTb.Text == "" || PinCOdeTB.Text == "" || RAddressTb.Text == "" || SAddressTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into MoneyTrTbl(TDate,SName,RName, SAdd, Sphone, Rphone, TrCode) values(@TD,@SN,@RN,@SA,@SP,@RP,@TC)", Con);
                    cmd.Parameters.AddWithValue("@TD", DateTime.Today.Date);
                    cmd.Parameters.AddWithValue("@SN", SNameTb.Text);
                    cmd.Parameters.AddWithValue("@RN", RNameTb.Text);
                    cmd.Parameters.AddWithValue("@SA", SAddressTb.Text);
                    cmd.Parameters.AddWithValue("@RA", RAddressTb.Text);
                    cmd.Parameters.AddWithValue("@SP", SPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@RP", RPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@TC", PinCOdeTB.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Successfull Transaction");
                    Con.Close();
                    ShowTransfers();
                    // Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (SNameTb.Text == "" || RNameTb.Text == "" || SPhoneTb.Text == "" || RPhoneTb.Text == "" || PinCOdeTB.Text == "" || SAddressTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("update MoneyTrTbl set TDate=@TD,SName=@SN,RName=@RN, SAdd=@SA, Sphone=@SP, Rphone=@RP, TrCode=@TC where TrNum=@TKey", Con);
                    cmd.Parameters.AddWithValue("@TD", DateTime.Today.Date);
                    cmd.Parameters.AddWithValue("@SN", SNameTb.Text);
                    cmd.Parameters.AddWithValue("@RN", RNameTb.Text);
                    cmd.Parameters.AddWithValue("@SA", SAddressTb.Text);
                    cmd.Parameters.AddWithValue("@RA", RAddressTb.Text);
                    cmd.Parameters.AddWithValue("@SP", SPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@RP", RPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@TC", PinCOdeTB.Text);
                    cmd.Parameters.AddWithValue("@TKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Updated Transaction");
                    Con.Close();
                    ShowTransfers();
                    // Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        int Key = 0;
        private void TransferDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            TrDate.Text = TransferDGV.SelectedRows[0].Cells[1].Value.ToString();
            SNameTb.Text = TransferDGV.SelectedRows[0].Cells[2].Value.ToString();
            RNameTb.Text = TransferDGV.SelectedRows[0].Cells[3].Value.ToString();
            SAddressTb.Text = TransferDGV.SelectedRows[0].Cells[4].Value.ToString();
            SPhoneTb.Text = TransferDGV.SelectedRows[0].Cells[5].Value.ToString();
            RPhoneTb.Text = TransferDGV.SelectedRows[0].Cells[6].Value.ToString();
            PinCOdeTB.Text = TransferDGV.SelectedRows[0].Cells[7].Value.ToString();
            if (SNameTb.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(TransferDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
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
                    SqlCommand cmd = new SqlCommand("delete from MoneyTrTbl where TrNum = @TKey", Con);
                    cmd.Parameters.AddWithValue("@TKey", Key);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Transfer Deleted");
                    Con.Close();
                    ShowTransfers();
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
    }
}
