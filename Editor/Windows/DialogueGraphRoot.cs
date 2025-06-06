using Nadsat.DialogueGraph.Editor.AssetManagement;
using Nadsat.DialogueGraph.Editor.Audios;
using Nadsat.DialogueGraph.Editor.Audios.Wwise;
using Nadsat.DialogueGraph.Editor.Backup;
using Nadsat.DialogueGraph.Editor.ContextualMenu;
using Nadsat.DialogueGraph.Editor.Data;
using Nadsat.DialogueGraph.Editor.DebugPlay;
using Nadsat.DialogueGraph.Editor.Drawing;
using Nadsat.DialogueGraph.Editor.Drawing.Controls;
using Nadsat.DialogueGraph.Editor.Drawing.Inspector;
using Nadsat.DialogueGraph.Editor.Extensions;
using Nadsat.DialogueGraph.Editor.Factories;
using Nadsat.DialogueGraph.Editor.Factories.NodeListeners;
using Nadsat.DialogueGraph.Editor.Localization;
using Nadsat.DialogueGraph.Editor.Manipulators;
using Nadsat.DialogueGraph.Editor.Manipulators.GraphViewManipulators;
using Nadsat.DialogueGraph.Editor.Serialization;
using Nadsat.DialogueGraph.Editor.Serialization.Exporters;
using Nadsat.DialogueGraph.Editor.Serialization.Importers;
using Nadsat.DialogueGraph.Editor.Shortcuts;
using Nadsat.DialogueGraph.Editor.Shortcuts.Concrete;
using Nadsat.DialogueGraph.Editor.Undo;
using Nadsat.DialogueGraph.Editor.Windows.CreateGraph;
using Nadsat.DialogueGraph.Editor.Windows.Search;
using Nadsat.DialogueGraph.Editor.Windows.Toolbar;
using Nadsat.DialogueGraph.Editor.Windows.Variables;
using Nadsat.DialogueGraph.Runtime;
using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Windows
{
    public class DialogueGraphRoot : BaseControl
    {
        private const string Uxml = "UXML/DialogueGraphWindow";
        
        private readonly DialogueGraphWindow _root;
        private readonly DialogueGraphToolbar _dialogueGraphToolbar;
        
        private readonly DialogueGraphExporter _graphExporter;
        private readonly DialogueGraphImporter _graphImporter;
        private readonly BackupService _backupService;
        private readonly IAudioEditorService _audioService;

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
            _audioService = audioService;

            var phraseRepository = new PhraseRepository(multiTable);
            var dialogueDatabase = new DialogueDatabase();
            var choicesRepository = new ChoicesRepository(multiTable);
            var undoHistory = new UndoHistory();
            var variables = new VariablesProvider();
            
            var searchWindow = new SearchWindowProvider(root, DialogueGraphView, phraseRepository, choicesRepository, audioEvents, variables);

            var variablesBlackboard = new VariablesBlackboard(variables, DialogueGraphView);

            var expressionVerifier = new ExpressionVerifier(choicesRepository, variables);
            var inspectorFactory = new InspectorViewFactory(dialogueDatabase, searchWindow, phraseRepository, choicesRepository, expressionVerifier, audioService);
            var nodesProvider = new NodesProvider(DialogueGraphView);
            
            var genericFactory = new GenericNodeViewFactory(_root, DialogueGraphView, inspectorView, inspectorFactory);
            var dialogueFactory = new DialogueNodeViewFactory(genericFactory, dialogueDatabase, phraseRepository, inspectorFactory);
            var nodeFactory = new NodeViewFactory(genericFactory, dialogueFactory, DialogueGraphView, choicesRepository, variables);
            var templateFactory = new TemplateDialogueFactory(dialogueDatabase, nodeFactory, DialogueGraphView);
            //var undoNodeFactory = new UndoNodeViewFactory(nodeFactory, undoHistory, _dialogueGraphView);
            var redirectNodeFactory = new RedirectNodeFactory(DialogueGraphView, nodeFactory);
            var nodesCreationMenuBuilder = new NodesCreationMenuBuilder(DialogueGraphView, nodeFactory, templateFactory);

            var elementsFactory = new ElementsFactory(DialogueGraphView);

            var pngExporter = new PngExporter(_root, DialogueGraphView);
            var shortcuts = CreateShortcuts(searchWindow, undoHistory, templateFactory);

            var graphSerializer = new DialogueGraphSerializer(DialogueGraphView, nodesProvider, dialogueGraphProvider);
            var csvExporter = new CsvExporter(multiTable);
            _graphExporter = new DialogueGraphExporter(graphSerializer, csvExporter, dialoguesProvider);
            
            var csvImporter = new CsvImporter(languageProvider, multiTable);
            _graphImporter = new DialogueGraphImporter(DialogueGraphView, nodeFactory, nodesProvider, 
                variables, csvImporter, dialogueGraphProvider, elementsFactory, dialoguesProvider);

            var backupExporter = new BackupGraphExporter(graphSerializer, csvExporter, dialoguesProvider);
            _backupService = new BackupService(backupExporter, 5f);

            var debugLauncher = new DebugLauncher(_graphExporter, graphSerializer, dialoguesProvider);
            
            audioService.Initialize();
            dialogueDatabase.Initialize();
            _dialogueGraphToolbar.Initialize(variablesBlackboard, languageProvider, debugLauncher);
            dialogueWindowToolbar.Initialize(this, createWindow, dialoguesProvider, pngExporter);
            variablesBlackboard.Initialize();
            languageProvider.AddLanguage("Russian");

            _dialogueGraphToolbar.Display(false);
            createWindow.Display(false);

            DialogueGraphView.focusable = true;
            DialogueGraphView.AddManipulator(new CustomShortcutsManipulator(shortcuts));
            //_dialogueGraphView.AddManipulator(new DragAndDropManipulator(undoNodeFactory));
            DialogueGraphView.AddManipulator(new DialogueContextualMenu(nodesProvider, nodesCreationMenuBuilder, elementsFactory));
            DialogueGraphView.AddManipulator(new CopyPasteManipulator(new CopyPaste(), new CopyPasteFactory(DialogueGraphView, nodeFactory)));
            DialogueGraphView.AddManipulator(new EdgeDoubleClickManipulator(redirectNodeFactory));
            DialogueGraphView.AddManipulator(new GraphViewUndoManipulator(undoHistory));
            DialogueGraphView.Display(false);

            // commented because change language update all models, that mark editor window as dirty, because model updated
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
            _backupService.Start();
            _graphImporter.Import(container);
            
            DialogueGraphView.Display(true);
            _dialogueGraphToolbar.Display(true);

            _root.IsDirty = false;
        }

        public void Save()
        { 
            _graphExporter.Export();
            _root.IsDirty = false;
        }

        public void Update()
        {
            _backupService.Update();
            _audioService.Update();
        }
    }
}