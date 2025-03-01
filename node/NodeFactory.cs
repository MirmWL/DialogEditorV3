using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections.Generic;

using NodeLib;
using UndefinedButtonFactoryLib;
using LabelFactoryLib;
using TextBoxFactoryLib;
using BackPanelLib;
using ConnectsOwnerLib;

namespace NodeFactoryLib
{
	public class NodeFactory
	{
		private readonly BackPanel _backPanel;
	
		private readonly Color _backColor;
		private readonly Color _dragColor;

		private readonly Size _nodeSize; 
		private readonly Point _startPosition; 
		private readonly BorderStyle _borderStyle;

		private readonly UndefinedButtonFactory _editButtonFactory;
		private readonly UndefinedButtonFactory _viewButtonFactory;
		private readonly UndefinedButtonFactory _connectNodeButtonFactory;
		private readonly UndefinedButtonFactory _deleteNodeButtonFactory;

		private readonly LabelFactory _nameLabelFactory;
		private readonly LabelFactory _speechLabelFactory;
		private readonly LabelFactory _lineSeparatingTextBoxesLabelFactory;
		private readonly LabelFactory _nodeIdLabelFactory;
		private readonly LabelFactory _controlPanelNodeIdLabelFactory;

		private readonly TextBoxFactory _nameTextBoxFactory;
		private readonly TextBoxFactory _speechTextBoxFactory;
		
		private readonly ConnectsOwner _connectsOwner;

		private readonly FlowLayoutPanel _controlPanel;

		private readonly Dictionary<int, KeyValuePair<Button, Button>> _connectsButtonsIdPairs;	

		private readonly Dictionary<int, Node> _nodeIdPairs;
		private readonly Dictionary<int, Label> _nodeControlIdLabelPairs;

		private readonly Control.ControlCollection _controls;

		private int _nodeId;

		public NodeFactory(	
			BackPanel backPanel,
			Color backColor,
			Color dragColor,
			Size nodeSize, 
			Point startPosition, 
			BorderStyle borderStyle,
			UndefinedButtonFactory editButtonFactory,
			UndefinedButtonFactory viewButtonFactory,
			UndefinedButtonFactory connectNodeButtonFactory,
			UndefinedButtonFactory deleteNodeButtonFactory,
			TextBoxFactory nameTextBoxFactory,
			TextBoxFactory speechTextBoxFactory,
			ConnectsOwner connectsOwner,
			LabelFactory nameLabelFactory,
			LabelFactory speechLabelFactory,
			LabelFactory lineSeparatingTextBoxesLabelFactory,
			LabelFactory nodeIdLabelFactory,
			LabelFactory controlPanelNodeIdLabelFactory,
			FlowLayoutPanel controlPanel,
			Dictionary<int, KeyValuePair<Button, Button>> connectsButtonsIdPairs,
			Dictionary<int, Node> nodeIdPairs,
			Dictionary<int, Label> nodeControlIdLabelPairs,
			Control.ControlCollection controls)
		{
			_backPanel = backPanel;
			_backColor = backColor;
			_dragColor = dragColor;
			_nodeSize = nodeSize;
			_startPosition = startPosition;
			_borderStyle = borderStyle;
			_editButtonFactory = editButtonFactory;
			_viewButtonFactory = viewButtonFactory;
			_connectNodeButtonFactory = connectNodeButtonFactory;
			_deleteNodeButtonFactory = deleteNodeButtonFactory;
			_nameTextBoxFactory = nameTextBoxFactory;
			_speechTextBoxFactory = speechTextBoxFactory;
			_connectsOwner = connectsOwner;
			_nameLabelFactory = nameLabelFactory;
			_speechLabelFactory = speechLabelFactory;
			_nodeIdLabelFactory = nodeIdLabelFactory;
			_controlPanelNodeIdLabelFactory = controlPanelNodeIdLabelFactory;
			_lineSeparatingTextBoxesLabelFactory = lineSeparatingTextBoxesLabelFactory;
			_controlPanel = controlPanel;
			_connectsButtonsIdPairs = connectsButtonsIdPairs;
			_nodeIdPairs = nodeIdPairs;
			_nodeControlIdLabelPairs = nodeControlIdLabelPairs;
			_controls = controls;
			_nodeId = 0;
		}

		public Node Get(int id = -1)
		{
		    Panel nodePanel = new Panel();
		    
		    Button editButton = _editButtonFactory.Get();
		    Button viewButton = _viewButtonFactory.Get();
		    Button topConnectButton = _connectNodeButtonFactory.Get();
		    Button bottomConnectButton = _connectNodeButtonFactory.Get();
		    Button deleteNodeButton = _deleteNodeButtonFactory.Get();

		    Label nameLabel = _nameLabelFactory.Get();
		    Label speechLabel = _speechLabelFactory.Get();
		    Label lineSeparatingTextBoxesLabel = _lineSeparatingTextBoxesLabelFactory.Get();
		    Label controlPanelNodeIdLabel = _controlPanelNodeIdLabelFactory.Get();
		    Label nodeIdLabel = _nodeIdLabelFactory.Get();

		    

		    TextBox nameTextBox = _nameTextBoxFactory.Get();
		    TextBox speechTextBox = _speechTextBoxFactory.Get();

		    nameTextBox.Visible = false;
		    speechTextBox.Visible = false;

		    if(id != -1)
		    {
			_nodeId = id;
		    }
		    
		    Node node = new Node(
			"testName", 
			"testSpeech", 
			_nodeId,
			editButton,
			viewButton,
			topConnectButton,
			bottomConnectButton,
			deleteNodeButton,
			nameLabel,
			speechLabel,
			nameTextBox,
			speechTextBox,
			lineSeparatingTextBoxesLabel,
			_connectsOwner,
			_dragColor,
			_backColor,
			nodePanel,
			_nodeIdPairs,
			_nodeControlIdLabelPairs,
			_connectsButtonsIdPairs,
			_controls, 
			_backPanel,
			_controlPanel);	

		    
		    _nodeIdPairs.Add(_nodeId, node);		
		    _nodeControlIdLabelPairs.Add(_nodeId, controlPanelNodeIdLabel);
		    _connectsButtonsIdPairs.Add(_nodeId, new KeyValuePair<Button, Button>(topConnectButton, bottomConnectButton));
		    _connectsOwner.CreateConnectsNode(_nodeId);

		    nodeIdLabel.Text = _nodeId.ToString();
		    controlPanelNodeIdLabel.Text = _nodeId.ToString();

		    _nodeId = _nodeIdPairs.Keys.Max() + 1;

		    editButton.Click += node.Edit;
		    viewButton.Click += node.View;
		    topConnectButton.Click += node.CompleteConnectClick;
		    bottomConnectButton.Click += node.StartConnectClick;
		    deleteNodeButton.Click += node.Delete;

	            nodePanel.BackColor = _backColor;
	            nodePanel.Size = _nodeSize;
	            nodePanel.Location = _startPosition;
	            nodePanel.BorderStyle = _borderStyle;

	            nodePanel.MouseDown += node.MouseDown;
	            nodePanel.MouseMove += node.MouseMove;
	            nodePanel.MouseUp += node.MouseUp;

		    _controls.Add(nodePanel);

		    nodePanel.Controls.Add(topConnectButton);
		    nodePanel.Controls.Add(bottomConnectButton);
		    nodePanel.Controls.Add(deleteNodeButton);
		    nodePanel.Controls.Add(nodeIdLabel);
		    nodePanel.Controls.Add(nameLabel);
		    nodePanel.Controls.Add(speechLabel);
		    
		    _backPanel.GetControls().Add(topConnectButton);
		    _backPanel.GetControls().Add(bottomConnectButton);
		    _backPanel.GetControls().Add(deleteNodeButton);
		    _backPanel.GetControls().Add(editButton);
		    _backPanel.GetControls().Add(viewButton);
		    _backPanel.GetControls().Add(nodePanel);

		    _controlPanel.Controls.Add(lineSeparatingTextBoxesLabel);
		    _controlPanel.Controls.Add(controlPanelNodeIdLabel);
		    _controlPanel.Controls.Add(nameTextBox);
		    _controlPanel.Controls.Add(speechTextBox);		    

		    return node;
		}
	}

}
