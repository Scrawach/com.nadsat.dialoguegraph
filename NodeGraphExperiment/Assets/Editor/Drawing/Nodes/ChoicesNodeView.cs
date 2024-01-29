using Runtime.Nodes;

namespace Editor.Drawing.Nodes
{
    public class ChoicesNodeView : BaseNodeView<ChoicesNode>
    {
        private const string UxmlPath = "Assets/Editor/Resources/UXML/ChoicesNodeView.uxml";

        public ChoicesNodeView() : base(UxmlPath) { }

        protected override void OnModelChanged()
        {
            outputContainer.Clear();
            foreach (var button in Model.Choices) 
                AddOutput(button);
        }
    }
}