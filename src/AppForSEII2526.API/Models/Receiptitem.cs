namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(ReceiptId),nameof(RepairId))]
    public class Receiptitem
    {
        public string Model { get; set; }
        public int ReceiptId { get; set; }
        public int RepairId { get; set; }
    }
}
