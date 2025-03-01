using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

using ConnectLib;
using ConnectsLib;
using UniqueConnectListLib;

namespace ConnectsOwnerLib
{
	public class ConnectsOwner
	{
		private readonly Panel _connectsPanel;
		private readonly Dictionary<int, Connects> _connectsNodeIdPairs;
		private readonly Dictionary<int, KeyValuePair<Button, Button>> _connectButtonsIdPairs;
		private readonly Pen _pen;
		private readonly Pen _hoverPen;
		private readonly int _connectCircleWidth;
		private int _currentOriginId;

		private MouseEventArgs _args;
		
		public ConnectsOwner(Panel connectsPanel, Dictionary<int ,Connects> connectNodeIdPairs, 
				Dictionary<int, KeyValuePair<Button, Button>> connectButtonsIdPairs, 
				Pen pen,
				Pen hoverPen,
				int connectCircleWidth)
		{		
			_connectsPanel = connectsPanel;
			_connectsNodeIdPairs = connectNodeIdPairs;
			_connectButtonsIdPairs = connectButtonsIdPairs;
			_pen = pen;
			_hoverPen = hoverPen;
			_connectCircleWidth = connectCircleWidth;
			_currentOriginId = -1;
			_connectsPanel.Paint += DrawConnects;
		}

		public void StartConnect(int originId)
		{
			_currentOriginId = originId;
		}

		public void CompleteConnect(int destinationId)
		{
			if(_currentOriginId == destinationId || _currentOriginId == -1) return;

			Connect connect = new Connect(_currentOriginId, destinationId);

			_connectsNodeIdPairs[_currentOriginId].Get().Add(connect);
		
			_currentOriginId = -1;
		}

		public void DrawConnects(object sender, PaintEventArgs e)
		{
			Point lineP1 = new Point(0);
			Point lineP2 = new Point(0);
			double distance = 0;

			foreach(var connectsIdPair in _connectsNodeIdPairs)
			{
				int i = connectsIdPair.Key;

				if(_connectsNodeIdPairs[i].Get().Count == 0) continue;
	
				for(int j = 0; j < _connectsNodeIdPairs[i].Get().Count; j++)
				{
					Connect connect = _connectsNodeIdPairs[i].Get()[j];

					Button button1 = _connectButtonsIdPairs[connect.GetOrigin()].Value;
					Button button2 = _connectButtonsIdPairs[connect.GetDestination()].Key;	

					lineP1.X = button1.Location.X + button1.Width / 2;
					lineP1.Y = button1.Location.Y + button1.Height;
					lineP2.X = button2.Location.X + button2.Width / 2;
					lineP2.Y = button2.Location.Y - _connectCircleWidth / 2;						

					distance = GetDistanceBetween(lineP1, lineP2, _args.Location);
					
					e.Graphics.DrawLine(distance <= _pen.Width * 2 ? _hoverPen : _pen, lineP1, lineP2);
					

					e.Graphics.DrawEllipse(_pen, 
					lineP2.X - _connectCircleWidth/2, 
					button2.Location.Y - _connectCircleWidth, 
					_connectCircleWidth, 
					_connectCircleWidth);
				}	
			    
			}
		}

		public void MouseMove(object sender, MouseEventArgs e) { _args = e; }

		public void MouseClick(object sender, MouseEventArgs e)
		{
			Point lineP1 = new Point(0);
			Point lineP2 = new Point(0);
			double distance = 0;
			
			foreach(var connectsIdPair in _connectsNodeIdPairs)
			{
				int i = connectsIdPair.Key;

				if(_connectsNodeIdPairs[i].Get().Count == 0) continue;
	
				for(int j = 0; j < _connectsNodeIdPairs[i].Get().Count; j++)
				{
					Connect connect = _connectsNodeIdPairs[i].Get()[j];

					Button button1 = _connectButtonsIdPairs[connect.GetOrigin()].Value;
					Button button2 = _connectButtonsIdPairs[connect.GetDestination()].Key;	

					lineP1.X = button1.Location.X + button1.Width / 2;
					lineP1.Y = button1.Location.Y + button1.Height;
					lineP2.X = button2.Location.X + button2.Width / 2;
					lineP2.Y = button2.Location.Y - _connectCircleWidth/2;
									
						
					distance = GetDistanceBetween(lineP1, lineP2, e.Location);
					
					if(distance <= _pen.Width * 2)
						DeleteConnect(connect.GetOrigin(), connect.GetDestination());
				}	
			    
			}
		}

		private double GetDistanceBetween(Point p1Line, Point p2Line, Point p3)
		{
			double x1 = p1Line.X;
		        double y1 = p1Line.Y;
		        double x2 = p2Line.X;
		        double y2 = p2Line.Y;
		        double x0 = p3.X;
		        double y0 = p3.Y;

			double dx = x2 - x1;
			double dy = y2 - y1;

		        if (dx == 0 && dy == 0)
		            return Math.Sqrt(Math.Pow(x0 - x1, 2) + Math.Pow(y0 - y1, 2));

		        double t = ((x0 - x1) * dx + (y0 - y1) * dy) / (dx * dx + dy * dy);

		        if (t < 0)
		            return Math.Sqrt(Math.Pow(x0 - x1, 2) + Math.Pow(y0 - y1, 2));
		        else if (t > 1)
		            return Math.Sqrt(Math.Pow(x0 - x2, 2) + Math.Pow(y0 - y2, 2));
		        else
		            return Math.Abs(dy * x0 - dx * y0 + x2 * y1 - y2 * x1) / Math.Sqrt(dx * dx + dy * dy);
		}

		public void DeleteConnectsNode(int originId)
		{
			foreach(var connectsIdPair in _connectsNodeIdPairs)
			{
				int i = connectsIdPair.Key;
				for(int j = 0; j < _connectsNodeIdPairs[i].Get().Count; j++)	
				{
					Connect connect = _connectsNodeIdPairs[i].Get()[j];
					if(connect.GetOrigin() == originId || connect.GetDestination() == originId)
						_connectsNodeIdPairs[i].Get().RemoveAt(j);
				}
			}			

			_connectsNodeIdPairs.Remove(originId);
		}

		public void DeleteConnect(int originId, int destinationId)
		{
			foreach(var connectsIdPair in _connectsNodeIdPairs)
			{
				int i = connectsIdPair.Key;
				for(int j = 0; j < _connectsNodeIdPairs[i].Get().Count; j++)	
				{
					Connect connect = _connectsNodeIdPairs[i].Get()[j];
					if(connect.GetOrigin() == originId && connect.GetDestination() == destinationId)
						_connectsNodeIdPairs[i].Get().RemoveAt(j);
				}
			}			
		}

		public void CreateConnectsNode(int nodeId)
		{	
			_connectsNodeIdPairs.Add(nodeId, new Connects(new UniqueConnectList(new List<Connect>())));
		}
	}
}
