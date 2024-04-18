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
using System.Data.SqlClient;

namespace Post
{
    public partial class Agents : Form
    {
        public Agents()
        {
            InitializeComponent();
            ShowAgents();
            DateLbl.Text = DateTime.Today.Day + "-" + DateTime.Today.Month + "-" + DateTime.Today.Year;
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Studio\Documents\PostOfficeDB.mdf;Integrated Security=True;Connect Timeout=30");
        private void ShowAgents()
        {
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("Select * from AgentTbl", Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            AgentsDGV.DataSource = ds.Tables[0];
            Con.Close();

        }
        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Reset()
        {
            ANameTb.Text = "";
            APhoneTb.Text = "";
            AddressTb.Text = "";
            APasswordTb.Text = "";
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (AddressTb.Text == "" || ANameTb.Text == "" || APasswordTb.Text == "" || APhoneTb.Text =="")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into AgentTbl(AgName,AgDOB, AgAdd, AgPhone,AgGen, AgPass) values(@AN,@AD,@AA,@AP,@AG,@APa)", Con);
                    cmd.Parameters.AddWithValue("@AN", ANameTb.Text);
                    cmd.Parameters.AddWithValue("@AD", ADOB.Value.Date);
                    cmd.Parameters.AddWithValue("@AA", AddressTb.Text);
                    cmd.Parameters.AddWithValue("@AP", APhoneTb.Text);
                    cmd.Parameters.AddWithValue("@AG", AgenCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@APa", APasswordTb.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Agent Recorded");
                    Con.Close();
                    ShowAgents();
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
            if (AddressTb.Text == "" || ANameTb.Text == "" || APasswordTb.Text == "" || APhoneTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("update AgentTbl Set AgName=@AN,AgDOB=@AD, AgAdd=@AA, AgPhone=@AP,AgGen=@AG, AgPass=@APa where AgNum=@Akey", Con);
                    cmd.Parameters.AddWithValue("@AN", ANameTb.Text);
                    cmd.Parameters.AddWithValue("@AD", ADOB.Value.Date);
                    cmd.Parameters.AddWithValue("@AA", AddressTb.Text);
                    cmd.Parameters.AddWithValue("@AP", APhoneTb.Text);
                    cmd.Parameters.AddWithValue("@AG", AgenCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@APa", APasswordTb.Text);
                    cmd.Parameters.AddWithValue("@AKey", Key);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Agent Updated");
                    Con.Close();
                    ShowAgents();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }
        int Key = 0;
        private void AgentsDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ANameTb.Text = AgentsDGV.SelectedRows[0].Cells[1].Value.ToString();
            ADOB.Value = Convert.ToDateTime(AgentsDGV.SelectedRows[0].Cells[2].Value.ToString());
            AddressTb.Text = AgentsDGV.SelectedRows[0].Cells[3].Value.ToString();
            APhoneTb.Text = AgentsDGV.SelectedRows[0].Cells[4].Value.ToString();
            AgenCb.Text = AgentsDGV.SelectedRows[0].Cells[5]. Value.ToString();   
            APasswordTb.Text = AgentsDGV.SelectedRows[0].Cells[6].Value.ToString();
            if (ANameTb.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(AgentsDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (Key == 0)
            {
                MessageBox.Show("Selecet a Agent");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete from AgentTbl where AgNum = @AKey", Con);
                    cmd.Parameters.AddWithValue("@AKey", Key);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Agent Deleted");
                    Con.Close();
                    ShowAgents();
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
            Login Obj = new Login();
            this.Hide();
            Obj.Show();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void AddressTb_TextChanged(object sender, EventArgs e)
        {

        }

        private void Agents_Load(object sender, EventArgs e)
        {

        }
    }
    }
    

