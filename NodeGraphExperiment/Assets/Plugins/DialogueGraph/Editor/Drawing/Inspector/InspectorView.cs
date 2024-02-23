using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Drawing.Inspector
{
    public class InspectorView : VisualElement
    {
        private VisualElement _selected;

        public void Populate(VisualElement visual)
        {
            Cleanup();
            _selected = visual;
            Add(_selected);
        }

        public void Cleanup()
        {
            if (_selected != null)
                Remove(_selected);
            _selected = null;
        }

        public new class UxmlFactory : UxmlFactory<InspectorView, UxmlTraits> { }
    }
}