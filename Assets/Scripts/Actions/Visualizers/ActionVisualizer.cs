using UnityEngine;

namespace Actions.Visualizers
{
    public interface ActionVisualizer
    {
        public void EnableVisualizerFor(GameObject activeChar, ActionInstance actionInstanceParam);

        public void DisableVisualizer();
    }
}