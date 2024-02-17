using Editor.Drawing.Controls;
using Runtime.Nodes;

namespace Editor.Drawing.Inspector
{
    public class AudioEventInspectorView : BaseControl
    {
        private const string Uxml = "UXML/Controls/AudioEventControl";

        private readonly AudioEventNode _node;
        
        public AudioEventInspectorView(AudioEventNode node) : base(Uxml)
        {
            _node = node;
        }
    }
}