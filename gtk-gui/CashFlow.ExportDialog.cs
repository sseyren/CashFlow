
// This file has been generated by the GUI designer. Do not modify.
namespace CashFlow
{
	public partial class ExportDialog
	{
		private global::Gtk.Table table1;

		private global::Gtk.ColorButton ColorButton;

		private global::Gtk.ComboBox ExportTypeSelection;

		private global::Gtk.HBox hbox1;

		private global::Gtk.SpinButton WidthInput;

		private global::Gtk.Label label2;

		private global::Gtk.SpinButton HeightInput;

		private global::Gtk.Label label1;

		private global::Gtk.Label label3;

		private global::Gtk.Label label4;

		private global::Gtk.Button buttonCancel;

		private global::Gtk.Button buttonOk;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget CashFlow.ExportDialog
			this.Name = "CashFlow.ExportDialog";
			this.Title = global::Mono.Unix.Catalog.GetString("Çıktı Seçenekleri");
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Internal child CashFlow.ExportDialog.VBox
			global::Gtk.VBox w1 = this.VBox;
			w1.Name = "dialog1_VBox";
			w1.BorderWidth = ((uint)(2));
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.table1 = new global::Gtk.Table(((uint)(3)), ((uint)(2)), false);
			this.table1.Name = "table1";
			this.table1.RowSpacing = ((uint)(20));
			this.table1.ColumnSpacing = ((uint)(16));
			this.table1.BorderWidth = ((uint)(10));
			// Container child table1.Gtk.Table+TableChild
			this.ColorButton = new global::Gtk.ColorButton();
			this.ColorButton.HeightRequest = 100;
			this.ColorButton.CanFocus = true;
			this.ColorButton.Events = ((global::Gdk.EventMask)(784));
			this.ColorButton.Name = "ColorButton";
			this.table1.Add(this.ColorButton);
			global::Gtk.Table.TableChild w2 = ((global::Gtk.Table.TableChild)(this.table1[this.ColorButton]));
			w2.TopAttach = ((uint)(2));
			w2.BottomAttach = ((uint)(3));
			w2.LeftAttach = ((uint)(1));
			w2.RightAttach = ((uint)(2));
			// Container child table1.Gtk.Table+TableChild
			this.ExportTypeSelection = global::Gtk.ComboBox.NewText();
			this.ExportTypeSelection.Name = "ExportTypeSelection";
			this.table1.Add(this.ExportTypeSelection);
			global::Gtk.Table.TableChild w3 = ((global::Gtk.Table.TableChild)(this.table1[this.ExportTypeSelection]));
			w3.LeftAttach = ((uint)(1));
			w3.RightAttach = ((uint)(2));
			w3.XOptions = ((global::Gtk.AttachOptions)(4));
			w3.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.hbox1 = new global::Gtk.HBox();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.WidthInput = new global::Gtk.SpinButton(0D, 50000D, 1D);
			this.WidthInput.WidthRequest = 80;
			this.WidthInput.HeightRequest = 35;
			this.WidthInput.CanFocus = true;
			this.WidthInput.Name = "WidthInput";
			this.WidthInput.Adjustment.PageIncrement = 10D;
			this.WidthInput.ClimbRate = 1D;
			this.WidthInput.Numeric = true;
			this.WidthInput.Value = 1920D;
			this.WidthInput.Wrap = true;
			this.hbox1.Add(this.WidthInput);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.WidthInput]));
			w4.Position = 0;
			w4.Expand = false;
			w4.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.label2 = new global::Gtk.Label();
			this.label2.Name = "label2";
			this.label2.LabelProp = global::Mono.Unix.Catalog.GetString("X");
			this.hbox1.Add(this.label2);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.label2]));
			w5.Position = 1;
			w5.Expand = false;
			w5.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.HeightInput = new global::Gtk.SpinButton(0D, 50000D, 1D);
			this.HeightInput.WidthRequest = 80;
			this.HeightInput.HeightRequest = 35;
			this.HeightInput.CanFocus = true;
			this.HeightInput.Name = "HeightInput";
			this.HeightInput.Adjustment.PageIncrement = 10D;
			this.HeightInput.ClimbRate = 1D;
			this.HeightInput.Numeric = true;
			this.HeightInput.Value = 1080D;
			this.hbox1.Add(this.HeightInput);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.HeightInput]));
			w6.Position = 2;
			w6.Expand = false;
			w6.Fill = false;
			this.table1.Add(this.hbox1);
			global::Gtk.Table.TableChild w7 = ((global::Gtk.Table.TableChild)(this.table1[this.hbox1]));
			w7.TopAttach = ((uint)(1));
			w7.BottomAttach = ((uint)(2));
			w7.LeftAttach = ((uint)(1));
			w7.RightAttach = ((uint)(2));
			// Container child table1.Gtk.Table+TableChild
			this.label1 = new global::Gtk.Label();
			this.label1.Name = "label1";
			this.label1.LabelProp = global::Mono.Unix.Catalog.GetString("Boyut:");
			this.label1.Justify = ((global::Gtk.Justification)(1));
			this.table1.Add(this.label1);
			global::Gtk.Table.TableChild w8 = ((global::Gtk.Table.TableChild)(this.table1[this.label1]));
			w8.TopAttach = ((uint)(1));
			w8.BottomAttach = ((uint)(2));
			w8.XOptions = ((global::Gtk.AttachOptions)(4));
			w8.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label3 = new global::Gtk.Label();
			this.label3.Name = "label3";
			this.label3.LabelProp = global::Mono.Unix.Catalog.GetString("Renk:");
			this.label3.Justify = ((global::Gtk.Justification)(1));
			this.table1.Add(this.label3);
			global::Gtk.Table.TableChild w9 = ((global::Gtk.Table.TableChild)(this.table1[this.label3]));
			w9.TopAttach = ((uint)(2));
			w9.BottomAttach = ((uint)(3));
			w9.XOptions = ((global::Gtk.AttachOptions)(4));
			w9.YOptions = ((global::Gtk.AttachOptions)(4));
			// Container child table1.Gtk.Table+TableChild
			this.label4 = new global::Gtk.Label();
			this.label4.Name = "label4";
			this.label4.LabelProp = global::Mono.Unix.Catalog.GetString("Çıktı Türü:");
			this.table1.Add(this.label4);
			global::Gtk.Table.TableChild w10 = ((global::Gtk.Table.TableChild)(this.table1[this.label4]));
			w10.XOptions = ((global::Gtk.AttachOptions)(4));
			w10.YOptions = ((global::Gtk.AttachOptions)(4));
			w1.Add(this.table1);
			global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(w1[this.table1]));
			w11.Position = 0;
			w11.Expand = false;
			w11.Fill = false;
			// Internal child CashFlow.ExportDialog.ActionArea
			global::Gtk.HButtonBox w12 = this.ActionArea;
			w12.Name = "dialog1_ActionArea";
			w12.Spacing = 10;
			w12.BorderWidth = ((uint)(5));
			w12.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonCancel = new global::Gtk.Button();
			this.buttonCancel.CanDefault = true;
			this.buttonCancel.CanFocus = true;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseStock = true;
			this.buttonCancel.UseUnderline = true;
			this.buttonCancel.Label = "gtk-cancel";
			this.AddActionWidget(this.buttonCancel, -6);
			global::Gtk.ButtonBox.ButtonBoxChild w13 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w12[this.buttonCancel]));
			w13.Expand = false;
			w13.Fill = false;
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.buttonOk = new global::Gtk.Button();
			this.buttonOk.CanDefault = true;
			this.buttonOk.CanFocus = true;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseStock = true;
			this.buttonOk.UseUnderline = true;
			this.buttonOk.Label = "gtk-ok";
			this.AddActionWidget(this.buttonOk, -5);
			global::Gtk.ButtonBox.ButtonBoxChild w14 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w12[this.buttonOk]));
			w14.Position = 1;
			w14.Expand = false;
			w14.Fill = false;
			if ((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.DefaultWidth = 319;
			this.DefaultHeight = 279;
			this.Show();
			this.Response += new global::Gtk.ResponseHandler(this.OnResponse);
			this.ExportTypeSelection.Changed += new global::System.EventHandler(this.OnExportTypeSelectionChanged);
		}
	}
}
