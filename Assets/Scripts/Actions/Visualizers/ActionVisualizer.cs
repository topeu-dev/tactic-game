using UnityEngine;

namespace Actions.Visualizers
{
    public interface ActionVisualizer
    {
        public void EnableVisualizerFor(GameObject activeChar);

        public void DisableVisualizer();
    }
}