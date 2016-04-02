namespace StickEmApp.Entities
{
    public class AmountReturned
    {
        public AmountReturned()
        {
            FiveHundreds = 0;
            TwoHundreds = 0;
            Hundreds = 0;
            Fifties = 0;
            Twenties = 0;
            Tens = 0;
            Fives = 0;
            Twos = 0;
            Ones = 0;

            FiftyCents = 0;
            TwentyCents = 0;
            TenCents = 0;
            FiveCents = 0;
            TwoCents = 0;
            OneCents = 0;
        }

        public virtual int FiveHundreds { get; set; }
        public virtual int TwoHundreds { get; set; }
        public virtual int Hundreds { get; set; }
        public virtual int Fifties { get; set; }
        public virtual int Twenties { get; set; }
        public virtual int Tens { get; set; }
        public virtual int Fives { get; set; }
        public virtual int Twos { get; set; }
        public virtual int Ones { get; set; }
        public virtual int FiftyCents { get; set; }
        public virtual int TwentyCents { get; set; }
        public virtual int TenCents { get; set; }
        public virtual int FiveCents { get; set; }
        public virtual int TwoCents { get; set; }
        public virtual int OneCents { get; set; }

        public virtual Money CalculateTotal()
        {
            var total = new Money(0);

            total += FiveHundreds * new Money(500);
            total += TwoHundreds * new Money(200);
            total += Hundreds * new Money(100);
            total += Fifties * new Money(50);
            total += Twenties * new Money(20);
            total += Tens * new Money(10);
            total += Fives * new Money(5);
            total += Twos * new Money(2);
            total += Ones * new Money(1);

            total += FiftyCents * new Money(0.50m);
            total += TwentyCents * new Money(0.20m);
            total += TenCents * new Money(0.10m);
            total += FiveCents * new Money(0.05m);
            total += TwoCents * new Money(0.02m);
            total += OneCents * new Money(0.01m);

            return total;
        }
    }
}