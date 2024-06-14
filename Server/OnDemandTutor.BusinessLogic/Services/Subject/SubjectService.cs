using System;
using OnDemandTutor.BusinessLogic.Interfaces;
using OnDemandTutor.BusinessLogic.Interfaces.Auth;
using OnDemandTutor.BusinessLogic.Interfaces.User;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models.Dtos.Subject;
using OnDemandTutor.Models.RequestModel.Subject;

namespace OnDemandTutor.BusinessLogic.Services.Subject
{
	public class SubjectService : ISubjectService
    {
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;
        private readonly IFireBaseAuthServices _fireBaseAuthServices;

        public SubjectService( IUnitOfWorkRepository unitOfWorkRepository, IFireBaseAuthServices fireBaseAuthServices)
        {
            unitOfWorkRepository = _unitOfWorkRepository;
            fireBaseAuthServices = _fireBaseAuthServices;
        }

        public async Task<bool> CheckSubjectExists(string subjectName)
        {
            return await _unitOfWorkRepository.SubjectRepository.CheckSubjectExists(subjectName);
        }

        public async Task<GetSubjectDtos> GetSubjectByCode(int code)
        {
            return await _unitOfWorkRepository.SubjectRepository.GetSubjectByCode(code);
        }


        public Task<GetSubjectDtos> GetSubjectByName(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<GetSubjectDtos>> GetSubjectsByCategory(string category)
        {
            return await _unitOfWorkRepository.SubjectRepository.GetSubjectsByCategory(category);
        }

        public async Task<bool> IsSubjectActive(int subjectId)
        {
            return await _unitOfWorkRepository.SubjectRepository.IsSubjectActive(subjectId);
        }

        public async Task<IEnumerable<GetSubjectDtos>> SearchSubjectsByName(string name)
        {
            return await _unitOfWorkRepository.SubjectRepository.SearchSubjectsByName(name);
        }

        public async Task UpdateSubjectDescription(SubjectRequestModel requset)
        {
            await _unitOfWorkRepository.SubjectRepository.UpdateSubjectDescription(requset);
            await _unitOfWorkRepository.SaveChangesAsync();
        }

     
    }
}

