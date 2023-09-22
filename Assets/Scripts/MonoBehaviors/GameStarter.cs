using MonoBehaviors.Common;
using Unity.Entities.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MonoBehaviors
{
    public class GameStarter : MonoBehaviour
    {
        public string remoteUrlRoot;
        public string initialContentSet;
        private void Start()
        {
            
#if ENABLE_CONTENT_DELIVERY
            ContentDeliveryGlobalState.LogFunc = LogUtility.ContentDeliveryLog;
            LogUtility.ContentDeliveryLog(Application.persistentDataPath);
            //如果remoteUrlRoot为空，则从本地加载
            ContentDeliveryGlobalState.Initialize(remoteUrlRoot, Application.persistentDataPath + "/content-cache",
                initialContentSet, s =>
                {
                    if (s >= ContentDeliveryGlobalState.ContentUpdateState.ContentReady)
                    {
                        LogUtility.ContentDeliveryLog("CurrentDeliveryGlobalState: " + ContentDeliveryGlobalState.CurrentContentUpdateState);
                        SwitchToMainScene();
                    }
                });
#else
            SwitchToMainScene();
#endif
        }
        void SwitchToMainScene()
        {
            LogUtility.Log("SwitchToMainScene");
            SceneManager.LoadScene("WeaklyRefScene", LoadSceneMode.Single);
        }
    }
}