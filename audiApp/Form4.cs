using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace audiApp
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            label1.BackColor = Color.Transparent;
            textBox1.Text = "E-posta adresi";
            textBox2.Text = "Şifre";
            textBox1.ForeColor = Color.LightGray;
            textBox2.ForeColor = Color.LightGray;
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "E-posta adresi")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "E-posta adresi";
                textBox1.ForeColor = Color.LightGray;
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == "Şifre")
            {
                textBox2.Text = "";
                textBox2.ForeColor = Color.Black;
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.Text = "Şifre";
                textBox2.ForeColor = Color.LightGray;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DatabaseHelper dbHelper = new DatabaseHelper();

            string eposta = textBox1.Text; // Kullanıcının girdiği e-posta
            string sifre = textBox2.Text;   // Kullanıcının girdiği şifre

            if (dbHelper.CheckUserLogin(eposta, sifre))
            {
                MessageBox.Show("Giriş Yapıldı Admin Sayfasına Yönlendiriliyorsunuz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Form6 admingiris = new Form6();
                admingiris.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("E-posta veya şifre hatalı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
