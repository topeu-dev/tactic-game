using UnityEngine;

namespace Actions
{
    public class ActionContext
    {
        public readonly GameObject CurrentActiveChar;

        public readonly GameObject ClickedObject;

        public readonly Vector3? BattleFieldClickedPos;

        public ActionContext(GameObject currentActiveChar, GameObject clickedObject, Vector3? battleFieldClickedPos)
        {
            CurrentActiveChar = currentActiveChar;
            ClickedObject = clickedObject;
            BattleFieldClickedPos = battleFieldClickedPos;
        }
    }
}