using UnityEngine;

public interface Char
{
  void OnTurnEnd();

  void CheckEffects();

  void UpdateInitHUD();

}

public class CharStats
{
  public int hp;
  public int speed;
  public int init;
  public int mp;

}
