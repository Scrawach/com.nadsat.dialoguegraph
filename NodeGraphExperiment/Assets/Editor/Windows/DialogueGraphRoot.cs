using Editor.AssetManagement;
using Editor.ContextualMenu;
using Editor.Drawing;
using Editor.Drawing.Controls;
using Editor.Drawing.Inspector;
using Editor.Exporters;
using Editor.Extensions;
using Editor.Factories;
using Editor.Factories.NodeListeners;
using Editor.Importers;
using Editor.Localization;
using Editor.Manipulators;
using Editor.Serialization;
using Editor.Shortcuts;
using Editor.Shortcuts.Concrete;
using Editor.Undo;
using Editor.Windows.CreateGraph;
using Editor.Windows.Search;
using Editor.Windows.Toolbar;
using Editor.Windows.Variables;
using Runtime;
using UnityEditor;
using UnityEngine.UIElements;

namespace Editor.Windows
{
    public class DialogueGraphRoot : BaseControl
    {
        private const string Uxml = "UXML/DialogueGraphWindow";
        
        private readonly MultiTable _multiTable;
        private readonly LanguageProvider _languageProvider;

        public DialogueGraphView DialogueGraphView { get; }
        public EditorWindow Root { get; }
        public CreateGraphWindow CreateWindow { get; }
        private DialogueGraphContainer _container;

        public DialogueGraphRoot(EditorWindow root) : base(Uxml)
        {
            Root = root;
            DialogueGraphView = this.Q<DialogueGraphView>();
            var inspectorView = this.Q<InspectorView>();
            var dialogueGraphToolbar = this.Q<DialogueGraphToolbar>();
            CreateWindow = this.Q<CreateGraphWindow>();

            _languageProvider = new LanguageProvider();
            _multiTable = new MultiTable(_languageProvider);

            var phraseRepository = new PhraseRepository(_multiTable);
            var dialogueDatabase = new DialogueDatabase();
            var choicesRepository = new ChoicesRepository(_multiTable);
            var undoHistory = new UndoHistory();
            var searchWindow = new SearchWindowProvider(root, DialogueGraphView, phraseRepository, choicesRepository);

            var saveShortcut = new SaveShortcut(DialogueGraphView);
            var findShortcut = new FindShortcut(searchWindow, DialogueGraphView);
            var undoShortcut = new UndoShortcut(undoHistory);
            var redoShortcut = new RedoShortcut(undoHistory);
            var shortcuts = new ShortcutsProfile(saveShortcut, findShortcut, undoShortcut, redoShortcut);

            var inspectorFactory = new InspectorViewFactory(dialogueDatabase, searchWindow, phraseRepository, choicesRepository);
            var nodeViewListener = new NodeViewListener();
            var nodesProvider = new NodesProvider();
            var nodeListeners = new DialogueNodeListeners(nodeViewListener, nodesProvider);

            var variables = new VariablesProvider();
            var variablesBlackboard = new VariablesBlackboard(variables, DialogueGraphView);
            
            var nodeFactory = new NodeViewFactory(dialogueDatabase, phraseRepository, DialogueGraphView, nodeListeners, choicesRepository, variables);
            var undoNodeFactory = new UndoNodeViewFactory(nodeFactory, undoHistory, DialogueGraphView);
            var redirectNodeFactory = new RedirectNodeFactory(DialogueGraphView, nodeFactory);
            var nodesCreationMenuBuilder = new NodesCreationMenuBuilder(DialogueGraphView, undoNodeFactory, dialogueDatabase);
            
            dialogueDatabase.Initialize();
            dialogueGraphToolbar.Initialize(variablesBlackboard, _languageProvider, Root, DialogueGraphView, CreateWindow, this);
            DialogueGraphView.Initialize(nodesProvider, undoNodeFactory, redirectNodeFactory, undoHistory, variables);
            variablesBlackboard.Initialize();
            _languageProvider.AddLanguage("Russian");

            CreateWindow.Display(false);

            DialogueGraphView.focusable = true;
            DialogueGraphView.AddManipulator(new CustomShortcutsManipulator(shortcuts));
            DialogueGraphView.AddManipulator(new DragAndDropManipulator(undoNodeFactory));
            DialogueGraphView.AddManipulator(new DialogueContextualMenu(nodesProvider, nodesCreationMenuBuilder, new ElementsFactory(DialogueGraphView)));
            DialogueGraphView.Display(false);

            nodeViewListener.Selected += (node) => inspectorView.Populate(inspectorFactory.Build(node));
            nodeViewListener.Unselected += (node) => inspectorView.Cleanup();
            _languageProvider.LanguageChanged += (language) => nodesProvider.UpdateLanguage();

            DialogueGraphView.Saved += () =>
            {
                var exporter = new CsvExporter(_multiTable);
                exporter.Initialize(_container.Graph.Name);
                exporter.Export();
            };
        }

        public void Populate(DialogueGraphContainer container)
        {
            _container = container;
            var csvImporter = new CsvImporter(_languageProvider, _multiTable);
            csvImporter.Initialize(container.Graph.Name);
            csvImporter.Import();
            _multiTable.Initialize(container.Graph.Name);
            DialogueGraphView.Populate(container);
            DialogueGraphView.Display(true);
        }
    }
}