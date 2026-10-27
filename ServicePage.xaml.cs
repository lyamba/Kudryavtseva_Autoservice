using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Kudryavtseva_Autoservice
{
    public partial class ServicePage : Page
    {
        int CountRecords;
        int CountPage;
        int CurrentPage = 0;
        List<Service> CurrentPageList = new List<Service>();
        List<Service> TableList;
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
            else if (ComboType.SelectedIndex == 1)
            {
                currentServices = currentServices.Where(p => p.Discount > 0 && p.Discount <= 0.05).ToList();
            }
            else if (ComboType.SelectedIndex == 2)
            {
                currentServices = currentServices.Where(p => p.Discount > 0.05 && p.Discount <= 0.15).ToList();
            }
            else if (ComboType.SelectedIndex == 3)
            {
                currentServices = currentServices.Where(p => p.Discount > 0.15 && p.Discount <= 0.30).ToList();
            }
            else if (ComboType.SelectedIndex == 4)
            {
                currentServices = currentServices.Where(p => p.Discount > 0.30 && p.Discount <= 0.70).ToList();
            }
            else if (ComboType.SelectedIndex == 5)
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
            TableList = currentServices;
            ChangePage(0, 0);
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

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                КудрявцеваАвтосервисEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                UpdateServices();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var currentService = (sender as Button).DataContext as Service;

            var currentClientServices = КудрявцеваАвтосервисEntities.GetContext().ClientService.ToList();
            currentClientServices = currentClientServices.Where(p => p.ServiceID == currentService.ID).ToList();
            if (currentClientServices.Count != 0)
            {
                MessageBox.Show("Невозможно выполнить удаление, так как существуют записи на эту услугу");
            }
            else
            {
                if (MessageBox.Show("Вы точно хотите удалить услугу?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        КудрявцеваАвтосервисEntities.GetContext().Service.Remove(currentService);
                        КудрявцеваАвтосервисEntities.GetContext().SaveChanges();
                        MessageBox.Show("Информация удалена");
                        UpdateServices();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
        }
        private void ChangePage(int direction, int? selectedPage)
        {
            CurrentPageList.Clear();
            CountRecords = TableList.Count;
            if (CountRecords % 10 > 0) CountPage = CountRecords / 10 + 1;
            else CountPage = CountRecords / 10;
            Boolean Ifupdate = true;
            int min;
            if (selectedPage.HasValue)
            {
                if (selectedPage >= 0 && selectedPage <= CountPage)
                {
                    CurrentPage = (int)selectedPage;
                    min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                    for (int i = CurrentPage * 10; i < min; i++)
                        CurrentPageList.Add(TableList[i]);
                }
            }
            else
            {
                switch (direction)
                {
                    case 1:
                        if (CurrentPage > 0)
                        {
                            CurrentPage--;
                            min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                            for (int i = CurrentPage * 10; i < min; i++)
                                CurrentPageList.Add(TableList[i]);
                        }
                        else Ifupdate = false;
                        break;
                    case 2:
                        if (CurrentPage < CountPage - 1)
                        {
                            CurrentPage++;
                            min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                            for (int i = CurrentPage * 10; i < min; i++)
                                CurrentPageList.Add(TableList[i]);
                        }
                        else Ifupdate = false;
                        break;
                }
            }
            if (Ifupdate)
            {
                PageListBox.Items.Clear();
                for (int i = 1; i <= CountPage; i++) PageListBox.Items.Add(i);
                PageListBox.SelectedIndex = CurrentPage;
                min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                TBCount.Text = min.ToString();
                TBAllRecords.Text = " из " + CountRecords.ToString();
                ServiceListView.ItemsSource = CurrentPageList;
                ServiceListView.Items.Refresh();
            }
        }

        private void LeftDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(1, null);
        }
        private void RightDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(2, null);
        }
        private void PageListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (PageListBox.SelectedItem != null)
            {
                ChangePage(0, Convert.ToInt32(PageListBox.SelectedItem.ToString()) - 1);
            }
        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            ((Image)sender).Source = new BitmapImage(new Uri("Resources/picture.png", UriKind.Relative));
        }
        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new SignUpPage((sender as Button).DataContext as Service));
        }
    }
}