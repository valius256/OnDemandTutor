using Mapster;
using Microsoft.AspNetCore.Http;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.TutorDegree;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Models.Dtos.TutorDegree;
using OnDemandTutor.Models.Dtos.User;
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
        public async Task<List<GetTutorDegreeDto>> GetTutorDegreesByTutorIdAndSubjectId(int tutorId, int subjectId)
        {
            var tutorDegree = await _unitOfWorkRepository.TutorDegreeRepository.WhereAsync(td => td.TutorId == tutorId && td.SubjectId == subjectId);
            return tutorDegree.Adapt<List<GetTutorDegreeDto>>();
        }

        public async Task<GetTutorDegreeDto> CreateTutorDegreeAsync(CreateTutorDegreeDto tutorDegreeDto)
        {
            var tutorDegree = tutorDegreeDto.Adapt<Models.Models.TutorDegree>();
            var createdTutorDegree = await _unitOfWorkRepository.TutorDegreeRepository.AddAsync(tutorDegree);
            await _unitOfWorkRepository.SaveChangesAsync();
            return createdTutorDegree.Entity.Adapt<GetTutorDegreeDto>();
        }

        public async Task UpsertTutorDegreeAsync(List<UpdateTutorDegreeDto> newDegreeDtos, List<GetTutorDegreeDto> oldTutorDegreeDtos , int userId, int subjectId)
        {
            foreach (var tutorDegree in newDegreeDtos)
            {
                // Retrieve the existing tutor degree entity from the database
                var existingTutorDegree = await _unitOfWorkRepository.TutorDegreeRepository.FirstOrDefaultAsync(td => td.Id == tutorDegree.Id);

                // Check if the entity is null
                if (existingTutorDegree == null)
                {
                    var createDto = tutorDegree.Adapt<CreateTutorDegreeDto>();
                    createDto.TutorId = userId;
                    createDto.SubjectId = subjectId;

                    await CreateTutorDegreeAsync(createDto);
                    continue;
                }

                // Adapt the incoming DTO to the existing entity
                existingTutorDegree = tutorDegree.Adapt(existingTutorDegree);

                // Set the updated fields
                existingTutorDegree.UpdatedById = userId; // Assuming there is an UpdatedById property
                existingTutorDegree.UpdatedDate = DateTime.Now; // Assuming there is an UpdatedDate property

                // Update the entity in the database
                var updatedTutorDegree = _unitOfWorkRepository.TutorDegreeRepository.Update(existingTutorDegree);
            }
            // Save the changes
            await _unitOfWorkRepository.SaveChangesAsync();

            //Delete the unused degrees
            foreach (var tutorDegree in oldTutorDegreeDtos)
            {
                if (!newDegreeDtos.Any(d => d.Id == tutorDegree.Id))
                {
                    await DeleteTutorDegreeAsync(tutorDegree.Id);
                }
            }

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