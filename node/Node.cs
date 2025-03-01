using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

using ConnectsOwnerLib;
using BackPanelLib;

namespace NodeLib
{
	public class Node
	{
		private string _name;
		private string _speech;

		private int _id;

		private bool isDragging = false;

		private Point _offset;	

		private readonly Button _editButton;	
		private readonly Button _viewButton;
		private readonly Button _topConnectButton;
		private readonly Button _bottomConnectButton;
		private readonly Button _deleteButton;

		private readonly Label _nameLabel;
		private readonly Label _speechLabel;

		private readonly TextBox _nameTextBox;
		private readonly TextBox _speechTextBox;

		private readonly Label _lineSeparatingLabel;

		private readonly ConnectsOwner _connectsOwner;		

		private readonly Color _dragColor;
		private readonly Color _defaultColor;

		private readonly Panel _panel;

		private readonly Dictionary<int, Node> _nodeIdPairs;
		private readonly Dictionary<int, Label> _controlIdLabelPairs;
		private readonly Dictionary<int, KeyValuePair<Button, Button>> _connectsButtonsIdPairs;
		private readonly Control.ControlCollection _controls;
		private readonly BackPanel _backPanel;
		private readonly Panel _controlPanel;

		public Node(
			string name, 
			string speech, 
			int id,
			Button editButton,
			Button viewButton,
			Button topConnectButton,
			Button bottomConnectButton,
			Button deleteButton,
			Label nameLabel,
			Label speechLabel,
			TextBox nameTextBox,
			TextBox speechTextBox,
			Label lineSeparatingLabel,
			ConnectsOwner connectsOwner,
			Color dragColor,
			Color defaultColor,
			Panel panel,
			Dictionary<int, Node> nodeIdPairs,
			Dictionary<int, Label> controlIdLabelPairs,
			Dictionary<int, KeyValuePair<Button, Button>> connectsButtonsIdPairs,
			Control.ControlCollection controls,
			BackPanel backPanel, 
			Panel controlPanel)
		{
			_name = name;
			_speech = speech;
			_id = id;
			_editButton = editButton;
			_viewButton = viewButton;
			_topConnectButton = topConnectButton;
			_bottomConnectButton = bottomConnectButton;
			_deleteButton = deleteButton;
			_nameLabel = nameLabel;
			_speechLabel = speechLabel;
			_nameTextBox = nameTextBox;
			_speechTextBox = speechTextBox;
			_lineSeparatingLabel = lineSeparatingLabel;
			_connectsOwner = connectsOwner;
			_dragColor = dragColor;
			_defaultColor = defaultColor;
			_panel = panel;
			_nodeIdPairs = nodeIdPairs;
			_controlIdLabelPairs = controlIdLabelPairs;
			_connectsButtonsIdPairs = connectsButtonsIdPairs;
			_controls = controls;
			_backPanel = backPanel;
			_controlPanel = controlPanel;
		}

		public void MouseDown(object sender, MouseEventArgs e)
	        {
	            isDragging = true;
	            _offset = new Point(e.X, e.Y);
	        }

	        public void MouseMove(object sender, MouseEventArgs e)
	        {
	            if (isDragging)
	            {
			
	                Panel panel = (Panel)sender;
			panel.Refresh();
			panel.BackColor = _dragColor;			
		
			Point newPoint = 
				new Point(
					panel.Location.X + e.X - _offset.X, 
					panel.Location.Y + e.Y - _offset.Y);

	                panel.Location = newPoint;

			_editButton.Location = 
				new Point(newPoint.X + panel.Size.Width / 2, 
				newPoint.Y + panel.Size.Height - _editButton.Size.Height);

			_viewButton.Location = 
				new Point(newPoint.X, 
				newPoint.Y + panel.Size.Height - _editButton.Size.Height);

			_topConnectButton.Location = 
				new Point(newPoint.X + panel.Size.Width / 2 - _topConnectButton.Size.Width / 2, 
				newPoint.Y - _topConnectButton.Size.Height);

			_bottomConnectButton.Location = 
				new Point(newPoint.X + panel.Size.Width / 2 - _topConnectButton.Size.Width / 2, 
				newPoint.Y + panel.Size.Height); 

			_deleteButton.Location = 
				new Point(newPoint.X + panel.Size.Width - _deleteButton.Size.Width, 
				newPoint.Y); 
	            }
	        }

	        public void MouseUp(object sender, MouseEventArgs e)
	        {
	            isDragging = false;
	            Panel panel = (Panel)sender;
		    panel.BackColor = _defaultColor;
	        }

		public void Edit(object sender, EventArgs e)
		{
			_nameLabel.Visible = false;
			_speechLabel.Visible = false;
			_nameTextBox.Visible = true;
			_speechTextBox.Visible = true;
		}

		public void View(object sender, EventArgs e)
		{
			_nameLabel.Visible = true;
			_speechLabel.Visible = true;
			_nameTextBox.Visible = false;
			_speechTextBox.Visible = false;

			_nameLabel.Text = _nameTextBox.Text;
			_speechLabel.Text = _speechTextBox.Text;
			
			_name = _nameTextBox.Text;
			_speech = _speechTextBox.Text;
		}

		public void StartConnectClick(object sender, EventArgs e)
		{
			_connectsOwner.StartConnect(_id);
		}

		public void CompleteConnectClick(object sender, EventArgs e)
		{
			_connectsOwner.CompleteConnect(_id);
		}

		public void Delete(object sender, EventArgs e)
		{
			Control.ControlCollection backPanelControls = _backPanel.GetControls();

			backPanelControls.Remove(_panel);
			backPanelControls.Remove(_topConnectButton);
			backPanelControls.Remove(_bottomConnectButton);
			backPanelControls.Remove(_editButton);
			backPanelControls.Remove(_viewButton);
			backPanelControls.Remove(_deleteButton);

			_controlPanel.Controls.Remove(_lineSeparatingLabel);
			_controlPanel.Controls.Remove(_controlIdLabelPairs[_id]);

			_nodeIdPairs.Remove(_id);

			_connectsButtonsIdPairs.Remove(_id);
			_controlIdLabelPairs.Remove(_id);
			_connectsOwner.DeleteConnectsNode(_id);
		}

		public int GetId() => _id;
		public string GetSpeech() => _speech;
		public string GetName() => _name;
		public Point GetPosition() => _panel.Location;

		public void SetId(int id) { _id = id; }
		public void SetName(string name) 
		{ 
			_name = name; 
			_nameLabel.Text = name;
			_nameTextBox.Text = name;
		}
		public void SetSpeech(string speech)
		{ 
			_speech = speech;
			_speechLabel.Text = speech;
			_speechTextBox.Text = speech;
		}

		public void SetPosition(Point position)
		{
			_panel.Location = position;

			_editButton.Location = 
				new Point(position.X + _panel.Size.Width / 2, 
				position.Y + _panel.Size.Height - _editButton.Size.Height);

			_viewButton.Location = 
				new Point(position.X, 
				position.Y + _panel.Size.Height - _editButton.Size.Height);

			_topConnectButton.Location = 
				new Point(position.X + _panel.Size.Width / 2 - _topConnectButton.Size.Width / 2, 
				position.Y - _topConnectButton.Size.Height);

			_bottomConnectButton.Location = 
				new Point(position.X + _panel.Size.Width / 2 - _topConnectButton.Size.Width / 2, 
				position.Y + _panel.Size.Height); 

			_deleteButton.Location = 
				new Point(position.X + _panel.Size.Width - _deleteButton.Size.Width, 
				position.Y); 
		}
	}
}
