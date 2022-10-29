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

namespace IssueMenagment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string Login;
        GithubLogic gith = new GithubLogic();
        public MainWindow(string login)
        {
            Login = login;
            InitializeComponent();
            var repos = gith.getRepos(Login);
            foreach (string name in repos)
            {
                ReposChoose.Items.Add(name);
            }
        }

        private void ReposChoose_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IssueBox.Items.Clear();
            var issues = gith.getIssues(Login, ReposChoose.SelectedValue.ToString());
            foreach (string name in issues)
            {
                IssueBox.Items.Add(name);
            }
        }
    }
}
