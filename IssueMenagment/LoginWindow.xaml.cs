using Newtonsoft.Json.Linq;
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
            if(LoginInput.Text != ""&& TokenInput.Text != "")
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
                                var newForm = new MainWindow(aoutGH);
                                newForm.Show();
                                this.Close();
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
                                var newForm = new MainWindow(aoutGL);
                                newForm.Show();
                                this.Close();
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
            provider = "GitHub";
        }

        private void gitlabRadio_Checked(object sender, RoutedEventArgs e)
        {
            provider = "GitLab";
        }
    }
}
