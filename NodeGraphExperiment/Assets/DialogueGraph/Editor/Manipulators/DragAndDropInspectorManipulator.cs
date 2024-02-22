using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Manipulators
{
    public class DragAndDropInspectorManipulator : PointerManipulator
    {
        private readonly VisualElement _root;

        private Vector2 _targetStartPosition;
        private Vector3 _pointerStartPosition;
        private bool _enabled;

        public DragAndDropInspectorManipulator(VisualElement container) => 
            _root = container;

        protected override void RegisterCallbacksOnTarget()
        {
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
            
            _root.parent.Add(target);
            _root.Remove(target);
        }
        
        private void OnPointerMove(PointerMoveEvent evt)
        {
            if (IsAnotherPointer(evt)) 
                return;
            
            var delta = evt.position - _pointerStartPosition;
            target.transform.position = new Vector2(_targetStartPosition.x + delta.x, _targetStartPosition.y + delta.y);
            ReconstructParentVisualTree();
        }

        private void ReconstructParentVisualTree()
        {
            var container = _root.Q<VisualElement>("button-container").Children().ToArray();
            for (var i = 0; i < container.Length; i++)
            {
                if (container[i] == target)
                    continue;

                var item = container[i];
                var itemPosition = item.worldTransform.GetPosition();
                Debug.Log($"{target.worldTransform.GetPosition().y}, {itemPosition.y}");
                
            }

            var targetPosition = target.worldTransform.GetPosition();
            var offset = 0;
            for (var i = 0; i < container.Length; i++)
            {
                if (container[i] == target)
                    continue;

                var item = container[i];
                var itemPosition = item.worldTransform.GetPosition();

                if (itemPosition.y > targetPosition.y)
                {
                    
                }
            }
        }

        private void OnPointerUp(PointerUpEvent evt)
        {
            if (IsAnotherPointer(evt)) 
                return;
            
            target.ReleasePointer(evt.pointerId);
            _root.parent.Remove(target);
            _root.Add(target);

        }
        
        private void OnPointerCaptureOut(PointerCaptureOutEvent evt)
        {
            Debug.Log($"On pointer capture out");
        }
        
        private bool IsAnotherPointer(IPointerEvent evt) => 
            !_enabled || !target.HasPointerCapture(evt.pointerId);
        
    }
}