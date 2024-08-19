using LinqKit;
using Mapster;
using Microsoft.AspNetCore.Http;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.FAQ;
using OnDemandTutor.BusinessLogic.Interfaces.Notification;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Helper;
using OnDemandTutor.Models.Dtos.FAQ;
using OnDemandTutor.Models.Dtos.Notification;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;

public class FAQService : IFAQService
{
    private readonly IUnitOfWorkRepository _unitOfWorkRepository;
    private readonly IAuthServices _authService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly INotificationService _notificationService;
    public FAQService(IUnitOfWorkRepository unitOfWorkRepository,INotificationService notificationService ,IAuthServices authService, IHttpContextAccessor HttpContextAccessor)
    {
        _unitOfWorkRepository = unitOfWorkRepository;
        _authService = authService;
        _httpContextAccessor = HttpContextAccessor;
        _notificationService = notificationService;
    }

    public async Task<PagedResult<FAQDTO>> GetFAQsAsync(PagingModel<QueryFAQDTO> request)
    {
        var pagedFAQs = await _unitOfWorkRepository.FAQRepository.GetFAQs(request);
        if (pagedFAQs is null)
        {
            throw new DataNotFoundException($"FAQs not found.");
        }
        return pagedFAQs.Adapt<PagedResult<FAQDTO>>();
    }

    public async Task<FAQDTO> GetFAQByIdAsync(int id)
    {
        var faqEntity = await _unitOfWorkRepository.FAQRepository.FirstOrDefaultAsync(f => f.Id == id);
        if (faqEntity == null)
        {
            throw new DataNotFoundException($"FAQ with ID {id} not found.");
        }
        return faqEntity.Adapt<FAQDTO>();
    }

    public async Task<CreateFAQDto> CreateFAQAsync(CreateFAQDto faqDto)
    {
        var user = await _authService.GetUserProfileByClaim(_httpContextAccessor.HttpContext.User);

        var faqEntity = faqDto.Adapt<FAQ>();
        faqEntity.CreateById = user.Id;
        faqEntity.CreateAt = DateTime.Now;

        var createdFAQEntity = await _unitOfWorkRepository.FAQRepository.AddAsync(faqEntity);
        await _unitOfWorkRepository.SaveChangesAsync();
        return createdFAQEntity.Entity.Adapt<CreateFAQDto>();
    }

    public async Task<UpdateFAQDto> UpdateFAQAsync(UpdateFAQDto faqDto)
    {
        var existingFAQEntity = await _unitOfWorkRepository.FAQRepository.FirstOrDefaultAsync(f => f.Id == faqDto.Id);
        if (existingFAQEntity == null)
        {
            throw new DataNotFoundException($"FAQ with ID {faqDto.Id} not found.");
        }

        var user = await _authService.GetUserProfileByClaim(_httpContextAccessor.HttpContext.User);
        existingFAQEntity = faqDto.Adapt(existingFAQEntity);
        existingFAQEntity.UpdatedDate = DateTime.Now; // Update this field if needed

        var updatedFAQEntity = _unitOfWorkRepository.FAQRepository.Update(existingFAQEntity);
        await _unitOfWorkRepository.SaveChangesAsync();
        return updatedFAQEntity.Entity.Adapt<UpdateFAQDto>();
    }

    public async Task<bool> DeleteFAQAsync(int id)
    {
        var existingFAQEntity = await _unitOfWorkRepository.FAQRepository.FirstOrDefaultAsync(f => f.Id == id);
        if (existingFAQEntity == null)
        {
            throw new DataNotFoundException($"FAQ with ID {id} not found.");
        }
        _unitOfWorkRepository.FAQRepository.Remove(existingFAQEntity);
        await _unitOfWorkRepository.SaveChangesAsync();

        return true;
    }
}
