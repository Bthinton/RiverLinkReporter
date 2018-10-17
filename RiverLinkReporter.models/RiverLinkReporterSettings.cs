using System;
using System.Collections.Generic;
using System.Text;

namespace RiverLinkReporter.models
{
    public class RiverLinkReporterSettings
    {
        public string SmtpServer { get; set; }
        public string FromAddress { get; set; }
        public string FromName { get; set; }
        public string ConfirmEmailUrl { get; set; }
        public string ForgotPasswordUrl { get; set; }
        public bool LockoutOnFailure { get; set; }
    }
}
