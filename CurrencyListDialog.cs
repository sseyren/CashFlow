using System;
namespace CashFlow
{
    public partial class CurrencyListDialog : Gtk.Dialog
    {
        public CurrencyListDialog()
        {
            this.Build();

            nodeview1.NodeStore = Store;

            // Create a column with title Artist and bind its renderer to model column 0
            nodeview1.AppendColumn("Artist", new Gtk.CellRendererText(), "text", 0);

            // Create a column with title 'Song Title' and bind its renderer to model column 1
            nodeview1.AppendColumn("Song Title", new Gtk.CellRendererText(), "text", 1);
        }

        Gtk.NodeStore store;
        Gtk.NodeStore Store
        {
            get
            {
                if (store == null)
                {
                    store = new Gtk.NodeStore(typeof(MyTreeNode));
                    store.AddNode(new MyTreeNode("The Beatles", "Yesterday"));
                    store.AddNode(new MyTreeNode("Peter Gabriel", "In Your Eyes"));
                    store.AddNode(new MyTreeNode("Rush", "Fly By Night"));
                }
                return store;
            }
        }
    }

    [Gtk.TreeNode(ListOnly = true)]
    public class MyTreeNode : Gtk.TreeNode
    {
        string song_title;

        public MyTreeNode(string artist, string song_title)
        {
            Artist = artist;
            this.song_title = song_title;
        }

        [Gtk.TreeNodeValue(Column = 0)]
        public string Artist;

        [Gtk.TreeNodeValue(Column = 1)]
        public string SongTitle { get { return song_title; } }
    }
}
