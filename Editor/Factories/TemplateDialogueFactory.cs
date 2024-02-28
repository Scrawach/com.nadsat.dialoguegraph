using System;
using Nadsat.DialogueGraph.Editor.AssetManagement;
using Nadsat.DialogueGraph.Editor.Drawing.Nodes;
using Nadsat.DialogueGraph.Runtime.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Factories
{
    public class TemplateDialogueFactory
    {
        private readonly GraphView _canvas;
        private readonly DialogueDatabase _database;
        private readonly INodeViewFactory _factory;

        public TemplateDialogueFactory(DialogueDatabase database, INodeViewFactory factory, GraphView canvas)
        {
            _database = database;
            _factory = factory;
            _canvas = canvas;
        }

        public string[] AvailableTemplates() =>
            _database.All();

        public DialogueNodeView Create(string personId) =>
            Create(personId, Vector2.zero);

        public DialogueNodeView Create(string personId, Vector2 worldPosition)
        {
            var localPosition = _canvas.contentViewContainer.WorldToLocal(worldPosition);
            return _factory.CreateDialogue(DialogueForPerson(personId, localPosition));
        }

        private DialogueNode DialogueForPerson(string personId, Vector2 position)
        {
            var model = NewModel<DialogueNode>(position);
            model.PersonId = personId;
            return model;
        }

        private TModel NewModel<TModel>(Vector2 position) where TModel : BaseDialogueNode, new()
        {
            var model = new TModel
            {
                Guid = Guid.NewGuid().ToString(),
                Position = new Rect(position, Vector2.zero)
            };
            return model;
        }
    }
}