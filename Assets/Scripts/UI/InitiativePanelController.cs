using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Turn;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
  public class InitiativePanelController : MonoBehaviour
  {
    public TurnManager _turnManager;

    public List<GameObject> localRoundQueue;
    public List<Image> portraits;
    public List<Sprite> portraitSprite;
    public List<GameObject> portraitByGameObject;
    public Sprite endTurn;
    int countPortraits;

    private void OnEnable()
    {
      EventManager.TurnEvent.OnNextTurnEvent += HandleNextTurn;
      EventManager.TurnEvent.OnNextTurnSequenceEvent += InitPanel;
      EventManager.DamageRelatedEvent.OnDeath += RemoveFromPanel;
    }

    private void RemoveFromPanel(Component source, GameObject whoKilled)
    {
      // play dead anim for specific Image(whoKilled)
      StartCoroutine("playDeadAnim");

      // play rerender
    }

    private void OnDisable()
    {
      EventManager.TurnEvent.OnNextTurnEvent -= HandleNextTurn;
      EventManager.TurnEvent.OnNextTurnSequenceEvent -= InitPanel;
      EventManager.DamageRelatedEvent.OnDeath -= RemoveFromPanel;
    }

    private void InitPanel(Component source, LinkedList<GameObject> roundQueueCopy)
    {
      Debug.LogWarning("InitPanelHere");
      localRoundQueue = new List<GameObject>(roundQueueCopy);
      int count = localRoundQueue.Count;
      localRoundQueue.Add(gameObject);
      for (int i = 0; i < count; i++)
      {
        localRoundQueue.Add(localRoundQueue[i]);
        roundQueueCopy.RemoveFirst();
      }
      localRoundQueue.Add(gameObject);
      
      // rerender panel
    }

    private void HandleNextTurn(Component source, GameObject nextChar)
    {
      // play anim
      portraitByGameObject.Clear();
      StartCoroutine(playRenenderPanelAnim());
    }

    public void ShowByPortrait(int i)
    {
      if (portraitByGameObject[i].tag == "UI")
      {
        return;
      }
      EventManager.CameraEvent.OnPlayableCharacterFocusEvent?.Invoke(this, portraitByGameObject[i]);
    }

    private IEnumerator playRenenderPanelAnim()
    {
      Debug.LogWarning("NewTurn");
      float duration = 1f; // Длительность анимации
      float elapsedTime = 0f;

      // Сохраняем текущие цвета каждого портрета
      var initialColors = new List<Color>();
      foreach (var portrait in portraits)
      {
        initialColors.Add(portrait.color);
      }

      while (elapsedTime < duration)
      {
        // Вычисляем текущий альфа-канал в зависимости от времени
        float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);

        // Применяем значение альфа-канала к каждому портрету
        for (int i = 0; i < portraits.Count; i++)
        {
          var initialColor = initialColors[i];
          portraits[i].color = new Color(
              initialColor.r,
              initialColor.g,
              initialColor.b,
              alpha);
        }

        // Ждём до следующего кадра
        yield return null;

        // Увеличиваем время
        elapsedTime += Time.deltaTime;
      }
      foreach (var o in localRoundQueue)
      {
        portraits[countPortraits].gameObject.SetActive(true);
        countPortraits++;         
        var spriteToRender = o.GetComponent<SpriteRenderer>().sprite;
        portraitByGameObject.Add(o);
        portraitSprite.Add(spriteToRender); 
        if (countPortraits == portraits.Count)
        {
          break;
        }

      }
      if (countPortraits < portraits.Count)
      {
        for (int i = countPortraits; i < portraits.Count; i++)
        {
          portraits[i].gameObject.SetActive(false);
        }
      }
      countPortraits = 0;
      for (int i = 0; i < portraits.Count; i++)
      {
        if (i >= portraitSprite.Count)
          break;
        portraits[i].sprite = portraitSprite[i];
      }
      portraitSprite.Clear();
      localRoundQueue.RemoveAt(0);
      float animBackDuration = 1f; // Длительность анимации
      float animBackElapsedTime = 0f;
      while (animBackElapsedTime < animBackDuration)
      {
        float alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);

        // Применяем значение альфа-канала к каждому портрету
        for (int i = 0; i < portraits.Count; i++)
        {
          var initialColor = initialColors[i];
          portraits[i].color = new Color(
              initialColor.r,
              initialColor.g,
              initialColor.b,
              alpha);
        }

        // Ждём до следующего кадра
        yield return null;

        // Увеличиваем время
        animBackElapsedTime += Time.deltaTime;
      }


      // Убедимся, что альфа-канал точно равен конечному значению после цикла
      // foreach (var portrait in portraits)
      // {
      //     var color = portrait.color;
      //     portrait.color = new Color(color.r, color.g, color.b, 0f);
      // }
    }


    private IEnumerator playDeadAnim()
    {
      Debug.LogWarning("Dead");
      yield return null;
    }
  }
}