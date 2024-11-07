using System;
using Actions;

namespace TaskApproachTest
{
    public interface TaskExecutor
    {
        void ExecuteTask(ActionSo actionSo, ActionContext actionContext, Action callback);
    }
}