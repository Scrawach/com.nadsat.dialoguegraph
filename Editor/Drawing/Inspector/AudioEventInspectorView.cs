using Nadsat.DialogueGraph.Editor.Audios;
using Nadsat.DialogueGraph.Editor.Drawing.Controls;
using Nadsat.DialogueGraph.Editor.Windows.Search;
using Nadsat.DialogueGraph.Runtime.Nodes;
using UnityEngine.UIElements;

namespace Nadsat.DialogueGraph.Editor.Drawing.Inspector
{
    public class AudioEventInspectorView : BaseControl
    {
        private const string Uxml = "UXML/AudioEventInspectorView";

        private readonly AudioEventNode _node;
        private readonly IAudioEditorService _audioService;
        private readonly SearchWindowProvider _searchWindow;

        private readonly Button _addButton;
        private readonly VisualElement _container;

        private AudioEventControl _activeAudioControl;
        
        public AudioEventInspectorView(AudioEventNode node, IAudioEditorService audioService, SearchWindowProvider searchWindow) : base(Uxml)
        {
            _node = node;
            _audioService = audioService;
            _searchWindow = searchWindow;
            _addButton = this.Q<Button>("add-button");
            _container = this.Q<VisualElement>("container");
            
            _addButton.clicked += OnAddEventButtonClicked;
            _node.Changed += OnModelChanged;
            _audioService.PlayingProgressChanged += OnPlayingProgressChanged;
            OnModelChanged();
        }

        private void OnPlayingProgressChanged(float ratio) => 
            _activeAudioControl.SetProgressWithoutNotification(ratio);

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
            var control = new AudioEventControl(_audioService, _searchWindow);
            control.Bind(data);

            control.Closed += () =>
            {
                _node.RemoveEvent(data);
            };

            control.PlayClicked += () =>
            {
                _activeAudioControl = control;
            };

            control.StopClicked += () =>
            {
                _activeAudioControl = null;
                _activeAudioControl.SetProgressWithoutNotification(0);
            };

            control.ProgressChanged += (value) =>
            {
                _activeAudioControl = control;
            };

            return control;
        }
    }
}