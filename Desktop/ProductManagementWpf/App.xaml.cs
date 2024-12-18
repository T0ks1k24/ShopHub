using Microsoft.Extensions.DependencyInjection;
using ProductManagementWpf.Services;
using ProductManagementWpf.ViewModels;
using ProductManagementWpf.Views;
using System.Configuration;
using System.Data;
using System.Net.Http;
using System.Windows;

namespace ProductManagementWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static HttpClient HttpClient = new HttpClient();
        public App()
        {
            InitializeComponent();
        }
    }

}
