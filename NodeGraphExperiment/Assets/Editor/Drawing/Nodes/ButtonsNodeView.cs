using UnityEditor.Experimental.GraphView;

namespace Editor.Drawing.Nodes
{
    public class ButtonsNodeView : Node
    {
        private const string UxmlPath = "Assets/Editor/Resources/UXML/ButtonsNodeView.uxml";

        public ButtonsNodeView() : base(UxmlPath) { }
    }
}