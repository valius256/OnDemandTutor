using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.FAQ;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Models.Dtos.FAQ;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;

public class FAQService : IFAQService
{
    private readonly IUnitOfWorkRepository _unitOfWorkRepository;
    private readonly IAuthServices _authService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public FAQService(IUnitOfWorkRepository unitOfWorkRepository, IAuthServices authService, IHttpContextAccessor HttpContextAccessor)
    {
        _unitOfWorkRepository = unitOfWorkRepository;
        _authService = authService;
        _httpContextAccessor = HttpContextAccessor;
    }

    public async Task<PagedResult<FAQDTO>> GetFAQsAsync(PagingModel<FAQDTO> request)
    {
        var pagedFAQs = await _unitOfWorkRepository.FAQRepository.PagingAsync(request.Adapt<PagingModel<FAQDTO>>());
        return pagedFAQs.Adapt<PagedResult<FAQDTO>>();
    }

    public async Task<FAQDTO> GetFAQByIdAsync(int id)
    {
        var faqEntity = await _unitOfWorkRepository.FAQRepository.FirstOrDefaultAsync(f => f.Id == id);
        if (faqEntity == null)
        {
            throw new NotFoundException($"FAQ with ID {id} not found.");
        }
        return faqEntity.Adapt<FAQDTO>();
    }

    public async Task<CreateFAQDto> CreateFAQAsync(CreateFAQDto faqDto)
    {
        var user = _authService.GetUserProfileByClaim(_httpContextAccessor.HttpContext.User);

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
            throw new NotFoundException($"FAQ with ID {faqDto.Id} not found.");
        }

        var user = await _authService.GetUserProfileByClaim(_httpContextAccessor.HttpContext.User);
        existingFAQEntity = faqDto.Adapt(existingFAQEntity);
        existingFAQEntity.CreateById = user.Id; // Update this field if needed
        existingFAQEntity.CreateAt = DateTime.Now; // Update this field if needed

        var updatedFAQEntity = _unitOfWorkRepository.FAQRepository.Update(existingFAQEntity);
        await _unitOfWorkRepository.SaveChangesAsync();

        return updatedFAQEntity.Entity.Adapt<UpdateFAQDto>();
    }

    public async Task<bool> DeleteFAQAsync(int id)
    {
        var existingFAQEntity = await _unitOfWorkRepository.FAQRepository.FirstOrDefaultAsync(f => f.Id == id);
        if (existingFAQEntity == null)
        {
            throw new NotFoundException($"FAQ with ID {id} not found.");
        }

        _unitOfWorkRepository.FAQRepository.Remove(existingFAQEntity);
        await _unitOfWorkRepository.SaveChangesAsync();

        return true;
    }
}
