using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using OnDemandTutor.BusinessLogic.Interfaces.TutorDegree;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Models.Dtos.TutorDegree;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Services.TutorDegreeService
{
    public class TutorDegreeService : ITutorDegreeService
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public TutorDegreeService(IUnitOfWorkRepository unitOfWorkRepository)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<PagedResult<GetTutorDegreeDto>> GetTutorDegreesAsync(PagingModel<GetTutorDegreeDto> request)
        {
            var pagedTutorDegrees = await _unitOfWorkRepository.TutorDegreeRepository.PagingAsync(request.Adapt<PagingModel<Models.Models.TutorDegree>>());
            return pagedTutorDegrees.Adapt<PagedResult<GetTutorDegreeDto>>();
        }

        public async Task<GetTutorDegreeDto> GetTutorDegreeByIdAsync(int id)
        {
            var tutorDegree = await _unitOfWorkRepository.TutorDegreeRepository.FirstOrDefaultAsync(td => td.Id == id);
            if (tutorDegree == null)
            {
                throw new NotFoundException($"TutorDegree with ID {id} not found.");
            }
            return tutorDegree.Adapt<GetTutorDegreeDto>();
        }

        public async Task<CreateTutorDegreeDto> CreateTutorDegreeAsync(CreateTutorDegreeDto tutorDegreeDto)
        {
            var tutorDegree = tutorDegreeDto.Adapt<Models.Models.TutorDegree>();
            var createdTutorDegree = await _unitOfWorkRepository.TutorDegreeRepository.AddAsync(tutorDegree);
            await _unitOfWorkRepository.SaveChangesAsync();
            return createdTutorDegree.Entity.Adapt<CreateTutorDegreeDto>();
        }

        public async Task<UpdateTutorDegreeDto> UpdateTutorDegreeAsync(UpdateTutorDegreeDto tutorDegreeDto)
        {
            var existingTutorDegree = await _unitOfWorkRepository.TutorDegreeRepository.FirstOrDefaultAsync(td => td.Id == tutorDegreeDto.Id);
            if (existingTutorDegree == null)
            {
                throw new NotFoundException($"TutorDegree with ID {tutorDegreeDto.Id} not found.");
            }

            existingTutorDegree = tutorDegreeDto.Adapt(existingTutorDegree);
            var updatedTutorDegree = _unitOfWorkRepository.TutorDegreeRepository.Update(existingTutorDegree);
            await _unitOfWorkRepository.SaveChangesAsync();

            return updatedTutorDegree.Entity.Adapt<UpdateTutorDegreeDto>();
        }

        public async Task<bool> DeleteTutorDegreeAsync(int id)
        {
            var existingTutorDegree = await _unitOfWorkRepository.TutorDegreeRepository.FirstOrDefaultAsync(td => td.Id == id);
            if (existingTutorDegree == null)
            {
                throw new NotFoundException($"TutorDegree with ID {id} not found.");
            }

            _unitOfWorkRepository.TutorDegreeRepository.Remove(existingTutorDegree);
            await _unitOfWorkRepository.SaveChangesAsync();

            return true;
        }
    }
}