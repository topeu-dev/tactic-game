using System;
using Actions;

namespace TaskApproachTest
{
    public interface TaskExecutor
    {
        void ExecuteTask(ActionInstance actionInstance, ActionContext actionContext, Action callback);
    }
}