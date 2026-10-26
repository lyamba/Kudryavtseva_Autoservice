using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Kudryavtseva_Autoservice
{
    public partial class ServicePage : Page
    {
        public ServicePage()
        {
            InitializeComponent();

            ComboType.SelectedIndex = 0;

            UpdateServices();
        }

        private void UpdateServices()
        {
            var currentServices = КудрявцеваАвтосервисEntities.GetContext().Service.ToList();

            if (ComboType.SelectedIndex == 0)
            {

            }
            else if (ComboType.SelectedIndex == 1) // от 0 до 5%
            {
                currentServices = currentServices.Where(p => p.Discount > 0 && p.Discount <= 0.05).ToList();
            }
            else if (ComboType.SelectedIndex == 2) // от 5 до 15%
            {
                currentServices = currentServices.Where(p => p.Discount > 0.05 && p.Discount <= 0.15).ToList();
            }
            else if (ComboType.SelectedIndex == 3) // от 15 до 30%
            {
                currentServices = currentServices.Where(p => p.Discount > 0.15 && p.Discount <= 0.30).ToList();
            }
            else if (ComboType.SelectedIndex == 4) // от 30 до 70%
            {
                currentServices = currentServices.Where(p => p.Discount > 0.30 && p.Discount <= 0.70).ToList();
            }
            else if (ComboType.SelectedIndex == 5) // от 70 до 100%
            {
                currentServices = currentServices.Where(p => p.Discount > 0.70 && p.Discount <= 1.0).ToList();
            }

            if (!string.IsNullOrWhiteSpace(TBoxSearch.Text))
            {
                currentServices = currentServices.Where(p => p.Title.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();
            }

            if (RadioButtonUp.IsChecked == true)
            {
                currentServices = currentServices.OrderBy(p => p.Cost).ToList();
            }
            else if (RadioButtonDown.IsChecked == true)
            {
                currentServices = currentServices.OrderByDescending(p => p.Cost).ToList();
            }

            ServiceListView.ItemsSource = currentServices;
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateServices();
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ServiceListView != null)
            {
                UpdateServices();
            }
        }

        private void RadioButtonDown_Checked(object sender, RoutedEventArgs e)
        {
            if (ServiceListView != null)
            {
                UpdateServices();
            }
        }

        private void RadioButtonUp_Checked(object sender, RoutedEventArgs e)
        {
            if (ServiceListView != null)
            {
                UpdateServices();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage((sender as Button).DataContext as Service));
        }

        private void ChangePage(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                КудрявцеваАвтосервисEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                UpdateServices();
            }
        }
    }
}