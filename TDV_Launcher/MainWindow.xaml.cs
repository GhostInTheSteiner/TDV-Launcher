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
using System.IO.Compression;
using System.Diagnostics;

namespace TDV_Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void useIndexFile(string language_code)
        {
            using var archive = new ZipArchive(File.Open(Environment.CurrentDirectory + @"\The Distant Valhalla.nvz", FileMode.Open, FileAccess.ReadWrite), ZipArchiveMode.Update);
            var entries = archive.Entries.ToArray();

            ZipArchiveEntry indexXML;

            foreach (var indexLanguageXML in entries)
            {
                //If ZipArchiveEntry is a directory it will have its FullName property ending with "/" (e.g. "some_dir/") 
                //and its Name property will be empty string ("").
                if (indexLanguageXML.Name.Equals($"index_{language_code}.xml"))
                {
                    archive.GetEntry($"index.xml").Delete();
                    indexXML = archive.CreateEntry("index.xml");

                    using (var indexLanguageXMLStream = indexLanguageXML.Open())
                    using (var indexXMLStream = indexXML.Open())
                        indexLanguageXMLStream.CopyTo(indexXMLStream);

                    break;
                }
            }
        }

        private void Button_Click_English(object sender, RoutedEventArgs e)
        {
            useIndexFile("en");

            Process.Start(Environment.CurrentDirectory + @"\The Distant Valhalla.exe");

            Application.Current.Shutdown();
        }

        private void Button_Click_French(object sender, RoutedEventArgs e)
        {
            useIndexFile("fr");

            Process.Start(Environment.CurrentDirectory + @"\The Distant Valhalla.exe");

            Application.Current.Shutdown();
        }
    }
}
