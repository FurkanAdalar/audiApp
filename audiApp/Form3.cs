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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        DatabaseHelper dbHelper = new DatabaseHelper();

        private void Form3_Load(object sender, EventArgs e)
        {
            label1.BackColor = Color.Transparent;
            textBox1.Text = "E-posta adresi";
            textBox2.Text = "Şifre";
            textBox3.Text = "Şifreyi Tekrarlayın";
            textBox1.ForeColor = Color.LightGray;
            textBox2.ForeColor = Color.LightGray;
            textBox3.ForeColor = Color.LightGray;
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
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                textBox2.Text = "Şifre";
                textBox2.ForeColor = Color.LightGray;
            }
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            if (textBox3.Text == "Şifreyi Tekrarlayın")
            {
                textBox3.Text = "";
                textBox3.ForeColor = Color.Black;
            }
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                textBox3.Text = "Şifreyi Tekrarlayın";
                textBox3.ForeColor = Color.LightGray;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string eposta = textBox1.Text.Trim(); // Kullanıcının girdiği e-posta
            string sifre = textBox2.Text.Trim();   // Kullanıcının girdiği şifre
            string sifreTekrar = textBox3.Text.Trim(); // Kullanıcının tekrar şifresi

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

            if (string.IsNullOrEmpty(sifreTekrar) || sifreTekrar == "Şifreyi Tekrarlayın")
            {
                MessageBox.Show("Lütfen şifreyi tekrar girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (sifre != sifreTekrar)
            {
                MessageBox.Show("Şifreler uyuşmuyor.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                dbHelper.AddUser(eposta, sifre);
                MessageBox.Show("Hesabınız oluşturuldu. Giriş ekranına yönlendiriliyorsunuz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Form2 girisForm = new Form2();
                girisForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kayıt sırasında bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
