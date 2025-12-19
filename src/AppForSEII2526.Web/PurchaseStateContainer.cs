using AppForSEII2526.Web.API;
namespace AppForSEII2526.Web
{
    public class PurchaseStateContainer
    {


        //we create an instance of Purchase when an instance of RentalStateContainer is created
        public PurchaseForCreateDTO Purchase { get; private set; } = new PurchaseForCreateDTO()
        {
            PurchaseItems = new List<PurchaseItemDTO>()
        };

        //we compute the TotalPrice of the movies we have selected for renting them
        public decimal TotalPrice
        {
            get
            {
                int numberOfDays = (Purchase.PurchaseDateTo - Purchase.PurchaseDateFrom).Days;
                return Convert.ToDecimal(Purchase.PurchaseItems.Sum(ri => ri.Price * ri.Quantity));
            }
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();



        public void AddDeviceToPurchase(DevicesParaComprarDTO device)
        {
            //before adding a movie we checked whether it has been already added
            if (!Purchase.PurchaseItems.Any(ri => ri.DeviceID == device.Id))
                //we add it if it is not in the list
                Purchase.PurchaseItems.Add(new PurchaseItemDTO()
                {
                    DeviceID = device.Id,
                    Description = device.Name,
                    Price = device.Price,
                    Brand = device.Brand,
                    Model = device.Model,
                    Color = device.Color,
                    Quantity = 1
                }
            );

        }

        //to delete movies from the list of selected movies
        public void RemovePurchaseItemToRent(PurchaseItemDTO item)
        {
            Purchase.PurchaseItems.Remove(item);

        }

        //we eliminate all the movies from the list
        public void ClearRentingCart()
        {
            Purchase.PurchaseItems.Clear();

        }

        //we have already finished the process of renting, thus, we create a new Rental 
        public void PurchaseProcessed()
        {
            //we have finished the rental process so we create a new object without data
            Purchase = new PurchaseForCreateDTO()
            {
                PurchaseItems = new List<PurchaseItemDTO>()
            };
        }
    }
}
