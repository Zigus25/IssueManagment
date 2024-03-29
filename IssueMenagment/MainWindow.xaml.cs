﻿using IssueManagment;
using IssueManagment.DataClass;
using IssueManagment.Providers;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace IssueMenagment
{
    public partial class MainWindow : Window
    {
        List<Repo> repos = new List<Repo>();
        List<Issue> issues;
        IssueProvider gith;
        public MainWindow(IssueProvider git, string provider)
        {

            gith = git;
            InitializeComponent();
            repos = gith.getRepos();
            if (repos != null)
            {
                foreach (Repo repo in repos)
                {
                    ReposChoose.Items.Add(repo.Name);
                }
            }
            else
            {
                ReposChoose.Items.Add("Brak  repo do pokazania");
            }
        }

        private void ReposChoose_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            refeshIssueBox();
        }

        private void IssueBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IssueBox.SelectedIndex != -1)
            {
                Save.Visibility = Visibility.Visible;
            }
            else
            {
                Save.Visibility = Visibility.Collapsed;
            }
            var i = IssueBox.SelectedIndex;
            try
            {
                NameInpute.Text = issues[i].Title;
                DescInpute.Text = issues[i].Body;
            }
            catch (Exception ex)
            {
                NameInpute.Text = "";
                DescInpute.Text = "";
            }
        }

        private void ExportujButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog ofd = new SaveFileDialog();
            ofd.FileName = "Issues";
            ofd.DefaultExt = ".db";
            ofd.Filter = "DB files|*.db";
            if (ofd.ShowDialog() == true)
            {
                try
                {
                    new DataBaseLogic().createDB(ofd.FileName, issues, new Repo { Name = ReposChoose.SelectedValue.ToString(), ID = 1 });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                gith.issueCreateUpdate(repos[ReposChoose.SelectedIndex], (int)issues[IssueBox.SelectedIndex].Number, NameInpute.Text, DescInpute.Text);
                NameInpute.Text = "";
                DescInpute.Text = "";
                refeshIssueBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void refeshIssueBox()
        {
            IssueBox.Items.Clear();
            try
            {
                issues = gith.getIssues(repos[ReposChoose.SelectedIndex]);
                if (issues != null)
                {
                    foreach (Issue issue in issues)
                    {
                        IssueBox.Items.Add(issue.Title);
                    }
                }
                else
                {
                    IssueBox.Items.Add("brak Issue do wyświetlenia");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            gith.endConnection();
            var newForm = new LoginWindow();
            newForm.Show();
            this.Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                gith.issueCreateUpdate(repos[ReposChoose.SelectedIndex], -1, NameInpute.Text, DescInpute.Text);
                NameInpute.Text = "";
                DescInpute.Text = "";
                refeshIssueBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
