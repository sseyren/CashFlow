using System;
using Gtk;

namespace CashFlow
{
    public partial class DateTimeDialog : Gtk.Dialog
    {
        public Calendar Calendar = new Calendar();

        public DateTimeDialog()
        {
            this.Build();

            CalendarAlignment.Add(Calendar);
            Calendar.Show();
        }
    }
}
