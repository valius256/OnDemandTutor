using System;
using Mapster;
using OnDemandTutor.BusinessLogic.Interfaces.StudentClass;
using OnDemandTutor.DataAccess;
using OnDemandTutor.Models.Dtos.StudentClass;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.BusinessLogic.Services.StudentClass
{
    public class StudentClassService : IStudentClassService
    {
        private readonly IUnitOfWorkRepository _unitOfWork;

        public StudentClassService(IUnitOfWorkRepository unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
            var studentClass = studentClassDto.Adapt<Models.Models.StudentClass>();
            var updatedStudentClass = _unitOfWork.StudentClassRepository.Update(studentClass);
            await _unitOfWork.SaveChangesAsync();
            return updatedStudentClass.Entity.Adapt<UpdateStudentClassDto>();
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

