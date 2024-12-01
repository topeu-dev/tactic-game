using System.Collections;
using System.Collections.Generic;
using Effects;
using Enemy;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBarFollow : MonoBehaviour
    {
        private Canvas healthBar;
        private Camera mainCamera;

        private TextMeshProUGUI healthText;

        [SerializeField]
        private List<Image> effectIcons = new();

        private Slider _slider;
        private GameObject _parentRef;
        private List<EffectInstance> _currentEffectsRef;
        private Vector3 initialScale;
        public float baseOrthographicSize = 10f;

        private bool _tempShow = false;

        private void Awake()
        {
            _parentRef = transform.parent.gameObject;
            healthBar = GetComponent<Canvas>();
            healthText = GetComponentInChildren<TextMeshProUGUI>();
            effectIcons.ForEach(it => it.enabled = false);
            getEffectsRef();
            _slider = GetComponentInChildren<Slider>();
            _slider.value = 1f;

            var initialMaxHp = getInitialMaxHp();
            healthText.text = initialMaxHp + "/" + initialMaxHp;

            healthBar.enabled = false;
        }

        private void getEffectsRef()
        {
            var genericChar = _parentRef.GetComponent<GenericChar>();
            if (genericChar != null)
            {
                _currentEffectsRef = genericChar.effectsInstances;
            }

            // var genericEnemy = _parentRef.GetComponent<GenericEnemy>();
            // if (genericEnemy)
            // {
            //     return genericEnemy.startHp;
            // }
        }

        //todo refactor
        private int getInitialMaxHp()
        {
            var genericChar = _parentRef.GetComponent<GenericChar>();
            if (genericChar != null)
            {
                return genericChar.startHp;
            }

            var genericEnemy = _parentRef.GetComponent<GenericEnemy>();
            if (genericEnemy)
            {
                return genericEnemy.startHp;
            }

            Debug.LogError("Fail to get initial max hp for health bar, no generic char or generic enemy found");
            return -1;
        }

        private void OnEnable()
        {
            EventManager.DamageRelatedEvent.UpdateHealthBar += UpdateHealthBar;
        }

        private void OnDisable()
        {
            EventManager.DamageRelatedEvent.UpdateHealthBar -= UpdateHealthBar;
        }


        //refactor
        private void UpdateHealthBar(Component source, GameObject character, int currentHp, int maxHp)
        {
            Debug.Log(character.name + " has " + currentHp + "/" + maxHp);

            if (character != _parentRef)
                return;

            StartCoroutine(showFor2Seconds());
            StartCoroutine(DelayedDamage(currentHp, maxHp));
        }

        private IEnumerator DelayedDamage(int currentHp, int maxHp)
        {
            yield return new WaitForSeconds(0.5f);

            healthText.text = currentHp + "/" + maxHp;
            // replace with animation Mathf.Lerp
            _slider.value = (float)currentHp / maxHp;
        }

        private IEnumerator showFor2Seconds()
        {
            float duration = 2f;
            float elapsedTime = 0f;
            _tempShow = true;
            while (elapsedTime < duration)
            {
                healthBar.enabled = true;

                // Wait for the next frame
                yield return null;

                // Increment elapsed time
                elapsedTime += Time.deltaTime;
            }

            _tempShow = false;
            healthBar.enabled = false;
        }

        void Start()
        {
            mainCamera = Camera.main;
            initialScale = transform.localScale;
        }

        void LateUpdate()
        {
            transform.forward = mainCamera.transform.forward;
        }

        private void Update()
        {
            transform.localScale = initialScale * mainCamera.orthographicSize / baseOrthographicSize;
        }

        public void ToggleHealthBar()
        {
            if (_tempShow)
                return;
            
            updateCurrentEffectsState();
            healthBar.enabled = !healthBar.enabled;
        }

        private void updateCurrentEffectsState()
        {
            if (_currentEffectsRef == null)
                return;
            
            for (var i = 0; i < 5; i++)
            {
                if (i < _currentEffectsRef.Count && _currentEffectsRef[i] != null)
                {
                    effectIcons[i].sprite = _currentEffectsRef[i].effectDescription.icon;
                    effectIcons[i].enabled = true;
                }
                else
                {
                    effectIcons[i].enabled = false;
                }
            }
        }
    }
}