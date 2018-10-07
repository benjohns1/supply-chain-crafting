using UnityEngine;

namespace SupplyChain
{
    public class Settings : MonoBehaviour
    {
        [Tooltip("Minimum time in seconds between simulation steps")]
        public float simulationStep = 0.1f;
    }
}