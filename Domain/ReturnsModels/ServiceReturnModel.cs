namespace Domain.ReturnsModels
{
    public class ServiceReturnModel<T>
    {
        public T Value { get; set; }
        public bool IsSucceeded { get; set; }
        public string Comment { get; set; }
    }
}
