using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  public void gm(GameObject go)
  {
    Debug.Log(go.name);
  }
}
