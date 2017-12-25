////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2009, Daniel Kollmann
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, are permitted
// provided that the following conditions are met:
//
// - Redistributions of source code must retain the above copyright notice, this list of conditions
//   and the following disclaimer.
//
// - Redistributions in binary form must reproduce the above copyright notice, this list of
//   conditions and the following disclaimer in the documentation and/or other materials provided
//   with the distribution.
//
// - Neither the name of Daniel Kollmann nor the names of its contributors may be used to endorse
//   or promote products derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR
// IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
// WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY
// WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Media;
using Brainiac.Design.Nodes;
using System.Windows;
using AvalonDock;

namespace Brainiac.Design
{
	/// <summary>
	/// This is the main window which connects the BehaviorTreeList with each BehaviorTreeView.
	/// </summary>
	public partial class MainWindow : Window
	{
		// the filename used for storing the layout
		private static readonly string __layoutFile= Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +"\\Brainiac Designer\\layout.xml";

		private static DocumentPane __dockPanel;
		internal static DocumentPane DockPanel
		{
			get { return __dockPanel; }
		}

		internal MainWindow()
		{
			// add the designers resource manager to the list of all available resource managers
			Plugin.AddResourceManager(Properties.Resources.ResourceManager);

			InitializeComponent();

			// display the file version
			System.Reflection.Assembly assembly= System.Reflection.Assembly.GetExecutingAssembly();
			System.Version version= assembly.GetName().Version;

			Title+= " "+ version.Major +"."+version.Minor;

			Title+= " ("+ version.Build +')';

			// create docking panels
			__dockPanel= BehaviorTreeViewDocumentPane;

			// if we have no stored layout generate a default one
			/*if(!System.IO.File.Exists(__layoutFile))
			{
				BehaviorTreeListDock btlDock= new BehaviorTreeListDock();
				RegisterBehaviorTreeList(btlDock);
				btlDock.Show(dockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockLeft);

				new PropertiesDock().Show(dockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockRight);
			}*/

			_edgePenReadOnly= new Pen(Brushes.LightGray, 3.0);
			//_edgePenReadOnly.DashCap= System.Drawing.Drawing2D.DashCap.Round;
			//_edgePenReadOnly.DashStyle= System.Drawing.Drawing2D.DashStyle.Dash;
			//_edgePenReadOnly.DashPattern= new float[] { 4.0f, 3.0f };
		}

		/// <summary>
		/// The pen used to draw the edges connecting the nodes.
		/// </summary>
		private Pen _edgePen= new Pen(Brushes.Black, 3.0);

		/// <summary>
		/// The pen used to draw the edges connecting sub-referenced nodes.
		/// </summary>
		private Pen _edgePenReadOnly;

		/// <summary>
		/// Handles when an event of a node is selected.
		/// </summary>
		/// <param name="node">The node whose event is selected.</param>
		private void control_ClickEvent(NodeViewData node)
		{
			// if there is no subitem selected, use the properties of the node
			if(node.SelectedSubItem ==null || node.SelectedSubItem.SelectableObject ==null)
			{
				PropertiesWindow.InspectObject(node.Node);
			}
			// publish the properties of the subitem's selection object
			else
			{
				PropertiesWindow.InspectObject(node.SelectedSubItem.SelectableObject);
			}
		}

		/// <summary>
		/// Handles when a node is clicked.
		/// </summary>
		/// <param name="node">The node that was clicked.</param>
		private void control_ClickNode(NodeViewData node)
		{
			PropertiesWindow.InspectObject(node ==null ? null : node.Node);
		}

		/// <summary>
		/// Handles when a node is double-clicked.
		/// </summary>
		/// <param name="node">The node that was double-clicked.</param>
		private void control_DoubleClickNode(NodeViewData node)
		{
		}

		/// <summary>
		/// Handles when a behaviour is supposed to be presented to the user.
		/// </summary>
		/// <param name="node">The behaviour which will be presented to the user.</param>
		private BehaviorTreeViewDock behaviorTreeList_ShowBehavior(BehaviorNode node)
		{
			// check if there is a tab for the behaviour
			BehaviorTreeViewDock dock= BehaviorTreeViewDock.GetBehaviorTreeViewDock(node);
			BehaviorTreeView control= dock ==null ? null : dock.BehaviorTreeView;

			// if not create one.
			if(control ==null)
			{
				dock= new BehaviorTreeViewDock();
				dock.Title= ((Node)node).Label;

				control= dock.BehaviorTreeView;

				control.RootNode= node;

				control.EdgePen= _edgePen;
				control.EdgePenReadOnly= _edgePenReadOnly;
				control.BehaviorTreeList= behaviorTreeList;

				control.ClickNode+= control_ClickNode;
				control.ClickEvent+= control_ClickEvent;
				control.DoubleClickNode+= control_DoubleClickNode;

				dock.Show(dockManager);
				dock.Activate();
			}

			dock.Focus();

			return dock;
		}

		/// <summary>
		/// Handles when a behaviour was renamed.
		/// </summary>
		/// <param name="node">The behaviour which was renamed.</param>
		private void behaviorTreeList_BehaviorRenamed(BehaviorNode node)
		{
			((Node)node).DoWasModified();

			BehaviorTreeViewDock dock= BehaviorTreeViewDock.GetBehaviorTreeViewDock(node);
			if(dock !=null)
			{
				dock.Title= ((Node)node).Label;
			}
		}

		/// <summary>
		/// Handles when behaviours must be cleared.
		/// </summary>
		/// <param name="nodes">The nodes which must be saved and closed.</param>
		/// <param name="result">Returns which behaviours could be saved or discarded.</param>
		/// <param name="error">Is true if there was an error and result contains any false.</param>
		private void behaviorTreeList_ClearBehaviors(List<BehaviorNode> nodes, out bool[] result, out bool error)
		{
			// try to save the behaviours
			SaveResult saveres= SaveBehaviors(nodes, out result);

			// update the error value
			error= saveres !=SaveResult.Succeeded;

			// for each behaviour which was...
			for(int i= 0; i <nodes.Count; ++i)
			{
				// successfully saved...
				if(result[i])
				{
					BehaviorTreeViewDock dock= BehaviorTreeViewDock.GetBehaviorTreeViewDock(nodes[i]);
					//if(dock !=null)
					//	dock.Close();
				}
			}
		}

		/// <summary>
		/// Handles when the main window becomes visible.
		/// </summary>
		private void MainWindow_Shown(object sender, RoutedEventArgs e)
		{
			// set the stored settings for the window
			/*if(Settings.Default.MainWindowLocation.X >0 && Settings.Default.MainWindowLocation.Y >0 && Settings.Default.MainWindowSize.Width >0 && Settings.Default.MainWindowSize.Height >0)
			{
				Location= Settings.Default.MainWindowLocation;
				Size= Settings.Default.MainWindowSize;
			}

			WindowState= Settings.Default.MainWindowState;

			// restore layout
			if(System.IO.File.Exists(__layoutFile))
				dockPanel.LoadFromXml(__layoutFile, new WeifenLuo.WinFormsUI.Docking.DeserializeDockContent(GetContentFromPersistString));*/

			// make sure the window is focused
			Focus();

			try
			{
				// let the user select the workspace
				WorkspaceDialog dialog= new WorkspaceDialog();
				DialogResult result= dialog.ShowDialog();
				if(result ==System.Windows.Forms.DialogResult.Cancel)
				{
					Close();
					return;
				}

				Workspace workspace= dialog.Workspace;

				// set the default export folder
				ExportDialog.LastUsedExportFolder= workspace.DefaultExportFolder;

				// load the plugins
				behaviorTreeList.LoadPlugins("plugins", workspace.Plugins);

				// set the default behaviour folder
				behaviorTreeList.BehaviorFolder= workspace.Folder;
			}
			catch(Exception ex)
			{
				System.Windows.Forms.MessageBox.Show(ex.Message, "Cannot Load Plugins", MessageBoxButtons.OK);

				// close the editor
				Close();
			}


			// show controls
			if(Properties.Settings.Default.ShowControlsOnStartUp)
			{
				ControlsDialog ctrldlg= new ControlsDialog();
				ctrldlg.ShowDialog();
			}
		}

		/// <summary>
		/// The result of the save process.
		/// </summary>
		internal enum SaveResult { Succeeded, Failed, Cancelled }

		/// <summary>
		/// Tries to save all of the behaviours in the list.
		/// </summary>
		/// <param name="behaviors">The list of the behaviours you want to save.</param>
		/// <param name="result">Holds if a behaviour could be saved or not.</param>
		/// <returns>Returns Failed if a bahevaiour could not be saved. Returns Cancelled if the user cancelled the process.</returns>
		private SaveResult SaveBehaviors(List<BehaviorNode> behaviors, out bool[] result)
		{
			// create anew dialogue
			using(MainWindowCloseDialog dialog= new MainWindowCloseDialog())
			{
				// create the results
				result= new bool[behaviors.Count];

				// check if there is any unsaved behaviour and update the result accordingly
				bool hasUnsavedBehaviors= false;
				for(int i= 0; i <behaviors.Count; ++i)
				{
					if(behaviors[i].IsModified)
					{
						hasUnsavedBehaviors= true;

						// add the behaviour to the dialogue
						dialog.AddUnsavedBehavior(behaviors[i]);

						result[i]= false;
					}
					else
					{
						result[i]= true;
					}
				}

				// if there is no unsaved behaviour, we are done
				if(!hasUnsavedBehaviors)
					return SaveResult.Succeeded;

				// show the dialogue
				DialogResult dialogResult= dialog.ShowDialog();

				// check if the user cancelled the process
				if(dialogResult ==System.Windows.Forms.DialogResult.Cancel)
				{
					return SaveResult.Cancelled;
				}
					// the user decided to discard all the unsaved changes
				else if(dialogResult ==System.Windows.Forms.DialogResult.No)
				{
					for(int i= 0; i <behaviors.Count; ++i)
						result[i]= true;

					return SaveResult.Succeeded;
				}
					// the user decided to save some of the unsaved changes
				else if(dialogResult ==System.Windows.Forms.DialogResult.Yes)
				{
					// check for all the behaviours
					bool saveErrorOccured= false;
					for(int i= 0; i <behaviors.Count; ++i)
					{
						// check if the user wants to save the behaviour
						if(dialog.IsSelected(behaviors[i]))
						{
							string saveresult= string.Empty;
							bool saveerror= false;

							// try to save the behaviour
							try { saveresult= behaviorTreeList.SaveBehavior(behaviors[i], false); }
							catch { saveerror= true; }

							// if there was an exception or the user aborted the save
							if(saveerror || saveresult ==string.Empty)
							{
								saveErrorOccured= true;

								// the user aborted the save
								if(saveresult ==string.Empty)
									return SaveResult.Cancelled;
							}
							else
							{
								// everything is fine
								result[i]= true;
							}
						}
						else
						{
							// this behaviour will be discarded. No error
							result[i]= true;
						}
					}

					// check if we had an error
					return saveErrorOccured ? SaveResult.Failed : SaveResult.Succeeded;
				}
			}

			throw new Exception("undealt dialog return");
		}

		/// <summary>
		/// Handles when the user wants to close the editor.
		/// </summary>
		private void MainWindow_FormClosing(object sender, CancelEventArgs e)
		{
			// add all the new behaviours to the list of unsaved ones
			List<BehaviorNode> behaviorsToSave= new List<BehaviorNode>();
			behaviorsToSave.AddRange(behaviorTreeList.NewBehaviors);

			// add all loaded and modified behaviours to the unsaved ones
			foreach(BehaviorNode node in behaviorTreeList.LoadedBehaviors)
			{
				if(node.IsModified)
					behaviorsToSave.Add(node);
			}

			// ask the user what to do with them
			bool[] result;
			SaveResult saveres= SaveBehaviors(behaviorsToSave, out result);

			// if there was an error we may not close the editor
			e.Cancel= saveres !=SaveResult.Succeeded;

			// store GUI related stuff
			if(saveres ==SaveResult.Succeeded)
			{
				// store the layout and ensure the folder exists
				string dir= System.IO.Path.GetDirectoryName(__layoutFile);
				if(!System.IO.Directory.Exists(dir))
					System.IO.Directory.CreateDirectory(dir);

				//dockPanel.SaveAsXml(__layoutFile);

				// store the application's settings
				/*if(WindowState ==FormWindowState.Normal)
				{
					Settings.Default.MainWindowLocation= Location;
					Settings.Default.MainWindowSize= Size;
					Settings.Default.MainWindowState= FormWindowState.Normal;
				}
				else if(WindowState ==FormWindowState.Maximized)
				{
					Settings.Default.MainWindowState= FormWindowState.Maximized;
				}*/

				Properties.Settings.Default.Save();
			}
		}
	}
}
