using Runtime.Nodes;

namespace Editor.Drawing.Nodes
{
    public class AudioEventNodeView : BaseNodeView<AudioEventNode>
    {
        private const string UxmlPath = "Assets/Editor/Resources/UXML/AudioEventNodeView.uxml";
        
        public AudioEventNodeView() : base(UxmlPath) { }
        
        protected override void OnModelChanged()
        {
            
        }
    }
}