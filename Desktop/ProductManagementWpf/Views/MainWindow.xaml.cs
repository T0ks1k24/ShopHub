﻿using ProductManagementWpf.Views;
using ProductManagementWpf.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Extensions.DependencyInjection;

namespace ProductManagementWpf.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    
    public MainWindow()
    {
        InitializeComponent();
        var mainWindow = (MainWindow)Application.Current.MainWindow;
        mainWindow.MainFrame.Navigate(new LoginPage());
    }
}