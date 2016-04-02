namespace StickEmApp.Entities
{
    public class Money
    {
        private readonly decimal _amount;

        public Money(decimal amount)
        {
            _amount = amount;
        }

        public decimal Value
        {
            get
            {
                return _amount;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            return ((Money) obj)._amount == _amount;
        }

        public override int GetHashCode()
        {
            return _amount.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("€{0}",_amount);
        }
    }
}