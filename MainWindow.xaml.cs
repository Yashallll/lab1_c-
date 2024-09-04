using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;
using System.Data;

namespace Лаба_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DateTime odds;
        public MainWindow()
        {
            InitializeComponent();
            WPF1_Load();
            odds = DateTime.Now;
        }
        private void WPF1_Load()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                cb1.Items.Add(drive);//список дисков
            }
        }//при запуске считаем диски

        private void listBox1(string path)//для подсчета папок на диске
        {
            LB1.Items.Clear();
            LB2.Items.Clear();
            string[] dirs = Directory.GetDirectories(path);
            foreach (string s in dirs)
            {
                LB1.Items.Add(s);
            }

        }

        private void listBox2(string path)//для подсчета файлов в папке
        {
            LB2.Items.Clear();
            string[] files = Directory.GetFiles(path);
            foreach (string s in files)
            {
                LB2.Items.Add(s);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)//если изменили диск
        {

            string path = cb1.SelectedItem.ToString();
            listBox1(path);
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                if (drive.ToString() == path)
                {
                    InfoDisk.Content = drive.Name + "\nРазмер:" + drive.TotalSize.ToString() + " байт" + "\nСвободно места: " + drive.TotalFreeSpace.ToString() + " байт";
                }
            }


        }

        private void ListBox1_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)//если изменили каталог
        {
            try
            {
                if (LB1.Items.Count != 0 || LB1.SelectedItem != null)
                {
                    string path = LB1.SelectedItem.ToString();
                    listBox2(path);
                    DirectoryInfo dirInfo = new DirectoryInfo(path);
                    InfoDir.Content = "Название: " + dirInfo.FullName + "\nВремя создания: " + dirInfo.CreationTime + "\nДиск: " + dirInfo.Root;
                }
            }
            catch
            {
                MessageBox.Show("Невозможно получить доступ!");
            }
        }

        int count = 0;
        Dictionary<string, DateTime> tensec = new Dictionary<string, DateTime>(); //словарь
        private void ListBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LB2.Items.Count != 0 || LB2.SelectedItem != null)
            {
                TimeSpan ts = TimeSpan.FromTicks(DateTime.Now.Ticks);
                double date = ts.TotalSeconds;
                Process.Start(new ProcessStartInfo { FileName = LB2.SelectedItem.ToString(), UseShellExecute = true });
                tensec.Add(LB2.SelectedItem.ToString(), DateTime.Now); //добавление данных в словарь
            }
        }//для запуска приложения

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StreamWriter writer = File.CreateText("File.txt");

            foreach (var s in tensec)
            {
                TimeSpan ts = odds - DateTime.Now;
                if (ts.Seconds <= 10)
                {
                    writer.WriteLine(s.Key);
                }
            }

            writer.Close();
            Close();
            Process.Start(new ProcessStartInfo { FileName = "File.txt", UseShellExecute = true });
        }
    }
}
