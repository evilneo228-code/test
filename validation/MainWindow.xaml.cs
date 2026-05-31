using System.IO;
using System.Linq;
using System.Net;
using System.Windows;


namespace validation
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string fio;

        public MainWindow()
        {
            InitializeComponent();
        }

        void GetData(object sender, RoutedEventArgs e)
        {
            var req = WebRequest.Create("http://localhost:4444/TransferSimulator/fullName");
            var res = req.GetResponse();
            var sr = new StreamReader(res.GetResponseStream());
            string json = sr.ReadToEnd();
            sr.Close();
            res.Close();

            fio = json.Split('"')[3];
            FioBox.Text = fio;
        }

        void PushData(object sender, RoutedEventArgs e)
        {
            bool ok = true;

            // Проверка на цифры
            foreach (char c in fio)
                if (c >= '0' && c <= '9')
                    ok = false;

            // Проверка на спецсимволы
            string spec = "!@#$%^&*();+/|=?:";
            foreach (char c in spec)
                if (fio.Contains(c))
                    ok = false;

            if (ok)
                ResultText.Text = "Успешно";
            else
                ResultText.Text = "ФИО содержит запрещенные символы";
        }
    }
}