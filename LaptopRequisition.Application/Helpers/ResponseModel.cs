using LaptopRequisition.Domain.Enums; // For ResponseCodeEnum

namespace LaptopRequisition.Application.Helpers
{
    public class ResponseModel<TCode, TData>
    {
        public bool IsSuccessful { get; set; }
        public string? Message { get; set; }
        public TCode Code { get; set; }
        public TData? Data { get; set; }

        public static ResponseModel<TCode, TData> Success(TData data, TCode code, string? message = "Operation Successful")
        {
            return new ResponseModel<TCode, TData>
            {
                IsSuccessful = true,
                Data = data,
                Code = code,
                Message = message
            };
        }

        public static ResponseModel<TCode, TData> Failure(TCode code, string? message = "Operation Failed", TData? data = default)
        {
            return new ResponseModel<TCode, TData>
            {
                IsSuccessful = false,
                Data = data,
                Code = code,
                Message = message
            };
        }
    }
}