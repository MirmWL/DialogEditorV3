using System;
using System.Drawing;
using System.Windows.Forms;

namespace LabelFactoryLib
{
	public class LabelFactory
	{
		private readonly Color _backColor;
		private readonly string _text;
		private readonly Point _position;
		private readonly Size _size;
		private readonly bool _autoSize;

		public LabelFactory(Color backColor, string text, Point position, Size size, bool autoSize)
		{
			_backColor = backColor;
			_text = text;
			_position = position;
			_size = size;
			_autoSize = autoSize;
		}

		public Label Get()
		{
			Label label = new Label();
			label.BackColor = _backColor;
			label.Location = _position;
			label.Size = _size;
			label.Text = _text;
			label.AutoSize = _autoSize;
			return label;
		}

	}


}
