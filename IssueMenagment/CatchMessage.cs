using System;
using System.Windows;

namespace IssueManagment
{
    internal class CatchMessage
    {
        public void Show(Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }
}
