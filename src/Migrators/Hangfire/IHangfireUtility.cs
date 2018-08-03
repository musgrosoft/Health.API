using System;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Migrators
{
    public interface IHangfireUtility
    {
       // void Enqueue(Func<Task> func);
        void Enqueue(Expression<Action> methodCall);
    }

}