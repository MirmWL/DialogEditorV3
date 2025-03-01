using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace BackPanelLib
{
	public class BackPanel
	{
		private readonly Panel _panel;
		private readonly int _cellSize;
		private readonly int _numOfCells;
		private readonly Pen _gridPen;
		private readonly Timer _timer;
		
		private bool _isDragging = false;
		private Point _dragCursorPoint;
		private Point _dragFormPoint;		

		public BackPanel(Panel panel, int cellSize, int numOfCells, Pen gridPen, int invalidateInterval)
		{
			_panel = panel;
			_cellSize = cellSize;
			_numOfCells = numOfCells;
			_gridPen = gridPen;

			_timer = new Timer();
			_timer.Interval = invalidateInterval;
			_timer.Tick += TimerTick;
			_timer.Start();
		}

		public void PanelMouseDown(object sender, MouseEventArgs e)
		{

			_isDragging = true;
		        _dragCursorPoint = Cursor.Position;
		        _dragFormPoint = _panel.Location;
		}

		public void PanelMouseMove(object sender, MouseEventArgs e)
		{
			
			if (_isDragging)
			{
				Point dif = Point.Subtract(Cursor.Position, new Size(_dragCursorPoint));
			        _panel.Location = Point.Add(_dragFormPoint, new Size(dif));
				//_panel.Invalidate(true);
			}
			
		}

		public void PanelMouseUp(object sender, MouseEventArgs e)
		{
			_isDragging = false;
		}

		public Control.ControlCollection GetControls()
		{
			return _panel.Controls;
		}		

		public void PanelDraw(object sender, PaintEventArgs e)
		{
			for (int i = 0; i < _numOfCells; i++)
			{
				e.Graphics.DrawLine(_gridPen, i * _cellSize, 0, i * _cellSize, _numOfCells * _cellSize);
				e.Graphics.DrawLine(_gridPen, 0, i * _cellSize, _numOfCells * _cellSize, i * _cellSize);
			}
		}
		
		private void TimerTick(object sender, EventArgs e)
		{
			_panel.Invalidate();
		}
	}

}
