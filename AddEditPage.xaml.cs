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

        // Конструктор, который принимает услугу (для редактирования) или null (для добавления)
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

            if (_currentService.Discount == null)
                errors.AppendLine("Укажите скидку");

            if (string.IsNullOrWhiteSpace(_currentService.Duration))
                errors.AppendLine("Укажите длительность услуги");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
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