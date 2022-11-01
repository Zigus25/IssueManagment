using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Windows;

namespace IssueMenagment
{
    public partial class LoginWindow : Window
    {
        string provider = "GitHub";
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
                        GithubLogic aoutGH = new GithubLogic();
                        var resGH = aoutGH.authentication(login, token);
                        if (resGH != "error")
                        {
                            dynamic d = JObject.Parse(resGH);
                            if (d.login == login)
                            {
                                var newForm = new MainWindow(aoutGH, provider);
                                newForm.Show();
                                this.Close();
                            }
                            else
                            {
                                ErrorMess.Content = "Konto o podanych danych nie istnije lub nie udało się z nim komunikować";
                            }
                        }
                        break;
                    case "GitLab":
                        GitlabLogic aoutGL = new GitlabLogic();
                        var resGL = aoutGL.authentication(login, token);
                        if (resGL != "error")
                        {
                            dynamic d = JObject.Parse(resGL);
                            if (d.username == login)
                            {
                                var newForm = new MainWindow(aoutGL, provider);
                                newForm.Show();
                                this.Close();
                            }
                            else
                            {
                                ErrorMess.Content = "Konto o podanych danych nie istnije lub nie udało się z nim komunikować";
                            }
                        }
                        break;
                    case "DataBase":
                        OpenFileDialog ofd = new OpenFileDialog();
                        ofd.Filter = "DB files|*.db";
                        DataBaseLogic dbl = new DataBaseLogic();
                        if (ofd.ShowDialog() == true)
                        {
                            var res = dbl.authentication(ofd.FileName, "");
                            if (res == "Istnieje")
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
                LoginInput.Visibility = Visibility.Visible;
                TokenInput.Visibility = Visibility.Visible;
                LoginLabel.Visibility = Visibility.Visible;
                TokenLabel.Visibility = Visibility.Visible;
            }
        }

        private void gitlabRadio_Checked(object sender, RoutedEventArgs e)
        {
            if (provider != "GitLab")
            {
                provider = "GitLab";
                LoginInput.Visibility = Visibility.Visible;
                TokenInput.Visibility = Visibility.Visible;
                LoginLabel.Visibility = Visibility.Visible;
                TokenLabel.Visibility = Visibility.Visible;
            }
        }

        private void OflineButton_Checked(object sender, RoutedEventArgs e)
        {
            if (provider != "DataBase")
            {
                provider = "DataBase";
                LoginInput.Visibility = Visibility.Collapsed;
                TokenInput.Visibility = Visibility.Collapsed;
                LoginLabel.Visibility = Visibility.Collapsed;
                TokenLabel.Visibility = Visibility.Collapsed;
            }
        }
    }
}
