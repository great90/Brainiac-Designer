////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2010, Daniel Kollmann
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

using System.Windows.Media;
using Brainiac.Design;
using Brainiac.Design.Nodes;

namespace Brainiac.HTN
{
	public class NodeViewDataPrecondition : NodeViewDataStyled
	{
		protected SolidColorBrush _backgroundBrush;
		protected SolidColorBrush _draggedBackgroundBrush;

		protected SolidColorBrush _backgroundBrushNeg;
		protected SolidColorBrush _draggedBackgroundBrushNeg;

		public NodeViewDataPrecondition(NodeViewData parent, BehaviorNode rootBehavior, Node node, Pen borderPen, SolidColorBrush backgroundBrush, SolidColorBrush backgroundBrushNeg, string label, string description) : base(parent, rootBehavior, node, borderPen, backgroundBrush, label, description)
		{
			_backgroundBrush= backgroundBrush;
			_draggedBackgroundBrush= GetDraggedBrush(backgroundBrush);

			_backgroundBrushNeg= backgroundBrushNeg;
			_draggedBackgroundBrushNeg= GetDraggedBrush(backgroundBrushNeg);
		}

		public override void SynchronizeWithNode(ProcessedBehaviors processedBehaviors)
		{
			Nodes.Precondition pre= (Nodes.Precondition)_node;

			_defaultStyle.Background= pre.Negate ? _backgroundBrushNeg : _backgroundBrush;
			_draggedStyle.Background= pre.Negate ? _draggedBackgroundBrushNeg : _draggedBackgroundBrush;

			base.SynchronizeWithNode(processedBehaviors);
		}
	}
}
