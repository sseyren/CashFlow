using System;
using System.Collections.Generic;
using Gtk;

namespace CashFlow
{
    public partial class CurrencyListDialog : Gtk.Dialog
    {
        public List<Currencies> CurrencySelections;

        private CheckButton[] CheckButtons;

        private Array CurrencyArray;

        public CurrencyListDialog(List<Currencies> currentSelections)
        {
            CurrencySelections = currentSelections;
            InitDialog();
        }

        public CurrencyListDialog()
        {
            CurrencySelections = new List<Currencies>();
            InitDialog();
        }

        private void InitDialog()
        {
            this.Build();

            CurrencyArray = Enum.GetValues(typeof(Currencies));

            CheckButtons = new CheckButton[CurrencyArray.Length];

            for (uint i = 0; i < (uint)CurrencyArray.Length; i++)
            {
                CheckButtons[i] = new CheckButton { Label = CurrencyArray.GetValue(i).ToString() };
                if ( CurrencySelections.Contains((Currencies)i) )
                    CheckButtons[i].Active = true;

                uint row = i / 6;
                uint column = i % 6;
                MainTable.Attach(CheckButtons[i], row, row + 1, column, column + 1);
            }

            MainTable.ShowAll();
        }

        protected void OnResponse(object o, ResponseArgs args)
        {
            CurrencySelections.Clear();

            for (int i = 0; i < CheckButtons.Length; i++)
            {
                if (CheckButtons[i].Active)
                    CurrencySelections.Add((Currencies)i);
            }
        }

        protected void OnSelectAllButtonClicked(object sender, EventArgs e)
        {
            foreach (ToggleButton button in CheckButtons)
                button.Active = true;
        }

        protected void OnReverseButtonClicked(object sender, EventArgs e)
        {
            foreach (ToggleButton button in CheckButtons)
                button.Active = !button.Active;
        }

        protected void OnSelectNoneButtonClicked(object sender, EventArgs e)
        {
            foreach (ToggleButton button in CheckButtons)
                button.Active = false;
        }
    }
}