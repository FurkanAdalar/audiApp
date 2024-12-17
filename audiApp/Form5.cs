using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace audiApp
{
    public partial class Form5 : Form
    {
        DatabaseHelper dbHelper = new DatabaseHelper();       
        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            List<string> modelNames = dbHelper.GetModelNames();

            // ComboBox'a verileri yükle
            comboBox1.Items.Clear();
            comboBox7.Items.Clear();
            foreach (string name in modelNames)
            {
                comboBox1.Items.Add(name);
                comboBox7.Items.Add(name);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Seçilen modelin adı
            string selectedModel = comboBox1.SelectedItem.ToString();

            // Modelin ID'sini al
            DatabaseHelper dbHelper = new DatabaseHelper();
            int modelId = dbHelper.GetModelIdByName(selectedModel);

            // Modelin özelliklerini al
            List<string> modelProperties = dbHelper.GetModelPropertiesById(modelId);

            // ListBox'a özellikleri yükle
            listBox2.Items.Clear();
            foreach (string property in modelProperties)
            {
                listBox2.Items.Add(property);
            }
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Placeholder for any functionality needed when the selection changes in comboBox7
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Seçilen modelin adı
            string selectedModel = comboBox1.SelectedItem.ToString();

            // Modelin ID'sini al
            DatabaseHelper dbHelper = new DatabaseHelper();
            int modelId = dbHelper.GetModelIdByName(selectedModel);

            // Modelin özelliklerini al
            List<string> modelProperties = dbHelper.GetModelPropertiesById(modelId);

            // ListBox'a özellikleri yükle
            listBox2.Items.Clear();
            foreach (string property in modelProperties)
            {
                listBox2.Items.Add(property);
            }
        }
    }
}
