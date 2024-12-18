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

namespace ProductManagementWpf.Views;

/// <summary>
/// Interaction logic for HomePage.xaml
/// </summary>
public partial class HelloPage : Page
{
    public HelloPage()
    {
        InitializeComponent();
        OpenWin();
    }

    public async Task OpenWin()
    {
        await Task.Delay(2000);
        var mainWindow = (MainWindow)Application.Current.MainWindow;
        mainWindow.MainFrame.Navigate(new ProductPage());
    }
}
