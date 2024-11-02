using UnityEngine;

public class PlayerControllerPtagus : MonoBehaviour
{
  GameObject activePlayer;
  bool spell;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      SpellSelected(false);
    }
  }   
  
  public void SetActivePlayer (GameObject go)
  {
    Debug.Log("sde");
    if (activePlayer is not null)
    {
      //go to ...
      if (!spell)
      {
        activePlayer = go;
      }
      else
      {
        if (go.tag == "Enemy")
        {
          Debug.Log("hit" + go.name + "player" + activePlayer.name);
        }
        else
        {
          Debug.Log("hint" + go.name);
        }
      }
    }
    if (activePlayer is null)
    {
      activePlayer = go;
      Debug.Log("active" + go.name);

    }
  }
  public void DisableActivePlayer()
  {
    activePlayer = null;
  }
  public void SpellSelected(bool b)
  {
    spell = b;
  }

}
