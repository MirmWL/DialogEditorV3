using System;
using System.Drawing;
using System.Windows.Forms;

namespace UndefinedButtonFactoryLib
{
	public class UndefinedButtonFactory
	{
		private readonly Size _size;
		private readonly Point _position;
		private readonly string _text;
		private readonly Color _backColor;

		public UndefinedButtonFactory(Size size, Point position, string text, Color backColor)
		{
			_size = size;
			_position = position;
			_text = text;		
			_backColor = backColor;	
		}

		public Button Get()
		{
			Button button = new Button();
			button.Size = _size;
			button.Location = _position;
			button.Text = _text;
			button.BackColor = _backColor;

			return button;
		}
	}
}
