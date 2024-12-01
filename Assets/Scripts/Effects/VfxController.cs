using System.Collections.Generic;
using UnityEngine;

namespace Effects
{
    public class VfxController : MonoBehaviour
    {
        public GameObject bleeding;
        public GameObject poisoned;
        public GameObject healing;
        public GameObject fired;
        public GameObject stun;
        public GameObject slow;
        public List<GameObject> rageObjects;

        public void EnableVfxFor(EffectInstance effect)
        {
            switch (effect.effectDescription.effectName)
            {
                case "Bleeding":
                {
                    Debug.Log("BleedingEffect ENABLED???");
                    bleeding.SetActive(true);
                    break;
                }

                case "Stun":
                {
                    stun.SetActive(true);
                    break;
                }

                case "Rage":
                {
                    rageObjects.ForEach(rageItem => rageItem.SetActive(true));
                    break;
                }

                case "Slow":
                    slow.SetActive(true);
                    break;

                default:
                {
                    Debug.LogWarning("Can't find vfx for " + effect.effectDescription.effectName);
                    break;
                }
            }
        }

        public void DisableVfxFor(EffectInstance effect)
        {
            switch (effect.effectDescription.effectName)
            {
                case "Bleeding":
                {
                    bleeding.SetActive(false);
                    break;
                }

                case "Stun":
                {
                    stun.SetActive(false);
                    break;
                }

                case "Rage":
                {
                    rageObjects.ForEach(rageItem => rageItem.SetActive(false));
                    break;
                }

                case "Slow":
                    slow.SetActive(false);
                    break;

                default:
                {
                    Debug.LogWarning("Can't find vfx for " + effect.effectDescription.effectName);
                    break;
                }
            }
        }
    }
}