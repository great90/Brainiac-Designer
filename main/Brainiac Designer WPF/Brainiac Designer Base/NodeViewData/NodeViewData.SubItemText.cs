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

using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace Brainiac.Design
{
	public partial class NodeViewData
	{
		/// <summary>
		/// A subitem used to draw text on the node.
		/// </summary>
		public abstract class SubItemText : SubItem
		{
			public enum Alignment { Left, Center, Right };

			/// <summary>
			/// The background color when the subitem is not selected.
			/// </summary>
			protected Brush _backgroundNormal;

			/// <summary>
			/// The background color when the subitem is selected.
			/// </summary>
			protected Brush _backgroundSelected;

			/// <summary>
			/// The font used to draw the text.
			/// </summary>
			protected Font _labelFont;

			/// <summary>
			/// The brush used to draw the text.
			/// </summary>
			protected Brush _labelBrush;

			/// <summary>
			/// The width of the font being drawn.
			/// </summary>
			protected double _width;

			/// <summary>
			/// The alignment of the text.
			/// </summary>
			protected Alignment _alignment;

			protected abstract string Label { get; }

			public override double Width
			{
				get { return _width; }
			}

			public override void Update(NodeViewData node, DrawingContext dc)
			{
				// calculate the extent used by the label
				FormattedText formattedText= _labelFont.FormatText(Label, Brushes.White);

				_width= formattedText.Width;
			}

			public override void Draw(DrawingContext dc, NodeViewData nvd, Rect boundingBox)
			{
				// render background
				DrawBackground(dc, nvd, _selected ? _backgroundSelected : _backgroundNormal);

				// render the label
				Point center= new Point(boundingBox.Left + boundingBox.Width *0.5f, boundingBox.Top + boundingBox.Height *0.5f);

				FormattedText formattedText= _labelFont.FormatText(Label, _labelBrush);

				// draw text
				switch(_alignment)
				{
					case(Alignment.Left):
						dc.DrawText( formattedText, new Point(boundingBox.Left +6.0, center.Y - formattedText.Height *0.5) );
					break;

					case(Alignment.Center):
						dc.DrawText( formattedText, new Point(center.X - formattedText.Width *0.5, center.Y - formattedText.Height *0.5) );
					break;

					case(Alignment.Right):
						dc.DrawText( formattedText, new Point(boundingBox.Right - formattedText.Width -6.0, center.Y - formattedText.Height *0.5) );
					break;
				}

				//graphics.DrawRectangle(Pens.Red, boundingBox.X, boundingBox.Y, boundingBox.Width, boundingBox.Height);
			}

			/// <summary>
			/// Creates a new subitem which can draw text on a node.
			/// </summary>
			/// <param name="backgroundNormal">The background brush used when the subitem is not selected.</param>
			/// <param name="backgroundSelected">The background brush used when the subitem is selected. If it cannot be selected, use null.</param>
			/// <param name="labelFont">The font used to draw the label.</param>
			/// <param name="labelBrush">The brush used to draw the label.</param>
			/// <param name="alignment">The alignment of the label.</param>
			/// <param name="showParallelToLabel">Holds if the subitem will be drawn next to the node's label or below it.</param>
			protected SubItemText(Brush backgroundNormal, Brush backgroundSelected, Font labelFont, Brush labelBrush, Alignment alignment, bool showParallelToLabel) : base(showParallelToLabel)
			{
				_backgroundNormal= backgroundNormal;
				_backgroundSelected= backgroundSelected;
				_labelFont= labelFont;
				_labelBrush= labelBrush;
				_alignment= alignment;
			}

			public override string ToString()
			{
				return Label;
			}
		}
	}
}
