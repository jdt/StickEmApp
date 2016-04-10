using System;

namespace StickEmApp.Entities
{
    public class Vendor
    {
        public Vendor()
        {
            Id = Guid.NewGuid();

            NumberOfStickersReceived = 0;
            NumberOfStickersReturned = 0;
            ChangeReceived = new Money(0);

            AmountReturned = new AmountReturned();

            Status = VendorStatus.Working;
        }

        public virtual Guid Id { get; set; }

        public virtual string Name { get; set; }
        public virtual int NumberOfStickersReceived { get; set; }
        public virtual int NumberOfStickersReturned { get; set; }
        public virtual Money ChangeReceived { get; set; }
        public virtual AmountReturned AmountReturned { get; set; }
        
        public virtual VendorStatus Status { get; set; }

        public virtual void Remove()
        {
            Status = VendorStatus.Removed;
        }

        public virtual void Finish()
        {
            Status = VendorStatus.Finished;
        }

        public virtual Money CalculateTotalAmountRequired()
        {
            return (NumberOfStickersReceived * Sticker.Price) - (NumberOfStickersReturned * Sticker.Price) + ChangeReceived;
        }

        public virtual Money CalculateTotalAmountReturned()
        {
            return AmountReturned.CalculateTotal();
        }

        public virtual SalesResult CalculateSalesResult()
        {
            var result = new SalesResult();

            var difference = AmountReturned.CalculateTotal() - CalculateTotalAmountRequired();
            if (difference > 0)
            {
                result.Status = ResultType.Surplus;
            }
            else if (difference < 0)
            {
                result.Status = ResultType.Shortage;
            }
            else
            {
                result.Status = ResultType.Exact;
            }

            result.Difference = difference.ToAbsolute();
            result.NumberOfStickersSold = NumberOfStickersReceived - NumberOfStickersReturned;
            return result;
        }
    }
}
