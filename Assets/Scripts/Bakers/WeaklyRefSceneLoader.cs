using System.Collections.Generic;
using Components;
using Unity.Entities;
using Unity.Entities.Content;
using UnityEngine;

namespace Bakers
{
    public class WeaklyRefSceneLoader : MonoBehaviour
    {
        public List<WeakObjectSceneReference> loadScenesRef;
        public WeakObjectSceneReference switchSceneRef;

        class WeaklyRefSceneLoaderBaker : Baker<WeaklyRefSceneLoader>
        {
            public override void Bake(WeaklyRefSceneLoader authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                DynamicBuffer<AdditiveSceneComponentData>
                    loadScenesData = AddBuffer<AdditiveSceneComponentData>(entity);
                loadScenesData.Length = authoring.loadScenesRef.Count;
                for (int i = 0; i < authoring.loadScenesRef.Count; i++)
                {
                    loadScenesData[i] = new AdditiveSceneComponentData
                    {
                        startedLoad = false, needUnload = false, sceneWeakRef = authoring.loadScenesRef[i],
                        scene = default
                    };
                }

                AddComponent(entity, new SwitchSceneComponentData { sceneWeakRef = authoring.switchSceneRef });
            }
        }
    }
}
