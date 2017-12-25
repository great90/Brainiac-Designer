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

using System.Windows;
using System.Windows.Media;
using Brainiac.Design.Nodes;

namespace Brainiac.Design
{
	public partial class NodeViewData
	{
		/// <summary>
		/// Represents a subitem which allows you to render stuff on a node.
		/// </summary>
		public abstract class SubItem
		{
			protected bool _showParallelToLabel;

			/// <summary>
			/// If true the subitem is rndered parallel to the label.
			/// </summary>
			public bool ShowParallelToLabel
			{
				get { return _showParallelToLabel; }
			}

			protected bool _selected;

			/// <summary>
			/// Holds if the subitem is currently selected.
			/// </summary>
			public bool IsSelected
			{
				get { return _selected; }
				set { _selected= value; }
			}

			/// <summary>
			/// The displayed height of the item in the untransformed graph.
			/// </summary>
			public virtual double Height
			{
				get { return 14.0; }
			}

			/// <summary>
			/// The required untransformed width of the subitem.
			/// </summary>
			public abstract double Width { get; }

			/// <summary>
			/// Returns the object which can be selected. Is null when the subitem cannot be selected.
			/// </summary>
			public virtual object SelectableObject
			{
				get { return null; }
			}

			/// <summary>
			/// Holds if the subitem can be deleted by the user.
			/// </summary>
			public virtual bool CanBeDeleted
			{
				get { return false; }
			}

			/// <summary>
			/// Called when the node gets updated.
			/// </summary>
			/// <param name="node">The node the subitem belongs to.</param>
			/// <param name="dc">The graphics object used for the update, NOT for drawing!</param>
			public abstract void Update(NodeViewData node, DrawingContext dc);

			/// <summary>
			/// Called when the node is drawn.
			/// </summary>
			/// <param name="dc">The graphics object used to draw the subitem.</param>
			/// <param name="nvd">The node view data of the node the subitem belongs to.</param>
			/// <param name="boundingBox">The bounding box of the subitem. Drawing is clipped to this.</param>
			public abstract void Draw(DrawingContext dc, NodeViewData nvd, Rect boundingBox);

			/// <summary>
			/// Draws the node's shape as the background.
			/// </summary>
			/// <param name="dc">The graphics object used for drawing.</param>
			/// <param name="nvd">The node view data of the node the subitem belongs to.</param>
			/// <param name="brush">The brush to draw the node's shape.</param>
			protected void DrawBackground(DrawingContext dc, NodeViewData nvd, Brush brush)
			{
				if(brush !=null)
				{
					nvd.DrawShape(dc, nvd.BoundingBox, brush, null);
				}
			}

			/// <summary>
			/// Clones the subitem.
			/// </summary>
			/// <param name="newnode">The node the cloned subitem will belong to.</param>
			/// <returns>Returns a new instance of this subitem.</returns>
			public abstract SubItem Clone(Node newnode);

			/// <summary>
			/// Creates a new subitem instance.
			/// </summary>
			/// <param name="showParallelToLabel">Holds if the subitem will be drawn next to the label.</param>
			protected SubItem(bool showParallelToLabel)
			{
				_showParallelToLabel= showParallelToLabel;

				// a parallel drawn subitem cannot be selected or deleted.
				Debug.Check(!_showParallelToLabel || SelectableObject ==null && !CanBeDeleted);
			}
		}
	}
}
