﻿using System;
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

namespace WorkClient
{
    /// <summary>
    /// Логика взаимодействия для AddUserForm.xaml
    /// </summary>
    public partial class AddUserForm : Window
    {
        public MainWindow mainWindow;
        public AddUserForm()
        {
            InitializeComponent();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }
        private void AddUserClick(object sender, RoutedEventArgs e)
        {
            string newLogin = LoginTextBox.Text;
            string newPass = PassTextBox.Text;

            MainWindow.ParametrsClass parametrsClass = new MainWindow.ParametrsClass();
            parametrsClass.operation = "addUser";
            parametrsClass.userName = mainWindow.userName;
            parametrsClass.answer = "login:" + newLogin + "/" + "password:" + newPass; 

            mainWindow.sendMessageToServer(mainWindow.JsonEncode(parametrsClass));

            this.Close();
        }
    }
}
