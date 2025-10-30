namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(ReceiptId),nameof(RepairId))]
    public class Receiptitem
    {
        [StringLength(50, ErrorMessage = "Model cannot be longer than 50 characters.", MinimumLength = 4)]
        public string Model { get; set; }

        public int ReceiptId { get; set; }
        public Receipt Receipt { get; set; }
        public int RepairId { get; set; }
        public Repair Repair { get; set; }

        public Receiptitem()
        {
        }

        public Receiptitem(string model,Receipt receipt ,Repair repairItem)
        {
            Model = model;
            Receipt = receipt;
            Repair = repairItem;
        }
    }
}
