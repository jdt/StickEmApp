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

            StartedAt = DateTime.Now;
            FinishedAt = null;

            AmountReturned = new AmountReturned();

            Status = VendorStatus.Working;
        }

        public virtual Guid Id { get; set; }

        public virtual string Name { get; set; }
        public virtual DateTime StartedAt { get; set; }

        public virtual int NumberOfStickersReceived { get; set; }
        public virtual int NumberOfStickersReturned { get; set; }
        public virtual Money ChangeReceived { get; set; }
        public virtual AmountReturned AmountReturned { get; set; }

        public virtual VendorStatus Status { get; protected set; }
        public virtual DateTime? FinishedAt { get; protected set; }

        public virtual void Remove()
        {
            Status = VendorStatus.Removed;
        }

        public virtual void Finish(DateTime finishDateTime)
        {
            Status = VendorStatus.Finished;
            FinishedAt = finishDateTime;
        }

        public virtual void KeepWorking()
        {
            Status = VendorStatus.Working;
            FinishedAt = null;
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
            result.NumberOfStickersReceived = NumberOfStickersReceived;

            return result;
        }
    }
}
