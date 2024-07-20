using Mapster;
using Microsoft.EntityFrameworkCore;
using OnDemandTutor.BusinessLogic.Interfaces.SlotStudent;
using OnDemandTutor.BusinessLogic.Interfaces.User;
using OnDemandTutor.BusinessLogic.Services.Slot;
using OnDemandTutor.DataAccess;
using OnDemandTutor.DataAccess.ExceptionModels;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Dtos.Slot;
using OnDemandTutor.Models.Dtos.SlotStudent;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.BusinessLogic.Services.SlotStudent;

public class SlotStudentService : ISlotStudentServices
{
    private readonly IUnitOfWorkRepository _unitOfWorkRepository;
    private readonly IUserServices _userServices;


    public SlotStudentService(IUnitOfWorkRepository unitOfWorkRepository, IUserServices userServices)
    {
        _unitOfWorkRepository = unitOfWorkRepository;
        _userServices = userServices;
    }
    public async Task<List<GetSlotStudentDetailDto>> QuerySlotStudent(QuerySlotStudentDto querySlotStudentDto, GetProfileUserDtos user)
    {
        var slotStudent =
            await _unitOfWorkRepository.SlotStudentRepository.GetStudentSlotsAsync(querySlotStudentDto, user.Id);
        return slotStudent.Adapt<List<GetSlotStudentDetailDto>>();
    }
    public async Task<GetSlotStudentDetailDto> GetClosestFutureSlot(GetProfileUserDtos user)
    {
        var slotStudent =
            await _unitOfWorkRepository.SlotStudentRepository.GetClosestFutureSlot(user.Id);
        return slotStudent.Adapt<GetSlotStudentDetailDto>();
    }
    public async Task<SlotStudentDto> GetSlotStudentAsync(int slotId, int studentId)
    {
        var slotStudent =
            await _unitOfWorkRepository.SlotStudentRepository.FirstOrDefaultAsync(st =>
                st.SlotId == slotId && st.UserId == studentId);
        return slotStudent.Adapt<SlotStudentDto>();
    }

    public async Task<IEnumerable<SlotStudentDto>> GetSlotStudentsOfSlotAsync(int slotId)
    {
        var slotStudents = await _unitOfWorkRepository.SlotStudentRepository
            .Where(st => st.SlotId == slotId)
            .ToListAsync();
        return slotStudents.Adapt<IEnumerable<SlotStudentDto>>();
    }
    public async Task<bool> SlotStudentPaidAsync(int slotId, int studentId)
    {
        var slotStudent =
            await _unitOfWorkRepository.SlotStudentRepository.FirstOrDefaultAsync(st =>
                st.SlotId == slotId && st.UserId == studentId);
        if (slotStudent.PaymentStatus == PaymentStatus.Paid)
        {
            throw new Exception($"this course has already paid by studentId {studentId}");
        }

        slotStudent.PaymentStatus = PaymentStatus.Paid;
        _unitOfWorkRepository.SlotStudentRepository.Update(slotStudent);
        await _unitOfWorkRepository.SaveChangesAsync();
        return true;
    }

    public async Task<Models.Models.SlotStudent> CreateSlotStudentIfNotExist(int slotId, int studentId)
    {
        var recordInDb = await _unitOfWorkRepository.SlotStudentRepository.FirstOrDefaultAsync(st =>
            st.SlotId == slotId && st.UserId == studentId);
        if (recordInDb == null)
        {
            recordInDb = new Models.Models.SlotStudent()
            {
                SlotId = slotId,
                UserId = studentId,
                PaymentStatus = PaymentStatus.Notpaid,
            };
            await _unitOfWorkRepository.SlotStudentRepository.AddAsync(recordInDb);
            await _unitOfWorkRepository.SaveChangesAsync();
        }

        return recordInDb;
    }

    public async Task<SlotStudentDto> GetSlotStudentById(int slotId)
    {
        var recordInDb = await _unitOfWorkRepository.SlotStudentRepository.FirstOrDefaultAsync(u => u.SlotId == slotId);
        return recordInDb.Adapt<SlotStudentDto>();
    }

    public async Task<List<GetStudentSlotDto>> GetListSLotStudentByStatus(PaymentStatus status)
    {
        var slotStudentModel = await _unitOfWorkRepository.SlotStudentRepository.Where(ss => ss.PaymentStatus == status).ToListAsync();
        return slotStudentModel.Adapt<List<GetStudentSlotDto>>();
    }

    public async Task<bool> SoftDeleteSlotStudent(int slotId, int studentId)
    {
        var slotstudent = await _unitOfWorkRepository.SlotStudentRepository.FirstOrDefaultAsync(sc => sc.SlotId == slotId && sc.UserId == studentId);
        if (slotstudent == null)
        {
            throw new Exception("Slot Student not found");
        }
       
        // studentClass.RecordStatus = RecordStatus.Deleted;
        // _unitOfWorkRepository.SlotStudentRepository.Update(studentClass);
        _unitOfWorkRepository.SlotStudentRepository.Remove(slotstudent);
        await _unitOfWorkRepository.SaveChangesAsync();
        return true;
    }

    public Task<bool> UpdateSlotStudentAsync(int slotId, int studentId, double rate, string feedback)
    {
        throw new NotImplementedException();
    }

    public async Task<List<SlotStudentDto>> GetListSlotStudentByStudentId(int studentId)
    {
        var slotStudentModel = await _unitOfWorkRepository.SlotStudentRepository.Where(ld => ld.UserId == studentId).ToListAsync();
        return slotStudentModel.Adapt<List<SlotStudentDto>>();
    }

    public async Task<bool> CreateSlotStudent(int slotId, int studentId)
    {
        var newSlotStudentModel = new Models.Models.SlotStudent()
        {
            UserId = studentId,
            SlotId = slotId,
            CreatedDate = DateTime.Now,
            PaymentStatus = PaymentStatus.Notpaid,
        };
        await _unitOfWorkRepository.SlotStudentRepository.AddAsync(newSlotStudentModel);
        await _unitOfWorkRepository.SaveChangesAsync();
        return true;
    }
    public async Task<bool> UpdateSlotStudentAsync(int slotId, int studentId, decimal rate, string feedback)
    {
        var slotStudent = await _unitOfWorkRepository.SlotStudentRepository.FirstOrDefaultAsync(st =>
            st.SlotId == slotId && st.UserId == studentId);

        if (slotStudent == null)
        {
            throw new NotFoundException("Slot Student not found");
        }

        slotStudent.Rating = rate;
        slotStudent.Feedback = feedback;

        var slot = await _unitOfWorkRepository.SlotRepository.FirstOrDefaultAsync(sl => sl.Id == slotId);

        await _userServices.RecalculateTutorRating(slot.CreateById);

        _unitOfWorkRepository.SlotStudentRepository.Update(slotStudent);
        await _unitOfWorkRepository.SaveChangesAsync();
        return true;
    }
}