using System;
using System.IO;
using System.Windows.Forms;

using DialogTextFactoryLib;

namespace DialogFileFactoryLib
{
	public class DialogFileFactory
	{
		private readonly DialogTextFactory _textFactory;
		private readonly TextBox _pathToFileTextBox;

		public DialogFileFactory(DialogTextFactory textFactory, TextBox pathToFileTextBox)
		{
			_textFactory = textFactory;
			_pathToFileTextBox = pathToFileTextBox;
		}

		public StreamReader Get()
		{
			StreamWriter writer = new StreamWriter(_pathToFileTextBox.Text, false);
			writer.Write(_textFactory.Get());
			writer.Close();
			return new StreamReader(_pathToFileTextBox.Text);
		}
	}
}
