using UnityEditor;

namespace Nadsat.DialogueGraph.Editor.Backup
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

            if (current < _targetTime) 
                return;
            
            _targetTime = current + _targetTimeInSeconds;
            _exporter.Export();
        }
    }
}