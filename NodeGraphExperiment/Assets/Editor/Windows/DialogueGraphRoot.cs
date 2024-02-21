using Editor.AssetManagement;
using Editor.Audios;
using Editor.Audios.Wwise;
using Editor.Backup;
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
using Editor.Manipulators.GraphViewManipulators;
using Editor.Serialization;
using Editor.Shortcuts;
using Editor.Shortcuts.Concrete;
using Editor.Undo;
using Editor.Windows.CreateGraph;
using Editor.Windows.Search;
using Editor.Windows.Toolbar;
using Editor.Windows.Variables;
using Runtime;
using UnityEngine.UIElements;

namespace Editor.Windows
{
    public class DialogueGraphRoot : BaseControl
    {
        private const string Uxml = "UXML/DialogueGraphWindow";
        
        private readonly DialogueGraphWindow _root;
        private readonly DialogueGraphToolbar _dialogueGraphToolbar;
        
        private readonly DialogueGraphExporter _graphExporter;
        private readonly DialogueGraphImporter _graphImporter;

        public DialogueGraphView DialogueGraphView { get; }

        public DialogueGraphRoot(DialogueGraphWindow root) : base(Uxml)
        {
            _root = root;
            DialogueGraphView = this.Q<DialogueGraphView>();
            var inspectorView = this.Q<InspectorView>();
            _dialogueGraphToolbar = this.Q<DialogueGraphToolbar>();
            var dialogueWindowToolbar = this.Q<DialogueWindowToolbar>();
            var createWindow = this.Q<CreateGraphWindow>();

            var dialoguesProvider = new DialoguesProvider();
            var dialogueGraphProvider = new DialogueGraphProvider();
            var languageProvider = new LanguageProvider();
            var multiTable = new MultiTable(languageProvider);

            var (audioEvents, audioService) = CreateAudio();

            var phraseRepository = new PhraseRepository(multiTable);
            var dialogueDatabase = new DialogueDatabase();
            var choicesRepository = new ChoicesRepository(multiTable);
            var undoHistory = new UndoHistory();
            var searchWindow = new SearchWindowProvider(root, DialogueGraphView, phraseRepository, choicesRepository, audioEvents);


            var inspectorFactory = new InspectorViewFactory(dialogueDatabase, searchWindow, phraseRepository, choicesRepository, audioService);
            var nodesProvider = new NodesProvider(DialogueGraphView);

            var variables = new VariablesProvider();
            var variablesBlackboard = new VariablesBlackboard(variables, DialogueGraphView);

            var genericFactory = new GenericNodeViewFactory(_root, DialogueGraphView, inspectorView, inspectorFactory);
            var dialogueFactory = new DialogueNodeViewFactory(genericFactory, dialogueDatabase, phraseRepository, inspectorFactory);
            var nodeFactory = new NodeViewFactory(genericFactory, dialogueFactory, DialogueGraphView, choicesRepository, variables);
            var templateFactory = new TemplateDialogueFactory(dialogueDatabase, nodeFactory, DialogueGraphView);
            //var undoNodeFactory = new UndoNodeViewFactory(nodeFactory, undoHistory, _dialogueGraphView);
            var redirectNodeFactory = new RedirectNodeFactory(DialogueGraphView, nodeFactory);
            var nodesCreationMenuBuilder = new NodesCreationMenuBuilder(DialogueGraphView, nodeFactory, templateFactory);

            var pngExporter = new PngExporter(_root, DialogueGraphView);
            var shortcuts = CreateShortcuts(searchWindow, undoHistory, templateFactory);

            var graphSerializer = new DialogueGraphSerializer(DialogueGraphView, nodesProvider, dialogueGraphProvider);
            var csvExporter = new CsvExporter(multiTable);
            _graphExporter = new DialogueGraphExporter(graphSerializer, csvExporter, dialoguesProvider);
            
            var csvImporter = new CsvImporter(languageProvider, multiTable);
            _graphImporter = new DialogueGraphImporter(DialogueGraphView, nodeFactory, nodesProvider, variables, csvImporter, dialogueGraphProvider);
            
            dialogueDatabase.Initialize();
            _dialogueGraphToolbar.Initialize(variablesBlackboard, languageProvider);
            _dialogueGraphToolbar.Display(false);
            dialogueWindowToolbar.Initialize(this, createWindow, dialoguesProvider, pngExporter);
            variablesBlackboard.Initialize();
            languageProvider.AddLanguage("Russian");

            createWindow.Display(false);

            DialogueGraphView.focusable = true;
            DialogueGraphView.AddManipulator(new CustomShortcutsManipulator(shortcuts));
            //_dialogueGraphView.AddManipulator(new DragAndDropManipulator(undoNodeFactory));
            DialogueGraphView.AddManipulator(new DialogueContextualMenu(nodesProvider, nodesCreationMenuBuilder,
                new ElementsFactory(DialogueGraphView)));
            DialogueGraphView.AddManipulator(new CopyPasteManipulator(new CopyPaste(), new CopyPasteFactory(DialogueGraphView, nodeFactory)));
            DialogueGraphView.AddManipulator(new EdgeDoubleClickManipulator(redirectNodeFactory));
            DialogueGraphView.AddManipulator(new GraphViewUndoManipulator(undoHistory));
            DialogueGraphView.Display(false);

            languageProvider.LanguageChanged += language => nodesProvider.UpdateLanguage();
        }
        
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

        private (IAudioEventsProvider eventsProvider, IAudioEditorService audioService) CreateAudio()
        {
#if HAS_WWISE
            var wwiseEvents = new WwiseAudioEventsProvider();
            return (wwiseEvents, new WwiseAudioEditorService(wwiseEvents));
#endif
            return (new DebugEventsProvider(), new DebugAudioEditorService());
        }

        public void Load(DialogueGraphContainer container)
        {
            _graphImporter.Import(container);
            
            DialogueGraphView.Display(true);
            _dialogueGraphToolbar.Display(true);
        }

        public void Save()
        { 
            _graphExporter.Export();
            _root.IsDirty = false;
        }
    }
}