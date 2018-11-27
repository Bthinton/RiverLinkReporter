using System;
using System.Collections.Generic;
using System.Text;

namespace RiverLinkReporter.models
{
    public enum WorkQueueType
    {
        TestPassword,
        GetData
    }
    //WorkQueue _ID, WorkQueueType (CheckPassword, GetData), User_ID, Complete (bit), CreatedDate, LastMOdifiedDate
    public class WorkQueue
    {
        public int WorkQueue_id { get; set; }

        public int User_id { get; set; }

        public bool Completed { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModifiedDate { get; set; }
    }
}
