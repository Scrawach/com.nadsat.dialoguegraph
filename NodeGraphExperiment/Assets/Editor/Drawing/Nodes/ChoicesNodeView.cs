using Editor.AssetManagement;
using Runtime.Nodes;

namespace Editor.Drawing.Nodes
{
    public class ChoicesNodeView : BaseNodeView<ChoicesNode>
    {
        private const string UxmlPath = "Assets/Editor/Resources/UXML/ChoicesNodeView.uxml";
        
        private readonly ChoicesRepository _choices;

        public ChoicesNodeView(ChoicesRepository choices) : base(UxmlPath) =>
            _choices = choices;

        protected override void OnModelChanged()
        {
            outputContainer.Clear();
            foreach (var button in Model.Choices) 
                AddOutput(_choices.Get(button), button);
        }
    }
}