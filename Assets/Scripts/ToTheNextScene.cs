using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ToTheNextScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  public void NewSceneMode(int i)
  {
    SceneManager.LoadSceneAsync(i);
  }
}
