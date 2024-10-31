using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace sharujewellery
{
    public partial class Billings : Form
    {
        public Billings()
        {
            InitializeComponent();
            populate();
            displayCust();
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
        private void displayCust()
        {
            Con.Open();
            String query = "Select * from CustomerTable";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            CustomerDGV.DataSource = ds.Tables[0];
            Con.Close();

        }
        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }
        int key = 0, stock = 0;
        int CustKey = 0;
        
        private void Update()
        {
            try
            {
                int newStock = stock - Convert.ToInt32(QtyTb.Text);
                Con.Open();
                String query = "update ItemTable set itQty=" + newStock + " where itId=" + key + ";";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Item Update Successfully");
                Con.Close();
                populate();

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
    }
        int n = 0, GrdTotal=0;

        private void button3_Click(object sender, EventArgs e)
        {
            if(QtyTb.Text == "" || Convert.ToInt32(QtyTb.Text) > stock)
            {
                MessageBox.Show("No Stock");
            }
            else
            {
                int total = Convert.ToInt32(QtyTb.Text)* Convert.ToInt32(PriceTb.Text);
                DataGridViewRow newRow = new DataGridViewRow();
                newRow.CreateCells(BillDGV);
                newRow.Cells[0].Value = n + 1;
                newRow.Cells[1].Value = ProdNameTb.Text;
                newRow.Cells[2].Value = PriceTb.Text;
                newRow.Cells[3].Value = QtyTb.Text;
                newRow.Cells[4].Value = total;
                BillDGV.Rows.Add(newRow);
                GrdTotal = GrdTotal + total;
                    TotalLbl.Text = "Rs" + GrdTotal;
                n++;
                Update();
            }
        }

        private void PrintBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Con.Open();
                String query = "insert into BillTable values(" + CustKey+ ",'" + CustNameTb.Text + "',"+GrdTotal+")";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Bill Saved Successfully");
                Con.Close();
                

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }

            printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("pprnm", 285, 600);
            if(printPreviewDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }
        int prodid, prodqty, prodprice, tottal, pos = 60;

        private void BillDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void QtyTb_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Login Obj = new Login();
            Obj.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ViewBills Obj = new ViewBills();
            Obj.Show();
            this.Hide();
        }

        string prodname;
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString("Jewellery Shop", new Font("Century Gothic", 12, FontStyle.Bold), Brushes.Red, new Point(80));
            e.Graphics.DrawString("ID Product Price Quantity Total", new Font("Century Gothic", 10, FontStyle.Bold), Brushes.Red, new Point(26, 40));
            foreach (DataGridViewRow row in BillDGV.Rows)
            {
                prodid = Convert.ToInt32(row.Cells["Column1"].Value);
                prodname = "" + row.Cells["Column2"].Value;
                prodprice = Convert.ToInt32(row.Cells["Column3"].Value);
                prodqty = Convert.ToInt32(row.Cells["Column4"].Value);
                tottal = Convert.ToInt32(row.Cells["Column5"].Value);
                e.Graphics.DrawString("" + prodid, new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Blue, new Point(26, pos));
                e.Graphics.DrawString("" + prodname, new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Blue, new Point(45, pos));
                e.Graphics.DrawString("" + prodprice, new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Blue, new Point(120, pos));
                e.Graphics.DrawString("" + prodqty, new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Blue, new Point(170, pos));
                e.Graphics.DrawString("" + tottal, new Font("Century Gothic", 8, FontStyle.Bold), Brushes.Blue, new Point(235, pos));
                pos = pos + 20;
            }
            e.Graphics.DrawString("Grand Total: RS" + GrdTotal, new Font("Century Gothic", 12, FontStyle.Bold), Brushes.Crimson, new Point(50, pos + 50));
            e.Graphics.DrawString("***********JewelleryShop***********", new Font("Century Gothic", 10, FontStyle.Bold), Brushes.Crimson, new Point(11, pos + 85));
            BillDGV.Rows.Clear(); 
            BillDGV.Refresh(); 
            pos = 100;
            GrdTotal = 0;
        }

        private void CustomerDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            CustNameTb.Text = CustomerDGV.SelectedRows[0].Cells[1].Value.ToString();
            

            if (CustNameTb.Text == "")
            {
                CustKey = 0;
            }
            else
            {
                CustKey = Convert.ToInt32(CustomerDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        
        private void ItemDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            ProdNameTb.Text = ItemDGV.SelectedRows[0].Cells[1].Value.ToString();
            
            PriceTb.Text = ItemDGV.SelectedRows[0].Cells[4].Value.ToString();
           

            if (ProdNameTb.Text == "")
            {
                key = 0;
                stock= 0;
            }
            else
            {
                key = Convert.ToInt32(ItemDGV.SelectedRows[0].Cells[0].Value.ToString());
                stock = Convert.ToInt32(ItemDGV.SelectedRows[0].Cells[5].Value.ToString());
            }
        }
    }
}
