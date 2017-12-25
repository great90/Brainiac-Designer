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

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Brainiac.Design;
using Brainiac.Design.Nodes;
using Brainiac.Design.Attributes;

namespace Brainiac.HTN.Nodes
{
	public class Method : Behavior
	{
		private readonly static Brush __theBackgroundBrush= new SolidBrush( Color.FromArgb(39,89,157) );

		public delegate void MethodWasModifiedEventDelegate(Method method);

		public static event MethodWasModifiedEventDelegate MethodWasModified;

		protected ConnectorMultiple _preconditions;

		public Method() : this(string.Empty)
		{
		}

		public Method(string label) : base(label, false)
		{
			_preconditions= new ConnectorPrecondition(_children, "Precondition {0}", "Preconditions", 1, int.MaxValue);
			_genericChildren= new ConnectorTask(_children, "Task {0}", "Tasks", 1, int.MaxValue);
		}

		public override NodeViewData CreateNodeViewData(NodeViewData parent, BehaviorNode rootBehavior)
		{
			NodeViewDataStyled nvd= new NodeViewDataStyled(parent, rootBehavior, this, null, __theBackgroundBrush, _label, _description);

			nvd.ChangeShape(NodeShape.Rectangle);

			return nvd;
		}

		protected string _methodType= string.Empty;

		[DesignerString("MethodType", "MethodTypeDesc", "CategoryBasic", DesignerProperty.DisplayMode.List, 0, DesignerProperty.DesignerFlags.NoFlags)]
		public string MethodType
		{
			get { return _methodType; }
			set { _methodType= value; }
		}

		protected int _priority = 0;

		[DesignerInteger("MethodPriority", "MethodPriorityDesc", "CategoryBasic", DesignerProperty.DisplayMode.List, 0, DesignerProperty.DesignerFlags.NoFlags, null, int.MinValue, int.MaxValue, 1, "UnitsPriority")]
		public int Priority
		{
			get { return _priority; }
			set { _priority= value; }
		}

		protected override void CloneProperties(Brainiac.Design.Nodes.Node newnode)
		{
			base.CloneProperties(newnode);

			Method node= (Method)newnode;
			node._methodType= _methodType;
		}

		public override void BehaviorWasModified()
		{
			base.BehaviorWasModified();

			if(MethodWasModified !=null)
				MethodWasModified(this);
		}
	}
}
