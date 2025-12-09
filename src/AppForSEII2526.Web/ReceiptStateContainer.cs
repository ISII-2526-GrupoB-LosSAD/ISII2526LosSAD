using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class ReceiptStateContainer
    {
        public ReceiptForCreateDTO Receipt { get; private set; } = new ReceiptForCreateDTO()
        {
            ReceiptItems = new List<ReceiptitemDTO>()
        };

        public decimal TotalPrice
        {
            get
            {
                return Convert.ToDecimal(Receipt.ReceiptItems.Sum(ri => ri.Cost));
            }
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();

        public void AddReceiptToRepair(RepairParaRepararDTO repair)
        {
            if (!Receipt.ReceiptItems.Any(ri => ri.RepairID == repair.Id))
            {
                Receipt.ReceiptItems.Add(new ReceiptitemDTO
                {
                    RepairID = repair.Id,
                    Name = repair.Name,
                    Scale = repair.Scale,
                    Cost = repair.Cost,
                    Model = repair.Description
                });

            }
            ;
        }
        public void RemoveReceiptItemToRepair(ReceiptitemDTO item)
        {
            Receipt.ReceiptItems.Remove(item);
        }

        public void ClearReceiptCart()
        {
            Receipt.ReceiptItems.Clear();
        }

        public void ReceiptProcessed()
        {
            Receipt = new ReceiptForCreateDTO()
            {
                ReceiptItems = new List<ReceiptitemDTO>()
            };
        }
    }
}
