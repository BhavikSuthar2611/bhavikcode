using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using USPSApi.Models;

namespace USPSApi.Controllers
{
    public class USPSController : Controller
    {
        // GET: USPS
        public ActionResult Index()
        {
            var dt = GetTrackingInfoAsync("teEEDDeR1w84noMHEG9B9whqVrDAsU8s", "9241990302943500423603");
            return View();
        }
        public async Task<TrackingInfo> GetTrackingInfoAsync(string userId, string trackingNumber)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var url = "https://secure.shippingapis.com/ShippingAPI.dll?API=TrackV2&XML=<TrackRequest USERID=\"" + userId + "\"><TrackID>" + trackingNumber + "</TrackID></TrackRequest>";

                    var response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var xmlString = await response.Content.ReadAsStringAsync();
                        var xmlDocument = XDocument.Parse(xmlString);

                        var trackSummary = xmlDocument.Descendants("TrackSummary").FirstOrDefault();

                        if (trackSummary != null)
                        {
                            // Extract tracking information as before
                            // ...
                            return new TrackingInfo
                            {
                                TrackingNumber = trackingNumber,
                                Status = trackSummary.Element("Status").Value,
                                ExpectedDeliveryDate = trackSummary.Element("ExpectedDeliveryDate").Value,
                                Events = trackSummary.Descendants("Event")
                                .Select(e => new TrackingEvent
                                {
                                    EventDate = DateTime.Parse(e.Element("EventDate").Value),
                                    EventTime = e.Element("EventTime").Value,
                                    EventCity = e.Element("EventCity").Value,
                                    EventState = e.Element("EventState").Value,
                                    EventLocation = e.Element("EventLocation").Value,
                                    EventDescription = e.Element("EventDescription").Value
                                })
                                .ToList()
                            };
                           
                        }
                        else
                        {
                            // Handle the case where the response doesn't contain expected data
                            // ...
                        }
                    }
                    else
                    {
                        // Handle non-successful HTTP status codes
                        // ...
                    }
                }
                catch (Exception ex)
                {
                    // Handle other exceptions
                    // Log or print the exception details for debugging
                    // ...
                }

                return null;
            }

            //try
            //{


            //using (var client = new HttpClient())
            //{
            //    var url = "https://secure.shippingapis.com/ShippingAPI.dll?API=TrackV2&XML=<TrackRequest USERID=\"" + userId + "\"><TrackID>" + trackingNumber + "</TrackID></TrackRequest>";

            //    var response = await client.GetAsync(url);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        var xmlString = await response.Content.ReadAsStringAsync();
            //        var xmlDocument = XDocument.Parse(xmlString);

            //        var trackSummary = xmlDocument.Descendants("TrackSummary").FirstOrDefault();

            //        if (trackSummary != null)
            //        {
            //            return new TrackingInfo
            //            {
            //                TrackingNumber = trackingNumber,
            //                Status = trackSummary.Element("Status").Value,
            //                ExpectedDeliveryDate = trackSummary.Element("ExpectedDeliveryDate").Value,
            //                Events = trackSummary.Descendants("Event")
            //                    .Select(e => new TrackingEvent
            //                    {
            //                        EventDate = DateTime.Parse(e.Element("EventDate").Value),
            //                        EventTime = e.Element("EventTime").Value,
            //                        EventCity = e.Element("EventCity").Value,
            //                        EventState = e.Element("EventState").Value,
            //                        EventLocation = e.Element("EventLocation").Value,
            //                        EventDescription = e.Element("EventDescription").Value
            //                    })
            //                    .ToList()
            //            };
            //        }
            //    }
            //}
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
            //return null;
        }
    }
}