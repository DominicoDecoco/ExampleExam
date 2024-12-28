using ExampleExam.DataBase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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
using System.Windows.Shapes;

namespace ExampleExam.View
{
    /// <summary>
    /// Логика взаимодействия для CreateAppication.xaml
    /// </summary>
    public partial class CreateAppication : Window
    {
        private readonly DataBase.TechnoserviceEntities technoserviceEntities;
        public ObservableCollection<DataBase.StatusApplication> StatusApplications { get; set; }
        public ObservableCollection<DataBase.UserService> UserServices { get; set; }
        public ObservableCollection<DataBase.UserService> SelectedSpecialists { get; set; }
        public CreateAppication()
        {
            InitializeComponent();
            technoserviceEntities = new DataBase.TechnoserviceEntities();
            var executorRole = technoserviceEntities.RoleUsers.FirstOrDefault(r => r.NameRole == "Исполнитель");
            UserServices = new ObservableCollection<DataBase.UserService>(
            UserServices = new ObservableCollection<DataBase.UserService>(
                technoserviceEntities.UserServices
                .Where(user => user.UserRole == executorRole.IDRole) // Используем IDRole для сравнения
                .ToList()
            ));
            SelectedSpecialists = new ObservableCollection<DataBase.UserService>(); // Инициализация коллекции для выбранных специалистов
            DataContext = this;
        }

        private void SafeApplication(object sender, RoutedEventArgs e)
        {
            // Создаем новую заявку
            var application = new DataBase.Application
            {
                ClientName = clientNameTextBox.Text, // Получаем значение из TextBox
                PhoneClient = phoneClientTextBox.Text, // Получаем значение из TextBox
                StatusApp = 1, // Предположим, что ID статуса "В ожидании" равен 1
                DateOpenApp = DateTime.Now, // Текущая дата
                Price = double.Parse(priceTextBox.Text) // Получаем значение из TextBox
            };

            // Сохраняем заявку в базе данных
            technoserviceEntities.Applications.Add(application);
            technoserviceEntities.SaveChanges();

            // Добавляем специалистов к заявке
            foreach (var specialist in SelectedSpecialists)
            {
                var userApplication = new DataBase.UserApplication
                {
                    UserInApplication = specialist.IDUser,
                    ApplicationInUser = application.IDApplication,
                    DateJoining = DateTime.Now // Дата присоединения
                };
                technoserviceEntities.UserApplications.Add(userApplication);
            }
            
            technoserviceEntities.SaveChanges();
            MessageBox.Show("Заявка успешно сохранена!");
            Close();
        }

        private void AddSpecialist_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранного специалиста из ComboBox
            var selectedSpecialist = cbSpec.SelectedItem as DataBase.UserService;
            if (selectedSpecialist != null && !SelectedSpecialists.Contains(selectedSpecialist))
            {
                SelectedSpecialists.Add(selectedSpecialist); // Добавляем специалиста в коллекцию
            }
        }
    }
}
