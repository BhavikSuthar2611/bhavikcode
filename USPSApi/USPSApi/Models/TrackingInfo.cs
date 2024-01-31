using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USPSApi.Models
{
    public class TrackingInfo
    {
        public string TrackingNumber { get; set; }
        public string Status { get; set; }
        public string ExpectedDeliveryDate { get; set; }
        public List<TrackingEvent> Events { get; set; }
    }

    public class TrackingEvent
    {
        public DateTime EventDate { get; set; }
        public string EventTime { get; set; }
        public string EventCity { get; set; }
        public string EventState { get; set; }
        public string EventLocation { get; set; }
        public string EventDescription { get; set; }
    }
}