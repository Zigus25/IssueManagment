using Newtonsoft.Json.Linq;
using System.Windows;

namespace IssueMenagment
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            if(LoginInput.Text != ""&& TokenInput.Text != "")
            {
                GithubLogic aout = new GithubLogic();
                var login = LoginInput.Text;
                var token = TokenInput.Text;
                var link = "https://api.github.com/user";

                var res = aout.authentication(login, token, link);
                if (res != "error")
                {
                    dynamic d = JObject.Parse(res);
                    if (d.login == login)
                    {
                        var newForm = new MainWindow(aout);
                        newForm.Show();
                        this.Close();
                    }
                }
            }
            else
            {
                ErrorMess.Content = "Podaj login i token";
            }
        }
    }
}
