using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using test.Classes;


namespace test.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void BtnSign_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxbLogin.Text))
                {
                    MessageBox.Show("Введите логин", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(TxbPassword.Text))
                {
                    MessageBox.Show("Введите пароль", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Проверка на блокировку
                var user = AppData.db.Users.FirstOrDefault
                    (u => u.Login == TxbLogin.Text);

                if (user != null && user.is_blocked == true)
                {
                    MessageBox.Show("Вы заблокированы. Обратитесь к администратору.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var CurrentUser = AppData.db.Users.FirstOrDefault
                    (u => u.Login == TxbLogin.Text && u.Password == TxbPassword.Text);

                if (CurrentUser == null)
                {
                    // Считаем попытки в БД
                    if (user != null)
                    {
                        user.attemps = (user.attemps ?? 0) + 1;
                        if (user.attemps >= 3)
                        {
                            user.is_blocked = true;
                            AppData.db.SaveChanges();
                            MessageBox.Show("Вы заблокированы. Обратитесь к администратору.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            AppData.db.SaveChanges();
                            MessageBox.Show("Вы ввели неверный логин или пароль.\nПожалуйста, проверьте ещё раз введенные данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Вы ввели неверный логин или пароль.\nПожалуйста, проверьте ещё раз введенные данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    // Сброс попыток при успешном входе
                    CurrentUser.attemps = 0;
                    AppData.db.SaveChanges();

                    switch (CurrentUser.RoleID)
                    {
                        case 1:
                            NavigationService.Navigate(new AdminPage());
                            MessageBox.Show("Вы успешно авторизовались");
                            break;
                        case 2:
                            NavigationService.Navigate(new UserPage());
                            MessageBox.Show("Вы успешно авторизовались");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}



