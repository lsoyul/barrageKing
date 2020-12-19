using SGoap;
using UnityEngine;

namespace Habun
{
    public class GridSensor : MonoBehaviour, IDataBind<AgentBasicData>
    {
        private AgentBasicData _agentData;

        [Effect]
        public State State;

        private void Update()
        {
            if (Camera.main.orthographic)
                _agentData.Agent.States.SetState(State.Key, 1);
            else
                _agentData.Agent.States.RemoveState(State.Key);
        }

        public void Bind(AgentBasicData data)
        {
            _agentData = data;
        }

    }
}
