using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Drawing
{
    public class DialogueGraphView : GraphView
    {
        private const string GraphSnapping = "GraphSnapping";
        private const string StyleSheetPath = "Styles/DialogueGraph";

        public DialogueGraphView()
        {
            EditorPrefs.SetBool(GraphSnapping, false);
            Insert(0, new GridBackground());

            this.AddManipulator(new ContentZoomer
                {maxScale = 2f, minScale = 0.1f});
            this.AddManipulator(ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var stylesheet = Resources.Load<StyleSheet>(StyleSheetPath);
            styleSheets.Add(stylesheet);
        }

        private static Manipulator ContentDragger()
        {
            var dragger = new ContentDragger();
            dragger.activators.Add(new ManipulatorActivationFilter
            {
                button = MouseButton.RightMouse
            });
            return dragger;
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter) =>
            ports.ToList();

        public void Find(Node view)
        {
            var viewPosition = view.GetPosition();
            MoveTo(viewPosition);

            ClearSelection();
            AddToSelection(view);
        }

        public void MoveTo(Rect target)
        {
            CalculateFrameTransform(target, layout, 0, out var translation, out var scaling);
            UpdateViewTransform(translation, scaling);
        }

        public new class UxmlFactory : UxmlFactory<DialogueGraphView, UxmlTraits> { }
    }
}