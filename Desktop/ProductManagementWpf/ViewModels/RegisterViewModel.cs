using ProductManagementWpf.Commands;
using ProductManagementWpf.Models.User;
using ProductManagementWpf.Services;
using ProductManagementWpf.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ProductManagementWpf.ViewModels;

public class RegisterViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
{
    private readonly IApiService _apiService;
    private string _username;
    private string _password;
    private string _confirmPassword;
    private string _errorMessage;

    private readonly Dictionary<string, List<string>> _errors = new();

    public string Username
    {
        get => _username;
        set
        {
            _username = value;
            OnPropertyChanged();
            ValidateProperty(nameof(Username), value);
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged();
            ValidateProperty(nameof(Password), value);
        }
    }

    public string ConfirmPassword
    {
        get => _confirmPassword;
        set
        {
            _confirmPassword = value;
            OnPropertyChanged();
            ValidateProperty(nameof(ConfirmPassword), value);
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

    public ICommand RegisterCommand { get; }

    public RegisterViewModel(IApiService apiService)
    {
        _apiService = apiService;
        RegisterCommand = new RelayCommand(async () => await RegisterAsync(), CanExecuteRegister);
    }

    private async Task RegisterAsync()
    {
        try
        {
            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Passwords do not match";
                return;
            }

            var registerRequest = new RegisterRequest
            {
                Username = Username,
                Password = Password
            };

            var authResponse = await _apiService.RegisterAsync(registerRequest);

            // Переходимо на екран логіну
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainFrame.Navigate(new LoginPage());
        }
        catch (Exception ex)
        {
            ErrorMessage = "Registration failed: " + ex.Message;
        }
    }

    private bool CanExecuteRegister()
    {
        return !HasErrors;
    }

    #region Validation Logic

    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

    public bool HasErrors => _errors.Any();

    public IEnumerable GetErrors(string propertyName)
    {
        if (string.IsNullOrEmpty(propertyName) || !_errors.ContainsKey(propertyName))
            return null;

        return _errors[propertyName];
    }

    private void ValidateProperty(string propertyName, string value)
    {
        _errors.Remove(propertyName);

        switch (propertyName)
        {
            case nameof(Username):
                if (string.IsNullOrWhiteSpace(value))
                    AddError(propertyName, "Username cannot be empty.");
                break;

            case nameof(Password):
                if (string.IsNullOrWhiteSpace(value))
                    AddError(propertyName, "Password cannot be empty.");
                else if (value.Length < 6)
                    AddError(propertyName, "Password must be at least 6 characters long.");
                break;

            case nameof(ConfirmPassword):
                if (value != Password)
                    AddError(propertyName, "Passwords do not match.");
                break;
        }

        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        ((RelayCommand)RegisterCommand).RaiseCanExecuteChanged();
    }

    private void AddError(string propertyName, string errorMessage)
    {
        if (!_errors.ContainsKey(propertyName))
            _errors[propertyName] = new List<string>();

        _errors[propertyName].Add(errorMessage);
    }

    #endregion

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

