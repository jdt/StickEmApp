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

        public static Money operator *(int a, Money b)
        {
            return new Money(b._amount * a);
        }

        public static Money operator -(Money a, Money b)
        {
            return new Money(a._amount - b._amount);
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