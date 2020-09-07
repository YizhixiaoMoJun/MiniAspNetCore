using BookApi.Model;
using Shirley.Book.Model;
using System.Threading.Tasks;

namespace Shirley.Book.Service.AuthServices
{
    public interface IAuthServices
    {
        Task<BaseResponse> Login(UserInfo userInfo);

    }
}
