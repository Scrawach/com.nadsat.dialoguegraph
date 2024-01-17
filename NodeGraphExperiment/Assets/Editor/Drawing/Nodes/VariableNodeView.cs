using UnityEditor.Experimental.GraphView;

namespace Editor.Drawing.Nodes
{
    public class VariableNodeView : Node
    {
        private const string UxmlPath = "Assets/Editor/Resources/UXML/VariableNodeView.uxml";

        public VariableNodeView() : base(UxmlPath)
        { }
    }
}