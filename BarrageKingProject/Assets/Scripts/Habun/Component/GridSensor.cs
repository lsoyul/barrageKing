using Adohi;
using SGoap;
using UnityEngine;

namespace Habun
{
    public class GridSensor : MonoBehaviour, IDataBind<AgentBasicData>
    {
        private AgentBasicData _agentData;

        [Effect]
        public State State;

        private void OnEnable()
        {
            ViewPointManager.Instance.OnViewChangedEndTo2D += OnViewChangedEndTo2D;
            ViewPointManager.Instance.OnViewChangedEndTo3D += OnViewChangedEndTo3D;
        }

        private void OnDisable()
        {
            ViewPointManager.Instance.OnViewChangedEndTo2D -= OnViewChangedEndTo2D;
            ViewPointManager.Instance.OnViewChangedEndTo3D -= OnViewChangedEndTo3D;
        }

        private void OnViewChangedEndTo2D()
        {
            _agentData.Agent.States.SetState(State.Key, 1);
            _agentData.Agent.AbortPlan();
        }

        private void OnViewChangedEndTo3D()
        {
            _agentData.Agent.States.RemoveState(State.Key);
            _agentData.Agent.AbortPlan();
        }

        public void Bind(AgentBasicData data)
        {
            _agentData = data;
            _agentData.Agent.States.SetState(State.Key, 1);
        }

    }
}
