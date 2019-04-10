using System;
using System.Collections.Generic;
using Gtk;

namespace CashFlow
{
    public partial class CurrencyListDialog : Gtk.Dialog
    {
        public List<Currencies> CurrencySelections = new List<Currencies>();

        private CheckButton[] CheckButtons;

        private Array CurrencyArray;

        public CurrencyListDialog()
        {
            this.Build();

            CurrencyArray = Enum.GetValues(typeof(Currencies));

            CheckButtons = new CheckButton[CurrencyArray.Length];

            for (uint i = 0; i < (uint)CurrencyArray.Length; i++)
            {
                CheckButtons[i] = new CheckButton { Label = CurrencyArray.GetValue(i).ToString() };
                uint row = i / 6;
                uint column = i % 6;
                MainTable.Attach(CheckButtons[i], row, row + 1, column, column + 1);
            }

            MainTable.ShowAll();
        }

        protected void OnResponse(object o, ResponseArgs args)
        {
            for (int i = 0; i < CheckButtons.Length; i++)
            {
                if (CheckButtons[i].Active)
                    CurrencySelections.Add((Currencies)i);
            }
        }
    }
}