using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace IssueMenagment
{
    public partial class MainWindow : Window
    {
        List<Issue> issues;
        GithubLogic gith = new GithubLogic();
        public MainWindow()
        {
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
    }
}
