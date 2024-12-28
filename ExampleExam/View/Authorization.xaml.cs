using ExampleExam.DataBase;
using System;
using System.Collections.Generic;
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
    
    public partial class Authorization : Window
    {
        // Экземпляр контекста базы данных
        private readonly TechnoserviceEntities entities;
        // Пользователь, который пытается войти в систему
        private UserService user;

        public Authorization()
        {
            InitializeComponent();
            entities = new TechnoserviceEntities();

        }
        //обработка нажатия на кнопку войти
        private void ClickAuto(object sender, RoutedEventArgs e)
        {
            // Получение логина и пароля из текстовых полей
            string login = Login.Text.Trim();
            string password = Password.Text.Trim();
            //Ищем пользователя
            user = entities.UserServices.Where(u => u.LoginUser ==  login && u.UserPassword == password).FirstOrDefault();
            if (user == null)
            {
                MessageBox.Show("Неверные данные (логин/пароль)");
            }
            else
            {
                switch(user.RoleUser.NameRole)
                {
                    case "Менеджер":
                        MenegerMenu meneger = new MenegerMenu(user);
                        meneger.Show();
                        Close();
                        break;
                    case "Исполнитель":
                        MasterMenu mmaster = new MasterMenu(user);
                        mmaster.Show();
                        Close();
                        break;
                    case "Администратор":
                        break;

                }
            }

        }

    }
}
