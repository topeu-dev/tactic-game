using UnityEngine;

public class PlayerControllerPtagus : MonoBehaviour
{
  GameObject activePlayer;
  int spell;
  
  //TODO: remove!!!
  private static int spellUnselected = -100; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      SpellSelected(spellUnselected);
    }
  }   
  
  public void SetActivePlayer (GameObject go)
  {
    Debug.Log("SetActivePlayer invoked" +go.name);
    if (activePlayer is not null)
    {
      //go to ...
      if (spell == spellUnselected)
      {
        activePlayer = go;
      }
      else
      {
        // <- castSpell
        castSpell(go);
      }
    }
    if (activePlayer is null && go.tag != "Cell")
    {
      activePlayer = go;
      Debug.Log("active" + go.name);

    }
  }

  private void castSpell(GameObject go)
  {
    // Move!
    if (spell == 0)
    {
      if (go.tag == "Cell")
      {
        var genericChar = activePlayer.GetComponent<GenericChar>();
        genericChar.moveToCell(go.GetComponent<Cell>());
        Debug.Log("Go to cell:" + go.name + "player: " + activePlayer.name);
      }
      else
      {
        Debug.Log("Can't go!");
      }
      
    }

    //hit
    if (spell == 1)
    {
      
      if (go.tag == "Enemy")
      {
        Debug.Log("hit" + go.name + "player" + activePlayer.name);
      }
      else
      {
        Debug.Log("Can't hit!!");
      }
      
    }
  }

  public void DisableActivePlayer()
  {
    activePlayer = null;
  }
  
  public void SpellSelected(int spellID)
  {
    spell = spellID;
    Debug.Log("Spell selected: " + spell);
  }

}
