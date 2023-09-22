using Components;
using Unity.Entities;
using UnityEngine;

namespace Systems
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(LoadSceneFromWeakObjectSceneReferenceSystem))]
    public partial class ScenesManagementSystem : SystemBase
    {
        protected override void OnUpdate()
        {
        }

        public void SwitchScene()
        {
            var switchScenesData = SystemAPI.GetSingletonRW<SwitchSceneComponentData>();
            switchScenesData.ValueRW.sceneWeakRef.LoadAsync(new Unity.Loading.ContentSceneParameters()
            {
                loadSceneMode = UnityEngine.SceneManagement.LoadSceneMode.Single
            });
            
        }
        public void MarkReloadScene()
        {
            var loadScenesData = SystemAPI.GetSingletonBuffer<AdditiveSceneComponentData>();
            for (int i = 0; i < loadScenesData.Length; i++)
            {
                var sceneData = loadScenesData[i];
                if(!sceneData.scene.IsValid() && !sceneData.scene.isLoaded)
                    sceneData.startedLoad = false;
                loadScenesData[i] = sceneData;
            }
        }

        public void MarkUnloadScene(string sceneName)
        {
            var loadScenesData = SystemAPI.GetSingletonBuffer<AdditiveSceneComponentData>();
            for (int i = 0; i < loadScenesData.Length; i++)
            {
                var sceneData = loadScenesData[i];
                if(sceneData.scene.name == sceneName)
                    sceneData.needUnload = true;
                loadScenesData[i] = sceneData;
            }
        }
    }
}