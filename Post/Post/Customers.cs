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

namespace Post
{
    public partial class Customers : Form
    {
        public Customers()
        {
            InitializeComponent();
            ShowCustomers();
            DateLbl.Text = DateTime.Today.Day + "-" + DateTime.Today.Month + "-" + DateTime.Today.Year;
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Studio\Documents\PostOfficeDB.mdf;Integrated Security=True;Connect Timeout=30");
        private void ShowCustomers()
        {
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("Select * from CustomerTbl", Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);   
            CustDGV.DataSource = ds.Tables[0];
            Con.Close();

        }
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (CusAddTb.Text == "" || CustNameTb.Text == "" || CustPhoneTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into CustomerTbl(CustName,CustDOB, CustPhone,CustAdd) values(@CN,@CD,@CP,@CA)", Con);
                    cmd.Parameters.AddWithValue("@CN",CustNameTb.Text);
                    cmd.Parameters.AddWithValue("@CD", CustDOB.Value.Date);
                    cmd.Parameters.AddWithValue("@CP", CustPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@CA", CusAddTb.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer Recorded");
                    Con.Close();
                    ShowCustomers();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (CusAddTb.Text == "" || CustNameTb.Text == "" || CustPhoneTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("update CustomerTbl set CustName=@CN,CustDOB=@CD, CustPhone=@CP,CustAdd=@CA where CustNum=@Ckey", Con);
                    cmd.Parameters.AddWithValue("@CN", CustNameTb.Text);
                    cmd.Parameters.AddWithValue("@CD", CustDOB.Value.Date);
                    cmd.Parameters.AddWithValue("@CP", CustPhoneTb.Text);
                    cmd.Parameters.AddWithValue("@CA", CusAddTb.Text);
                    cmd.Parameters.AddWithValue("@Ckey", Key );
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer Updated");
                    Con.Close();
                    ShowCustomers();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }
        int Key = 0;
        private void CustDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            CustNameTb.Text = CustDGV.SelectedRows[0].Cells[1].Value.ToString();
            CustDOB.Text = CustDGV.SelectedRows[0].Cells[2].Value.ToString();
            CustPhoneTb.Text = CustDGV.SelectedRows[0].Cells[3].Value.ToString();
            CusAddTb.Text = CustDGV.SelectedRows[0].Cells[4].Value.ToString();
            if(CustNameTb.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(CustDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }
        private void Reset()
        {
            CustNameTb.Text = "";
            CustPhoneTb.Text = "";
            CusAddTb.Text = "";
        }
        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (Key == 0 )
            {
                MessageBox.Show("Selecet a Customer");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete from CustomerTbl where Custnum = @CKey", Con);
                    cmd.Parameters.AddWithValue("@CKey", Key);
                   
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer Deleted");
                    Con.Close();
                    ShowCustomers();
                    Reset();
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

