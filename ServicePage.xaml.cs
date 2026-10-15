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

            var currentServices = КудрявцеваАвтосервисEntities.GetContext().Service.ToList();

            ServiceListView.ItemsSource = currentServices;
        }
    }
}