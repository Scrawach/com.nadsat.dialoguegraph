using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Drawing
{
    public class DialogueGraphView : GraphView
    {
        private const string GraphSnapping = "GraphSnapping";
        private const string StyleSheetPath = "Styles/DialogueGraph";

        private readonly MiniMap _miniMap;

        public DialogueGraphView()
        {
            EditorPrefs.SetBool(GraphSnapping, false);
            Insert(0, new GridBackground());

            this.AddManipulator(new ContentZoomer {maxScale = 2f, minScale = 0.1f});
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            _miniMap = AddMiniMap();

            var stylesheet = Resources.Load<StyleSheet>(StyleSheetPath);
            styleSheets.Add(stylesheet);
        }

        private MiniMap AddMiniMap()
        {
            var miniMap = new MiniMap()
            {
                anchored = true,
            };

            miniMap.SetPosition(new Rect(15, 15, 200, 180));

            Add(miniMap);

            miniMap.visible = true;
            return miniMap;
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