using Editor.AssetManagement;
using Editor.Audios;
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

        private readonly DialogueGraphToolbar _dialogueGraphToolbar;
        private readonly INodeViewFactory _factory;
        private readonly LanguageProvider _languageProvider;

        private readonly MultiTable _multiTable;
        private readonly VariablesProvider _variablesProvider;

        private DialogueGraphContainer _container;

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
            var audioEventsProvider = new AudioEventsProvider();
            var searchWindow = new SearchWindowProvider(root, DialogueGraphView, phraseRepository, choicesRepository, audioEventsProvider);

            var audioService = new AudioEditorService();

            var inspectorFactory = new InspectorViewFactory(dialogueDatabase, searchWindow, phraseRepository, choicesRepository, audioService);
            var nodesProvider = new NodesProvider(DialogueGraphView);
            NodesProvider = nodesProvider;

            var variables = new VariablesProvider();
            _variablesProvider = variables;
            var variablesBlackboard = new VariablesBlackboard(variables, DialogueGraphView);

            var genericFactory = new GenericNodeViewFactory(DialogueGraphView, inspectorView, inspectorFactory);
            var dialogueFactory = new DialogueNodeViewFactory(genericFactory, dialogueDatabase, phraseRepository, inspectorFactory);
            var nodeFactory = new NodeViewFactory(genericFactory, dialogueFactory, DialogueGraphView, choicesRepository, variables);
            _factory = nodeFactory;
            var templateFactory = new TemplateDialogueFactory(dialogueDatabase, nodeFactory, DialogueGraphView);
            var undoNodeFactory = new UndoNodeViewFactory(nodeFactory, undoHistory, DialogueGraphView);
            var redirectNodeFactory = new RedirectNodeFactory(DialogueGraphView, nodeFactory);
            var nodesCreationMenuBuilder = new NodesCreationMenuBuilder(DialogueGraphView, undoNodeFactory, templateFactory);

            var pngExporter = new PngExporter(Root, DialogueGraphView);
            var shortcuts = CreateShortcuts(searchWindow, undoHistory, templateFactory);

            audioService.Initialize();
            dialogueDatabase.Initialize();
            _dialogueGraphToolbar.Initialize(variablesBlackboard, _languageProvider);
            _dialogueGraphToolbar.Display(false);
            dialogueWindowToolbar.Initialize(this, CreateWindow, new DialoguesProvider(), pngExporter);
            variablesBlackboard.Initialize();
            _languageProvider.AddLanguage("Russian");

            CreateWindow.Display(false);

            DialogueGraphView.focusable = true;
            DialogueGraphView.AddManipulator(new CustomShortcutsManipulator(shortcuts));
            DialogueGraphView.AddManipulator(new DragAndDropManipulator(undoNodeFactory));
            DialogueGraphView.AddManipulator(new DialogueContextualMenu(nodesProvider, nodesCreationMenuBuilder,
                new ElementsFactory(DialogueGraphView)));
            DialogueGraphView.AddManipulator(new CopyPasteManipulator(new CopyPaste(), new CopyPasteFactory(DialogueGraphView, _factory)));
            DialogueGraphView.AddManipulator(new EdgeDoubleClickManipulator(redirectNodeFactory));
            DialogueGraphView.AddManipulator(new GraphViewUndoManipulator(undoHistory));
            DialogueGraphView.Display(false);

            _languageProvider.LanguageChanged += language => nodesProvider.UpdateLanguage();
        }

        public DialogueGraphView DialogueGraphView { get; }
        public DialogueGraphWindow Root { get; }
        public CreateGraphWindow CreateWindow { get; }

        public NodesProvider NodesProvider { get; }

        private ShortcutsProfile CreateShortcuts(SearchWindowProvider searchWindow, IUndoHistory undoHistory, TemplateDialogueFactory templateFactory)
        {
            var saveShortcut = new SaveShortcut(this);
            var findShortcut = new FindShortcut(searchWindow, DialogueGraphView);
            var undoShortcut = new UndoShortcut(undoHistory);
            var redoShortcut = new RedoShortcut(undoHistory);
            var templateHotkeys = new TemplateHotkeyShortcuts(templateFactory);
            var shortcuts = new ShortcutsProfile(saveShortcut, findShortcut, undoShortcut, redoShortcut, templateHotkeys);
            return shortcuts;
        }

        public void Load(DialogueGraphContainer container)
        {
            container = Object.Instantiate(container);
            _container = container;

            _multiTable.Clear();
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