using UnityEngine;

public class TestInvoker : MonoBehaviour
{
    public void ggwp()
    {
        EventManager.SelectableObject.OnObjectSelectedEvent.Invoke(this, null);
    }
}