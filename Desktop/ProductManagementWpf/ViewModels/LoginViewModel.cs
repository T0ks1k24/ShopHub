using ProductManagementWpf.Commands;
using ProductManagementWpf.Models.User;
using ProductManagementWpf.Services;
using ProductManagementWpf.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Xps;

namespace ProductManagementWpf.ViewModels;

public class LoginViewModel : INotifyPropertyChanged
{
    private readonly IApiService _apiService;
    private string _username;
    private string _password;
    private string _errorMessage;

    public string Username
    {
        get => _username;
        set
        {
            _username = value;
            OnPropertyChanged();
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged();
        }
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged();
        }
    }

    public ICommand LoginCommand { get; }

    public LoginViewModel(IApiService apiService)
    {
        _apiService = apiService;
        LoginCommand = new RelayCommand(async () => await LoginAsync());
    }

    private async Task LoginAsync()
    {
        try
        {
            var loginRequest = new LoginRequest
            {
                Username = Username,
                Password = Password
            };

            var authResponse = await _apiService.LoginAsync(loginRequest);

            if (authResponse == null || string.IsNullOrEmpty(authResponse.Token))
            {
                ErrorMessage = "Login failed: Invalid username or password.";
                return;
            }

            // Saving the token and role
            App.Current.Properties["Token"] = authResponse.Token;
            App.Current.Properties["Role"] = authResponse.Role;

            // Go to the main window
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainFrame.Navigate(new HelloPage());
        }
        catch (ApiException apiEx)
        {
            ErrorMessage = $"Login failed: {apiEx.Message}";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Login failed: {ex.Message}";
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
