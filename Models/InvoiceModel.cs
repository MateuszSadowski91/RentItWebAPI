using Newtonsoft.Json;
using System.Collections.Generic;

namespace RentItAPI.Models
{
    public class InvoiceModel
    {
        /// <summary>
        /// Properties match fields of Invoice Generator API.
        /// </summary>
        ///
        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("tax")]
        public int Tax { get; set; }

        [JsonProperty("items")]
        public List<LineItemModel> Items { get; set; }

        [JsonProperty("fields")]
        public Fields fields { get; set; }

        public class Fields
        {
            [JsonProperty("tax")]
            public string Tax { get; set; }

            [JsonProperty("discounts")]
            public bool Discounts { get; set; }

            [JsonProperty("shipping")]
            public bool Shipping { get; set; }
        }
    }
}