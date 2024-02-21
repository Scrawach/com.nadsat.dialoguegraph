using Editor.Drawing;
using Editor.Exporters;
using Editor.Factories.NodeListeners;
using Runtime;
using UnityEditor;
using UnityEngine;

namespace Editor.Backup
{
    public class BackupService
    {
        private readonly BackupGraphExporter _exporter;
        private readonly double _targetTimeInSeconds;
        private double _targetTime;
        
        public BackupService(BackupGraphExporter exporter, float timeInMinutes)
        {
            _exporter = exporter;
            _targetTimeInSeconds = timeInMinutes * 60;
            _targetTime = EditorApplication.timeSinceStartup + _targetTimeInSeconds;
        }
        
        public void Update()
        {
            var current = EditorApplication.timeSinceStartup;
            if (current >= _targetTime)
            {
                _targetTime = current + _targetTimeInSeconds;
                SaveBackup();
            }
        }

        private void SaveBackup()
        {
            Debug.Log($"processing save backup...");
            _exporter.Export();
        }
    }
}