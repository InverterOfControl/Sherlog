namespace Sherlog.Shared.Models
{
    public class ServiceConfiguration
    {
        public string ServiceName { get; set; }

        public string LogPath { get; set; }

        public string DateFormat { get; set; }

        public string BackupPath { get; set; }

        public bool DoBackups { get; set; }

        public int RotationInterval { get; set; }

        public int DaysToKeepUnprocessed { get; set; }
    }
}
