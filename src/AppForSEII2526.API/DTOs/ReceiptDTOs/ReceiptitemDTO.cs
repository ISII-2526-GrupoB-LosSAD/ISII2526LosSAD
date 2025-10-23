namespace AppForSEII2526.API.DTOs.ReceiptDTOs
{
    public class ReceiptitemDTO
    {
        public string Name { get; set; }
        public string Scale { get; set; }
        public float Cost { get; set; }
        public string Model { get; set; }
        public ReceiptitemDTO(string Name, string Scale, float Cost, string Model)
        {
            this.Name = Name;
            this.Scale = Scale;
            this.Cost = Cost;
            this.Model = Model;
        }
    }
}
