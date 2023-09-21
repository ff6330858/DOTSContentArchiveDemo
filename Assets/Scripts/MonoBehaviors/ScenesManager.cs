using System;
using Components;
using Systems;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    private ScenesManagementSystem scenesManagementSystem;
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        scenesManagementSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged(typeof(ScenesManagementSystem)) as ScenesManagementSystem;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(scene.name + " Loaded");
    }
    void OnSceneUnloaded(Scene scene)
    {
        Debug.Log(scene.name + " UnLoaded");
    }

    public void MarkReLoadScene()
    {
        scenesManagementSystem.MarkReLoadScene();
    }
    public void MarkUnloadScene(string sceneName)
    {
        scenesManagementSystem.MarkUnloadScene(sceneName);
    }

    public void MarkSwitchScene()
    {
        scenesManagementSystem.SwitchScene();
    }

    public void UnloadScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }
}
