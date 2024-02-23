using System;

namespace Nadsat.DialogueGraph.Runtime
{
    [Serializable]
    public class NodeLinks
    {
        public string FromGuid;
        public string FromPortId;
        public string ToGuid;
        public string ToPortId;
    }
}