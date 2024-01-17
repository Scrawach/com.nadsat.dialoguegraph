using Editor.AssetManagement;
using Editor.Data;
using Editor.Drawing.Controls;
using Editor.Drawing.Inspector;
using Editor.Drawing.Nodes;
using Editor.Factories;
using Editor.Factories.NodeListeners;
using Editor.Shortcuts;
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


        public DialogueGraphRoot(EditorWindow root) : base(Uxml)
        {
            DialogueGraphView = this.Q<DialogueGraphView>();
            var inspectorView = this.Q<InspectorView>();
            var dialogueGraphToolbar = this.Q<DialogueGraphToolbar>();

            var phraseRepository = new PhraseRepository();
            var personRepository = new PersonRepository();
            var searchWindow = new SearchWindowProvider(root, DialogueGraphView, phraseRepository);
            var shortcuts = new ShortcutsProfile(searchWindow, DialogueGraphView);

            var inspectorFactory = new InspectorViewFactory(personRepository, searchWindow, phraseRepository);
            var nodeViewListener = new NodeViewListener();
            var nodesProvider = new NodesProvider();
            var nodeListeners = new DialogueNodeListeners(nodeViewListener, nodesProvider);
            var nodeFactory = new DialogueNodeFactory(personRepository, phraseRepository, nodeListeners, DialogueGraphView);
            var contextualMenu = new ContextualMenuBuilder(personRepository, nodesProvider, nodeFactory);

            var variables = new VariablesProvider();
            var variablesBlackboard = new VariablesBlackboard(variables, DialogueGraphView);
            var variableNodeFactory = new VariableNodeFactory(DialogueGraphView, variables);

            phraseRepository.Initialize();
            personRepository.Initialize();
            dialogueGraphToolbar.Initialize(variablesBlackboard, phraseRepository);
            DialogueGraphView.Initialize(nodeFactory, variableNodeFactory, contextualMenu);
            variablesBlackboard.Initialize();

            DialogueGraphView.focusable = true;
            DialogueGraphView.RegisterCallback<KeyDownEvent>(shortcuts.Handle);
            
            nodeViewListener.Selected += (node) => inspectorView.Populate(inspectorFactory.Build(node));
            nodeViewListener.Unselected += (node) => inspectorView.Cleanup();
            phraseRepository.LanguageChanged += (language) => nodesProvider.UpdateLanguage();
        }
    }
}