using System;
using System.Collections.Generic;

using ConnectLib;
using UniqueConnectListLib;

namespace ConnectsLib
{
	public class Connects
	{
		private readonly UniqueConnectList _connects;

		public Connects(UniqueConnectList connects)
		{
			_connects = connects;
		}

		public UniqueConnectList Get() 
		{
			/*foreach(var con in _connects)
			{
				Console.WriteLine(con.GetOrigin());
				Console.WriteLine(con.GetDestination());
			}*/

			return _connects;
		}
	}
}

