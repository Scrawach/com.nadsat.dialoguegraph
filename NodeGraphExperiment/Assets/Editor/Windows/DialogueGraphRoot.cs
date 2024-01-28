using Editor.AssetManagement;
using Editor.Drawing;
using Editor.Drawing.Controls;
using Editor.Drawing.Inspector;
using Editor.Exporters;
using Editor.Extensions;
using Editor.Factories;
using Editor.Factories.NodeListeners;
using Editor.Serialization;
using Editor.Shortcuts;
using Editor.Shortcuts.Concrete;
using Editor.Undo;
using Editor.Windows.CreateGraph;
using Editor.Windows.Search;
using Editor.Windows.Toolbar;
using Editor.Windows.Variables;
using UnityEditor;
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
            var createWindow = this.Q<CreateGraphWindow>();
            
            var phraseRepository = new PhraseRepository();
            var personRepository = new PersonRepository();
            var undoHistory = new UndoHistory();
            var searchWindow = new SearchWindowProvider(root, DialogueGraphView, phraseRepository);

            var saveShortcut = new SaveShortcut(DialogueGraphView);
            var findShortcut = new FindShortcut(searchWindow, DialogueGraphView);
            var undoShortcut = new UndoShortcut(undoHistory);
            var redoShortcut = new RedoShortcut(undoHistory);
            var shortcuts = new ShortcutsProfile(saveShortcut, findShortcut, undoShortcut, redoShortcut);

            var inspectorFactory = new InspectorViewFactory(personRepository, searchWindow, phraseRepository);
            var nodeViewListener = new NodeViewListener();
            var nodesProvider = new NodesProvider();
            var nodeListeners = new DialogueNodeListeners(nodeViewListener, nodesProvider);

            var nodeFactory = new NodeViewFactory(personRepository, phraseRepository, DialogueGraphView, nodeListeners);
            var undoNodeFactory = new UndoNodeViewFactory(nodeFactory, undoHistory, DialogueGraphView);
            var elementFactory = new ElementsFactory(DialogueGraphView);
            var personTemplateFactory = new PersonTemplateFactory(undoNodeFactory, DialogueGraphView);
            var redirectNodeFactory = new RedirectNodeFactory(DialogueGraphView, nodeFactory);
            var nodesCreationMenuBuilder = new NodesCreationMenuBuilder(DialogueGraphView, undoNodeFactory, personRepository);
            var contextualMenu = new ContextualMenuBuilder(personRepository, nodesProvider, elementFactory, nodesCreationMenuBuilder);

            var variables = new VariablesProvider();
            var variablesBlackboard = new VariablesBlackboard(variables, DialogueGraphView);
            var variableNodeFactory = new VariableNodeFactory(DialogueGraphView, variables);

            var copyPasteNodes = new CopyPasteNodes(nodeFactory, nodesProvider, undoHistory);

            createWindow.Display(false);
            phraseRepository.Initialize();
            personRepository.Initialize();
            dialogueGraphToolbar.Initialize(variablesBlackboard, phraseRepository, Root, DialogueGraphView, createWindow);
            DialogueGraphView.Initialize(nodesProvider, undoNodeFactory, redirectNodeFactory, copyPasteNodes, variableNodeFactory, contextualMenu, undoHistory);
            variablesBlackboard.Initialize();

            DialogueGraphView.focusable = true;
            DialogueGraphView.RegisterCallback<KeyDownEvent>(shortcuts.Handle);
            
            nodeViewListener.Selected += (node) => inspectorView.Populate(inspectorFactory.Build(node));
            nodeViewListener.Unselected += (node) => inspectorView.Cleanup();
            phraseRepository.LanguageChanged += (language) => nodesProvider.UpdateLanguage();
        }
    }
}