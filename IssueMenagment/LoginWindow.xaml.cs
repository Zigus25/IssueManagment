using IssueManagment;
using IssueManagment.DataClass;
using IssueManagment.Providers;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Windows;

namespace IssueMenagment
{
    public partial class LoginWindow : Window
    {
        string provider = "GitHub";
        IssueProvider aout;
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            if((LoginInput.Text != ""&& TokenInput.Text != "")|| provider== "DataBase")
            {
                var login = LoginInput.Text;
                var token = TokenInput.Text;
                switch (provider)
                {
                    case "GitHub":
                        aout = new GithubLogic();
                        break;
                    case "GitLab":
                        aout = new GitlabLogic();
                        break;
                    case "DataBase":
                        OpenFileDialog ofd = new OpenFileDialog();
                        ofd.Filter = "DB files|*.db";
                        DataBaseLogic dbl = new DataBaseLogic();
                        if (ofd.ShowDialog() == true)
                        {
                            if (dbl.authentication(ofd.FileName, ""))
                            {
                                var newForm = new MainWindow(dbl, provider);
                                newForm.Show();
                                this.Close();
                            }
                            else
                            {
                                ErrorMess.Content = "Nie udało się otworzyć bazy danych";
                            }
                        }
                        break;
                }
                if (aout.authentication(login, token))
                {
                    var newForm = new MainWindow(aout, provider);
                    newForm.Show();
                    this.Close();
                }
                else
                {
                    ErrorMess.Content = "Konto o podanych danych nie istnije lub nie udało się z nim komunikować";
                }
            }
            else
            {
                ErrorMess.Content = "Podaj login i token";
            }
        }

        private void githubRadio_Checked(object sender, RoutedEventArgs e)
        {
            if (provider != "GitHub")
            {
                provider = "GitHub";
                VisiblChange(true);
            }
        }

        private void gitlabRadio_Checked(object sender, RoutedEventArgs e)
        {
            if (provider != "GitLab")
            {
                provider = "GitLab";
                VisiblChange(true);
            }
        }

        private void OflineButton_Checked(object sender, RoutedEventArgs e)
        {
            if (provider != "DataBase")
            {
                provider = "DataBase";
                VisiblChange(false);
            }
        }

        private void VisiblChange(bool whi) 
        {
            if (whi)
            {
                LoginInput.Visibility = Visibility.Visible;
                TokenInput.Visibility = Visibility.Visible;
                LoginLabel.Visibility = Visibility.Visible;
                TokenLabel.Visibility = Visibility.Visible;
            }
            else
            {
                LoginInput.Visibility = Visibility.Collapsed;
                TokenInput.Visibility = Visibility.Collapsed;
                LoginLabel.Visibility = Visibility.Collapsed;
                TokenLabel.Visibility = Visibility.Collapsed;
            }
        }
    }
}
