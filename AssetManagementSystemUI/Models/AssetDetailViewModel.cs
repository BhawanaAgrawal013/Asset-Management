namespace AssetManagementSystemUI
{
    public class AssetDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Source { get; set; }

        public string Type { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public int Quantity { get; set; }

        public int? CategoryId { get; set; }
    }
}