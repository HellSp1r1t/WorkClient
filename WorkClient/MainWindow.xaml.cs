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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
using System.IO;


namespace WorkClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string ip = "127.0.0.1";
        const int port = 8080;


        public MainWindow()
        {
            InitializeComponent();


        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginBox.Text;
            string pass = PassBox.Password;
            sendMessageToServer("Connect/" + login + "/" + pass);

        }
        public void hideUILogin(bool visible) {
            if (visible)
            {
                backgroundUI.Visibility = Visibility.Hidden;
                LoginBox.Visibility = Visibility.Hidden;
                PassBox.Visibility = Visibility.Hidden;
                LableLogin.Visibility = Visibility.Hidden;
                LablePass.Visibility = Visibility.Hidden;
                ButtonConnect.Visibility = Visibility.Hidden;

            }
            else {
                backgroundUI.Visibility = Visibility.Visible;
                LoginBox.Visibility = Visibility.Visible;
                PassBox.Visibility = Visibility.Visible;
                LableLogin.Visibility = Visibility.Visible;
                LablePass.Visibility = Visibility.Visible;
                ButtonConnect.Visibility = Visibility.Visible;
            }
        }
        public void sendMessageToServer(string message) {
            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            var data = Encoding.UTF8.GetBytes(message);

            tcpSocket.Connect(tcpEndPoint);
            tcpSocket.Send(data);

            var buffer = new byte[256];
            var size = 0;
            var answer = new StringBuilder();

            do
            {
                size = tcpSocket.Receive(buffer);
                answer.Append(Encoding.UTF8.GetString(buffer, 0, size));
            }
            while (tcpSocket.Available > 0);

            string[] answerFromServer = answer.ToString().Split('/');

            switch (answerFromServer[0].Split(':')[0]) {
                case "authentication":
                    if (answerFromServer[0].Split(':')[1] == "true")
                    {
                       hideUILogin(true);
                        string[] listElements = answerFromServer[1].Split(':')[1].Split(',');
                        for (int i = 0; i < listElements.Length; i++)
                        {
                           fileList.Items.Insert(i, listElements[i]);
                        }
                    }
                    else if (answerFromServer[0].Split(':')[1] == "false")
                    {
                        MessageBox.Show("Неправильный логин или пароль");
                    }
                    break;
                case "bodyFile":
                    ReadFileForm readFileForm = new ReadFileForm(answerFromServer[1]);
                    readFileForm.Show();
                    break;
                case "fileDeleted":
                    break;
                case "bodyEditFile":
                    EditFileForm editFileFrom = new EditFileForm(answerFromServer[1]);
                    editFileFrom.mainWindow = this;
                    editFileFrom.nameFile = answerFromServer[2];
                    editFileFrom.Show();
                    break;
                case "filesList":
                    string[] elements = answerFromServer[1].Split(':')[1].Split(',');
                   fileList.Items.Clear();
                    for (int i = 0; i < elements.Length; i++)
                    {
                        fileList.Items.Insert(i, elements[i]);
                    }
                    break;
                case "userAdded":
                    MessageBox.Show("Пользователь успешно создан");
                    break;
                case "usersList":
                    AllUsersForm allUsersForm = new AllUsersForm(answerFromServer[1]);
                    allUsersForm.mainWindow = this;
                    allUsersForm.Show();
                    break;
                default:
                    break;
            }
            tcpSocket.Shutdown(SocketShutdown.Both);
            tcpSocket.Close();
        }
        private void AddFile(object sender, RoutedEventArgs e)
        {
            AddFileForm addFileForm = new AddFileForm();
            addFileForm.mainWindow = this;
            addFileForm.Show();
        }
        private void ReadFile(object sender, RoutedEventArgs e)
        {
            string nameFile = fileList.SelectedItem.ToString();
            sendMessageToServer("readFiles/" + nameFile);
        }
        private void EditFile(object sender, RoutedEventArgs e)
        {
            string nameFile = fileList.SelectedItem.ToString();
            sendMessageToServer("editFiles/" + nameFile);
        }
        private void DeleteFile(object sender, RoutedEventArgs e)
        {
            string textFile = fileList.SelectedItem.ToString();
            sendMessageToServer("deleteFile/" + textFile);
            sendMessageToServer("loadFiles");
        }
        private void AddUserClick(object sender, RoutedEventArgs e)
        {
            AddUserForm addUserForm = new AddUserForm();
            addUserForm.mainWindow = this;
            addUserForm.Show();
        }
        private void UsersButtonClick(object sender, RoutedEventArgs e)
        {
            sendMessageToServer("checkUsers/");
        }
    }
}

