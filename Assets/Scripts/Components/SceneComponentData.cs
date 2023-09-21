using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Content;
using UnityEngine.SceneManagement;

namespace Components
{
    [InternalBufferCapacity(8)]
    public struct AdditiveSceneComponentData : IBufferElementData
    {
        public bool startedLoad;
        public bool needUnload;
        public WeakObjectSceneReference sceneWeakRef;
        public Scene scene;
    }

    public struct SwitchSceneComponentData : IComponentData
    {
        public WeakObjectSceneReference sceneWeakRef;
    }
}