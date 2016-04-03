namespace StickEmApp.Entities
{
    public class SalesResult
    {
        public SalesResult()
        {
            Status = ResultType.Exact;
            Difference = new Money(0);
        }

        public ResultType Status { get; set; }
        public Money Difference { get; set; }
    }
}