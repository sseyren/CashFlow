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

using Gtk;

namespace CashFlow
{
    public partial class ControlsDialog : Dialog
    {
        [TreeNode(ListOnly = true)]
        public class Control : TreeNode
        {
            public Control(string binding, string key)
            {
                Key = key;
                Binding = binding;
            }

            [TreeNodeValue(Column = 0)]
            public string Binding { get; }

            [TreeNodeValue(Column = 1)]
            public string Key;
        }

        NodeStore store;
        NodeStore Store
        {
            get
            {
                if (store == null)
                {
                    store = new NodeStore(typeof(Control));
                    store.AddNode(new Control("Panoyu sürükleme", "Sol tık, WASD tuşları"));
                    store.AddNode(new Control("Panoyu hassas sürükleme", "↑, ←, ↓, → tuşları"));
                    store.AddNode(new Control("Yakınlaştırma", "Fare tekerleği"));
                    store.AddNode(new Control("Hassas yakınlaştırma", "+ ve - tuşları"));
                    store.AddNode(new Control("Dikdötgen yakınlaştırma", "Ctrl+Sağ tık"));
                    store.AddNode(new Control("Nokta bilgisi", "Sağ tık"));
                    store.AddNode(new Control("Eksenleri sıfırla\n(verilere odaklan)", "Home tuşu"));
                }
                return store;
            }
        }

        public ControlsDialog()
        {
            this.Build();

            MainNodeView.NodeStore = Store;
            MainNodeView.AppendColumn("Eylem", new CellRendererText(), "text", 0);
            MainNodeView.AppendColumn("Tuş", new CellRendererText(), "text", 1);
        }
    }
}
