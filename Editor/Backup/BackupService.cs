using UnityEditor;
using UnityEngine;

namespace Nadsat.DialogueGraph.Editor.Backup
{
    public class BackupService
    {
        private readonly BackupGraphExporter _exporter;
        private readonly double _targetTimeInSeconds;
        private double _targetTime;

        private bool _isStarted = false;
        
        public BackupService(BackupGraphExporter exporter, float timeInMinutes)
        {
            _exporter = exporter;
            _targetTimeInSeconds = timeInMinutes * 60f;
            _targetTime = EditorApplication.timeSinceStartup + _targetTimeInSeconds;
        }

        public void Start() => 
            _isStarted = true;

        public void Stop() => 
            _isStarted = false;

        public void Update()
        {
            if (!_isStarted)
                return;
            
            var current = EditorApplication.timeSinceStartup;

            if (current < _targetTime) 
                return;
            
            _targetTime = current + _targetTimeInSeconds;
            _exporter.Export();
            Debug.Log("Create Backup!");
        }
    }
}