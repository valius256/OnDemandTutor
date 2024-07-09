using System;
using Mapster;
using Microsoft.AspNetCore.Http;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.StudentClass;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Models.Dtos.StudentClass;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Services.StudentClass
{
    public class StudentClassService : IStudentClassService
    {
        private readonly IUnitOfWorkRepository _unitOfWork;
        private readonly IAuthServices _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StudentClassService(IUnitOfWorkRepository unitOfWork, IAuthServices authService, IHttpContextAccessor HttpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
            _httpContextAccessor = HttpContextAccessor;
        }
        public async Task<PagedResult<GetStudentClassDto>> GetStudentClassesAsync(PagingModel<GetStudentClassDto> pagingModel)
        {
            var pagedResult = await _unitOfWork.StudentClassRepository.PagingAsync(pagingModel.Adapt<PagingModel<Models.Models.StudentClass>>());
            return pagedResult.Adapt<PagedResult<GetStudentClassDto>>();
        }

        public async Task<GetStudentClassDto> GetStudentClassByIdAsync(int id)
        {
            var studentClass = await _unitOfWork.StudentClassRepository.FirstOrDefaultAsync(sc => sc.Id == id);
            return studentClass?.Adapt<GetStudentClassDto>();
        }

        public async Task<CreateStudentClassDto> CreateStudentClassAsync(CreateStudentClassDto studentClassDto)
        {
            var studentClass = studentClassDto.Adapt<Models.Models.StudentClass>();
            var createdStudentClass = await _unitOfWork.StudentClassRepository.AddAsync(studentClass);
            await _unitOfWork.SaveChangesAsync();
            return createdStudentClass.Entity.Adapt<CreateStudentClassDto>();
        }
        public async Task<UpdateStudentClassDto> UpdateStudentClassAsync(UpdateStudentClassDto studentClassDto)
        {
            // Retrieve the existing student class entity from the database
            var existingStudentClassEntity = await _unitOfWork.StudentClassRepository.FirstOrDefaultAsync(sc => sc.Id == studentClassDto.Id);

            // Check if the entity is null
            if (existingStudentClassEntity == null)
            {
                throw new NotFoundException($"StudentClass with ID {studentClassDto.Id} not found.");
            }

            // Retrieve the user profile
            var user = await _authService.GetUserProfileByClaim(_httpContextAccessor.HttpContext.User);

            // Adapt the incoming DTO to the existing entity
            existingStudentClassEntity = studentClassDto.Adapt(existingStudentClassEntity);

            // Update the entity fields if needed
            existingStudentClassEntity.UpdatedDate = DateTime.Now; // Assuming you want to update this field

            // Update the entity in the database
            var updatedStudentClassEntity = _unitOfWork.StudentClassRepository.Update(existingStudentClassEntity);

            // Save the changes
            await _unitOfWork.SaveChangesAsync();

            // Return the updated DTO
            return updatedStudentClassEntity.Entity.Adapt<UpdateStudentClassDto>();
        }


        public async Task<bool> DeleteStudentClassAsync(int id)
        {
            var studentClass = await _unitOfWork.StudentClassRepository.FirstOrDefaultAsync(sc => sc.Id == id);
            if (studentClass == null)
            {
                throw new Exception("StudentClass not found");
            }
            _unitOfWork.StudentClassRepository.Remove(studentClass);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}

