using UnityEngine;

namespace MonoBehaviors.Common
{
    public class LogUtility
    {
        public static void Log(string s)
        {
            Debug.Log("[DebugInfo] : " +s);
        }
        public static void ContentDeliveryLog(string s)
        {
            Debug.Log("[ContentDelivery] : " + s);
        }
    }
}