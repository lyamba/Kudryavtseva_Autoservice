using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
namespace Kudryavtseva_Autoservice
{
    public partial class SignUpPage : Page
    {
        private Service _currentService = new Service();
        private ClientService _currentClientService = new ClientService();
        public SignUpPage(Service SelectedService)
        {
            InitializeComponent();
            if (SelectedService != null)
                _currentService = SelectedService;
            DataContext = _currentService;
            var _currentClient = КудрявцеваАвтосервисEntities.GetContext().Client.ToList();
            ComboClient.ItemsSource = _currentClient;
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (ComboClient.SelectedItem == null)
                errors.AppendLine("Укажите ФИО клиента");
            if (StartDate.Text == "")
                errors.AppendLine("Укажите дату услуги");
            if (TBStart.Text == "")
                errors.AppendLine("Укажите время начала услуги");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            _currentClientService.ClientID = (ComboClient.SelectedItem as Client).ID;
            _currentClientService.ServiceID = _currentService.ID;

            try
            {
                _currentClientService.StartTime = Convert.ToDateTime(StartDate.Text + " " + TBStart.Text);
            }
            catch
            {
                MessageBox.Show("Неверный формат даты или времени");
                return;
            }

            if (_currentClientService.ID == 0)
                КудрявцеваАвтосервисEntities.GetContext().ClientService.Add(_currentClientService);
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

        private void TBStart_TextChanged(object sender, TextChangedEventArgs e)
        {
            string s = TBStart.Text;
            TBEnd.ClearValue(TextBox.ForegroundProperty);
            if (s.Length < 3 || !s.Contains(':'))
            {
                TBEnd.Text = "";
            }
            else
            {
                try
                {
                    string[] start = s.Split(new char[] { ':' });
                    if (start.Length != 2) throw new Exception();
                    int startHour = Convert.ToInt32(start[0]);
                    int startMin = Convert.ToInt32(start[1]);
                    if (startHour > 23 || startHour < 0 || startMin > 59 || startMin < 0)
                    {
                        TBEnd.Text = "Неверное время";
                        return;
                    }

                    TBEnd.Text = "";
                    int totalStartMin = startHour * 60 + startMin;
                    int sum = totalStartMin + Convert.ToInt32(_currentService.Duration);
                    int EndHour = (sum / 60) % 24;
                    int EndMin = sum % 60;
                    TBEnd.Text = EndHour.ToString("00") + ":" + EndMin.ToString("00");
                }
                catch
                {
                    TBEnd.Text = "Ошибка формата";
                }
            }
        }
    }
}