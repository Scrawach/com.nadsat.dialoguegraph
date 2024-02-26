using Nadsat.DialogueGraph.Runtime.Nodes;

namespace Nadsat.DialogueGraph.Editor.Drawing.Nodes
{
    public class AudioEventNodeView : BaseNodeView<AudioEventNode>
    {
        private const string UxmlPath = "UXML/AudioEventNodeView";
        
        public AudioEventNodeView() : base(UxmlPath) { }
        
        protected override void OnModelChanged()
        {
            
        }
    }
}