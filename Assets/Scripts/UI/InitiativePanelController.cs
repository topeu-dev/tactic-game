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

        public LinkedList<GameObject> localRoundQueue;
        public List<Image> portraits;


        private void OnEnable()
        {
            EventManager.TurnEvent.OnNextTurnEvent += HandleNextTurn;
            EventManager.TurnEvent.OnBattleStartEvent += InitPanel;
            EventManager.TurnEvent.OnRoundEndedEvent += InitPanel;
            EventManager.DamageRelatedEvent.OnDeath += RemoveFromPanel;
        }

        private void RemoveFromPanel(Component source, GameObject whoKilled)
        {
            // play dead anim for specific Image(whoKilled)
            StartCoroutine(
                playDeadAnim(() => { StartCoroutine(playRenenderPanelAnim()); })
            );
            //


            // play rerender
        }

        private void OnDisable()
        {
            EventManager.TurnEvent.OnNextTurnEvent -= HandleNextTurn;
            EventManager.TurnEvent.OnBattleStartEvent -= InitPanel;
            EventManager.TurnEvent.OnRoundEndedEvent -= InitPanel;
        }


        private void InitPanel(Component arg0)
        {
            localRoundQueue = _turnManager.GetRoundQueue();
            foreach (var o in localRoundQueue)
            {
                var spriteToRender = o.GetComponent<SpriteRenderer>().sprite;
            }
            // rerender panel
        }

        private void HandleNextTurn(Component source, GameObject nextChar)
        {
            // play anim
            StartCoroutine(playRenenderPanelAnim());
        }


        private IEnumerator playRenenderPanelAnim()
        {
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


        private IEnumerator playDeadAnim(Action onComplete)
        {

            onComplete.Invoke();
            return null;
        }
    }
}