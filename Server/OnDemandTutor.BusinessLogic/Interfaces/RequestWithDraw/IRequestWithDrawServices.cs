using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Dtos.WithDrawDto;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Interfaces.RequestWithDraw;

public interface IRequestWithDrawServices
{
    Task<PagedResult<GetRequestWithdrawDto>> ViewAllRequestWithDraw(RequestWithDrawFilterDto request,
        GetProfileUserDtos user);

    Task<PagedResult<GetRequestWithdrawDto>> ViewAllRequestWithDrawAsAdmin(RequestWithDrawFilterDto request);
    Task<bool> CreateWithdrawRequest(CreateRequestWithdrawDto request, GetProfileUserDtos user);
    Task<bool> ApproveWithDraw(ApproveWithDrawDto request, GetProfileUserDtos user);
}