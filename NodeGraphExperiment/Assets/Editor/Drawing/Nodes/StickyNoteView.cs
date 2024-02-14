using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Editor.Drawing.Nodes
{
    public class StickyNoteView : StickyNote
    {
        public StickyNoteView()
        {
            var content = this.Q<Label>(nameof(contents));
            content.RegisterValueChangedCallback(OnContentChanged);
        }

        private void OnContentChanged(ChangeEvent<string> evt)
        {
            if (string.IsNullOrWhiteSpace(title))
                title = "title";
            
            FitText(true);
        }

        public override string title
        {
            get => base.title;
            set
            {
                base.title = value;
                FitText(true);
            }
        }
    }
}