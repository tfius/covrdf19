using System;
using System.Collections.Generic;
using System.Text;

namespace covrdf.Model
{
    public class ParagraphAnalysis
    {
        public string[] sentences;
        public string[] tokens;
        public string[] partsOfSpeech;
        public string coreference;
    }
    // Graph holds
    public class Graph : Node
    {
        public Graph()
        {
            paperIdx = -1;
        }
    }

    public class Node
    {
        public Node()
        {

        }
        public int AddNode(int property, Paper p)
        {
            //nodes.Add(n);
            return 0;
        }

        public long paperIdx { get; set; } // they all have index except root node
        public List<Node> nodes = new List<Node>();
    }
}
