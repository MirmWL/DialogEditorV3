using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DialogPlayer
{
    public class Connect
    {
        public int origin { get; set; }
        public List<int> destination { get; set; } = new List<int>();
    }

    public class Node
    {
        public int id { get; set; }
        public int positionX { get; set; }
        public int positionY { get; set; }
        public string name { get; set; }
        public string speech { get; set; }
    }

    public class DialogData
    {
        public List<Connect> ConnectsList { get; set; } = new List<Connect>();
        public List<Node> NodeList { get; set; } = new List<Node>();
    }


   public class DialogXmlReader
    {
           public DialogData Read(string filePath)
        {
            try
            {
                XDocument doc = XDocument.Load(filePath);
                return ParseDocument(doc);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"File not found: {filePath}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while reading/parsing XML: {ex.Message}");
                return null;
            }
        }

        private DialogData ParseDocument(XDocument doc)
        {
            var dialogData = new DialogData();
            var root = doc.Root;

              // Чтение блоков <connects>
            var connectsElements = root.Elements("connects");
            foreach (var connectsElement in connectsElements)
            {
               var connect = new Connect();
               var originElement = connectsElement.Element("origin");
               if (originElement != null)
               {
                   connect.origin = int.Parse(originElement.Value);
               }

               var destinationElements = connectsElement.Elements("destination");
               foreach (var destinationElement in destinationElements)
                {
                  connect.destination.Add(int.Parse(destinationElement.Value));
                }
                dialogData.ConnectsList.Add(connect);
            }

             // Чтение блоков <nodes>
            var nodesElement = root.Element("nodes");
            if(nodesElement != null)
            {
                var nodeElements = nodesElement.Elements("node");
                 foreach (var nodeElement in nodeElements)
                {
                   var node = new Node();
                   node.id = int.Parse(nodeElement.Element("id").Value);
                   node.positionX = int.Parse(nodeElement.Element("positionX").Value);
                   node.positionY = int.Parse(nodeElement.Element("positionY").Value);
                   node.name = nodeElement.Element("name").Value;
                   node.speech = nodeElement.Element("speech").Value;
                   dialogData.NodeList.Add(node);
                }
            }

            // Find the root node (no incoming connections) and move it to the front
            var rootNodeId = FindRootNodeId(dialogData.ConnectsList, dialogData.NodeList);
            if (rootNodeId != -1)
            {
                var rootNode = dialogData.NodeList.FirstOrDefault(n => n.id == rootNodeId);
                if (rootNode != null)
                {
                    dialogData.NodeList.Remove(rootNode);
                    dialogData.NodeList.Insert(0, rootNode);
                }
            }

           return dialogData;
        }

      private int FindRootNodeId(List<Connect> connectsList, List<Node> nodeList)
        {
           var destinationNodes = new HashSet<int>();
            foreach (var connects in connectsList)
            {
                foreach(var destNode in connects.destination)
                     destinationNodes.Add(destNode);
            }

            // Find the node that doesn't appear in any destination
             foreach (var node in nodeList)
             {
                 if(!destinationNodes.Contains(node.id))
                 {
                     return node.id;
                 }
             }
          return -1;
        }
    }


    public class DialogPlayer
    {
        private DialogData _dialogData;
        private string _playerName;


        public DialogPlayer(string filePath, string playerName)
        {
            var reader = new DialogXmlReader();
            _dialogData = reader.Read(filePath);
            _playerName = playerName;

            if (_dialogData == null)
            {
                Console.WriteLine("Failed to load dialog data.");
                return;
            }
        }

        public void Play()
        {
             if(_dialogData == null || _dialogData.NodeList.Count == 0)
            {
                Console.WriteLine("No dialog data is available. ");
                return;
            }


            int currentNodeId = _dialogData.NodeList.FirstOrDefault().id;

            while (true)
            {
                var currentNode = _dialogData.NodeList.FirstOrDefault(node => node.id == currentNodeId);

                if (currentNode == null)
                {
                    Console.WriteLine("Invalid Node ID. Aborting dialog.");
                    return;
                }

                if (currentNode.name != _playerName)
                {
                    Console.WriteLine($"\n{currentNode.name}: {currentNode.speech}");
                } else
                {
                   Console.WriteLine($"\n{currentNode.speech}");
                }


                var currentConnect = _dialogData.ConnectsList.FirstOrDefault(c => c.origin == currentNodeId);

                if (currentConnect == null || currentConnect.destination.Count == 0)
                {
                   Console.WriteLine("Конец.");
                   break;
                }

                if (currentConnect.destination.Count > 1)
                {
                    for (int i = 0; i < currentConnect.destination.Count; i++)
                    {
                        var nextNode = _dialogData.NodeList.FirstOrDefault(node => node.id == currentConnect.destination[i]);
                        if (nextNode != null)
                           Console.WriteLine($"{i + 1}. {nextNode.speech}");
                        else
                           Console.WriteLine($"{i + 1}. Node not found");
                    }
                    int choice;
                    while (true)
                    {
                        Console.Write("Сделайте выбор: ");
                       if(int.TryParse(Console.ReadLine(), out choice) && choice > 0 && choice <= currentConnect.destination.Count)
                       {
                          currentNodeId = currentConnect.destination[choice - 1];
                          break;
                       } else
                       {
                         Console.WriteLine("Invalid choice. Try again.");
                       }
                    }
                }
                else if (currentConnect.destination.Count == 1)
                {
                   currentNodeId = currentConnect.destination[0];
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string filePath = "../mainDialog"; // Replace with your actual file path
            string playerName = "Рыцарь"; // Replace with the name of your player character

             var player = new DialogPlayer(filePath, playerName);
             player.Play();
        }
    }
}
