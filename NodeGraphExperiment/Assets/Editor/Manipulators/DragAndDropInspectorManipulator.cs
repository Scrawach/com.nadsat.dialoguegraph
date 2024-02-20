using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Manipulators
{
    public class DragAndDropInspectorManipulator : PointerManipulator
    {
        private Vector2 _targetStartPosition;
        private Vector3 _pointerStartPosition;
        private bool _enabled;
        private VisualElement _root;
        
        protected override void RegisterCallbacksOnTarget()
        {
            _root = target.parent;
            target.RegisterCallback<PointerDownEvent>(OnPointerDown);
            target.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            target.RegisterCallback<PointerUpEvent>(OnPointerUp);
            target.RegisterCallback<PointerCaptureOutEvent>(OnPointerCaptureOut);
        }
        
        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<PointerDownEvent>(OnPointerDown);
            target.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
            target.UnregisterCallback<PointerUpEvent>(OnPointerUp);
            target.UnregisterCallback<PointerCaptureOutEvent>(OnPointerCaptureOut);
        }
        
        private void OnPointerDown(PointerDownEvent evt)
        {
            _targetStartPosition = target.transform.position;
            _pointerStartPosition = evt.position;
            target.CapturePointer(evt.pointerId);
            _enabled = true;
        }
        
        private void OnPointerMove(PointerMoveEvent evt)
        {
            if (IsAnotherPointer(evt)) 
                return;
            
            var delta = evt.position - _pointerStartPosition;
            target.transform.position = new Vector2(_targetStartPosition.x + delta.x, _targetStartPosition.y + delta.y);
        }
        
        private void OnPointerUp(PointerUpEvent evt)
        {
            if (IsAnotherPointer(evt)) 
                return;
            
            target.ReleasePointer(evt.pointerId);
        }
        
        private void OnPointerCaptureOut(PointerCaptureOutEvent evt)
        {
            Debug.Log($"On pointer capture out");
        }
        
        private bool IsAnotherPointer(IPointerEvent evt) => 
            !_enabled || !target.HasPointerCapture(evt.pointerId);
        
    }
}