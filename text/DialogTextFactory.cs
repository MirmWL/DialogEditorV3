using System;
using System.Text;
using System.Drawing;
using System.Collections.Generic;

using ConnectLib;
using ConnectsLib;
using NodeLib;
using UniqueConnectListLib;

namespace DialogTextFactoryLib
{
	public class DialogTextFactory
	{
		private readonly Dictionary<int, Connects> _connectsIdPairs;
		private readonly Dictionary<int, Node> _nodeIdPairs;
		private readonly StringBuilder _stringBuilder;
		

		public DialogTextFactory(Dictionary<int, Connects> connectsIdPairs, Dictionary<int, Node> nodeIdPairs)
		{		
			_connectsIdPairs = connectsIdPairs;
			_nodeIdPairs = nodeIdPairs;
			_stringBuilder = new StringBuilder();
		}	

		public string Get()
		{
			_stringBuilder.Clear();
			_stringBuilder.AppendLine("<dialog>");

			foreach (var connectsIdPair in _connectsIdPairs)
			{
				if(connectsIdPair.Value.Get().Count == 0) continue;

				_stringBuilder.AppendLine($"  <connects>");
				_stringBuilder.AppendLine($"     <origin>{connectsIdPair.Value.Get()[0].GetOrigin()}</origin>");					

				foreach(Connect connect in connectsIdPair.Value.Get())
					_stringBuilder.AppendLine($"     <destination>{connect.GetDestination()}</destination>");

				_stringBuilder.AppendLine("  </connects>");				
			}

			_stringBuilder.AppendLine("  <nodes>");

			foreach (var nodeIdPair in _nodeIdPairs)
			{
				_stringBuilder.AppendLine($"    <node>");
				_stringBuilder.AppendLine($"       <id>{nodeIdPair.Key}</id>");
				_stringBuilder.AppendLine($"       <positionX>{nodeIdPair.Value.GetPosition().X}</positionX>");
				_stringBuilder.AppendLine($"       <positionY>{nodeIdPair.Value.GetPosition().Y}</positionY>");
				_stringBuilder.AppendLine($"       <name>{nodeIdPair.Value.GetName()}</name>");
				_stringBuilder.AppendLine($"       <speech>{nodeIdPair.Value.GetSpeech()}</speech>");
				_stringBuilder.AppendLine($"    </node>");
			}

			_stringBuilder.AppendLine("  </nodes>");
			_stringBuilder.AppendLine("</dialog>");

			return _stringBuilder.ToString();
		}
	}

}
