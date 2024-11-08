using System.Collections.Generic;
using Actions;
using Effects;
using UnityEngine;

public class GenericChar : MonoBehaviour
{
    public List<ActionSo> actions = new();
    public List<Effect> currentEffects = new();
}