using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для MenegerMenu.xaml
    /// </summary>
    public partial class MenegerMenu : Window
    {
        // Контекст базы данных
        private readonly DataBase.TechnoserviceEntities technoserviceEntities;
        //Пользователь
        private DataBase.UserService userService;
        //Коллекция товаров для отображения в списке
        public ObservableCollection<DataBase.Application> Applications { get; set; }

        public MenegerMenu(DataBase.UserService user)
        {
            InitializeComponent();
            technoserviceEntities = new DataBase.TechnoserviceEntities();
            userService = user;
            // Заполнение коллекции заявок из базы данных
            Applications = new ObservableCollection<DataBase.Application>(technoserviceEntities.Applications);
            DataContext = this;
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Authorization auto = new Authorization();
            auto.Show();
            Close();
        }

        private void CreateApplication(object sender, RoutedEventArgs e)
        {
            CreateAppication createAppication = new CreateAppication();
            createAppication.Show();
        }

        private void EditApplication(object sender, RoutedEventArgs e)
        {
            var selectedAppicacion = lvApplication.SelectedItem as DataBase.Application;
            if (selectedAppicacion != null)
            {
                EditApplication editApplication = new EditApplication(technoserviceEntities, lvApplication.SelectedItem as DataBase.Application);
                editApplication.Show();

            }
            else
            {
                MessageBox.Show("Выберите заявку для изменения информации");
            }
        }
    }
}
