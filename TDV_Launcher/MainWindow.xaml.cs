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

        private void changeLanguage(string language_code)
        {
            string[] imageResources = new[] { "Begin", "Chapters", "Exit", "Options", "Load", "Credits" };

            using var archive = new ZipArchive(File.Open(Environment.CurrentDirectory + @"\The Distant Valhalla.nvz", FileMode.Open, FileAccess.ReadWrite), ZipArchiveMode.Update);
            var entries = archive.Entries.ToArray();

            ZipArchiveEntry targetFile;
            ZipArchiveEntry targetFilePrevious;

            char[] ts = new char[] { 'T', 't' };

            foreach (var sourceFile in entries)
            {
                //If ZipArchiveEntry is a directory it will have its FullName property ending with "/" (e.g. "some_dir/") 
                //and its Name property will be empty string ("").
                if (sourceFile.Name.Equals($"index_{language_code}.xml"))
                {
                    archive.GetEntry($"index.xml").Delete();
                    targetFile = archive.CreateEntry("index.xml");

                    using (var indexLanguageXMLStream = sourceFile.Open())
                    using (var indexXMLStream = targetFile.Open())
                        indexLanguageXMLStream.CopyTo(indexXMLStream);

                    break;
                }

                //rename language specific image files
                foreach (var resource in imageResources)
                {
                    if (sourceFile.Name.Equals($"{resource}_{language_code}.png"))
                    {
                        foreach (var t in ts) //char[] ts = new char[] { 'T', 't' };
                        {

                            if ((targetFilePrevious = archive.GetEntry($"Assets/{t}extures/{resource}.png")) != null)
                                targetFilePrevious.Delete();

                            targetFile = archive.CreateEntry($"Assets/{t}extures/{resource}.png");

                            using (var sourceFileStream = sourceFile.Open())
                            using (var targetFileStream = targetFile.Open())
                                sourceFileStream.CopyTo(targetFileStream);

                        }

                        break;
                    }
                }
            }
        }

        private void Button_Click_English(object sender, RoutedEventArgs e)
        {
            changeLanguage("en");

            Process.Start(Environment.CurrentDirectory + @"\The Distant Valhalla.exe");

            Application.Current.Shutdown();
        }

        private void Button_Click_French(object sender, RoutedEventArgs e)
        {
            changeLanguage("fr");

            Process.Start(Environment.CurrentDirectory + @"\The Distant Valhalla.exe");

            Application.Current.Shutdown();
        }
    }
}
