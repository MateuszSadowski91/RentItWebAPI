namespace RentItAPI.Models
{
    public class LineItemModel
    {/// <summary>
     /// Properties match fields of Invoice Generator API.
     /// </summary>
        public string name { get; set; }

        public int quantity { get; set; }
        public double unit_cost { get; set; }
    }
}