using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.IO;
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
        private const string ReadyStatusText = "Hazır.";

        private PlotView MainPlotView;
        private PlotModel MainPlotModel;
        private PlotController MainPlotController = new PlotController();

        private CurrencyFetcher Fetcher = new CurrencyFetcher();

        private void ChangeStatus(string message)
        {
            const uint MsgID = 0;
            MainStatusBar.Pop(MsgID);
            MainStatusBar.Push(MsgID, message);
        }

        public MainWindow() : base(Gtk.WindowType.Toplevel)
        {
            Build();

            DateStartEntry.Text = Fetcher.StartAt.ToString(DateStringFormat);
            DateEndEntry.Text = Fetcher.EndAt.ToString(DateStringFormat);

            foreach (Currencies currency in Enum.GetValues(typeof(Currencies)))
                CurrencyBaseSelection.AppendText(currency.ToString() + " - " + currency.GetStringValue());
            CurrencyBaseSelection.Active = (int)Fetcher.Base;

            MainPlotController.BindMouseDown(OxyMouseButton.Left, PlotCommands.PanAt);
            MainPlotController.BindMouseDown(OxyMouseButton.Right, PlotCommands.PointsOnlyTrack);

            MainPlotModel = new PlotModel();
            MainPlotModel.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                IntervalType = DateTimeIntervalType.Days,
                StringFormat = "dd-MM\nyyyy",
                MajorGridlineStyle = LineStyle.Solid,
                MinimumMajorStep = 1
            });
            MainPlotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                TickStyle = TickStyle.Outside
            });

            MainPlotView = new PlotView { Model = MainPlotModel, Controller = MainPlotController };
            MainPlotAlignment.Add(MainPlotView);
            MainPlotView.Show();

            ChangeStatus(ReadyStatusText);
        }

        protected void OnDeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
            a.RetVal = true;
        }

        protected void OnDateStartEntryFocusGrabbed(object sender, EventArgs e)
        {
            DateTimeDialog dialog;
            if (Fetcher.StartAt == new DateTime())
                dialog = new DateTimeDialog();
            else
                dialog = new DateTimeDialog(Fetcher.StartAt);

            dialog.Response += delegate (object obj, ResponseArgs resp)
            {
                if (resp.ResponseId == ResponseType.Ok)
                {
                    Fetcher.StartAt = dialog.Calendar.Date;
                    DateStartEntry.Text = Fetcher.StartAt.ToString(DateStringFormat);
                }
                else if (resp.ResponseId != ResponseType.Ok && Fetcher.StartAt == new DateTime())
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
            if (Fetcher.EndAt == new DateTime())
                dialog = new DateTimeDialog();
            else
                dialog = new DateTimeDialog(Fetcher.EndAt);

            dialog.Response += delegate (object obj, ResponseArgs resp)
            {
                if (resp.ResponseId == ResponseType.Ok)
                {
                    Fetcher.EndAt = dialog.Calendar.Date;
                    DateEndEntry.Text = Fetcher.EndAt.ToString(DateStringFormat);
                }
                else if (resp.ResponseId != ResponseType.Ok && Fetcher.EndAt == new DateTime())
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

        protected void OnCurrencyBaseSelectionChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            Fetcher.Base = (Currencies)box.Active;
        }

        protected void OnCurrencyListButtonClicked(object sender, EventArgs e)
        {
            CurrencyListDialog dialog = new CurrencyListDialog(Fetcher.Symbols);
            dialog.Response += (object obj, ResponseArgs resp) => Fetcher.Symbols = dialog.CurrencySelections;
            dialog.Run();
            dialog.Destroy();
        }

        private static int FetchThreadFlag;

        private void FetchThread()
        {
            if (Interlocked.CompareExchange(ref FetchThreadFlag, 1, 0) == 0)
            {
                Application.Invoke((sender, e) => ChangeStatus("Veriler getiriliyor..."));
                try
                {
                    Dictionary<Currencies, List<Node>> dict = Fetcher.Fetch();
                    Application.Invoke(delegate
                    {
                        MainPlotModel.Series.Clear();
                        foreach (KeyValuePair<Currencies, List<Node>> pair in dict)
                        {
                            MainPlotModel.Series.Add(new LineSeries
                            {
                                Title = pair.Key.ToString() + " - " + pair.Key.GetStringValue(),
                                ItemsSource = pair.Value,
                                DataFieldX = "Time",
                                DataFieldY = "Value",
                                MarkerType = MarkerType.Circle
                            });
                        }
                        MainPlotModel.InvalidatePlot(true);
                        MainPlotModel.ResetAllAxes();
                    });
                }
                catch (Exception ex) when (ex is DateException || ex is SymbolsException)
                {
                    Application.Invoke(delegate
                    {
                        MessageDialog dialog = new MessageDialog(this, DialogFlags.DestroyWithParent,
                            MessageType.Error, ButtonsType.Ok, ex.Message);
                        dialog.Run();
                        dialog.Destroy();
                    });
                }
                catch (WebException ex)
                {
                    Application.Invoke(delegate
                    {
                        MessageDialog dialog = new MessageDialog(this, DialogFlags.DestroyWithParent,
                            MessageType.Error, ButtonsType.Ok,
                        "Sunucuya istekte bulunurken bir hata ile karşılaşıldı.\n\n" +
                        "Mesaj: " + ex.Message + "\n" +
                        "HResult: " + ex.HResult + "\n" +
                        "Status: " + ex.Status);
                        dialog.Run();
                        dialog.Destroy();
                    });
                }
                Application.Invoke((sender, e) => ChangeStatus(ReadyStatusText));

                Interlocked.Decrement(ref FetchThreadFlag);
            }
            else
            {
                Application.Invoke(delegate
                {
                    MessageDialog dialog = new MessageDialog(this, DialogFlags.DestroyWithParent,
                        MessageType.Error, ButtonsType.Ok, "Lütfen çalışan işlemin bitmesini bekleyin.");
                    dialog.Run();
                    dialog.Destroy();
                });
            }
        }

        protected void OnFetchButtonClicked(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(FetchThread));
            thread.Start();
        }



        protected void OnCloseActionActivated(object sender, EventArgs e)
        {
            Destroy();
            Application.Quit();
        }

        private ExportProperties GetExportProps(ExportType exportType)
        {
            ExportProperties props = null;

            ExportDialog export = new ExportDialog(exportType);
            export.Response += delegate (object obj, ResponseArgs resp) {
                if (resp.ResponseId == ResponseType.Ok)
                    props = export.Properties;
            };
            export.Run();
            export.Destroy();

            if (props == null)
                return null;

            FileChooserDialog fc = new FileChooserDialog("Kaydedilecek Yeri Seçin", this,
                FileChooserAction.Save, "Vazgeç", ResponseType.Cancel, "Kaydet", ResponseType.Accept);
            if (fc.Run() == (int)ResponseType.Accept)
                props.FileName = fc.Filename;
            fc.Destroy();

            if (props.FileName == null)
                return null;

            return props;
        }

        protected void OnPDFExportActionActivated(object sender, EventArgs e)
        {
            ExportProperties props = GetExportProps(ExportType.PDF);
            if (props == null)
                return;

            PdfExporter exporter = new PdfExporter
            {
                Width = props.Width,
                Height = props.Height,
                Background = OxyColor.FromRgb(props.Color[0], props.Color[1], props.Color[2])
            };
            try
            {
                using (FileStream file = File.Create(props.FileName))
                    exporter.Export(MainPlotModel, file);
            }
            catch (Exception ex)
            {
                MessageDialog dialog = new MessageDialog(this, DialogFlags.DestroyWithParent,
                    MessageType.Error, ButtonsType.Ok,
                    "Dışarı aktarılırken bir sorunla karşılaşıldı.\n\n" +
                    "Mesaj: " + ex.Message);
                dialog.Run();
                dialog.Destroy();
            }
        }

        protected void OnPNGExportActionActivated(object sender, EventArgs e)
        {
            ExportProperties props = GetExportProps(ExportType.PNG);
            if (props == null)
                return;

            PngExporter exporter = new PngExporter
            {
                Width = (int)props.Width,
                Height = (int)props.Height,
                Background = OxyColor.FromRgb(props.Color[0], props.Color[1], props.Color[2])
            };
            try
            {
                using (FileStream file = File.Create(props.FileName))
                    exporter.Export(MainPlotModel, file);
            }
            catch (Exception ex)
            {
                MessageDialog dialog = new MessageDialog(this, DialogFlags.DestroyWithParent,
                    MessageType.Error, ButtonsType.Ok,
                    "Dışarı aktarılırken bir sorunla karşılaşıldı.\n\n" +
                    "Mesaj: " + ex.Message);
                dialog.Run();
                dialog.Destroy();
            }
        }

        protected void OnSVGExportActionActivated(object sender, EventArgs e)
        {
            ExportProperties props = GetExportProps(ExportType.SVG);
            if (props == null)
                return;

            SvgExporter exporter = new SvgExporter
            {
                Width = (int)props.Width,
                Height = (int)props.Height
            };
            try
            {
                using (FileStream file = File.Create(props.FileName))
                    exporter.Export(MainPlotModel, file);
            }
            catch (Exception ex)
            {
                MessageDialog dialog = new MessageDialog(this, DialogFlags.DestroyWithParent,
                    MessageType.Error, ButtonsType.Ok,
                    "Dışarı aktarılırken bir sorunla karşılaşıldı.\n\n" +
                    "Mesaj: " + ex.Message);
                dialog.Run();
                dialog.Destroy();
            }
        }
    }
}