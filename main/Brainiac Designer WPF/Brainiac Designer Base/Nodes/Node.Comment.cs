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
using System.Windows;
using System.Windows.Media;
using Brainiac.Design.Properties;
using Brainiac.Design.Attributes;

namespace Brainiac.Design.Nodes
{
	public partial class Node
	{
		public enum CommentColor { NoColor, Blue, Red, Green, Violet, Orange }

		/// <summary>
		/// This class represents a comment drawn in the graph.
		/// </summary>
		public sealed class Comment
		{
			private static readonly Font __font= new Font("Calibri,Arial", true, 8.0);

			private Brush _backgroundBrush;
			private Brush _labelBrush;
			private CommentColor _backgroundColor;

			/// <summary>
			/// The background color used for drawing.
			/// </summary>
			[DesignerEnum("NodeCommentBackground", "NodeCommentBackgroundDesc", "CategoryComment", DesignerProperty.DisplayMode.NoDisplay, 0, DesignerProperty.DesignerFlags.NoExport, null)]
			public CommentColor Background
			{
				get { return _backgroundColor; }

				set
				{
					_backgroundColor= value;

					switch(_backgroundColor)
					{
						case(CommentColor.NoColor):
							_backgroundBrush= null;
							_labelBrush= Brushes.Black;
						break;

						case(CommentColor.Blue):
							_backgroundBrush= new SolidColorBrush( Color.FromRgb(184, 204, 228) );
							_labelBrush= new SolidColorBrush( Color.FromRgb(54, 96, 146) );
						break;

						case(CommentColor.Red):
							_backgroundBrush= new SolidColorBrush( Color.FromRgb(229, 185, 183) );
							_labelBrush= new SolidColorBrush( Color.FromRgb(149, 55, 52) );
						break;

						case(CommentColor.Green):
							_backgroundBrush= new SolidColorBrush( Color.FromRgb(215, 227, 188) );
							_labelBrush= new SolidColorBrush( Color.FromRgb(118, 146, 60) );
						break;

						case(CommentColor.Violet):
							_backgroundBrush= new SolidColorBrush( Color.FromRgb(204, 193, 217) );
							_labelBrush= new SolidColorBrush( Color.FromRgb(95, 73, 122) );
						break;

						case(CommentColor.Orange):
							_backgroundBrush= new SolidColorBrush( Color.FromRgb(251, 213, 181) );
							_labelBrush= new SolidColorBrush( Color.FromRgb(227, 108, 9) );
						break;

						default: throw new Exception(Resources.ExceptionUnhandledCommentColor);
					}
				}
			}

			private string _comment;

			/// <summary>
			/// The comment stored.
			/// </summary>
			[DesignerString("NodeCommentText", "NodeCommentTextDesc", "CategoryComment", DesignerProperty.DisplayMode.NoDisplay, 0, DesignerProperty.DesignerFlags.NoExport)]
			public string Text
			{
				get { return _comment; }
				set { _comment= value; }
			}

			/// <summary>
			/// Creates a new comment for a node.
			/// </summary>
			/// <param name="comment">The comment shown.</param>
			public Comment(string comment)
			{
				_comment= comment;
				Background= CommentColor.NoColor;
			}

			/// <summary>
			/// Draws the background.
			/// </summary>
			/// <param name="dc">The graphics object we render to.</param>
			/// <param name="nvd">The view data of this node in the current view.</param>
			/// <param name="renderDepth">The depth which is still rendered.</param>
			/// <param name="padding">The padding between the nodes.</param>
			public void DrawBackground(DrawingContext dc, NodeViewData nvd, int renderDepth, Size padding)
			{
				Size size= nvd.GetTotalSize(padding.Width, renderDepth);

				if(_backgroundColor !=CommentColor.NoColor)
				{
					double commentPadding= Math.Min(padding.Height, padding.Width) *0.75;

					dc.DrawRoundedRectangle(_backgroundBrush, null, new Rect(nvd.LayoutRectangle.X - commentPadding *0.5, nvd.LayoutRectangle.Y - commentPadding *0.5, size.Width + commentPadding, size.Height + commentPadding), 6.0, 6.0);
				}
			}

			/// <summary>
			/// Draws the text.
			/// </summary>
			/// <param name="dc">The graphics object we render to.</param>
			/// <param name="nvd">The view data of this node in the current view.</param>
			public void DrawText(DrawingContext dc, NodeViewData nvd)
			{
				if(_comment !=string.Empty)
				{
					FormattedText formattedText= __font.FormatText(_comment, _labelBrush);

					dc.DrawText(formattedText, nvd.LayoutRectangle.Location);
				}
			}

			/// <summary>
			/// Returns a list of all properties which have a designer attribute attached.
			/// </summary>
			/// <returns>A list of all properties relevant to the designer.</returns>
			public IList<DesignerPropertyInfo> GetDesignerProperties()
			{
				return DesignerProperty.GetDesignerProperties(GetType());
			}

			/// <summary>
			/// Creates a copy of this comment.
			/// </summary>
			/// <returns>The copy of this comment.</returns>
			public Comment Clone()
			{
				Comment cm= new Comment(_comment);

				cm.Background= Background;

				return cm;
			}
		}
	}
}
