using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace IssueMenagment
{
    public partial class MainWindow : Window
    {
        List<Issue> issues;
        GithubLogic gith;
        public MainWindow(GithubLogic git)
        {
            gith = git;
            InitializeComponent();
            var repos = gith.getRepos();
            foreach (string name in repos)
            {
                ReposChoose.Items.Add(name);
            }
        }

        private void ReposChoose_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            refeshIssueBox();
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
                DescInpute.Text = ""    ;
            }
        }

        private void ExportujButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO doać wybór ścieżki
            new DBL().createDB(@"C:\Users\zigus\Downloads\Issues.db",issues);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            int num = -1;
            if(IssueBox.SelectedIndex != -1)
            {
                num = issues[IssueBox.SelectedIndex].number;
            }
            gith.Issue(ReposChoose.SelectedValue.ToString(),num,NameInpute.Text,DescInpute.Text);
            NameInpute.Text = "";
            DescInpute.Text = "";
            refeshIssueBox();
        }

        public void refeshIssueBox()
        {
            IssueBox.Items.Clear();
            issues = gith.getIssues(ReposChoose.SelectedValue.ToString());
            if (issues != null)
            {
                foreach (Issue issue in issues)
                {
                    IssueBox.Items.Add(issue.title);
                }
            }
            else
            {
                IssueBox.Items.Add("brak Issue do wyświetlenia");
            }
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            DBL dbl = new DBL();
            List<Issue> iss = dbl.importDB(@"C:\Users\zigus\Downloads\Issues.db");
            foreach (Issue issue in iss)
            {
                IssueBox.Items.Add(issue.title);
            }
        }
    }
}
