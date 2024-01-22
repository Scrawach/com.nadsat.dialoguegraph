using Editor.AssetManagement;
using Editor.Data;
using Editor.Drawing.Controls;
using Editor.Drawing.Inspector;
using Editor.Drawing.Nodes;
using Editor.Exporters;
using Editor.Factories;
using Editor.Factories.NodeListeners;
using Editor.Shortcuts;
using Editor.Shortcuts.Concrete;
using Editor.Undo;
using Editor.Windows.Search;
using Editor.Windows.Toolbar;
using Editor.Windows.Variables;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Editor.Windows
{
    public class DialogueGraphRoot : BaseControl
    {
        private const string Uxml = "UXML/DialogueGraphWindow";

        public DialogueGraphView DialogueGraphView { get; }
        public EditorWindow Root { get; }

        public DialogueGraphRoot(EditorWindow root) : base(Uxml)
        {
            Root = root;
            DialogueGraphView = this.Q<DialogueGraphView>();
            var inspectorView = this.Q<InspectorView>();
            var dialogueGraphToolbar = this.Q<DialogueGraphToolbar>();

            var phraseRepository = new PhraseRepository();
            var personRepository = new PersonRepository();
            var undoHistory = new UndoHistory();
            var searchWindow = new SearchWindowProvider(root, DialogueGraphView, phraseRepository);

            var saveShortcut = new SaveShortcut();
            var findShortcut = new FindShortcut(searchWindow, DialogueGraphView);
            var undoShortcut = new UndoShortcut(undoHistory);
            var redoShortcut = new RedoShortcut(undoHistory);
            var shortcuts = new ShortcutsProfile(saveShortcut, findShortcut, undoShortcut, redoShortcut);

            var inspectorFactory = new InspectorViewFactory(personRepository, searchWindow, phraseRepository);
            var nodeViewListener = new NodeViewListener();
            var nodesProvider = new NodesProvider();
            var nodeListeners = new DialogueNodeListeners(nodeViewListener, nodesProvider);
            var nodeFactory = new DialogueNodeFactory(personRepository, phraseRepository, nodeListeners, DialogueGraphView, undoHistory);
            var contextualMenu = new ContextualMenuBuilder(personRepository, nodesProvider, nodeFactory);

            var variables = new VariablesProvider();
            var variablesBlackboard = new VariablesBlackboard(variables, DialogueGraphView);
            var variableNodeFactory = new VariableNodeFactory(DialogueGraphView, variables);

            phraseRepository.Initialize();
            personRepository.Initialize();
            dialogueGraphToolbar.Initialize(variablesBlackboard, phraseRepository);
            DialogueGraphView.Initialize(nodeFactory, variableNodeFactory, contextualMenu, undoHistory);
            variablesBlackboard.Initialize();

            DialogueGraphView.focusable = true;
            DialogueGraphView.RegisterCallback<KeyDownEvent>(shortcuts.Handle);
            DialogueGraphView.RegisterCallback<ContextualMenuPopulateEvent>(OnBuildMenu);
            
            nodeViewListener.Selected += (node) => inspectorView.Populate(inspectorFactory.Build(node));
            nodeViewListener.Unselected += (node) => inspectorView.Cleanup();
            phraseRepository.LanguageChanged += (language) => nodesProvider.UpdateLanguage();
        }

        private void OnBuildMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Export to png", action =>
            {
                var exporter = new PngExporter(Root, DialogueGraphView);
                exporter.Export();
            });
        }
    }
}