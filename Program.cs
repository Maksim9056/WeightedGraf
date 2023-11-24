using System.Linq;

namespace ConsoleApp15
{
    public class Graf
    {
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


        public void AddEdge(string startVertex, string endVertex, double weight)
        {
            if (graph.ContainsKey(startVertex) && graph.ContainsKey(endVertex))
            {
                if (!graph[startVertex].Any(t => t.Item1 == endVertex) && !graph[endVertex].Any(t => t.Item1 == startVertex))
                {
                    graph[startVertex].Add(new Tuple<string, double>(endVertex, weight));
                    graph[endVertex].Add(new Tuple<string, double>(startVertex, weight));
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

            //graf.AddEdge("MIT", "Park", 5.0);
            //graf.AddEdge("Boylston", "Park", 7.5);
            graf.AddEdge("MIT", "Park",1);
            graf.AddEdge("Boylston", "Park",2);
            graf.AddEdge("Downtown", "Park",3);
            graf.AddEdge("Gov", "Park",4);
            graf.AddEdge("Boylston", "Tufts",5);
            graf.AddEdge("Boylston", "Downtown",6);
            graf.AddEdge("Tufts", "China Town",7);
            graf.AddEdge("Tufts", "South",8);
            graf.AddEdge("Gov", "Hay",9);
            graf.AddEdge("Gov", "State",10);
            graf.AddEdge("State", "Hay", 11);
            graf.AddEdge("State", "Downtown", 12);
            graf.AddEdge("China Town", "Downtown", 13);
            graf.AddEdge("South", "Downtown", 14);
            graf.AddEdge("South", "China Town", 15);
            graf.AddEdge("South", "Airport", 16);
            graf.AddEdge("State", "Airport", 17);

            // другие вызовы AddEdge
            graf.VisualizeGraph();
            graf.dfs("MIT");

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