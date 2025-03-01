using System;
using System.Drawing;
using System.Windows.Forms;

using AddNodeButtonFactoryLib;
using MainFormLib;

public class Program{
	[STAThread]
	static void Main()
	{
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
	}
}
