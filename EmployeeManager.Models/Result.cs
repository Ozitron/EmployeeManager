namespace EmployeeManager.Models
{
    public class Result<T>
    {
        public T Data { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsSuccessful => string.IsNullOrEmpty(ErrorMessage);
    }

}
