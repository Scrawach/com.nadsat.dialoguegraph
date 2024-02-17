using Editor.Drawing.Controls;
using Runtime.Nodes;
using UnityEngine.UIElements;

namespace Editor.Drawing.Inspector
{
    public class AudioEventInspectorView : BaseControl
    {
        private const string Uxml = "UXML/AudioEventInspectorView";

        private readonly AudioEventNode _node;
        
        private readonly Button _addButton;
        private readonly VisualElement _container;
        
        public AudioEventInspectorView(AudioEventNode node) : base(Uxml)
        {
            _node = node;
            _addButton = this.Q<Button>("add-button");
            _container = this.Q<VisualElement>("container");
            
            _addButton.clicked += OnAddEventButtonClicked;
            _node.Changed += OnModelChanged;
            OnModelChanged();
        }

        private void OnAddEventButtonClicked()
        {
            _node.AddEvent(new AudioEventData());
        }

        private void OnModelChanged()
        {
            _container.Clear();
            foreach (var eventData in _node.Events)
            {
                var audioEventControl = CreateControl(eventData);
                _container.Add(audioEventControl);
            }
        }

        private AudioEventControl CreateControl(AudioEventData data)
        {
            var control = new AudioEventControl(data);

            control.Closed += () =>
            {
                _node.RemoveEvent(data);
            };
            
            return control;
        }
    }
}