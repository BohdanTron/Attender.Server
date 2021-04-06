﻿namespace Attender.Server.Infrastructure.Sms
{
    public static class TwilioConstants
    {
        public static class Channel
        {
            public const string Sms = "sms";
        }

        public static class Status
        {
            public const string Approved = "approved";
            public const string Pending = "pending";
        }
    }
}
