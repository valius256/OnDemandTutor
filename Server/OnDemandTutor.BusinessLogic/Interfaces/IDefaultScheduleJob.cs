using System.Linq.Expressions;

namespace OnDemandTutor.BusinessLogic.Interfaces;

public interface IDefaultScheduleJob
{
    string Enqueue<T>(Expression<Action<T>> methodCall);
}