using System;
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

        private readonly AudioEventData _data;

        public AudioEventControl(AudioEventData data) : base(Uxml)
        {
            _data = data;
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
            
            _delayField.value = _data.Delay;
        }

        public event Action Closed
        {
            add => _closeButton.clicked += value;
            remove => _closeButton.clicked -= value;
        }
        
        private void OnSelectClicked()
        {
            
        }

        private void OnPlayClicked()
        {
            
        }

        private void OnStopClicked()
        {
            
        }

        private void OnProgressValueChanged(ChangeEvent<float> evt)
        {
            
        }

        private void OnDelayValueChanged(ChangeEvent<float> evt)
        {
            _data.Delay = evt.newValue;
        }
    }
}