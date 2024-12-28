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

    public partial class MasterMenu : Window
    {
        private readonly DataBase.TechnoserviceEntities technoserviceEntities;
        private DataBase.UserService userService;
        public ObservableCollection<DataBase.Application> Applications { get; set; }
        public MasterMenu(DataBase.UserService user)
        {
            InitializeComponent();
            technoserviceEntities = new DataBase.TechnoserviceEntities();
            userService = user;
            // Заполнение коллекции товаров из базы данных
            var applicationIds = technoserviceEntities.UserApplications
        .Where(ua => ua.UserInApplication == userService.IDUser) // Фильтруем по ID специалиста
        .Select(ua => ua.ApplicationInUser) // Получаем идентификаторы заявок
        .Distinct() // Убираем дубликаты
        .ToList();

            // Теперь получаем сами заявки по идентификаторам
            Applications = new ObservableCollection<DataBase.Application>(
                technoserviceEntities.Applications
                    .Where(a => applicationIds.Contains(a.IDApplication)) // Фильтруем заявки по идентификаторам
                    .ToList()
            );
            DataContext = this;
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Authorization auto = new Authorization();
            auto.Show();
            Close();
        }

        private void ClickCreateComment(object sender, RoutedEventArgs e)
        {
            var selectedApplication = lvApplication.SelectedItem as DataBase.Application;
            if (selectedApplication != null)
            {
                CommentMaster commentMaster = new CommentMaster(technoserviceEntities, lvApplication.SelectedItem as DataBase.Application, userService);
                commentMaster.Show();
            }
            else
            {
                MessageBox.Show("Выберите заявку для чтения комментариев!");
            }
        }
    }
}
