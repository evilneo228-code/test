using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using test.Classes;
using test.Models;

namespace test.Pages
{
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
            LoadUsers();
        }
        
        void LoadUsers()
        {
            LbUsers.Items.Clear();
            foreach (var u in AppData.db.Users)
                LbUsers.Items.Add(u);
            LbUsers.DisplayMemberPath = "Login";
        }

        void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxbLogin.Text))
            {
                MessageBox.Show("Введите логин!");
                return;
            }

            if (AppData.db.Users.Any
                (u => u.Login == TxbLogin.Text))

            {
                MessageBox.Show("Пользователь с таким логином уже существует!");
                return;
            }

            AppData.db.Users.Add(new Users
            {
                Login = TxbLogin.Text,
                Password = TxbPassword.Text,
                RoleID = CmbRole.SelectedIndex == 0 ? 2 : 1,
                is_blocked = (bool)ChkBlocked.IsChecked,
                attemps = 0
            });

            AppData.db.SaveChanges();
            LoadUsers();
            ClearForm();
            MessageBox.Show("Пользователь добавлен!");
        }

        void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var u = LbUsers.SelectedItem as Users;
            if (u == null)
            {
                MessageBox.Show("Выберите пользователя!");
                return;
            }

            u.Login = TxbLogin.Text;
            u.Password = TxbPassword.Text;
            u.RoleID = CmbRole.SelectedIndex == 0 ? 2 : 1;
            u.is_blocked = (bool)ChkBlocked.IsChecked;
            AppData.db.SaveChanges();
            LoadUsers();
            ClearForm();
            MessageBox.Show("Данные обновлены!");
        }

        void LbUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var u = LbUsers.SelectedItem as Users;
            if (u != null)
            {
                TxbLogin.Text = u.Login;
                TxbPassword.Text = u.Password;
                CmbRole.SelectedIndex = u.RoleID == 2 ? 0 : 1;
                ChkBlocked.IsChecked = u.is_blocked;
            }
        }

        void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }

        void ClearForm()
        {
            TxbLogin.Text = "";
            TxbPassword.Text = "";
            CmbRole.SelectedIndex = 0;
            ChkBlocked.IsChecked = false;
            LbUsers.SelectedItem = null;
        }
    }
}