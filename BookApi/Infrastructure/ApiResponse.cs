using Shirley.Book.Model;

namespace Shirley.Book.Web.Infrastructure
{
    public class ApiResponse
    {
        /// <summary>
        /// generate a response object with status 200
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static BaseResponse OK<T>(T data)
        {
            return new BaseResponse<T> { StatusCode = 200, Data = data, Result = true };
        }

        /// <summary>
        /// generate a response object with special error message and status code
        /// </summary>
        /// <param name="message">error message</param>
        /// <param name="status">status code.default is 500.</param>
        /// <returns></returns>
        public static BaseResponse Error(string message, int status = 500)
        {
            return new BaseResponse { StatusCode = status, Message = message, Result = false };
        }
    }
}
