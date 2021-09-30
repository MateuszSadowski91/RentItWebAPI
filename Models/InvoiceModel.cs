using Newtonsoft.Json;
using RentItAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentItAPI.Models
{
    public class InvoiceModel
    {
        /// <summary>
        /// Properties match fields of Invoice Generator API.
        /// </summary>
        /// 
        [JsonProperty("number")]
        public string number { get; set; }
        [JsonProperty("date")]
        public string date { get; set; }
        [JsonProperty("from")]
        public string from { get; set; }
        [JsonProperty("to")]
        public string to { get; set; }
        [JsonProperty("currency")]
        public string currency { get; set; }
        [JsonProperty("tax")]
        public int tax { get; set; }
        [JsonProperty("items")]
        public List<LineItemModel> items { get; set; }
        [JsonProperty("fields")]
        public Fields fields { get; set; }
        public class Fields
        {
            [JsonProperty("tax")]
            public string tax { get; set; }
            [JsonProperty("discounts")]
            public bool discounts { get; set; }
            [JsonProperty("shipping")]
            public bool shipping { get; set; }
        }
    }
}
