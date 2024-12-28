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
    /// Логика взаимодействия для EditApplication.xaml
    /// </summary>
    public partial class EditApplication : Window
    {
        private readonly DataBase.TechnoserviceEntities technoserviceEntities;
        public ObservableCollection<DataBase.UserService> UserServices { get; set; }
        public ObservableCollection<DataBase.StatusApplication> StatusApplications { get; set; }
        public ObservableCollection<DataBase.UserService> SelectedSpecialists { get; set; }
        public DataBase.Application SelectedApplication {  get; set; }
        public EditApplication(DataBase.TechnoserviceEntities entities, DataBase.Application selectedApplication)
        {
            InitializeComponent();
            technoserviceEntities = entities;
            StatusApplications = new ObservableCollection<StatusApplication>(entities.StatusApplications);
            SelectedApplication = selectedApplication;
            clientNameTextBox.Text = SelectedApplication.ClientName;
            phoneClientTextBox.Text = SelectedApplication.PhoneClient;
            priceTextBox.Text = SelectedApplication.Price.ToString();
            cbStatus.SelectedItem = SelectedApplication.StatusApplication.NameStatus;
            var executorRole = technoserviceEntities.RoleUsers.FirstOrDefault(r => r.NameRole == "Исполнитель");
            UserServices = new ObservableCollection<DataBase.UserService>(
            UserServices = new ObservableCollection<DataBase.UserService>(
                technoserviceEntities.UserServices
                .Where(user => user.UserRole == executorRole.IDRole) // Используем IDRole для сравнения
                .ToList()
            ));
            SelectedSpecialists = new ObservableCollection<DataBase.UserService>(); // Инициализация коллекции для выбранных специалистов
            var specialists = technoserviceEntities.UserApplications
            .Where(ua => ua.ApplicationInUser == SelectedApplication.IDApplication)
            .Select(ua => ua.UserInApplication)
            .ToList();

            foreach (var specialistId in specialists)
            {
                var specialist = technoserviceEntities.UserServices
                    .FirstOrDefault(u => u.IDUser == specialistId); // Заменяем Find на FirstOrDefault с фильтром

                if (specialist != null)
                {
                    SelectedSpecialists.Add(specialist);
                }
            }
            DataContext = this;
        }

        private void AddSpecialist_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = cbSpec.SelectedItem as DataBase.UserService;
            if (selectedUser != null && !SelectedSpecialists.Contains(selectedUser))
            {
                SelectedSpecialists.Add(selectedUser);
            }
            else
            {
                MessageBox.Show("Специалист уже добавлен или не выбран.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void SafeApplication(object sender, RoutedEventArgs e)
        {
            try
            {
                // Обновляем свойства заявки
                SelectedApplication.ClientName = clientNameTextBox.Text;
                SelectedApplication.PhoneClient = phoneClientTextBox.Text;

                if (decimal.TryParse(priceTextBox.Text, out var price))
                {
                    SelectedApplication.Price = (double?)price;
                }
                else
                {
                    MessageBox.Show("Некорректное значение цены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Обновляем внешний ключ для статуса напрямую через StatusApp
                var selectedStatusId = (int?)cbStatus.SelectedValue;
                if (selectedStatusId.HasValue)
                {
                    SelectedApplication.StatusApp = selectedStatusId.Value; // Устанавливаем внешний ключ
                }
                else
                {
                    MessageBox.Show("Статус заявки не выбран.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Устанавливаем состояние объекта как измененное
                technoserviceEntities.Entry(SelectedApplication).State = EntityState.Modified;

                // Сохраняем изменения в базе данных
                technoserviceEntities.SaveChanges();

                MessageBox.Show("Данные сохранены успешно.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

