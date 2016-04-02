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
    }
}