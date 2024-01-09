using UnityEngine.UIElements;

namespace Editor
{
    public class InspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<InspectorView, UxmlTraits> { }

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
    }
}