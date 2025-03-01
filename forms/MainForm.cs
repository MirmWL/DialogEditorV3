using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing.Drawing2D;

using AddNodeButtonFactoryLib;
using SaveButtonFactoryLib;
using NodeFactoryLib;
using NodeLib;
using UndefinedButtonFactoryLib;
using LabelFactoryLib;
using TextBoxFactoryLib;
using BackPanelLib;
using ConnectsOwnerLib;
using ConnectsLib;
using ConnectLib;
using DialogTextFactoryLib;
using DialogFileFactoryLib;
using LoadButtonFactoryLib;

namespace MainFormLib
{
    public class MainForm : Form
    {
        private const int WINDOW_WIDTH = 1000;
        private const int WINDOW_HEIGHT = 500;

        private const int CONTROL_PANEL_WIDTH = 300;

        private const int ADD_NODE_BUTTON_HEIGHT = 25;
	
	private const int NODE_WIDTH = 150;
	private const int NODE_HEIGHT = 150;
	private const int NODE_START_POS_X = 0;
	private const int NODE_START_POS_Y = 0;

	private const int NODE_NAME_LABEL_HEIGHT = 20;
	private const int NODE_SPEECH_LABEL_HEIGHT = 30;	

	private const int NODE_NAME_TEXT_BOX_HEIGHT = 20;
	private const int NODE_SPEECH_TEXT_BOX_HEIGHT = 100;

	private const int CONNECT_BUTTON_WIDTH = 20;
	private const int CONNECT_BUTTON_HEIGHT = 20;

	private const int DELETE_NODE_BUTTON_WIDTH = 20;
	private const int DELETE_NODE_BUTTON_HEIGHT = 20;

	private const int NODE_DRAG_AREA_WIDTH = 50;

	private const int EDIT_NODE_BUTTON_HEIGHT = 20;

	private const int LINE_SEPARATING_TEXT_BOXES_HEIGHT = 15;
	private const int NODE_ID_LABEL_HEIGHT = 15;

	private const int GRID_CELL_SIZE = 30;


        public MainForm()
        {
	    this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);

	    Panel nodesPanel = new Panel();
		
	    int backPanelWidth = WINDOW_WIDTH*10;
	    int backPanelHeight = WINDOW_HEIGHT*10;

	    BackPanel backPanel = new BackPanel(nodesPanel, GRID_CELL_SIZE, backPanelWidth, new Pen(Color.Black), 5);
	     
	    nodesPanel.Paint += backPanel.PanelDraw;

	    nodesPanel.MouseDown += backPanel.PanelMouseDown;
	    nodesPanel.MouseMove += backPanel.PanelMouseMove;
	    nodesPanel.MouseUp += backPanel.PanelMouseUp;

	    nodesPanel.Width = backPanelWidth;
	    nodesPanel.Height = backPanelHeight;
	    nodesPanel.BackColor = Color.LightGray;
	    nodesPanel.AutoScroll = true;
	    
            this.Size = new Size(WINDOW_WIDTH, WINDOW_HEIGHT);
	    this.DoubleBuffered = true;

	    FlowLayoutPanel controlPanel = GetControlPanel();
	    
	    Color nodeBackColor = Color.LightBlue;
	    Color nodeDragBackColor = Color.LightYellow;
	    
	    BorderStyle nodeBorderStyle = BorderStyle.FixedSingle;

	    Point nodeStartPosition = new Point(NODE_START_POS_X, NODE_START_POS_Y);

	    UndefinedButtonFactory editButtonFactory = GetEditButtonFactory(nodeStartPosition);
	    UndefinedButtonFactory viewButtonFactory = GetViewButtonFactory(nodeStartPosition);
	    UndefinedButtonFactory connectButtonFactory = GetConnectButtonFactory();
	    UndefinedButtonFactory deleteNodeButtonFactory = GetDeleteNodeButtonFactory();
	    
	    LabelFactory nameLabelFactory = GetNameLabelFactory();
	    LabelFactory speechLabelFactory = GetSpeechLabelFactory();
	    LabelFactory lineSeparatingTextBoxesLabelFactory = GetLineSeparatingTextBoxesLabelFactory();
	    LabelFactory nodeIdLabelFactory = GetNodeIdLabelFactory();
	    LabelFactory controlPanelNodeIdLabelFactory = GetControlPanelNodeIdLabelFactory();

	    TextBoxFactory nodeNameTextBoxFactory = GetNodeNameTextBoxFactory();
	    TextBoxFactory nodeSpeechTextBoxFactory = GetNodeSpeechTextBoxFactory();
	    TextBoxFactory pathToFileTextBoxFactory = GetPathToFileTextBoxFactory();

	    Dictionary<int, KeyValuePair<Button, Button>> connectsButtonsIdPairs = new Dictionary<int, KeyValuePair<Button, Button>>();
	    Dictionary<int, Connects> connects = new Dictionary<int, Connects>();

	    Color connectDefaultColor = ColorTranslator.FromHtml("#0000FF");
	    Color connectHoverColor = ColorTranslator.FromHtml("#FFFF00");
	    Pen defaultConnectPen = new Pen(connectDefaultColor, 3);
	    Pen hoverConnectPen = new Pen(connectHoverColor, 3);
	    int connectCircleWidth = 15;
	    ConnectsOwner connectsOwner = new ConnectsOwner(nodesPanel, connects, connectsButtonsIdPairs, defaultConnectPen, hoverConnectPen, connectCircleWidth);		
	    nodesPanel.MouseMove += connectsOwner.MouseMove;
	    nodesPanel.MouseClick += connectsOwner.MouseClick;

	    Dictionary<int, Node> nodeIdPairs = new Dictionary<int, Node>();
	    Dictionary<int, Label> nodeControlIdLabelPairs = new Dictionary<int, Label>();
	
	    NodeFactory nodeFactory = new NodeFactory(
		backPanel,
		nodeBackColor,
		nodeDragBackColor,
		new Size(NODE_WIDTH, NODE_HEIGHT),
		nodeStartPosition,
		nodeBorderStyle,
		editButtonFactory,
		viewButtonFactory,
		connectButtonFactory,
		deleteNodeButtonFactory,
		nodeNameTextBoxFactory,
		nodeSpeechTextBoxFactory,
		connectsOwner,
		nameLabelFactory,
		speechLabelFactory,
		nodeIdLabelFactory,
		controlPanelNodeIdLabelFactory,
		lineSeparatingTextBoxesLabelFactory,
		controlPanel,
		connectsButtonsIdPairs,
		nodeIdPairs,
		nodeControlIdLabelPairs,
		this.Controls);

	    TextBox pathToFileTextBox = pathToFileTextBoxFactory.Get();
	    DialogTextFactory dialogTextFactory = new DialogTextFactory(connects, nodeIdPairs);
	    DialogFileFactory dialogFileFactory = new DialogFileFactory(dialogTextFactory, pathToFileTextBox);
	
            Button addNodeButton = GetAddNodeButton(nodeFactory);
            Button saveButton = GetSaveButton(dialogFileFactory);
	    Button loadButton = GetLoadButton(pathToFileTextBox, nodeFactory, connectsOwner);

            controlPanel.Controls.Add(addNodeButton);
	    controlPanel.Controls.Add(pathToFileTextBox);
            controlPanel.Controls.Add(saveButton);
	    controlPanel.Controls.Add(loadButton);
	    
            this.Controls.Add(controlPanel);
	    this.Controls.Add(nodesPanel);

	   
        }

        private Button GetAddNodeButton(NodeFactory nodeFactory)
        {
            AddNodeButtonFactory addNodeButtonFactory = new AddNodeButtonFactory(
		nodeFactory, 
		"Add node", 
		0, 
		0,
		CONTROL_PANEL_WIDTH,
		ADD_NODE_BUTTON_HEIGHT);

            Button addNodeButton = addNodeButtonFactory.Get();
            return addNodeButton;
        }

        private Button GetSaveButton(DialogFileFactory factory)
        {
            SaveButtonFactory saveButtonFactory = new SaveButtonFactory("Save", factory, 0, ADD_NODE_BUTTON_HEIGHT);
            Button saveButton = saveButtonFactory.Get();
            saveButton.Size = new Size(CONTROL_PANEL_WIDTH, ADD_NODE_BUTTON_HEIGHT);
            return saveButton;
        }

	private Button GetLoadButton(TextBox pathToFileTextBox, NodeFactory nodeFactory, ConnectsOwner connectsOwner)
	{
	    LoadButtonFactory loadButtonFactory = new LoadButtonFactory("Load", pathToFileTextBox, 0, ADD_NODE_BUTTON_HEIGHT, nodeFactory, connectsOwner);
            Button loadButton = loadButtonFactory.Get();
            loadButton.Size = new Size(CONTROL_PANEL_WIDTH, ADD_NODE_BUTTON_HEIGHT);
            return loadButton;
	}

	private UndefinedButtonFactory GetDeleteNodeButtonFactory()
	{
		Color backColor = Color.White;

		Point position = new Point(NODE_WIDTH - DELETE_NODE_BUTTON_WIDTH, 0);		
	
		return new UndefinedButtonFactory(
			new Size(DELETE_NODE_BUTTON_WIDTH, DELETE_NODE_BUTTON_HEIGHT),
			position,
			"X",
			backColor);
	}

	private UndefinedButtonFactory GetConnectButtonFactory()
	{
		Color backColor = Color.LightBlue;

		Point position = new Point(NODE_WIDTH / 2 - CONNECT_BUTTON_WIDTH / 2, 
				NODE_HEIGHT);		
	
		return new UndefinedButtonFactory(
			new Size(CONNECT_BUTTON_WIDTH, CONNECT_BUTTON_HEIGHT),
			position,
			"",
			backColor);
	}

	private UndefinedButtonFactory GetViewButtonFactory(Point nodeStartPosition)
	{
		Color backColor = Color.Pink;

		Point viewNodeButtonPosition = 
		new Point(nodeStartPosition.X, 
		nodeStartPosition.Y + NODE_HEIGHT - EDIT_NODE_BUTTON_HEIGHT);
		
		return	new UndefinedButtonFactory(
			new Size(NODE_WIDTH/2, EDIT_NODE_BUTTON_HEIGHT),
			viewNodeButtonPosition,
			"view",
			backColor);
	}

	private UndefinedButtonFactory GetEditButtonFactory(Point nodeStartPosition)
	{ 
		Color backColor = Color.Pink;

		Point editNodeButtonPosition = 
		new Point(nodeStartPosition.X + NODE_WIDTH / 2, 
		nodeStartPosition.Y + NODE_HEIGHT - EDIT_NODE_BUTTON_HEIGHT);

		return new UndefinedButtonFactory(
			new Size(NODE_WIDTH/2, EDIT_NODE_BUTTON_HEIGHT),
			editNodeButtonPosition,
			"edit",
			backColor);
	}

	private LabelFactory GetNameLabelFactory()
	{
		Color backColor = Color.Transparent;

		Point offset = new Point(0, NODE_ID_LABEL_HEIGHT);

		Size size = new Size(NODE_WIDTH - NODE_DRAG_AREA_WIDTH, NODE_NAME_LABEL_HEIGHT);

		return new LabelFactory(backColor, "name", offset, size, false);
	}

	private LabelFactory GetSpeechLabelFactory()
	{
		Color backColor = Color.Transparent;

		Point offset = new Point(0, NODE_NAME_LABEL_HEIGHT + NODE_ID_LABEL_HEIGHT);

		Size size = new Size(NODE_WIDTH - NODE_DRAG_AREA_WIDTH, NODE_SPEECH_LABEL_HEIGHT);

		return new LabelFactory(backColor, "speech", offset, size, true);		
	}

	private LabelFactory GetLineSeparatingTextBoxesLabelFactory()
	{
		Color backColor = ColorTranslator.FromHtml("#7DA8B6");
		Point offset = new Point(0,4);
		Size size = new Size(CONTROL_PANEL_WIDTH, LINE_SEPARATING_TEXT_BOXES_HEIGHT);

		return new LabelFactory(backColor, 
		"", 
		offset, 
		size, 
		false);

	}

	private LabelFactory GetControlPanelNodeIdLabelFactory()
	{
		Color backColor = Color.Transparent;
		Point offset = new Point(0, 0);
		Size size = new Size(CONTROL_PANEL_WIDTH, NODE_ID_LABEL_HEIGHT);

		return new LabelFactory(backColor,
		"",
		offset,
		size,
		false);
	}

	private LabelFactory GetNodeIdLabelFactory()
	{
		Color backColor = Color.Transparent;
		Point offset = new Point(0, 0);
		Size size = new Size(NODE_WIDTH, NODE_ID_LABEL_HEIGHT);

		return new LabelFactory(backColor,
		"",
		offset,
		size,
		false);
	}

	private TextBoxFactory GetNodeNameTextBoxFactory()
	{
		Point offset = new Point(0, ADD_NODE_BUTTON_HEIGHT * 2);
		Size size = new Size(CONTROL_PANEL_WIDTH, NODE_NAME_TEXT_BOX_HEIGHT);

		return new TextBoxFactory(offset, size, true, true, true, ScrollBars.None);
	}

	private TextBoxFactory GetNodeSpeechTextBoxFactory()
	{
		Point offset = new Point(0, ADD_NODE_BUTTON_HEIGHT * 3);
		Size size = new Size(CONTROL_PANEL_WIDTH, NODE_SPEECH_TEXT_BOX_HEIGHT);

		return new TextBoxFactory(offset, size, true, true, true, ScrollBars.Vertical);
	}

	private TextBoxFactory GetPathToFileTextBoxFactory()
	{
		Point offset = new Point(0, ADD_NODE_BUTTON_HEIGHT * 2);
		Size size = new Size(CONTROL_PANEL_WIDTH, ADD_NODE_BUTTON_HEIGHT);

		return new TextBoxFactory(offset, size, true, true, true, ScrollBars.Vertical);
	
	}
	
	private FlowLayoutPanel GetControlPanel()
	{
		FlowLayoutPanel controlPanel = new FlowLayoutPanel();
		controlPanel.Dock = DockStyle.Right;
	        controlPanel.Width = CONTROL_PANEL_WIDTH;
	        controlPanel.BackColor = Color.LightBlue;
	
		return controlPanel;

	}
    }
}
