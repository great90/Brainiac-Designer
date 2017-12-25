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
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Brainiac.Design.Attributes
{
	public partial class DesignerPropertyEditor : UserControl
	{
		public DesignerPropertyEditor()
		{
			InitializeComponent();
		}

		protected bool _valueWasAssigned= false;

		protected Attributes.DesignerPropertyInfo _property;
		public Attributes.DesignerPropertyInfo Property
		{
			get { return _property; }
		}

		protected object _object;
		public object SelectedObject
		{
			get { return _object; }
		}

		public virtual void SetProperty(Attributes.DesignerPropertyInfo property, object obj)
		{
			_property= property;
			_object= obj;
		}

		public void ValueWasAssigned()
		{
			_valueWasAssigned= true;
		}

		public delegate void ValueChanged(DesignerPropertyInfo property);
		public event ValueChanged ValueWasChanged;

		protected void OnValueChanged(DesignerPropertyInfo property)
		{
			if(!_valueWasAssigned)
				return;

			Nodes.Node node= _object as Nodes.Node;
			if(node !=null)
			{
				node.OnPropertyValueChanged(true);

				if(ValueWasChanged !=null)
					ValueWasChanged(property);

				return;
			}

			Attachments.Attachment attach= _object as Attachments.Attachment;
			if(attach !=null)
			{
				attach.OnPropertyValueChanged(true);

				if(ValueWasChanged !=null)
					ValueWasChanged(property);

				return;
			}
		}

		public virtual void ReadOnly()
		{
		}
	}
}
