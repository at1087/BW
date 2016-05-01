using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTreeLib
{
    using System.ComponentModel;
    using System.Security.Cryptography.X509Certificates;
    public enum NodeAt { Root, Left, Right }

    public class Node
    {
        public string Key { get; set; }
        public Node LeftNode { get; set; }
        public Node RightNode { get; set; }

        public NodeAt NodeAt { get; set; }

        public int Level { get; set; }
    }
}
