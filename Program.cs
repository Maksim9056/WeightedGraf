using System.Linq;

namespace ConsoleApp15
{
    public class Graf
    {
        // Найти крайчайщий путь  от стартортовой ноды до каждой ноды обход в ширину по соседям мы идем в ширину по соседям 
        // List<string> counter = new List<string>();
        // private Dictionary<string, List<Tuple<string, double>>> graph = new Dictionary<string, List<Tuple<string, double>>>();
        //// Use Tuple to store vertex and weight


        List<string> counter = new List<string>();

       // private Dictionary<string, Action<string,double>> graph = new Dictionary<string, Action<string, double>>();
        private Dictionary<string, List<Tuple<string, double>>> graph = new Dictionary<string, List<Tuple<string, double>>>();  // Use Tuple to store vertex and weight
        private List<string> vertexList;

        public Graf()
        {
            graph = new Dictionary<string, List<Tuple<string, double>>>();
            vertexList = new List<string>();
        }



        public void AddNode(string node)
        {
            if (!graph.ContainsKey(node))
            {
                graph.Add(node, new List<Tuple<string, double>>());
                counter.Add(node);
            }
        }


        public void AddEdge(string startVertex, string endVertex, double weight,double weight2)
        {
            if (graph.ContainsKey(startVertex) && graph.ContainsKey(endVertex))
            {
                if (!graph[startVertex].Any(t => t.Item1 == endVertex) && !graph[endVertex].Any(t => t.Item1 == startVertex))
                {
                    graph[startVertex].Add(new Tuple<string, double>(endVertex, weight));
                    graph[endVertex].Add(new Tuple<string, double>(startVertex, weight2));
                }
            }
        }



        public void DeleteNode(string node)
        {
            if (graph.ContainsKey(node))
            {
                foreach (var adjacentNodes in graph.Values)
                {
                    for (int i = adjacentNodes.Count - 1; i >= 0; i--)
                    {
                        if (adjacentNodes[i].Item1 == node)
                        {
                            adjacentNodes.RemoveAt(i);
                        }
                    }
                }
                graph.Remove(node);
                counter.Remove(node);
            }
        }

        public void DeleteEdge(string node1, string node2)
        {
            if (graph.ContainsKey(node1) && graph.ContainsKey(node2))
            {
                graph[node1].RemoveAll(t => t.Item1 == node2);
                graph[node2].RemoveAll(t => t.Item1 == node1);
            }
        }


        public void VisualizeGraph()
        {
            Console.WriteLine("Graph Visualization:");
            foreach (var startVertex in graph.Keys)
            {
                foreach (var endVertex in graph[startVertex])
                {
                    Console.WriteLine($"{startVertex} --- {endVertex}");
                }
            }
        }


        //public void bfs(string node)
        //{
        //    int[] distance = new int[counter.Count];
        //    string curr_node;
        //    HashSet<string> visited_bfs = new HashSet<string>();
        //    Queue<string> queue = new Queue<string>();
        //    visited_bfs.Add(node);
        //    queue.Enqueue(node);
        //    while (queue.Count > 0)
        //    {
        //        curr_node = queue.Dequeue();
        //        foreach (var NextNode in graph[curr_node])
        //        {
        //            if (!visited_bfs.Contains(NextNode))
        //            {
        //                queue.Enqueue(NextNode);
        //                visited_bfs.Add(NextNode);
        //                distance[counter.IndexOf(NextNode)] = distance[counter.IndexOf(curr_node)] + 1;
        //                Console.WriteLine($"Текущая нода: {curr_node}, Следующая нода: {NextNode}");
        //                Console.WriteLine(string.Join(", ", distance));
        //            }
        //        }
        //    }
        //}
        public void dfs(string node, HashSet<string> visited = null, double[] distance = null)
        {
            if (visited == null)
            {
                visited = new HashSet<string>();
            }
            if (distance == null)
            {
                distance = new double[counter.Count];
            }
            visited.Add(node);

            if (graph.ContainsKey(node))
            {
                foreach (var nextNode in graph[node])
                {
                    if (!visited.Contains(nextNode.Item1))
                    {
                        int currentNodeIndex = counter.IndexOf(node);
                        int nextNodeIndex = counter.IndexOf(nextNode.Item1);
                        if (currentNodeIndex != -1 && nextNodeIndex != -1)
                        {
                            distance[nextNodeIndex] = distance[currentNodeIndex] + nextNode.Item2;
                        }
                        Console.WriteLine($"Текущая нода: {node}");
                        Console.WriteLine(string.Join(", ", distance));
                        dfs(nextNode.Item1, visited, distance);
                    }
                }
            }
        }


        /// <summary>
        /// Найти крайчайщий путь  от стартортовой ноды до каждой ноды
        /// </summary>
        /// <returns></returns>
         public int[,] AdjacencyMatrixCalc()
         {

            int n = counter.Count;

            int[,] adjacencyMatrix = new int[n, n];

            for (int i = 0; i < n; i++)

            {

                var ithList = graph[counter[i]];

                foreach (var j in ithList)

                {

                    int indexToChange = counter.IndexOf(j.Item1);

                    adjacencyMatrix[i, indexToChange] = (int)j.Item2;

                }
            }

            return adjacencyMatrix;

        }

        public void ShortestPathsBFS(string startNode)
        {
            Dictionary<string, double> distances = new Dictionary<string, double>();
            Dictionary<string, string> previousNode = new Dictionary<string, string>();

            foreach (var node in graph.Keys)
            {
                distances[node] = double.PositiveInfinity;
            }
            distances[startNode] = 0;

            Queue<string> queue = new Queue<string>();
            queue.Enqueue(startNode);

            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();
                foreach (var neighbor in graph[currentNode])
                {
                    double distanceToNeighbor = distances[currentNode] + neighbor.Item2;
                    if (distanceToNeighbor < distances[neighbor.Item1])
                    {
                        distances[neighbor.Item1] = distanceToNeighbor;
                        previousNode[neighbor.Item1] = currentNode;
                        queue.Enqueue(neighbor.Item1);
                    }
                }
            }

            // Выводим кратчайшие пути в консоль
            foreach (var node in graph.Keys)
            {
                if (distances[node] == double.PositiveInfinity)
                {
                    Console.WriteLine($"Нет пути от {startNode} до {node}");
                }
                else
                {
                    Console.Write($"Кратчайший путь от {startNode} до {node}: {startNode}");
                    var current = node;
                    while (previousNode.ContainsKey(current))
                    {
                        Console.Write($" -> {current}");
                        current = previousNode[current];
                    }
                    Console.WriteLine($", расстояние: {distances[node]}");
                }
            }
        }


        public static void Main(string[] args)
        {




            Graf graf = new Graf();
            graf.AddNode("MIT");
            graf.AddNode("Park");
            graf.AddNode("Boylston");
            graf.AddNode("Tufts");
            graf.AddNode("Gov");
            graf.AddNode("Hay");
            graf.AddNode("State");
            graf.AddNode("Downtown");
            graf.AddNode("China Town");
            graf.AddNode("South");
            graf.AddNode("Airport");

        
            graf.AddEdge("MIT", "Park",1,40);
            graf.AddEdge("Boylston", "Park",2,43);
            graf.AddEdge("Downtown", "Park",3,45);
            graf.AddEdge("Gov", "Park",4,46);
            graf.AddEdge("Boylston", "Tufts",5,47);
            graf.AddEdge("Boylston", "Downtown",6,48);
            graf.AddEdge("Tufts", "China Town",7,49);
            graf.AddEdge("Tufts", "South",8,50);
            graf.AddEdge("Gov", "Hay",9,51);
            graf.AddEdge("Gov", "State",10,52);
            graf.AddEdge("State", "Hay", 11,53);
            graf.AddEdge("State", "Downtown", 12,54);
            graf.AddEdge("China Town", "Downtown", 13,55);
            graf.AddEdge("South", "Downtown", 14,56);
            graf.AddEdge("South", "China Town", 15,57);
            graf.AddEdge("South", "Airport", 16,58);
            graf.AddEdge("State", "Airport", 17,59);

            // другие вызовы AddEdge
            graf.VisualizeGraph();
            graf.dfs("MIT");


            Console.WriteLine("Алгоритм ближайщиего пути");

            graf.ShortestPathsBFS("MIT");

            Console.WriteLine("Выводим матрицу");
            var arr = graf.AdjacencyMatrixCalc();
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    Console.Write(arr[i, j] + " ");
                }
                Console.WriteLine();
            }

            Console.ReadLine();   
            //graf.AddEdge("MIT", "Park", 5.0);
            //graf.AddEdge("Boylston", "Park", 7.5);
            //Graf graf = new Graf();
            //graf.AddNode("MIT");
            //graf.AddNode("Park");
            //graf.AddNode("Boylston");
            //graf.AddNode("Tufts");
            //graf.AddNode("Gov");
            //graf.AddNode("Hay");
            //graf.AddNode("State");
            //graf.AddNode("Downtown");
            //graf.AddNode("China Town");
            //graf.AddNode("South");
            //graf.AddNode("Airport");

            //graf.AddEdge("MIT", "Park");
            //graf.AddEdge("Boylston", "Park");
            //graf.AddEdge("Downtown", "Park");
            //graf.AddEdge("Gov", "Park");
            //graf.AddEdge("Boylston", "Tufts");
            //graf.AddEdge("Boylston", "Downtown");
            //graf.AddEdge("Tufts", "China Town");
            //graf.AddEdge("Tufts", "South");
            //graf.AddEdge("Gov", "Hay");
            //graf.AddEdge("Gov", "State");
            //graf.AddEdge("State", "Hay");
            //graf.AddEdge("State", "Downtown");
            //graf.AddEdge("China Town", "Downtown");
            //graf.AddEdge("South", "Downtown");
            //graf.AddEdge("South", "China Town");
            //graf.AddEdge("South", "Airport");
            //graf.AddEdge("State", "Airport");
            //graf.VisualizeGraph();
            //graf.dfs("MIT");
        }
    }
}