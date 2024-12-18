using ProductManagementWpf.Services;
using ProductManagementWpf.ViewModels;
using System.Diagnostics;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;


namespace ProductManagementWpf.Views;

/// <summary>
/// Interaction logic for RegisterPage.xaml
/// </summary>
public partial class RegisterPage : Page
{
   

    public RegisterPage()
    {
        InitializeComponent();
        var httpClient = new HttpClient();
        DataContext = new RegisterViewModel(new ApiService(httpClient));
    }

    private void LoginForAnAccount_Click(object sender, RoutedEventArgs e)
    {
        var mainWindow = (MainWindow)Application.Current.MainWindow;
        mainWindow.MainFrame.Navigate(new LoginPage());
    }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is RegisterViewModel viewModel)
        {
            if (sender is PasswordBox passwordBox)
            {
                if (passwordBox.Name == "PasswordTextBox1")
                {
                    viewModel.Password = passwordBox.Password;
                }
                else if (passwordBox.Name == "PasswordTextBox2")
                {
                    viewModel.ConfirmPassword = passwordBox.Password;
                }

                // Перевірка паролів
                if (viewModel.Password != viewModel.ConfirmPassword)
                {
                    viewModel.ErrorMessage = "Passwords do not match.";
                }
                else
                {
                    viewModel.ErrorMessage = string.Empty; // Очищення помилки
                }
            }
        }
    }


}



