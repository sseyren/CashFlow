using Gtk;

namespace CashFlow
{
    public partial class ControlsDialog : Dialog
    {
        [TreeNode(ListOnly = true)]
        public class Control : TreeNode
        {
            public Control(string key, string binding)
            {
                Key = key;
                Binding = binding;
            }

            [TreeNodeValue(Column = 0)]
            public string Key;

            [TreeNodeValue(Column = 1)]
            public string Binding { get; }
        }

        NodeStore store;
        NodeStore Store
        {
            get
            {
                if (store == null)
                {
                    store = new NodeStore(typeof(Control));
                }
                return store;
            }
        }

        public ControlsDialog()
        {
            this.Build();

            MainNodeView.NodeStore = Store;
            MainNodeView.AppendColumn("Tuş", new CellRendererText(), "text", 0);
            MainNodeView.AppendColumn("Eylem", new CellRendererText(), "text", 1);
        }
    }
}
