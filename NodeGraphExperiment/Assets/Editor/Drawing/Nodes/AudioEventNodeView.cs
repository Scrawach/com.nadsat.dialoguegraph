using Editor.Paths;
using Runtime.Nodes;

namespace Editor.Drawing.Nodes
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