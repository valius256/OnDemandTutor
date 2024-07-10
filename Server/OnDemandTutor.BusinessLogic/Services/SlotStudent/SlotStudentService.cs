using Mapster;
using OnDemandTutor.BusinessLogic.Interfaces.SlotStudent;
using OnDemandTutor.DataAccess;
using OnDemandTutor.Models.Dtos.SlotStudent;
using OnDemandTutor.Models.Enum;

namespace OnDemandTutor.BusinessLogic.Services.SlotStudent;

public class SlotStudentService : ISlotStudentServices
{
    private readonly IUnitOfWorkRepository _unitOfWorkRepository;


    public SlotStudentService(IUnitOfWorkRepository unitOfWorkRepository)
    {
        _unitOfWorkRepository = unitOfWorkRepository;
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
        var recordInDb =  await _unitOfWorkRepository.SlotStudentRepository.FirstOrDefaultAsync(st =>
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
            await  _unitOfWorkRepository.SaveChangesAsync();
        }
        
    }
}