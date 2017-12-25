////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) 2011, Daniel Kollmann
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
using System.IO;
using Brainiac.Design.Nodes;
using Brainiac.Design.Attributes;
using Brainiac.Design;
using Brainiac.Design.Attachments;
using Brainiac.Design.Attachments.Overrides;
using Uprising.Properties;

namespace Uprising.Exporters
{
	/// <summary>
	/// This exporter generates .cs files which generate a static variable which holds the behaviour tree.
	/// </summary>
	public class ExporterJavaScript : Brainiac.Design.Exporters.Exporter
	{
		public ExporterJavaScript(BehaviorNode node, string outputFolder, string filename) : base(node, outputFolder, filename +".js")
		{
		}

		/// <summary>
		/// Mak sure the string fits the javascript guidelines.
		/// </summary>
		/// <param name="str">The string we want to process.</param>
		/// <returns>Returns the input string with the first letter being lower case.</returns>
		protected static string Lowercase(string str)
		{
			string firstLetter= str.Substring(0, 1).ToLowerInvariant();

			return firstLetter + str.Substring(1);
		}

		/// <summary>
		/// Returns the used class name of a given behaviour.
		/// </summary>
		/// <param name="bn">The behaviour whose classname we want to get.</param>
		/// <returns>The desired classname for the given behaviour.</returns>
		protected static string GetBehaviorClassName(BehaviorNode bn)
		{
			return "HTNMethod_"+ Path.GetFileNameWithoutExtension(bn.FileManager.Filename).Replace(" ", string.Empty);
		}

		protected void CollectRequiredModules(List<Object> requiredModules, Node node)
		{
			foreach(Node child in node.Children)
			{
				if(!requiredModules.Contains( child.GetType() ))
				{
					requiredModules.Add( child.GetType() );
				}

				// for referenced behaviours we add the behaviours as a reference
				ReferencedBehaviorNode refbn= child as ReferencedBehaviorNode;
				if(refbn !=null && !requiredModules.Contains(refbn.Reference))
				{
					requiredModules.Add(refbn.Reference);
				}

				CollectRequiredModules(requiredModules, child);
			}
		}

		/// <summary>
		/// Exports a behaviour to the given file.
		/// </summary>
		/// <param name="file">The file we want to export to.</param>
		/// <param name="behavior">The behaviour we want to export.</param>
		protected void ExportBehavior(StreamWriter file, BehaviorNode behavior)
		{
			// collect required modules
			List<Object> requiredModules= new List<Object>();
			CollectRequiredModules(requiredModules, (Node)behavior);

			// generate list of required module names
			List<string> requiredModulesNames= new List<string>( requiredModules.Count +1 );

			requiredModulesNames.Add("game.htn.nodes.method");

			foreach(Object obj in requiredModules)
			{
				Type t= obj as Type;
				if(t !=null)
				{
					requiredModulesNames.Add( "game.htn.nodes."+ Lowercase(t.Name) );
				}
			}

			foreach(Object obj in requiredModules)
			{
				BehaviorNode bn= obj as BehaviorNode;
				if(bn !=null)
				{
					requiredModulesNames.Add( GetBehaviorClassName(bn) );
				}
			}

			string classname= GetBehaviorClassName(behavior);
			string exportname= Lowercase( _filename.Replace('\\', '.') );

			// write comments
			file.Write( string.Format("// Exported behavior: {0}\n", _filename) );
			file.Write( string.Format("// Exported file:     {0}\n\n", behavior.FileManager.Filename) );

			// create module
			file.Write( string.Format("ig.module('game.htn.methods.{0}')\n\n", classname) );
			file.Write( ".requires(\n" );

			// list all required modules
			for(int i= 0; i <requiredModulesNames.Count; ++i)
			{
				if(i ==requiredModulesNames.Count -1)
				{
					file.Write( string.Format("\t'{0}'\n", requiredModulesNames[i]) );
				}
				else
				{
					file.Write( string.Format("\t'{0}',\n", requiredModulesNames[i]) );
				}
			}

			file.Write( ")\n\n.defines(function(){\n\n" );

			file.Write( string.Format("{0} = HTNNode_Method.extend({{\n\n", classname) );

			ExportProperties(file, (Node)behavior, "\t");

			file.Write( "\n});\n\n});\n" );
		}

		protected void ExportOverrides(StreamWriter file, Node node, string indent)
		{
			string none= Plugin.GetResourceString("DesignerNodePropertyNone");

			bool export= false;

			foreach(Attachment attach in node.Attachments)
			{
				Override overr= attach as Override;
				if(overr !=null && overr.PropertyToOverride !=none)
				{
					if(!export)
					{
						export= true;

						file.Write( string.Format("{0}applyOverrides: function()\n{0}{{\n", indent) );
					}

					string overrideValue;

					if(overr is OverrideRandom)
					{
						OverrideRandom or= (OverrideRandom)overr;

						overrideValue= string.Format("{0} + {1} * Math.random()", or.Min, or.Max - or.Min);
					}
					else
					{
						throw new Exception("Unhandled override found!");
					}

					file.Write( string.Format("{0}\tthis.{1}= {2};\n", indent, Lowercase( overr.PropertyToOverride ), overrideValue) );
				}
			}

			if(export)
			{
				file.Write( string.Format("{0}}},\n", indent) );
			}
		}

		/// <summary>
		/// Exports all the properties of a ode and assigns them.
		/// </summary>
		/// <param name="file">The file we are exporting to.</param>
		/// <param name="node">The node whose properties we are exporting.</param>
		/// <param name="indent">The indent for the currently generated code.</param>
		protected void ExportProperties(StreamWriter file, Node node, string indent)
		{
			// export all the properties
			IList<DesignerPropertyInfo> properties= node.GetDesignerProperties();
			for(int p= 0; p <properties.Count; ++p)
			{
				// we skip properties which are not marked to be exported
				if(properties[p].Attribute.HasFlags(DesignerProperty.DesignerFlags.NoExport))
					continue;

				// create the code which assigns the value to the node's property
				file.Write( string.Format("{0}{1}: {2},\n", indent, Lowercase( properties[p].Property.Name ), properties[p].GetExportValue(node)) );
			}

			// export any overrides present
			ExportOverrides(file, node, indent);

			// export children
			ExportChildren(file, node, indent);
		}

		protected void ExportChildren(StreamWriter file, Node node, string indent)
		{
			foreach(BaseNode.Connector conn in node.Connectors)
			{
				if(conn.ChildCount >0)
				{
					string connName= Lowercase( conn.Identifier );

					file.Write( string.Format("{0}{1}: [\n", indent, connName) );

					for(int i= 0; i <conn.ChildCount; ++i)
					{
						ExportNode(file, (Node)conn.GetChild(i), indent +'\t');
					}

					file.Write( string.Format("{0}],\n", indent) );
				}
			}
		}

		protected void ExportNode(StreamWriter file, Node node, string indent)
		{
			file.Write( string.Format("{0}new HTNNode_{1}( {{\n", indent, node.GetType().Name) );

			ExportProperties(file, node, indent +'\t');

			file.Write( string.Format("{0}}} ),\n", indent) );
		}

		/// <summary>
		/// Export the assigned node to the assigned file.
		/// </summary>
		public override void Export()
		{
			// get the abolute folder of the file we want toexport
			string folder= Path.GetDirectoryName(_outputFolder +'\\'+ _filename);

			// if the directory does not exist, create it
			if(!Directory.Exists(folder))
				Directory.CreateDirectory(folder);

			// export to the file
			StreamWriter file= new StreamWriter(_outputFolder +'\\'+ _filename);
			ExportBehavior(file, _node);
			file.Close();
		}
	}
}
