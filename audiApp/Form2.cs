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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            label1.BackColor = Color.Transparent;
            textBox1.Text = "E-posta adresi";
            textBox2.Text = "Şifre";
            textBox1.ForeColor = Color.LightGray;
            textBox2.ForeColor = Color.LightGray;

            // Şifre alanını maskele
            textBox2.UseSystemPasswordChar = false;
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
            if (string.IsNullOrWhiteSpace(textBox1.Text))
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
                textBox2.UseSystemPasswordChar = true; // Şifre maskesini etkinleştir
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                textBox2.UseSystemPasswordChar = false; // Şifre maskesini devre dışı bırak
                textBox2.Text = "Şifre";
                textBox2.ForeColor = Color.LightGray;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form3 kayitForm = new Form3();
            kayitForm.Show();
            this.Hide();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form4 adminForm = new Form4();
            adminForm.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DatabaseHelper dbHelper = new DatabaseHelper();

            string eposta = textBox1.Text.Trim(); // Kullanıcının girdiği e-posta (Boşlukları temizledik)
            string sifre = textBox2.Text.Trim();   // Kullanıcının girdiği şifre (Boşlukları temizledik)

            if (string.IsNullOrEmpty(eposta) || eposta == "E-posta adresi")
            {
                MessageBox.Show("Lütfen geçerli bir e-posta adresi girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(sifre) || sifre == "Şifre")
            {
                MessageBox.Show("Lütfen şifre girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (dbHelper.CheckUserLogin(eposta, sifre))
                {
                    MessageBox.Show("Giriş başarılı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Form6 anaForm = new Form6();
                    anaForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("E-posta veya şifre hatalı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Giriş hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
