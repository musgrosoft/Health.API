using System;
using System.Threading.Tasks;

namespace Migrators
{
    public interface IHangfireUtility
    {
        void Enqueue(Func<Task> func);
    }

}