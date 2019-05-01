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
using Gtk;

namespace CashFlow
{
    public enum ExportType
    {
        PDF, PNG, SVG
    }

    public class ExportProperties
    {
        public ExportType Type;
        public uint Height, Width;
        public byte[] Color;
        public string FileName;
    }

    public partial class ExportDialog : Gtk.Dialog
    {
        public ExportProperties Properties;

        public ExportDialog(ExportType type)
        {
            this.Build();

            foreach (ExportType option in Enum.GetValues(typeof(ExportType)))
                ExportTypeSelection.AppendText(option.ToString());
            ExportTypeSelection.Active = (int)type;

            ColorButton.Color = new Gdk.Color(255, 255, 255);
        }

        private static byte MapToByte(ushort color)
        {
            return Convert.ToByte((Convert.ToDouble(color) / Convert.ToDouble(ushort.MaxValue)) * byte.MaxValue);
        }

        protected void OnResponse(object o, ResponseArgs args)
        {
            if (args.ResponseId == ResponseType.Ok)
            {
                Properties = new ExportProperties
                {
                    Type = (ExportType)ExportTypeSelection.Active,
                    Height = (uint)HeightInput.Value,
                    Width = (uint)WidthInput.Value,
                    Color = new byte[3]
                    {
                        MapToByte(ColorButton.Color.Red),
                        MapToByte(ColorButton.Color.Green),
                        MapToByte(ColorButton.Color.Blue)
                    }
                };
            }
        }

        protected void OnExportTypeSelectionChanged(object sender, EventArgs e)
        {
            if (ExportTypeSelection.Active == (int)ExportType.SVG)
            {
                ColorButton.Hide();
                label3.Hide();
            }
            else
            {
                ColorButton.Show();
                label3.Show();
            }
        }
    }
}
