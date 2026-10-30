using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Kudryavtseva_Autoservice
{
    public partial class AddEditPage : Page
    {
        private Service _currentService = new Service();

        public AddEditPage(Service selectedService)
        {
            InitializeComponent();

            if (selectedService != null)
                _currentService = selectedService;

            DataContext = _currentService;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentService.Title))
                errors.AppendLine("Укажите название услуги");

            if (_currentService.Cost == 0)
                errors.AppendLine("Укажите стоимость услуги");

            if (_currentService.Duration < 0 || _currentService.Duration > 240)
                errors.AppendLine("Длительность не может быть больше 240 минут или меньше 0");

            if (_currentService.DiscountInt < 0 || _currentService.DiscountInt > 100)
                errors.AppendLine("Укажите скидку от 0 до 100");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            var allServices = КудрявцеваАвтосервисEntities.GetContext().Service.ToList();
            var duplicateService = allServices.FirstOrDefault(p => p.Title == _currentService.Title && p.ID != _currentService.ID);
            if (duplicateService != null)
            {
                MessageBox.Show("Уже существует такая услуга");
                return;
            }
            if (_currentService.ID == 0)
                КудрявцеваАвтосервисEntities.GetContext().Service.Add(_currentService);
            try
            {
                КудрявцеваАвтосервисEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                КудрявцеваАвтосервисEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
            }
        }
    }
}