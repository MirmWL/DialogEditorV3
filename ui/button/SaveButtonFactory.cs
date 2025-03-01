using System;
using System.Drawing;
using System.Windows.Forms;

using DialogFileFactoryLib;

namespace SaveButtonFactoryLib
{
	public class SaveButtonFactory
	{
		private readonly string _text;
		private readonly DialogFileFactory _fileFactory;
		private readonly int _positionX;
		private readonly int _positionY;

		public SaveButtonFactory(string text, DialogFileFactory fileFactory, int positionX, int positionY)
		{
			_text = text;
			_fileFactory = fileFactory;
			_positionX = positionX;
			_positionY = positionY;			
		}

		public Button Get()
		{
			Button button = new Button();
			button.Text = _text;
			button.Location = new System.Drawing.Point(_positionX, _positionY);
			button.Click += new EventHandler(Save);

			return button;
		}

		private void Save(object obj, EventArgs args)
		{
			Console.WriteLine(_fileFactory.Get().ReadToEnd());
		}
	}

}
