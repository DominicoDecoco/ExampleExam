using ExampleExam.DataBase;
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
    /// Логика взаимодействия для CommentMaster.xaml
    /// </summary>
    public partial class CommentMaster : Window
    {
        public readonly DataBase.TechnoserviceEntities entities;
        public ObservableCollection<DataBase.Application> Applications { get; set; }
        public ObservableCollection<DataBase.CommentApplication> Comments { get; set; }
        public DataBase.Application SelectedApplication { get; set; }
        private UserService serveseUser;

        public CommentMaster(DataBase.TechnoserviceEntities entities, DataBase.Application selectedApplication, UserService user)
        {
            InitializeComponent();
            this.entities = entities;
            SelectedApplication = selectedApplication;
            serveseUser = user;
            Comments = new ObservableCollection<DataBase.CommentApplication>();
            listViewComments.ItemsSource = Comments;
            LoadComments();
            DataContext = this;
        }
        private void LoadComments()
        {
            // Загрузка комментариев из базы данных для выбранной заявки
            var commentsFromDb = entities.CommentApplications
                .Where(c => c.AppComment == SelectedApplication.IDApplication)
                .ToList();

            foreach (var comment in commentsFromDb)
            {
                Comments.Add(comment);
            }
        }
        private void ClickSaveComm(object sender, RoutedEventArgs e)
        {
            string commentText = tbComment.Text.Trim();
            if (string.IsNullOrEmpty(commentText))
            {
                MessageBox.Show("Комментарий не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Создание нового комментария
            var newComment = new DataBase.CommentApplication
            {
                Comment = commentText,
                Specialist = serveseUser.IDUser, // Предполагаем, что у вас есть ID пользователя
                AppComment = SelectedApplication.IDApplication,
            };

            // Сохранение комментария в базе данных
            entities.CommentApplications.Add(newComment);
            entities.SaveChanges();

            // Добавление комментария в коллекцию для обновления ListView
            Comments.Add(newComment);
            tbComment.Clear(); // Очистка текстового поля после сохранения
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
