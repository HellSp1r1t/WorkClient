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
using System.Net;
using System.Net.Sockets;
using System.IO;



namespace WorkClient
{
    /// <summary>
    /// Логика взаимодействия для AddFileForm.xaml
    /// </summary>
    public partial class AddFileForm : Window
    {
        public MainWindow mainWindow;

        
        
        public AddFileForm()
        {
            InitializeComponent();
        }
        private void AddFileButton(object sender, RoutedEventArgs e)
        {
            string nameFile = NameFileBox.Text;
            string bodyFile = BodyFileBox.Text;

            MainWindow.ParametrsClass parametrsClass = new MainWindow.ParametrsClass();
            parametrsClass.userName = mainWindow.userName;
            parametrsClass.operation = "addOrEditFile";
            parametrsClass.fileName = nameFile;
            parametrsClass.fileBody = bodyFile;

            mainWindow.sendMessageToServer(mainWindow.JsonEncode(parametrsClass));

            this.Close();
        }

    }
}
