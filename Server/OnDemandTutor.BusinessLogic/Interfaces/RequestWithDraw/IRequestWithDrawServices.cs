using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Dtos.WithDrawDto;
using OnDemandTutor.Models.Paging;
using System.Security.Claims;

namespace OnDemandTutor.BusinessLogic.Interfaces.RequestWithDraw;

public interface IRequestWithDrawServices
{
    Task<PagedResult<GetRequestWithdrawDto>> ViewAllRequestWithDraw(RequestWithDrawFilterDto request, GetProfileUserDto user);
    Task<PagedResult<GetRequestWithdrawDto>> ViewAllRequestWithDrawAsAdmin(RequestWithDrawFilterDto request);
    Task<bool> CreateWithdrawRequest(CreateRequestWithdrawDto request, GetProfileUserDto user);
    Task<bool> ApproveWithDraw(ApproveWithDrawDto request, GetProfileUserDto user);
}