using System;


//Bring over automation and CLI over to this project(services replaces BAL)
//for automation choose latest .NET standard libraries if possible(check compatibility)
//Decide on used entity or stored procedure for queue sql update

namespace RiverLinkReporter.models
{
    public enum WorkQueueTypes
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

        public WorkQueueTypes WorkQueueType { get; set; }
    }
}
