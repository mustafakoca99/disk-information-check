using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace dosyavedizinkontrol
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public DriveInfo[] suruculer;
        public DirectoryInfo suankikonum;
        private void Form1_Load(object sender, EventArgs e)
        {
            suruculer = DriveInfo.GetDrives();
            foreach (DriveInfo item in suruculer.Where(s=>s.IsReady))
            {
                comboBox1.Items.Add(item.Name);
            }
            
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex>-1)
            {
                
                DriveInfo bilgi =
                    suruculer.Where((s => s.Name == comboBox1.SelectedItem.ToString())).SingleOrDefault();

                double bosalan = bilgi.AvailableFreeSpace / (1024 * 1024 * 1024);
                double toplamalan = bilgi.TotalSize / (1024 * 1024 * 1024);

                toolStripStatusLabel1.Text = string.Format("{0}, {1} - boş alan: {2} gb - toplam {3} gb",
                    bilgi.Name, bilgi.VolumeLabel, bosalan, toplamalan
                    );

                suankikonum = bilgi.RootDirectory;

                dizinleriguncelle();
            }
        }

        private void dizinleriguncelle()
        {
            listBox1.Items.Clear();
            listBox1.Items.Add("...");
            DirectoryInfo[] dizinler = suankikonum.GetDirectories();
            foreach  (DirectoryInfo dizin in dizinler)
            {
                listBox1.Items.Add(dizin.Name);
            }

            listBox2.Items.Clear();
            FileInfo[] dosyalar = suankikonum.GetFiles();
            foreach (FileInfo item in dosyalar)
            {
                listBox2.Items.Add(item.Name);
            }
        }

        private void ListBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (listBox1.SelectedIndex > -1)
            {
                string secilen_ismi = listBox1.SelectedItem.ToString();
                
                    if (secilen_ismi == "...")
                    {
                        suankikonum = suankikonum.Parent;
                        dizinleriguncelle();
                        return;
                    }
                
                suankikonum = suankikonum.GetDirectories().Where(
                    s => s.Name == secilen_ismi
                    ).SingleOrDefault();
                //bunu koymazsan hata verir :)
                dizinleriguncelle();
            }
            }
            catch (Exception)
            {
                MessageBox.Show("en üst dizin burası!!");
            }
        }
    }
}
