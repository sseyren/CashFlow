#region License
// This file is part of CashFlow.
//
// The MIT License (MIT)
//
// Copyright (c) 2019 Serhat Seyren
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
#endregion

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
                Currencies current = (Currencies)CurrencyArray.GetValue(i);
                CheckButtons[i] = new CheckButton
                {
                    Label = current.ToString(),
                    TooltipText = current.GetStringValue()
                };

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
            if (args.ResponseId == ResponseType.Ok)
            {
                CurrencySelections.Clear();
                for (int i = 0; i < CheckButtons.Length; i++)
                {
                    if (CheckButtons[i].Active)
                        CurrencySelections.Add((Currencies)i);
                }

                if (CurrencySelections.Count == 0)
                {
                    MessageDialog message = new MessageDialog(this, DialogFlags.DestroyWithParent,
                        MessageType.Warning, ButtonsType.Ok, "Gösterilecek kur seçmediniz.");
                    message.Run();
                    message.Destroy();
                }
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