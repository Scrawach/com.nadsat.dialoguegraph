using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

namespace Nadsat.DialogueGraph.Editor.Drawing.Nodes.Abstract
{
    public interface IRemovablePorts
    {
        event Action<IEnumerable<Port>> PortRemoved;
    }
}