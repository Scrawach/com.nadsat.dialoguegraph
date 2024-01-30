using Runtime.Nodes;

namespace Editor.Drawing.Nodes
{
    public class SwitchNodeView : BaseNodeView<SwitchNode>
    {
        private const string UxmlPath = "Assets/Editor/Resources/UXML/SwitchNodeView.uxml";

        public SwitchNodeView() : base(UxmlPath) { }

        protected override void OnModelChanged()
        {
            outputContainer.Clear();
            foreach (var branch in Model.Branches) 
                AddOutput(branch.Condition);
        }
    }
}