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
using System.Net;
using System.Threading;
using System.IO;
using Gtk;
using OxyPlot;
using OxyPlot.GtkSharp;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Annotations;

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

        private Currencies FetchedCurrency;

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
            DateStartEntry.Xalign = 0.5F;
            DateEndEntry.Text = Fetcher.EndAt.ToString(DateStringFormat);
            DateEndEntry.Xalign = 0.5F;

            foreach (Currencies currency in Enum.GetValues(typeof(Currencies)))
                CurrencyBaseSelection.AppendText(currency.ToString() + " - " + currency.GetStringValue());
            CurrencyBaseSelection.Active = (int)Fetcher.Base;

            MainPlotController.UnbindKeyDown(OxyKey.A);
            MainPlotController.BindMouseDown(OxyMouseButton.Left, PlotCommands.PanAt);
            MainPlotController.BindMouseDown(OxyMouseButton.Right, PlotCommands.PointsOnlyTrack);
            MainPlotController.BindKeyDown(OxyKey.Add, PlotCommands.ZoomInFine);
            MainPlotController.BindKeyDown(OxyKey.Subtract, PlotCommands.ZoomOutFine);
            MainPlotController.BindKeyDown(OxyKey.Up, PlotCommands.PanDownFine);
            MainPlotController.BindKeyDown(OxyKey.W, PlotCommands.PanDown);
            MainPlotController.BindKeyDown(OxyKey.Down, PlotCommands.PanUpFine);
            MainPlotController.BindKeyDown(OxyKey.S, PlotCommands.PanUp);
            MainPlotController.BindKeyDown(OxyKey.Left, PlotCommands.PanRightFine);
            MainPlotController.BindKeyDown(OxyKey.A, PlotCommands.PanRight);
            MainPlotController.BindKeyDown(OxyKey.Right, PlotCommands.PanLeftFine);
            MainPlotController.BindKeyDown(OxyKey.D, PlotCommands.PanLeft);

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

        protected void OnDateStartButtonClicked(object sender, EventArgs e)
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

        protected void OnDateEndButtonClicked(object sender, EventArgs e)
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
                    Fetcher.Fetch();
                    FetchedCurrency = Fetcher.Base;
                    Application.Invoke(delegate
                    {
                        MainPlotModel.Series.Clear();
                        string suffix = " (" + FetchedCurrency.ToString() + ")";
                        foreach (KeyValuePair<Currencies, List<Node>> pair in Fetcher.Data)
                        {
                            MainPlotModel.Series.Add(new LineSeries
                            {
                                Title = pair.Key.ToString() + " - " + pair.Key.GetStringValue() + suffix,
                                ItemsSource = pair.Value,
                                DataFieldX = "Time",
                                DataFieldY = "Value",
                                MarkerType = MarkerType.Circle
                            });
                        }
                        RenderAnnotations();
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

        private void Export(ExportType exportType)
        {
            ExportProperties props = null;

            ExportDialog export = new ExportDialog(exportType);
            export.Response += delegate (object obj, ResponseArgs resp)
            {
                if (resp.ResponseId == ResponseType.Ok)
                    props = export.Properties;
            };
            export.Run();
            export.Destroy();

            if (props == null)
                return;

            FileChooserDialog fc = new FileChooserDialog("Kaydedilecek Yeri Seçin", this,
                FileChooserAction.Save, "Vazgeç", ResponseType.Cancel, "Kaydet", ResponseType.Accept);
            if (fc.Run() == (int)ResponseType.Accept)
                props.FileName = fc.Filename;
            fc.Destroy();

            if (props.FileName == null)
                return;

            if (props.Type == ExportType.PDF)
            {
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
            else if (props.Type == ExportType.PNG)
            {
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
            else if (props.Type == ExportType.SVG)
            {
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

        protected void OnPDFExportActionActivated(object sender, EventArgs e) => Export(ExportType.PDF);

        protected void OnPNGExportActionActivated(object sender, EventArgs e) => Export(ExportType.PNG);

        protected void OnSVGExportActionActivated(object sender, EventArgs e) => Export(ExportType.SVG);

        protected void OnCSVExportActionActivated(object sender, EventArgs e)
        {
            if (Fetcher.Data == null || Fetcher.Data.Count == 0)
            {
                MessageDialog dialog = new MessageDialog(this, DialogFlags.DestroyWithParent,
                    MessageType.Error, ButtonsType.Ok, "Dışarı aktarılacak veri yok.");
                dialog.Run();
                dialog.Destroy();
                return;
            }

            string FileName = null;

            FileChooserDialog fc = new FileChooserDialog("Kaydedilecek Yeri Seçin", this,
                FileChooserAction.Save, "Vazgeç", ResponseType.Cancel, "Kaydet", ResponseType.Accept);
            if (fc.Run() == (int)ResponseType.Accept)
                FileName = fc.Filename;
            fc.Destroy();

            if (FileName == null)
                return;

            try
            {
                using (StreamWriter sw = File.CreateText(FileName))
                {
                    const string sep = ",";
                    sw.NewLine = "\r\n";

                    List<Currencies> keys = new List<Currencies>(Fetcher.Data.Keys);

                    string header = "Date" + sep;
                    string suffix = "/" + FetchedCurrency.ToString();

                    foreach (Currencies cur in keys)
                        header += cur.ToString() + suffix + sep;
                    sw.WriteLine(header.Substring(0, header.Length - 1));

                    for (int i = 0; i < Fetcher.Data[keys[0]].Count; i++)
                    {
                        string line = Fetcher.Data[keys[0]][i].Time.ToString(DateStringFormat) + sep;
                        foreach (Currencies cur in keys)
                            line += Fetcher.Data[cur][i].Value.ToString("0.0000000000", System.Globalization.CultureInfo.InvariantCulture) + sep;
                        sw.WriteLine(line.Substring(0, line.Length - 1));
                    }
                }
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



        protected void OnClearPlotActionActivated(object sender, EventArgs e)
        {
            Fetcher.Data.Clear();
            MainPlotModel.Series.Clear();
            MainPlotModel.Annotations.Clear();
            MainPlotModel.InvalidatePlot(true);
        }

        protected void OnFocusOnDataActionActivated(object sender, EventArgs e)
        {
            MainPlotModel.ResetAllAxes();
            MainPlotModel.InvalidatePlot(true);
        }

        private bool SyncActionAndCheckButton(object sender, ToggleAction action, CheckButton button)
        {
            bool status = false;

            try { CheckButton check = (CheckButton)sender; status = check.Active; }
            catch (InvalidCastException) { }

            try { ToggleAction check = (ToggleAction)sender; status = check.Active; }
            catch (InvalidCastException) { }

            button.Active = status;
            action.Active = status;

            return status;
        }

        private void RenderAnnotations()
        {
            MainPlotModel.Annotations.Clear();

            if (BiggestValuesAction.Active)
            {
                foreach (Node node in Fetcher.TopValues)
                {
                    MainPlotModel.Annotations.Add(new PointAnnotation
                    {
                        X = DateTimeAxis.ToDouble(node.Time),
                        Y = node.Value,
                        Size = 8,
                        Shape = MarkerType.Triangle,
                        Fill = OxyColors.RoyalBlue,
                    });
                }
                foreach (Node node in Fetcher.BottomValues)
                {
                    MainPlotModel.Annotations.Add(new PointAnnotation
                    {
                        X = DateTimeAxis.ToDouble(node.Time),
                        Y = node.Value,
                        Size = 8,
                        Shape = MarkerType.Triangle,
                        Fill = OxyColors.Red
                    });
                }
            }

            if (BiggestHopsAction.Active)
            {
                foreach (Node[] nodes in Fetcher.TopChanges)
                {
                    MainPlotModel.Annotations.Add(new ArrowAnnotation
                    {
                        StartPoint = new DataPoint(DateTimeAxis.ToDouble(nodes[0].Time), nodes[0].Value),
                        EndPoint = new DataPoint(DateTimeAxis.ToDouble(nodes[1].Time), nodes[1].Value),
                        Color = OxyColors.DeepSkyBlue,
                        Text = "+" + (nodes[1].Value - nodes[0].Value).ToString("0.000"),
                        FontSize = 10,
                        TextColor = OxyColors.DarkBlue,
                    });
                }

                foreach (Node[] nodes in Fetcher.BottomChanges)
                {
                    MainPlotModel.Annotations.Add(new ArrowAnnotation
                    {
                        StartPoint = new DataPoint(DateTimeAxis.ToDouble(nodes[0].Time), nodes[0].Value),
                        EndPoint = new DataPoint(DateTimeAxis.ToDouble(nodes[1].Time), nodes[1].Value),
                        Color = OxyColors.DarkOrange,
                        Text = (nodes[1].Value - nodes[0].Value).ToString("0.000"),
                        FontSize = 10,
                        TextColor = OxyColors.DarkRed,
                    });
                }
            }

            MainPlotModel.InvalidatePlot(true);
        }

        protected void OnBiggestValuesActionToggled(object sender, EventArgs e)
        {
            SyncActionAndCheckButton(sender, BiggestValuesAction, BiggestValuesCheck);
            RenderAnnotations();
        }

        protected void OnBiggestHopsActionToggled(object sender, EventArgs e)
        {
            SyncActionAndCheckButton(sender, BiggestHopsAction, BiggestHopsCheck);
            RenderAnnotations();
        }



        protected void OnControlsActionActivated(object sender, EventArgs e)
        {
            ControlsDialog dialog = new ControlsDialog();
            dialog.Run();
            dialog.Destroy();
        }

        protected void OnAboutActionActivated(object sender, EventArgs e)
        {
            AboutDialog dialog = new AboutDialog
            {
                ProgramName = "CashFlow",
                Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                Comments = "Döviz verilerini grafiğe döken bir program.",
                License = License.MergedLicenses,
                Authors = new string[] { "Serhat Seyren <sseyren@hotmail.com>" },
                Website = "https://github.com/thesseyren/CashFlow",
            };
            dialog.Run();
            dialog.Destroy();
        }
    }
}