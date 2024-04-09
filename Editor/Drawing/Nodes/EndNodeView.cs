using Nadsat.DialogueGraph.Runtime.Nodes;

namespace Nadsat.DialogueGraph.Editor.Drawing.Nodes
{
    public class EndNodeView : BaseNodeView<EndNode>
    {
        private const string UxmlPath = "UXML/EndNodeView";
        
        public EndNodeView() : base(UxmlPath) { }
        
        protected override void OnModelChanged() { }

        public override void Initialize() => 
            AddInput();
    }
}