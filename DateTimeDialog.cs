using System;
using Gtk;

namespace CashFlow
{
    public partial class DateTimeDialog : Gtk.Dialog
    {
        public Calendar Calendar = new Calendar();

        public DateTimeDialog(DateTime date)
        {
            Calendar.Date = date;
            this.Build();
            InitCalendar();
        }

        public DateTimeDialog()
        {
            this.Build();
            InitCalendar();
        }

        private void InitCalendar()
        {
            CalendarAlignment.Add(Calendar);
            Calendar.Show();
        }
    }
}
