using OnDemandTutor.Models.Dtos.WithDrawDto;
using OnDemandTutor.Models.Paging;
using System.Security.Claims;

namespace OnDemandTutor.BusinessLogic.Interfaces.RequestWithDraw;

public interface IRequestWithDrawServices
{
    Task<PagedResult<GetRequestWithdrawDto>> ViewAllRequestWithDraw(RequestWithDrawFilterDto request, ClaimsPrincipal userClaims);
    Task<PagedResult<GetRequestWithdrawDto>> ViewAllRequestWithDrawAsAdmin(RequestWithDrawFilterDto request);
    Task<bool> CreateWithdrawRequest(CreateRequestWithdrawDto request, ClaimsPrincipal userClaims);
    Task<bool> ApproveWithDraw(ApproveWithDrawDto request, ClaimsPrincipal userClaims);
}