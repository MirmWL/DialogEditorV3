using System;
using System.IO;
using System.Drawing;
using System.Xml.Linq;
using System.Windows.Forms;

using NodeLib;
using NodeFactoryLib;
using ConnectsOwnerLib;

namespace LoadButtonFactoryLib
{
	public class LoadButtonFactory
	{
		private readonly string _text;
		private readonly TextBox _pathToFileTextBox;
		private readonly int _positionX;
		private readonly int _positionY;

		private readonly NodeFactory _nodeFactory;
		private readonly ConnectsOwner _connectsOwner;

		public LoadButtonFactory(string text, TextBox pathToFileTextBox, int positionX, int positionY, NodeFactory nodeFactory, ConnectsOwner connectsOwner)
		{
			_text = text;
			_pathToFileTextBox = pathToFileTextBox;
			_positionX = positionX;
			_positionY = positionY;
			_nodeFactory = nodeFactory;
			_connectsOwner = connectsOwner;
		}

		public Button Get()
		{
			Button button = new Button();
			button.Text = _text;
			button.Location = new Point(_positionX, _positionY);
			button.Click += new EventHandler(Load);

			return button;
		}

		private void Load(object sender, EventArgs e)
		{
			if(File.Exists(_pathToFileTextBox.Text) == false) return;

			XDocument doc = XDocument.Load(_pathToFileTextBox.Text);
			ApplyDocument(doc);
		}

		private void ApplyDocument(XDocument doc)
	        {
	            var root = doc.Root;
		    int originId = 0;

		    var nodesElement = root.Element("nodes");
	            if(nodesElement != null)
	            {
	                var nodeElements = nodesElement.Elements("node");
	                 foreach (var nodeElement in nodeElements)
	                {
	                   var node = _nodeFactory.Get(int.Parse(nodeElement.Element("id").Value));
	                   node.SetPosition(new Point(int.Parse(nodeElement.Element("positionX").Value), int.Parse(nodeElement.Element("positionY").Value)));
	                   node.SetName(nodeElement.Element("name").Value);
	                   node.SetSpeech(nodeElement.Element("speech").Value);
	                }
	            }

	            var connectsElements = root.Elements("connects");
	            foreach (var connectsElement in connectsElements)
	            {
	               var originElement = connectsElement.Element("origin");
		       
	               if (originElement != null)
	                   originId = int.Parse(originElement.Value);
		       
	               var destinationElements = connectsElement.Elements("destination");

	               foreach (var destinationElement in destinationElements)
	               {
	                  _connectsOwner.StartConnect(originId);
			  _connectsOwner.CompleteConnect(int.Parse(destinationElement.Value));
	               }
	            }

	            
	        } 
	}
}

