using System.Linq;
using Runtime.Nodes;

namespace Editor.Drawing.Nodes
{
    public class ChoicesNodeView : BaseNodeView<ChoicesNode>
    {
        private const string UxmlPath = "Assets/Editor/Resources/UXML/ChoicesNodeView.uxml";

        public ChoicesNodeView() : base(UxmlPath) { }
        
        public void AddChoice(string buttonId) =>
            Model.AddChoice(buttonId);

        protected override void OnModelChanged()
        {
            var outputChildren = outputContainer.Children().ToArray();
            foreach (var child in outputChildren) 
                outputContainer.Remove(child);
            
            foreach (var button in Model.Buttons) 
                AddOutput(button);
        }
    }
}