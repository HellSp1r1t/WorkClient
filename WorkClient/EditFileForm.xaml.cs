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
    /// Логика взаимодействия для EditFileForm.xaml
    /// </summary>
    public partial class EditFileForm : Window
    {
        public MainWindow mainWindow;
        public string nameFile;
        public EditFileForm(string bodyFile)
        {
            InitializeComponent();
            bodyText.Text = bodyFile;
            
        }
        
        private void saveEditFile(object sender, RoutedEventArgs e)
        {
            string FinalBodyFile = bodyText.Text;

            MainWindow.ParametrsClass parametrsClass = new MainWindow.ParametrsClass();
            parametrsClass.userName = mainWindow.userName;
            parametrsClass.operation = "addOrEditFile";
            parametrsClass.fileName = nameFile;
            parametrsClass.fileBody = FinalBodyFile;

            mainWindow.sendMessageToServer(mainWindow.JsonEncode(parametrsClass));
            this.Close();
        }
    }
}
