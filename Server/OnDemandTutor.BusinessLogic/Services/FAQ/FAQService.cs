using System;
using Mapster;
using OnDemandTutor.BusinessLogic.Interfaces.FAQ;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models.Dtos.FAQ;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Services.FAQ
{
    public class FAQService : IFAQService
    {
        private readonly IUnitOfWorkRepository _unitOfWork;

        public FAQService(IUnitOfWorkRepository unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<FAQDto>> GetFAQsAsync(PagingModel<FAQDto> pagingModel)
        {
            var pagedResult = await _unitOfWork.FAQRepository.PagingAsync(pagingModel);
            return pagedResult.Adapt<PagedResult<FAQDto>>();
        }

        public async Task<FAQDto> GetFAQByIdAsync(int id)
        {
            var faq = await _unitOfWork.FAQRepository.FirstOrDefaultAsync(c => c.Id == id);
            return faq?.Adapt<FAQDto>();
        }

        public async Task<FAQDto> CreateFAQAsync(FAQDto faqDto)
        {
            var faqEntity = faqDto.Adapt<Models.Models.FAQ>();
            var addedEntity = _unitOfWork.FAQRepository.Add(faqEntity);
            await _unitOfWork.SaveChangesAsync();
            return addedEntity.Entity.Adapt<FAQDto>();
        }

        public async Task<FAQDto> UpdateFAQAsync(FAQDto faqDto)
        {
            var faqEntity = await _unitOfWork.FAQRepository.FirstOrDefaultAsync(c => c.Id == faqDto.Id);
            if (faqEntity == null)
            {
                throw new KeyNotFoundException($"FAQ with ID {faqDto.Id} not found.");
            }

            faqEntity.Question = faqDto.Question;
            faqEntity.Answer = faqDto.Answer;

            var updatedEntity = _unitOfWork.FAQRepository.Update(faqEntity);
            await _unitOfWork.SaveChangesAsync();
            return updatedEntity.Entity.Adapt<FAQDto>();
        }

        public async Task<bool> DeleteFAQAsync(int id)
        {
            var faqEntity = await _unitOfWork.FAQRepository.FirstOrDefaultAsync(c => c.Id == id);
            if (faqEntity == null)
            {
                throw new KeyNotFoundException($"FAQ with ID {id} not found.");
            }

            _unitOfWork.FAQRepository.Remove(faqEntity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}

