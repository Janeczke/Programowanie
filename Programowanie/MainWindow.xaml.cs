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
using Newtonsoft.Json;
using Programowanie.models;
using System.IO;

namespace Programowanie
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<narzedzia_io> narzedzia_list;
        public MainWindow()
        {
            InitializeComponent();
            fetchJson();
        }
        public void fetchJson()
        {
            try
            {
                // Pobieranie danych z pliku json
                string json = System.IO.File.ReadAllText(@"narzedzia.json");
                
                // Konwersja do klasy News_io
                this.narzedzia_list = JsonConvert.DeserializeObject<List<narzedzia_io>>(json);
                var gridElem = FindName("MyToolBar") as ToolBar;
                for (int i = 0; i < this.narzedzia_list.Count; i++)
                {
                    var button = FindName("zdjecie"+i.ToString()) as Image;
                    button.Source = new BitmapImage(new Uri(this.narzedzia_list[i].background, UriKind.RelativeOrAbsolute));
                    Button button1 = new Button()
                    {
                        Content = string.Format(this.narzedzia_list[i].title, i),
                        Tag = i
                    };
                    button1.Click += new RoutedEventHandler(button_Click);
                    gridElem.Items.Add(button1);

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            string name = button.Content as string;
            int id = Convert.ToInt32(button.Tag);
            // Pobieranie danych z pliku json
            string json = System.IO.File.ReadAllText(@"narzedzia_org.json");
            // Konwersja do klasy News_io
            List<narzedzia_io> News_list_org = JsonConvert.DeserializeObject<List<narzedzia_io>>(json);

            string bg = "";
            for (int i = 0; i < News_list_org.Count; i++)
            {
                if (News_list_org[i].title == name)
                {
                    bg = News_list_org[i].background;
                }
            }

            bg = bg !="" ? bg : "/zdjecie/biale.png";

            foreach (var item in this.narzedzia_list)
            {
                if (item.id == id)
                {
                    item.background = bg;
                }
            }

            var newJson = JsonConvert.SerializeObject(this.narzedzia_list);
            System.IO.File.WriteAllText(@"narzedzia.json", newJson);
            fetchJson();
        }

        private void btnClick(object sender, RoutedEventArgs e)
        {

            if (MessageBox.Show("Czy chcesz zabrać narzędzie z organizera?",
        "",
        MessageBoxButton.YesNo,
        MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var button = sender as Button;
                int id = Convert.ToInt32(button.Tag);
                foreach (var item in this.narzedzia_list)
                {
                    if (item.id == id)
                    {
                        item.background = "/zjecie/biale.jpg";
                    }
                }

                var newJson = JsonConvert.SerializeObject(this.narzedzia_list);
                System.IO.File.WriteAllText(@"narzedzia.json", newJson);
                fetchJson();
            }




        }
        private void OpenBtn_Click(object sender, RoutedEventArgs e)
        {

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Text|*.txt";


            Nullable<bool> result = dlg.ShowDialog();


            if (result == true)
            {

               string filePath = dlg.FileName;


                //TextArea.Text = System.IO.File.ReadAllText(filePath);
            }
        }
    }

}
