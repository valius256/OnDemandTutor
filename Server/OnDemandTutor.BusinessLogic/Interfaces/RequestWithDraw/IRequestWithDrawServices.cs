using OnDemandTutor.Models.Dtos.WithDrawDto;
using System.Security.Claims;

namespace OnDemandTutor.BusinessLogic.Interfaces.RequestWithDraw;

public interface IRequestWithDrawServices
{
    Task<List<RequestWithDrawDto>> ViewAllRequestWithDraw(RequestWithDrawFilterDto request, ClaimsPrincipal userClaims);
    Task<bool> CreateWithdrawRequest(RequestWithDrawDto request, ClaimsPrincipal userClaims);
    Task<bool> ApproveWithDraw(ApproveWithDrawDto request, ClaimsPrincipal userClaims);
}