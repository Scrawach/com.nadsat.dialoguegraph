using Editor.Factories;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Editor.Manipulators
{
    public class EdgeDoubleClickManipulator : Manipulator
    {
        private readonly RedirectNodeFactory _redirectFactory;

        public EdgeDoubleClickManipulator(RedirectNodeFactory redirectFactory) =>
            _redirectFactory = redirectFactory;

        protected override void RegisterCallbacksOnTarget() =>
            target.RegisterCallback<MouseDownEvent>(OnMouseDown, TrickleDown.TrickleDown);

        protected override void UnregisterCallbacksFromTarget() =>
            target.UnregisterCallback<MouseDownEvent>(OnMouseDown, TrickleDown.TrickleDown);

        private void OnMouseDown(MouseDownEvent evt)
        {
            if (evt.clickCount >= 2 && evt is {target: Edge edge})
                _redirectFactory.CreateRedirect(edge, evt.mousePosition, OnMouseDown);
        }
    }
}