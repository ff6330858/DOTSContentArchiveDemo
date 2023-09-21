using Components;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Systems
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct LoadSceneFromWeakObjectSceneReferenceSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<AdditiveSceneComponentData>();
        }
        
        public void OnUpdate(ref SystemState state)
        {
            var loadScenesData = SystemAPI.GetSingletonBuffer<AdditiveSceneComponentData>();
            for (int i = 0; i < loadScenesData.Length; i++)
            {
                var sceneData = loadScenesData[i];
                if (!sceneData.sceneWeakRef.IsReferenceValid)
                    continue;

                if (!sceneData.scene.IsValid() &&!sceneData.scene.isLoaded && !sceneData.startedLoad)
                {
                    Scene scene = sceneData.sceneWeakRef.LoadAsync(new Unity.Loading.ContentSceneParameters()
                    {
                        loadSceneMode = UnityEngine.SceneManagement.LoadSceneMode.Additive
                    });
                    sceneData.scene = scene;
                    sceneData.startedLoad = true;
                    loadScenesData[i] = sceneData;
                }
                else if (sceneData.scene.IsValid() && sceneData.scene.isLoaded && sceneData.needUnload)
                {
                    string sceneName = sceneData.scene.name;
                    sceneData.sceneWeakRef.Unload(ref sceneData.scene);
                    sceneData.scene = default;
                    sceneData.needUnload = false;
                    loadScenesData[i] = sceneData;
                }
            }
        }
    }
}