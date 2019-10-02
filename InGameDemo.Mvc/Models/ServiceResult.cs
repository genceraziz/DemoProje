namespace InGameDemo.Mvc.Models
{
    public class ServiceResult
    {
        public ResultStatus ResultStatus { get; set; }

        public ServiceResult()
        {
            ResultStatus = new ResultStatus
            {
                Status = Enums.ResultStatus.Error
            };
        }
    }

    public class ServiceResult<T> : ServiceResult
    {
        public T Result { get; set; }
    }

    public class ResultStatus
    {
        public string Explanation { get; set; }

        public Enums.ResultStatus Status { get; set; }
    }
}
