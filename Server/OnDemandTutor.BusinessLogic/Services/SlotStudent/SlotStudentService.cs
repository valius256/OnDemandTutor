using Mapster;
using Microsoft.EntityFrameworkCore;
using OnDemandTutor.BusinessLogic.Interfaces.SlotStudent;
using OnDemandTutor.BusinessLogic.Services.Slot;
using OnDemandTutor.DataAccess;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Dtos.Slot;
using OnDemandTutor.Models.Dtos.SlotStudent;
using OnDemandTutor.Models.Dtos.User;
using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.BusinessLogic.Services.SlotStudent;

public class SlotStudentService : ISlotStudentServices
{
    private readonly IUnitOfWorkRepository _unitOfWorkRepository;


    public SlotStudentService(IUnitOfWorkRepository unitOfWorkRepository)
    {
        _unitOfWorkRepository = unitOfWorkRepository;
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

    public async Task CreateSlotStudentIfNotExist(int slotId, int studentId)
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
        var studentClass = await _unitOfWorkRepository.SlotStudentRepository.FirstOrDefaultAsync(sc => sc.SlotId == slotId && sc.UserId == studentId);
        if (studentClass == null)
        {
            throw new Exception("Slot Student not found");
        }
        // _unitOfWork.StudentClassRepository.Remove(studentClass);
        studentClass.RecordStatus = RecordStatus.Deleted;
        _unitOfWorkRepository.SlotStudentRepository.Update(studentClass);
        await _unitOfWorkRepository.SaveChangesAsync();
        return true;
    }
}