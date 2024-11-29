using System;
using System.Collections;
using System.Collections.Generic;
using Actions;

namespace Enemy
{
    public interface BotController
    {
        IEnumerator MakeMove(List<ActionInstance> actions, Action onFinish);
    }
}