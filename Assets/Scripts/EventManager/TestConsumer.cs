using System;
using UnityEngine;

public class TestConsumer : MonoBehaviour
{
    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        EventManager.SelectableObject.OnObjectSelectedEvent += Consume;
    }

    private void Consume(Component arg0, GameObject arg1)
    {
        throw new NotImplementedException();
    }

}