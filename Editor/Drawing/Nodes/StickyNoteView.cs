using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Drawing.Nodes
{
    public class StickyNoteView : StickyNote
    {
        public StickyNoteView()
        {
            var content = this.Q<Label>(nameof(contents));
            content.RegisterValueChangedCallback(OnContentChanged);
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

        public Vector2 GetSize()
        {
            var width = this.style.width.value.value;
            var height = this.style.height.value.value;
            var size = new Vector2(width, height);
            return size;
        }

        private void OnContentChanged(ChangeEvent<string> evt)
        {
            if (string.IsNullOrWhiteSpace(title))
                title = "title";

            FitText(true);
        }
    }
}