using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AvalonDock;
using Brainiac.Design.Attributes;

namespace Brainiac.Design
{
	/// <summary>
	/// Interaction logic for PropertiesWindow.xaml
	/// </summary>
	public partial class PropertiesWindow : DockableContent
	{
		public PropertiesWindow()
		{
			InitializeComponent();

			__propertyGrids.Add(this);

			PropertiesVisible(false);
		}

		// ----------------------------------------------------------

		private const int _padding= 1;

		private void PropertiesVisible(bool visible)
		{
			//propertiesSplitContainer.Visible= visible;
			//propertyNameLabel.Visible= visible;
			//propertyDescriptionLabel.Visible= visible;
		}

		private int _location= 0;

		private void ClearProperties()
		{
			_location= 0;

			//propertiesSplitContainer.Panel1.Controls.Clear();
			//propertiesSplitContainer.Panel2.Controls.Clear();
		}

		private void AddCategory(string name, bool expanded)
		{
			/*Label label= new Label();
			label.AutoSize= false;
			label.Text= name;
			label.Location= new Point(0, _location);
			label.Height= 15;
			label.BackColor= Color.DarkGray;
			label.Font= new Font(label.Font.FontFamily, label.Font.Size, FontStyle.Bold, GraphicsUnit.Point);
			propertiesSplitContainer.Panel1.Controls.Add(label);

			Panel panel= new Panel();
			panel.Location= new Point(0, _location);
			panel.Height= 15;
			panel.BackColor= Color.DarkGray;
			propertiesSplitContainer.Panel2.Controls.Add(panel);

			_location+= 15 + _padding;
			propertiesSplitContainer.Height= _location;*/
		}

		private Label AddProperty(string name, Type editorType, bool isReadOnly)
		{
			/*Label label= new Label();
			label.AutoSize= false;
			label.Text= name;
			label.TextAlign= ContentAlignment.MiddleLeft;
			label.Location= new Point(0, _location);
			propertiesSplitContainer.Panel1.Controls.Add(label);

			Control ctrl= null;
			if(editorType ==null)
			{
				ctrl= new Panel();
				ctrl.Height= 20;
				ctrl.BackColor= Color.Red;
			}
			else
			{
				DesignerPropertyEditor editor= (DesignerPropertyEditor)editorType.InvokeMember(string.Empty, BindingFlags.CreateInstance, null, null, new object[0]);

				if(isReadOnly)
					editor.ReadOnly();

				ctrl= editor;
			}

			label.Height= ctrl.Height;
			label.Tag= ctrl;
			ctrl.Location= new Point(0, _location);

			propertiesSplitContainer.Panel2.Controls.Add(ctrl);

			_location+= ctrl.Height + _padding;
			propertiesSplitContainer.Height= _location - _padding;

			return label;*/

			return null;
		}

		private void UpdateSizes()
		{
			/*foreach(Control ctrl in propertiesSplitContainer.Panel1.Controls)
				ctrl.Width= propertiesSplitContainer.Panel1.Width;

			foreach(Control ctrl in propertiesSplitContainer.Panel2.Controls)
				ctrl.Width= propertiesSplitContainer.Panel2.Width;*/
		}

		/*private void propertiesSplitContainer_SplitterMoved(object sender, SplitterEventArgs e)
		{
			UpdateSizes();
		}

		private void propertiesSplitContainer_Resize(object sender, EventArgs e)
		{
			UpdateSizes();
		}*/

		private void ShowDescription(string name, string description)
		{
			//propertyNameLabel.Text= name;
			//propertyDescriptionLabel.Text= description;
		}
		
		// ----------------------------------------------------------

		private static List<PropertiesWindow> __propertyGrids= new List<PropertiesWindow>();

		/// <summary>
		/// The number of property grids available.
		/// </summary>
		public static int Count
		{
			get { return __propertyGrids.Count; }
		}

		/// <summary>
		/// Forces all property grids to reinspect their objects.
		/// </summary>
		public static void UpdatePropertyGrids()
		{
			foreach(PropertiesWindow dock in __propertyGrids)
				dock.SelectedObject= dock.SelectedObject;
		}

		private object _selectedObject= null;

		internal object SelectedObject
		{
			set
			{
				ClearProperties();

				_selectedObject= value;

				string text= _selectedObject ==null ? "Properties" : "Properties of "+ _selectedObject.ToString();
				//Text= text;
				//TabText= text;

				// this is a hack to work around a bug in the docking suite
				text+= ' ';
				//Text= text;
				//TabText= text;

				if(_selectedObject ==null)
				{
					PropertiesVisible(false);
				}
				else
				{
					PropertiesVisible(false);

					IList<DesignerPropertyInfo> properties= DesignerProperty.GetDesignerProperties(_selectedObject.GetType(), DesignerProperty.SortByDisplayOrder);

					List<string> categories= new List<string>();
					foreach(DesignerPropertyInfo property in properties)
					{
						if(!categories.Contains(property.Attribute.CategoryResourceString))
							categories.Add(property.Attribute.CategoryResourceString);
					}

					categories.Sort();

					foreach(string category in categories)
					{
						AddCategory(Plugin.GetResourceString(category), true);

						foreach(DesignerPropertyInfo property in properties)
						{
							if(property.Attribute.CategoryResourceString ==category)
							{
								Type type= property.Attribute.GetEditorType(property.Property.GetValue(_selectedObject, null));
								Label label= AddProperty(property.Attribute.DisplayName, type, property.Attribute.HasFlags(DesignerProperty.DesignerFlags.ReadOnly));

								// register description showing
								//label.MouseEnter+= new EventHandler(label_MouseEnter);

								// when we found an editor we connect it to the object
								if(type !=null)
								{
									DesignerPropertyEditor editor= (DesignerPropertyEditor)label.Tag;
									editor.SetProperty(property, _selectedObject);
									editor.ValueWasAssigned();
									editor.MouseEnter += new EventHandler(editor_MouseEnter);
									editor.ValueWasChanged += new DesignerPropertyEditor.ValueChanged(editor_ValueWasChanged);
								}
							}
						}
					}

					UpdateSizes();
					PropertiesVisible(true);
				}
			}

			get { return _selectedObject; }
		}

		void editor_ValueWasChanged(DesignerPropertyInfo property)
		{
			string text= _selectedObject ==null ? "Properties" : "Properties of "+ _selectedObject.ToString();
			//Text= text;
			//TabText= text;

			// if we change a DesignerNodeProperty other properties of that object might be affected
			if(property.Attribute is DesignerNodeProperty)
			{
				UpdatePropertyGrids();
			}
		}

		void label_MouseEnter(object sender, EventArgs e)
		{
			Label label= (Label)sender;
			DesignerPropertyEditor editor= (DesignerPropertyEditor)label.Tag;

			ShowDescription(editor.Property.Attribute.DisplayName, editor.Property.Attribute.Description);
		}

		void editor_MouseEnter(object sender, EventArgs e)
		{
			DesignerPropertyEditor editor= (DesignerPropertyEditor)sender;

			ShowDescription(editor.Property.Attribute.DisplayName, editor.Property.Attribute.Description);
		}

		void item_Click(object sender, EventArgs e)
		{
			MenuItem item= (MenuItem)sender;

			Type editorType= (Type)item.Tag;
			Label label= null;  //(Label)item.Parent.Tag;
			DesignerPropertyEditor editor= (DesignerPropertyEditor)label.Tag;

			Debug.Check(_selectedObject ==editor.SelectedObject);

			Nodes.Node node= _selectedObject as Nodes.Node;
			if(node !=null)
				node.OnPropertyValueChanged(true);

			Attachments.Attachment attach= _selectedObject as Attachments.Attachment;
			if(attach !=null)
				attach.OnPropertyValueChanged(true);

			SelectedObject= _selectedObject;
		}

		internal static bool InspectObject(object obj)
		{
			if(__propertyGrids.Count <1)
				return false;

			__propertyGrids[0].SelectedObject= obj;
			return true;
		}

		/*protected override void OnClosed(EventArgs e)
		{
			SelectedObject= null;
			__propertyGrids.Remove(this);

			base.OnClosed(e);
		}*/
	}
}
