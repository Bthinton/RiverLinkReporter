using System;

namespace RiverLinkReporter.models
{
    public enum WorkQueueType
    {
        TestPassword,
        GetData
    }

    public class WorkQueue
    {
        public int WorkQueue_id { get; set; }

        public int User_id { get; set; }

        public bool Completed { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModifiedDate { get; set; }
    }
}
