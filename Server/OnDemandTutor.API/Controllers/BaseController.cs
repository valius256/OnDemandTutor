using Microsoft.AspNetCore.Mvc;
using OnDemandTutor.API.Models;

namespace OnDemandTutor.API.Controllers
{
    public class BaseController<T> : ControllerBase
    {
        protected readonly ILogger<T> _logger;

        public BaseController(ILogger<T> logger)
        {
            _logger = logger;
        }

        protected async Task<IApiResult<F>> OKAsync<F>(Task<F> action, string? op = null)
        {
            var result = await Task.Run(() => action);

            return new ApiResult<F>()
            {
                Op = op,
                Status = "OK",
                Data = result
            };
        }

        protected IApiResult<F> OKAsync<F>(F data, string? op = null)
        {

            return new ApiResult<F>()
            {
                Op = op,
                Status = "OK",
                Data = data
            };
        }
    }
}
