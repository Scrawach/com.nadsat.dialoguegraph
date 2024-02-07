using Editor.AssetManagement;
using Editor.ContextualMenu;
using Editor.Data;
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
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Windows
{
    public class DialogueGraphRoot : BaseControl
    {
        private const string Uxml = "UXML/DialogueGraphWindow";
        
        private readonly MultiTable _multiTable;
        private readonly LanguageProvider _languageProvider;

        public DialogueGraphView DialogueGraphView { get; }
        public DialogueGraphWindow Root { get; }
        public CreateGraphWindow CreateWindow { get; }
        
        public NodesProvider NodesProvider { get; }
        
        private DialogueGraphContainer _container;

        private readonly DialogueGraphToolbar _dialogueGraphToolbar;
        private readonly INodeViewFactory _factory;
        private readonly VariablesProvider _variablesProvider;

        public DialogueGraphRoot(DialogueGraphWindow root) : base(Uxml)
        {
            Root = root;
            DialogueGraphView = this.Q<DialogueGraphView>();
            var inspectorView = this.Q<InspectorView>();
            _dialogueGraphToolbar = this.Q<DialogueGraphToolbar>();
            var dialogueWindowToolbar = this.Q<DialogueWindowToolbar>();
            CreateWindow = this.Q<CreateGraphWindow>();

            _languageProvider = new LanguageProvider();
            _multiTable = new MultiTable(_languageProvider);

            var phraseRepository = new PhraseRepository(_multiTable);
            var dialogueDatabase = new DialogueDatabase();
            var choicesRepository = new ChoicesRepository(_multiTable);
            var undoHistory = new UndoHistory();
            var searchWindow = new SearchWindowProvider(root, DialogueGraphView, phraseRepository, choicesRepository);

            var saveShortcut = new SaveShortcut(this);
            var findShortcut = new FindShortcut(searchWindow, DialogueGraphView);
            var undoShortcut = new UndoShortcut(undoHistory);
            var redoShortcut = new RedoShortcut(undoHistory);
            var shortcuts = new ShortcutsProfile(saveShortcut, findShortcut, undoShortcut, redoShortcut);

            var inspectorFactory = new InspectorViewFactory(dialogueDatabase, searchWindow, phraseRepository, choicesRepository);
            var nodeViewListener = new NodeViewListener();
            var nodesProvider = new NodesProvider();
            NodesProvider = nodesProvider;
            var nodeListeners = new DialogueNodeListeners(nodeViewListener, nodesProvider);

            var variables = new VariablesProvider();
            _variablesProvider = variables;
            var variablesBlackboard = new VariablesBlackboard(variables, DialogueGraphView);
            
            var nodeFactory = new NodeViewFactory(dialogueDatabase, phraseRepository, DialogueGraphView, nodeListeners, choicesRepository, variables);
            _factory = nodeFactory;
            var undoNodeFactory = new UndoNodeViewFactory(nodeFactory, undoHistory, DialogueGraphView);
            var redirectNodeFactory = new RedirectNodeFactory(DialogueGraphView, nodeFactory);
            var nodesCreationMenuBuilder = new NodesCreationMenuBuilder(DialogueGraphView, undoNodeFactory, dialogueDatabase);

            var pngExporter = new PngExporter(Root, DialogueGraphView);
            
            dialogueDatabase.Initialize();
            _dialogueGraphToolbar.Initialize(variablesBlackboard, _languageProvider);
            _dialogueGraphToolbar.Display(false);
            dialogueWindowToolbar.Initialize(this, CreateWindow, new DialoguesProvider(), pngExporter);
            DialogueGraphView.Initialize(redirectNodeFactory, undoHistory);
            variablesBlackboard.Initialize();
            _languageProvider.AddLanguage("Russian");

            CreateWindow.Display(false);

            DialogueGraphView.focusable = true;
            DialogueGraphView.AddManipulator(new CustomShortcutsManipulator(shortcuts));
            DialogueGraphView.AddManipulator(new DragAndDropManipulator(undoNodeFactory));
            DialogueGraphView.AddManipulator(new DialogueContextualMenu(nodesProvider, nodesCreationMenuBuilder, new ElementsFactory(DialogueGraphView)));
            DialogueGraphView.AddManipulator(new CopyPasteManipulator(new CopyPaste(), new CopyPasteFactory(DialogueGraphView, _factory)));
            DialogueGraphView.Display(false);

            nodeViewListener.Selected += (node) => inspectorView.Populate(inspectorFactory.Build(node));
            nodeViewListener.Unselected += (node) => inspectorView.Cleanup();
            _languageProvider.LanguageChanged += (language) => nodesProvider.UpdateLanguage();
        }
        
        public void Load(DialogueGraphContainer container)
        {
            container = Object.Instantiate(container);
            _container = container;
            
            var csvImporter = new CsvImporter(_languageProvider, _multiTable);
            csvImporter.Import(container.Graph.Name);
            _multiTable.Initialize(container.Graph.Name);

            var graphImporter = new DialogueGraphImporter(DialogueGraphView, _factory, NodesProvider, _variablesProvider);
            graphImporter.Import(container.Graph);
            
            DialogueGraphView.Display(true);
            _dialogueGraphToolbar.Display(true);
        }
        
        public void Save()
        {
            var graphExporter = new DialogueGraphExporter(DialogueGraphView, NodesProvider, _container);
            graphExporter.Export();
            
            var exporter = new CsvExporter(_multiTable);
            exporter.Export(_container.Graph.Name);

            Root.IsDirty = false;
        }
    }
}