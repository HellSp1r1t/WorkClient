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

namespace WorkClient
{
    /// <summary>
    /// Логика взаимодействия для AllUsersForm.xaml
    /// </summary>
    public partial class AllUsersForm : Window
    {
        public MainWindow mainWindow;
        public AllUsersForm(string userList)
        {
            InitializeComponent();
            string[] names = userList.Split(',');
            for (int i = 0; i < names.Length; i++)
            {
                usersList.Items.Insert(i, names[i]);
            }
        }
    }
}
