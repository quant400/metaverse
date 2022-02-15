
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UniRx;
using UniRx.Triggers;
using UniRx.Toolkit;
[Serializable]
public struct sceneAdditiveModel
{
    [Serializable]
    public enum sceneState
    {
        unloaded,
        loading,
        loaded,
    }
    [Serializable]

    public class sceneAdditiveClass
    {
        public string sceneName;
        public Scene sceneRelated;
        public List<Scene> visibleScene = new List<Scene>();
        public sceneState currentState;
        public ReactiveProperty<bool> isLoaded = new ReactiveProperty<bool>();
        public Vector2 gridValue;
        public Vector3 centerPosition;
        public int matrixIndex;
        public Vector2 matrixVectorIndex;
        public int maxIndexFromPlayerToUnload;

    }

    public static List<Scene> loadedScenes = new List<Scene>();
    public static List<sceneAdditiveClass> lodedScenesData = new List<sceneAdditiveClass>();
    public static ReactiveProperty<Vector2> playerGridPostion = new ReactiveProperty<Vector2>();
    public static int gridScaleFActor = 100;
}
