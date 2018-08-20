using System;
using System.Linq.Expressions;

namespace Migrators.Hangfire
{
    public interface IHangfireUtility
    {
       // void Enqueue(Func<Task> func);
        void Enqueue(Expression<Action> methodCall);
    }

}