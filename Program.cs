using System;
using Gtk;

namespace CashFlow
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Application.Init();
            MainWindow win = new MainWindow();
            CurrencyFetcher fetch = new CurrencyFetcher();
            fetch.Fetch();
            win.Show();
            Application.Run();
        }
    }
}
