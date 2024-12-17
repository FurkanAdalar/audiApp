using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace audiApp
{
    public partial class Form6 : Form
    {
        private readonly DatabaseHelper dbHelper;
        private byte[] selectedPhotoData = null;

        public Form6()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;
        }




        private void button1_Click(object sender, EventArgs e)
        {
            if (selectedPhotoData == null)
            {
                MessageBox.Show("Lütfen önce bir resim seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kullanıcıdan alınan bilgiler
            string name = textBox8.Text;
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Model ismini girin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(textBox7.Text, out decimal price))
            {
                MessageBox.Show("Geçerli bir fiyat girin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string color = textBox6.Text;
            if (string.IsNullOrWhiteSpace(color))
            {
                MessageBox.Show("Renk bilgisini girin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string type = textBox1.Text;
            if (string.IsNullOrWhiteSpace(type))
            {
                MessageBox.Show("Tip bilgisini girin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(textBox5.Text, out int year))
            {
                MessageBox.Show("Geçerli bir yıl girin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Veritabanına ekleme
            try
            {
                dbHelper.AddModel(name, type, price, color, year, selectedPhotoData);
                MessageBox.Show("Model ve resim başarıyla kaydedildi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.bmp";
                openFileDialog.Title = "Bir resim seçin";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    selectedPhotoData = File.ReadAllBytes(filePath); // Resmi binary olarak oku
                    MessageBox.Show("Resim başarıyla seçildi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Resim seçilmedi!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void LoadDataToListBox(int id)
        {
            try
            {
                List<string> data = dbHelper.GetDataById(id);

                if (data.Count > 0)
                {
                    listBox1.Items.Clear(); // ListBox'ı temizle
                    foreach (var item in data)
                    {
                        listBox1.Items.Add(item); // Her bir veri satırını ListBox'a ekle
                    }
                }
                else
                {
                    MessageBox.Show("Bu ID'ye ait veri bulunamadı.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox12.Text, out int id))
            {
                LoadDataToListBox(id); // ID'ye göre veriyi çek ve ListBox'a aktar
            }
            else
            {
                MessageBox.Show("Lütfen geçerli bir ID girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Kullanıcının girdiği ID'yi al
            if (int.TryParse(textBox12.Text, out int id))
            {
                // Kullanıcıdan emin olup olmadığını sor
                DialogResult result = MessageBox.Show(
                    $"ID {id} olan kaydı silmek istediğinizden emin misiniz?",
                    "Silme Onayı",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes) // Kullanıcı 'Evet' dediyse
                {
                    bool isDeleted = dbHelper.DeleteById(id);

                    if (isDeleted)
                    {
                        MessageBox.Show($"ID {id} başarıyla silindi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"ID {id} bulunamadı veya silinemedi!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Silme işlemi iptal edildi.", "İptal", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Geçerli bir ID giriniz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Kullanıcılar sekmesine gelindiyse veriyi yükle
            if (tabControl1.SelectedTab == tabPage3)
            {
                LoadUsersData();
            }
            else if (tabControl1.SelectedTab == tabPage5)
            {
                LoadModelsData();
            }
        }

        private void LoadUsersData()
        {
            try
            {
                string query = "SELECT * FROM Users"; // Kullanıcıları getiren sorgu
                var usersData = dbHelper.ExecuteQuery(query);
                dataGridView1.DataSource = usersData; // DataGridView'e veri bağla
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Veriler yüklenirken bir hata oluştu: {ex.Message}");
            }
        }

        private void LoadModelsData()
        {
            try
            {
                string query = "SELECT * FROM models"; // Models tablosundan verileri çek
                var modelsData = dbHelper.ExecuteQuery(query);
                dataGridView2.DataSource = modelsData; // DataGridView'e veri bağla
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Modeller yüklenirken bir hata oluştu: {ex.Message}");
            }
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            // Placeholder for any functionality needed when the text changes
        }



    }
}
