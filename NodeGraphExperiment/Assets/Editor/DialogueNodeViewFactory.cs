using Runtime;
using Runtime.Nodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class DialogueNodeViewFactory
    {
        private readonly DialogueGraphView _canvas;
        private readonly DialoguePersonDatabase _personDatabase;
        private readonly PhraseRepository _phraseRepository;

        public DialogueNodeViewFactory(DialogueGraphView canvas, DialoguePersonDatabase personDatabase, PhraseRepository phraseRepository)
        {
            _canvas = canvas;
            _personDatabase = personDatabase;
            _phraseRepository = phraseRepository;
        }

        public RedirectNode CreateRedirectNode(Vector2 position, Edge target, EventCallback<MouseDownEvent> onMouseDown = null)
        {
            target.input.Disconnect(target);
            target.output.Disconnect(target);
            target.Clear();
            _canvas.RemoveElement(target);

            var redirectNode = new RedirectNode
            {
                title = ""
            };
            
            redirectNode.styleSheets.Add(Resources.Load<StyleSheet>("Styles/RedirectNode"));

            var (input, output) = CreatePortsFor(redirectNode);
            redirectNode.SetPosition(new Rect(position, Vector2.zero));
            
            var leftEdge = CreateEdge(target.output, input, onMouseDown);
            var rightEdge = CreateEdge(target.input, output, onMouseDown);
                        
            _canvas.AddElement(redirectNode);
            _canvas.AddElement(leftEdge);
            _canvas.AddElement(rightEdge);
            return redirectNode;
        }

        public DialogueNodeView From(DialogueNodeViewData data)
        {
            var personNode = new PersonDialogueNode() {PersonId = data.PersonName};
            var phraseNode = new PhraseDialogueNode() {PhraseId = data.Title};
            var compositeNode = new CompositeDialogueNode(personNode, phraseNode);
            var dialogueTree = Resources.Load<Runtime.DialogueGraph>("Dialogue Graph");
            dialogueTree.Add(compositeNode);
            var node = new DialogueNodeView(compositeNode);
            
            compositeNode.Guid = node.viewDataKey;
            node.Update(data);

            compositeNode.Updated += () =>
            {
                var personId = compositeNode.FindOrDefault<PersonDialogueNode>().PersonId;
                var person = _personDatabase.FindByName(personId);
                node.ChangePerson(person);

                var phrase = compositeNode.FindOrDefault<PhraseDialogueNode>();

                if (phrase != null)
                {
                    node.SetTitle(phrase.PhraseId);
                    node.SetDescription(_phraseRepository.Find(phrase.PhraseId));
                }

                var image = compositeNode.FindOrDefault<ImageDialogueNode>();
                if (image != null) 
                    node.AddImage(AssetDatabase.LoadAssetAtPath<Sprite>(image.PathToImage));
                else
                    node.RemoveImage();
            };

            _phraseRepository.LanguageChanged += (language) =>
            {
                compositeNode.InvokeUpdate();
            };

            CreatePortsFor(node);
            return node;
        }
        
        private static Edge CreateEdge(Port a, Port b, EventCallback<MouseDownEvent> onMouseDown = null)
        {
            var edge = a.ConnectTo(b);
            
            if (onMouseDown != null)
                edge.RegisterCallback(onMouseDown);
            
            return edge;
        }

        private static (Port input, Port output) CreatePortsFor(Node node)
        {
            var input = node.InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
            input.portName = "";
            node.inputContainer.Add(input);
            var output = node.InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
            node.outputContainer.Add(output);
            output.portName = "";
            
            node.RefreshPorts();
            node.RefreshExpandedState();
            return (input, output);
        }

        public DialogueNodeView CreatePersonNode(DialoguePersonData data, Vector2 position)
        {
            var viewData = new DialogueNodeViewData()
            {
                PersonName = data.Name,
                HeaderColor = data.Color,
                Title = "none",
                Description = "none",
                Icon = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GetAssetPath(data.Icon))
            };

            var dialogueNode = From(viewData);
            dialogueNode.SetPosition(new Rect(_canvas.contentViewContainer.WorldToLocal(position), Vector2.zero));
            _canvas.AddNode(dialogueNode);
            return dialogueNode;
        }

        public DialogueNodeView Copy(DialogueNodeView dialogueNodeView)
        {
            return null;
           // var personSettings = _personDatabase.FindByName(dialogueNodeView.DialogueNode.PersonName.Value);
            //var node = CreatePersonNode(personSettings, dialogueNodeView.worldTransform.GetPosition());
            //node.DialogueNode.Title.Value = dialogueNodeView.DialogueNode.Title.Value; 
            //node.DialogueNode.Description.Value = dialogueNodeView.DialogueNode.Description.Value; 
           // return node;
        }

        public void CreateGroup(Vector2 at)
        {
            var group = new Group();
            var worldPosition = _canvas.contentViewContainer.WorldToLocal(at);
            group.SetPosition(new Rect(worldPosition, Vector2.zero));
            _canvas.AddElement(group);
        }
    }
}