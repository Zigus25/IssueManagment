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
    public partial class MainWindow : Window
    {
        string Login;
        List<Issue> issues;
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
            issues = gith.getIssues(Login, ReposChoose.SelectedValue.ToString());
            if(issues != null)
            {
                foreach (Issue issu in issues)
                {
                    IssueBox.Items.Add(issu.title);
                }
            }
            else
            {
                IssueBox.Items.Add("brak Issue do wyświetlenia");
            }
        }

        private void IssueBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var i = IssueBox.SelectedIndex;
            try
            {
                NameInpute.Text = issues[i].title;
                DescInpute.Text = issues[i].body;
            }
            catch (Exception ex)
            {
                NameInpute.Text = "";
                DescInpute.Text = "";
            }
        }
    }
}
