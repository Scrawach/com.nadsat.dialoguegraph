using System;
using Editor.Audios;
using Runtime.Nodes;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Drawing.Controls
{
    public class AudioEventControl : BaseControl
    {
        private const string Uxml = "UXML/Controls/AudioEventControl";

        private readonly Button _selectEventButton;
        private readonly Button _playButton;
        private readonly Button _stopButton;

        private readonly Slider _progressSlider;
        private readonly FloatField _delayField;
        private readonly Button _closeButton;

        private readonly AudioEditorService _audioEditorService;
        private AudioEventData _data;

        public AudioEventControl(AudioEditorService audioEditorService) : base(Uxml)
        {
            _audioEditorService = audioEditorService;
            _selectEventButton = this.Q<Button>("select-button");
            _playButton = this.Q<Button>("play-button");
            _stopButton = this.Q<Button>("stop-button");
            _progressSlider = this.Q<Slider>("progress-slider");
            _delayField = this.Q<FloatField>("delay-field");
            _closeButton = this.Q<Button>("close-button");

            _selectEventButton.clicked += OnSelectClicked;
            _playButton.clicked += OnPlayClicked;
            _stopButton.clicked += OnStopClicked;
            _progressSlider.RegisterValueChangedCallback(OnProgressValueChanged);
            _delayField.RegisterValueChangedCallback(OnDelayValueChanged);
        }

        public void Bind(AudioEventData data)
        {
            _data = data;
            _delayField.value = _data.Delay;
        }

        public event Action Closed
        {
            add => _closeButton.clicked += value;
            remove => _closeButton.clicked -= value;
        }

        public string EventName => _selectEventButton.text;
        
        private void OnSelectClicked()
        {
            
        }

        private void OnPlayClicked() =>
            _audioEditorService.Play(EventName);

        private void OnStopClicked() =>
            _audioEditorService.Stop(EventName);

        private void OnProgressValueChanged(ChangeEvent<float> evt)
        {
            var targetTime = evt.newValue;
            _audioEditorService.Seek(EventName, targetTime);
        }

        private void OnDelayValueChanged(ChangeEvent<float> evt)
        {
            _data.Delay = evt.newValue;
        }
    }
}