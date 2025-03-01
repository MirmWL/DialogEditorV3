using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

using NodeLib;
using NodeFactoryLib;

namespace AddNodeButtonFactoryLib
{
	public class AddNodeButtonFactory
	{
		private NodeFactory _nodeFactory;

		private readonly String _text;

		private readonly int _positionX;
		private readonly int _positionY;

		private readonly int _width;
		private readonly int _height;

		public AddNodeButtonFactory(
		NodeFactory nodeFactory, 
		String text, 
		int positionX, 
		int positionY, 
		int width, 
		int height)
		{
			_nodeFactory = nodeFactory;
			_text = text;
			_positionX = positionX;
			_positionY = positionY;
			_width = width;
			_height = height;
		}
	
		public Button Get()
		{
			Button button = new Button();
			button.Text = _text;
			button.Location = new Point(_positionX, _positionY);
			button.Size = new Size(_width, _height);
	                button.Click += AddNodeButtonClick;

			return button;
		}

		private void AddNodeButtonClick(object sender, EventArgs e)
	        {
		    _nodeFactory.Get();
	        }
	
	 
	}
}
