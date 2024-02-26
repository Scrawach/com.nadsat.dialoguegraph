using UnityEngine;
using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Drawing.Controls
{
    public class BaseControl : VisualElement
    {
        public BaseControl() { }

        public BaseControl(string uiFile)
        {
            var uxml = Resources.Load<VisualTreeAsset>(uiFile);
            uxml.CloneTree(this);
        }
    }
}