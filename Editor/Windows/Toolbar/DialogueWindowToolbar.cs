using System.IO;
using Nadsat.DialogueGraph.Editor.Data;
using Nadsat.DialogueGraph.Editor.Drawing.Controls;
using Nadsat.DialogueGraph.Editor.Serialization.Exporters;
using Nadsat.DialogueGraph.Editor.Windows.CreateGraph;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Windows.Toolbar
{
    public class DialogueWindowToolbar : BaseControl
    {
        private const string Uxml = "UXML/Toolbars/DialogueWindowToolbar";

        private readonly ToolbarMenu _toolbar;
        private CreateGraphWindow _createGraphWindow;
        private DialoguesProvider _dialogues;

        private DialogueGraphRoot _graphRoot;
        private PngExporter _pngExporter;

        public DialogueWindowToolbar() : base(Uxml)
        {
            _toolbar = this.Q<ToolbarMenu>("assets-menu");
            _toolbar.RegisterCallback<PointerEnterEvent>(OnPointerEnter);
        }

        public void Initialize(DialogueGraphRoot root, CreateGraphWindow createGraphWindow, DialoguesProvider dialogues, PngExporter pngExporter)
        {
            _graphRoot = root;
            _createGraphWindow = createGraphWindow;
            _dialogues = dialogues;
            _pngExporter = pngExporter;
        }

        private void OnPointerEnter(PointerEnterEvent evt)
        {
            _toolbar.menu.ClearItems();
            AppendMenuOptions(_toolbar);
        }

        private void AppendMenuOptions(IToolbarMenuElement toolbar)
        {
            toolbar.menu.AppendAction("Create New...", a => { _createGraphWindow.Open(graph => _graphRoot.Load(graph)); });
            toolbar.menu.AppendAction("Open/Open...", a => OpenAsset());
            toolbar.menu.AppendAction("Save", a => _graphRoot.Save());
            AppendExistingDialogueGraphs(toolbar);

            toolbar.menu.AppendSeparator();
            toolbar.menu.AppendAction("Export/To Png", a => _pngExporter.Export());
        }

        private void AppendExistingDialogueGraphs(IToolbarMenuElement toolbar)
        {
            toolbar.menu.AppendSeparator("Open/");
            foreach (var graph in _dialogues.LoadAll())
                toolbar.menu.AppendAction($"Open/{graph.Graph.Name}", a => { _graphRoot.Load(graph); });
        }

        private void OpenAsset()
        {
            var filePath = EditorUtility.OpenFilePanel("Dialogue Graph", _dialogues.GetRootPath(), "asset");
            if (string.IsNullOrWhiteSpace(filePath))
                return;

            var dialogueName = Path.GetFileNameWithoutExtension(filePath);
            var asset = _dialogues.Load(dialogueName);

            if (asset != null)
                _graphRoot.Load(asset);
            else
                OpenInvalidAssetWarningWindow();
        }

        private bool OpenInvalidAssetWarningWindow() =>
            EditorUtility.DisplayDialog("Error", "You trying open invalid asset! You can load only DialogueGraph asset!", "OK");

        public new class UxmlFactory : UxmlFactory<DialogueWindowToolbar, UxmlTraits> { }
    }
}