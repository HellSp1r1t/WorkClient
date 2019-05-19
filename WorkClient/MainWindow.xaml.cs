using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using System;

namespace WorkClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string ip = "127.0.0.1";
        const int port = 8080;
        public string userName = "";


        public MainWindow()
        {
            InitializeComponent();


        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginBox.Text;
            string pass = PassBox.Password;

            ParametrsClass parametrsClass = new ParametrsClass();
            parametrsClass.userName = login;
            parametrsClass.userPass = pass;
            parametrsClass.operation = "Connect";

            sendMessageToServer(JsonEncode(parametrsClass));

        }
        //Скрыть интефейс входа
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
            ParametrsClass receivedData = JsonConvert.DeserializeObject<ParametrsClass>(answer.ToString());

            switch (receivedData.operation) {
                case "authentication":
                    if (Convert.ToBoolean(receivedData.authentication))
                    {
                        hideUILogin(true);
                        userName = receivedData.userName;
                    }
                    else
                    {
                        MessageBox.Show("Неправильный логин или пароль");
                    }
                    break;
                case "bodyFile":
                    ReadFileForm readFileForm = new ReadFileForm(receivedData.fileBody);
                    readFileForm.Show();
                    break;
                case "fileDeleted":

                    break;
                case "bodyEditFile":
                    EditFileForm editFileFrom = new EditFileForm(receivedData.fileBody);
                    editFileFrom.mainWindow = this;
                    editFileFrom.nameFile = receivedData.fileName;
                    editFileFrom.Show();
                    break;
                case "filesList":

                    break;
                case "userAdded":
                    if (Convert.ToBoolean(receivedData) == true)
                    {
                        MessageBox.Show("Пользователь успешно создан");
                    }
                    break;
                case "usersList":

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

            ParametrsClass parametrsClass = new ParametrsClass();
            parametrsClass.fileName = nameFile;
            parametrsClass.operation = "readFile";
            parametrsClass.userName = userName;

            sendMessageToServer(JsonEncode(parametrsClass));
        }
        private void EditFile(object sender, RoutedEventArgs e)
        {
            string nameFile = fileList.SelectedItem.ToString();
            ParametrsClass parametrsClass = new ParametrsClass();
            parametrsClass.fileName = nameFile;
            parametrsClass.operation = "readFileForEdit";
            parametrsClass.userName = userName;

            sendMessageToServer(JsonEncode(parametrsClass));
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

        //Метод для создания запроса json
        public string JsonEncode(ParametrsClass parametrsClass) {
            return JsonConvert.SerializeObject(parametrsClass);
        }
        public class ParametrsClass {
            public string operation = null;
            public string userName = null;
            public string userPass = null;
            public string fileName = null;
            public string fileBody = null;
            public string answer = null;
            public string authentication = null;
        }

    }
}

