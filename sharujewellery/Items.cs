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
namespace sharujewellery
{
    public partial class Items : Form
    {
        public Items()
        {
            InitializeComponent();
            populate();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\Naajiul hasan\Documents\jewelleryDb.mdf"";Integrated Security=True;Connect Timeout=30");
        private void populate()
        {
            Con.Open();
            String query = "Select * from ItemTable";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            ItemDGV.DataSource = ds.Tables[0];
            Con.Close();

            
        }
        private void FilterByCat()
        {
            Con.Open();
            String query = "Select * from ItemTable where itCat= '"+FilterCat.SelectedItem.ToString()+"'";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            ItemDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void FilterByType()
        {
            Con.Open();
            String query = "Select * from ItemTable where itType= '" + FilterType.SelectedItem.ToString() + "'";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            ItemDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (itName.Text == "" || PriceTb.Text == "" || QtyTb.Text == "" || CatCb.SelectedIndex == -1 || TypeCb.SelectedIndex == -1)
            {
                MessageBox.Show ("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open ();
                    String query = "insert into ItemTable values('" + itName.Text + "','" + CatCb.SelectedItem.ToString() + "','" + TypeCb.SelectedItem.ToString() + "','" + PriceTb.Text + "','" + QtyTb.Text + "')";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Item Saved Successfully");
                    Con.Close ();
                    populate();
                    Reset();

                }
                catch (Exception Ex) { 
                    MessageBox.Show(Ex.Message);
                }        
            }
        }

        
        private void ItemDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
                }

        private void Reset()
        {
            itName.Text = "";
            CatCb.SelectedIndex = -1;
            TypeCb.SelectedIndex = -1;
            PriceTb.Text = "";
            QtyTb.Text = "";
            key = 0;
                }
        private void ResetBtn_Click(object sender, EventArgs e)
        {
            Reset();
        }

        int key = 0;
        private void ItemDGV_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            itName.Text = ItemDGV.SelectedRows[0].Cells[1].Value.ToString();
            CatCb.SelectedItem = ItemDGV.SelectedRows[0].Cells[2].Value.ToString();
            TypeCb.SelectedItem = ItemDGV.SelectedRows[0].Cells[3].Value.ToString();
            PriceTb.Text = ItemDGV.SelectedRows[0].Cells[4].Value.ToString();
            QtyTb.Text = ItemDGV.SelectedRows[0].Cells[5].Value.ToString();

            if (itName.Text == "")
            {
                key = 0;
            }
            else
            {
                key = Convert.ToInt32(ItemDGV.SelectedRows[0].Cells[0].Value.ToString());
            }

        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {

            if (key == 0)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    String query = "Delete From ItemTable where itId=" + key + ";";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Item Deleted Successfully");
                    Con.Close();
                    populate();
                    Reset();

                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            if (itName.Text == "" || PriceTb.Text == "" || QtyTb.Text == "" || CatCb.SelectedIndex == -1 || TypeCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    String query = "update ItemTable set itName='" + itName.Text + "',itCat='" + CatCb.SelectedItem.ToString() + "',itType='" + TypeCb.SelectedItem.ToString() + "',itPrice='" + PriceTb.Text + "',itQty='" + QtyTb.Text + "' where itId=" + key + ";";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Item Update Successfully");
                    Con.Close();
                    populate();
                    Reset();    

                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }

        }

        private void label2_Click(object sender, EventArgs e)
        {
            Customers Obj = new Customers();
            Obj.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void FilterCat_SelectionChangeCommitted(object sender, EventArgs e)
        {
            FilterByCat();
        }

        private void FilterType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            FilterByType();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            populate();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
