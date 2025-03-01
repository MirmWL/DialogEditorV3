#Connect building
mcs -t:library ./connect/Connect.cs -out:./lib/Connect.dll

#Unique connect list building
mcs -r:./lib/Connect.dll -t:library ./connect/customList/UniqueConnectList.cs -out:./lib/UniqueConnectList.dll

#Connects building
mcs -r:./lib/Connect.dll -r:./lib/UniqueConnectList.dll -t:library ./connect/Connects.cs -out:./lib/Connects.dll

#Connects owner building
mcs -r:System.Drawing -r:System.Windows.Forms -r:./lib/Connect.dll -r:./lib/UniqueConnectList.dll -r:./lib/Connects.dll -t:library ./connect/ConnectsOwner.cs -out:./lib/ConnectsOwner.dll

#BackPanel building
mcs -r:System.Drawing -r:System.Windows.Forms -t:library ./ui/backPanel/BackPanel.cs -out:./lib/BackPanel.dll

#TextBox factory building
mcs -r:System.Drawing -r:System.Windows.Forms -t:library ./ui/textBox/TextBoxFactory.cs -out:./lib/TextBoxFactory.dll

#label factory building
mcs -r:System.Drawing -r:System.Windows.Forms -t:library ./label/LabelFactory.cs -out:./lib/LabelFactory.dll

#undefined button factory building
mcs -r:System.Drawing -r:System.Windows.Forms -t:library ./ui/button/UndefinedButtonFactory.cs -out:./lib/UndefinedButtonFactory.dll

#Node building
mcs -r:System.Drawing -r:System.Windows.Forms -r:./lib/ConnectsOwner.dll -r:./lib/BackPanel.dll -t:library ./node/Node.cs -out:./lib/Node.dll

#Dialog text factory building
mcs -r:System.Drawing -r:./lib/Connect.dll -r:./lib/Connects.dll -r:./lib/Node.dll -r:./lib/UniqueConnectList.dll -t:library  ./text/DialogTextFactory.cs -out:./lib/DialogTextFactory.dll

#Dialog file factory
mcs -r:System.Windows.Forms -r:./lib/DialogTextFactory.dll -t:library ./file/DialogFileFactory.cs -out:./lib/DialogFileFactory.dll

#Node factory building
mcs -r:System.Drawing -r:System.Windows.Forms -r:./lib/Node.dll -r:./lib/UndefinedButtonFactory.dll -r:./lib/LabelFactory.dll -r:./lib/TextBoxFactory.dll -r:./lib/BackPanel.dll -r:./lib/ConnectsOwner.dll -t:library ./node/NodeFactory.cs -out:./lib/NodeFactory.dll

#Button factories building
mcs -r:System.Drawing -r:System.Windows.Forms -r:./lib/Node.dll -r:./lib/NodeFactory.dll -t:library ./ui/button/AddNodeButtonFactory.cs -out:./lib/AddNodeButtonFactory.dll
mcs -r:System.Drawing -r:System.Windows.Forms -r:./lib/DialogFileFactory.dll -t:library ./ui/button/SaveButtonFactory.cs -out:./lib/SaveButtonFactory.dll
mcs -r:System.Windows.Forms -r:System.Drawing -r:System.Xml.Linq -r:./lib/NodeFactory.dll -r:./lib/ConnectsOwner.dll -r:./lib/Node.dll -t:library ./ui/button/LoadButtonFactory.cs -out:./lib/LoadButtonFactory.dll 

#forms building
mcs -r:System.Drawing -r:System.Windows.Forms -r:System.Xml.Linq -r:./lib/AddNodeButtonFactory.dll -r:./lib/SaveButtonFactory.dll -r:./lib/NodeFactory.dll -r:./lib/Node.dll -r:./lib/UndefinedButtonFactory.dll -r:./lib/LabelFactory.dll -r:./lib/TextBoxFactory.dll -r:./lib/BackPanel.dll -r:./lib/ConnectsOwner.dll -r:./lib/Connects.dll -r:./lib/Connect.dll -r:./lib/DialogTextFactory.dll -r:./lib/DialogFileFactory.dll -r:./lib/LoadButtonFactory.dll -t:library ./forms/MainForm.cs -out:./lib/MainForm.dll
