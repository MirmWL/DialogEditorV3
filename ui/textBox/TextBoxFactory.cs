using System;
using System.Drawing;
using System.Windows.Forms;

namespace TextBoxFactoryLib
{
	public class TextBoxFactory
	{
		private readonly Point _position;
		private readonly Size _size;
		private readonly bool _multiline;
		private readonly bool _wordWrap;
		private readonly bool _autoSize;
		private readonly ScrollBars _scrollBars;
		
		public TextBoxFactory(Point position, Size size, bool multiline, bool wordWrap, bool autoSize, ScrollBars scrollBars)
		{
			_position = position;
			_size = size;
			_multiline = multiline;
			_wordWrap = wordWrap;
			_autoSize = autoSize;
			_scrollBars = scrollBars;
		}

		public TextBox Get()
		{
			TextBox textBox = new TextBox();
			textBox.Location = _position;
			textBox.Size = _size;
			textBox.Multiline = _multiline;
			textBox.WordWrap = _wordWrap;
			textBox.AutoSize = _autoSize;
			textBox.ScrollBars = _scrollBars;
			return textBox;
		}
	}


}
