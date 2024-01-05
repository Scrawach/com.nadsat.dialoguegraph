using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class InspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<InspectorView, UxmlTraits> { }

        public void Populate(DialogueNodeView node)
        {
            this.Q<Label>("GuidLabel").text = node.Guid;
        }
    }
}