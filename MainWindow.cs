using System;
using System.Collections.Generic;
using Gtk;
using OxyPlot;
using OxyPlot.GtkSharp;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace CashFlow
{
    public partial class MainWindow : Gtk.Window
    {
        private const string DateStringFormat = "dd-MM-yyyy";

        private PlotView MainPlotView;

        private DateTime StartDate, EndDate;

        public MainWindow() : base(Gtk.WindowType.Toplevel)
        {
            Build();
            MainPlotView = new PlotView { Model = TestPlotModel() };
            MainPlotAlignment.Add(MainPlotView);
            MainPlotView.Show();
        }

        protected void OnDeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
            a.RetVal = true;
        }

        protected void OnDateStartEntryFocusGrabbed(object sender, EventArgs e)
        {
            DateTimeDialog dialog;
            if (StartDate == new DateTime())
                dialog = new DateTimeDialog();
            else
                dialog = new DateTimeDialog(StartDate);

            dialog.Response += delegate (object obj, ResponseArgs resp) {
                if (resp.ResponseId == ResponseType.Ok)
                {
                    StartDate = dialog.Calendar.Date;
                    DateStartEntry.Text = StartDate.ToString(DateStringFormat);
                }
                else if (resp.ResponseId != ResponseType.Ok && StartDate == new DateTime())
                {
                    MessageDialog message = new MessageDialog(dialog, DialogFlags.DestroyWithParent,
                        MessageType.Warning, ButtonsType.Ok, "Başlangıç tarihi seçmediniz.");
                    message.Run();
                    message.Destroy();
                }
            };
            dialog.Run();
            dialog.Destroy();
        }

        protected void OnDateEndEntryFocusGrabbed(object sender, EventArgs e)
        {
            DateTimeDialog dialog;
            if (EndDate == new DateTime())
                dialog = new DateTimeDialog();
            else
                dialog = new DateTimeDialog(EndDate);

            dialog.Response += delegate (object obj, ResponseArgs resp) {
                if (resp.ResponseId == ResponseType.Ok)
                {
                    EndDate = dialog.Calendar.Date;
                    DateEndEntry.Text = EndDate.ToString(DateStringFormat);
                }
                else if (resp.ResponseId != ResponseType.Ok && EndDate == new DateTime())
                {
                    MessageDialog message = new MessageDialog(dialog, DialogFlags.DestroyWithParent,
                        MessageType.Warning, ButtonsType.Ok, "Bitiş tarihi seçmediniz.");
                    message.Run();
                    message.Destroy();
                }
            };
            dialog.Run();
            dialog.Destroy();
        }

        protected void OnCurrencyListButtonClicked(object sender, EventArgs e)
        {
            CurrencyListDialog dialog = new CurrencyListDialog();
            dialog.Response += delegate (object obj, ResponseArgs resp)
            {
                foreach (Currencies item in dialog.CurrencySelections)
                {
                    Console.WriteLine(item);
                }
            };
            dialog.Run();
            dialog.Destroy();
        }




        protected class Item
        {
            public DateTime X { get; set; }
            public double Y { get; set; }
        }

        protected static PlotModel TestPlotModel()
        {
            var tmp = new PlotModel { Title = "Test" };
            tmp.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                TickStyle = TickStyle.Outside
            });

            var dt = new DateTime(2010, 1, 1);
            tmp.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = DateTimeAxis.ToDouble(dt),
                Maximum = DateTimeAxis.ToDouble(dt.AddDays(1)),
                IntervalType = DateTimeIntervalType.Hours,
                MajorGridlineStyle = LineStyle.Solid,
                Angle = 90,
                StringFormat = "HH:mm",
                MajorStep = 1.0 / 24 / 2, // 1/24 = 1 hour, 1/24/2 = 30 minutes
                IsZoomEnabled = true,
                MaximumPadding = 0,
                MinimumPadding = 0,
                TickStyle = TickStyle.None
            });

            var ls = new LineSeries { Title = "Line1", DataFieldX = "X", DataFieldY = "Y" };
            var ii = new List<Item>();

            for (int i = 0; i < 24; i++)
                ii.Add(new Item { X = dt.AddHours(i), Y = i * i });
            ii.Add(new Item { X = dt.AddHours(26), Y = 800 });
            ls.ItemsSource = ii;
            tmp.Series.Add(ls);
            return tmp;
        }
    }
}