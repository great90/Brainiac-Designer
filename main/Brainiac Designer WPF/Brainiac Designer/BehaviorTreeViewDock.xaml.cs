using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms;
using AvalonDock;
using Brainiac.Design.Nodes;
using MessageBox = System.Windows.MessageBox;

namespace Brainiac.Design
{
	/// <summary>
	/// Interaction logic for BehaviorTreeViewDock.xaml
	/// </summary>
	public partial class BehaviorTreeViewDock : DocumentContent
	{
		public BehaviorTreeViewDock()
		{
			InitializeComponent();

			//behaviorTreeView.MouseDown += new MouseEventHandler(BehaviorTreeView_MouseDown);
		}

		public BehaviorTreeView BehaviorTreeView
		{
			get { return behaviorTreeView; }
		}

		private static readonly List<BehaviorTreeViewDock> __instances = new List<BehaviorTreeViewDock>();
		internal static IList<BehaviorTreeViewDock> Instances
		{
			get { return __instances.AsReadOnly(); }
		}

		private static BehaviorTreeViewDock __lastFocusedInstance = null;
		internal static BehaviorTreeViewDock LastFocused
		{
			get { return __lastFocusedInstance; }
		}

		internal static BehaviorTreeViewDock GetBehaviorTreeViewDock(Nodes.BehaviorNode node)
		{
			foreach (BehaviorTreeViewDock dock in __instances)
			{
				if (dock.BehaviorTreeView.RootNode == node)
					return dock;
			}

			return null;
		}

		internal static BehaviorTreeView GetBehaviorTreeView(Nodes.BehaviorNode node)
		{
			foreach (BehaviorTreeViewDock dock in __instances)
			{
				if (dock.BehaviorTreeView.RootNode == node)
					return dock.BehaviorTreeView;
			}

			return null;
		}

		private void MakeFocused()
		{
			__lastFocusedInstance = this;
		}

		/*void BehaviorTreeView_MouseDown(object sender, MouseEventArgs e)
		{
			MakeFocused();
		}

		protected override void OnGotFocus(EventArgs e)
		{
			MakeFocused();

			base.OnGotFocus(e);
		}

		void RootNode_WasSaved(Brainiac.Design.Nodes.BehaviorNode node)
		{
			Text = ((Nodes.Node)node).Label;
			//TabText= ((Nodes.Node)node).Label;
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);

			__instances.Add(this);
		}

		protected override void OnClosed(EventArgs e)
		{
			if(__lastFocusedInstance ==this)
				__lastFocusedInstance= null;

			__instances.Remove(this);

			_behaviorTreeView.RootNode.WasSaved-= RootNode_WasSaved;

			base.OnClosed(e);
		}*/
	}
}
