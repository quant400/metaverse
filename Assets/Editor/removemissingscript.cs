using UnityEditor;
using UnityEngine;

public class Example
{
    [MenuItem("Example / Log Missing Script Count")]
    private static void LogMissingScriptCount()
    {
        var gameObject = Selection.activeGameObject;
        if (gameObject == null)
        {
            return;
        }

        var missingCount = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(gameObject);
        Debug.Log(missingCount);
    }

    [MenuItem("Example / Remove Missing Scripts")]
    private static void Clean()
    {
        var objectMain = Selection.gameObjects;

        if (Selection.gameObjects != null)
        {
            if (Selection.gameObjects.Length > 1)
            {
                for(int i = 0; i < objectMain.Length; i++)
                {
                    GameObjectUtility.RemoveMonoBehavioursWithMissingScript(objectMain[i].gameObject);
                    for (int j = 0; j < objectMain[i].transform.childCount; j++)
                    {
                        cleaningOBject(objectMain[i].transform.GetChild(j));
                    }
                }
                
            }
            else
            {
                var objectone = Selection.activeGameObject;
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(objectone.gameObject);
                for (int i = 0; i < objectone.transform.childCount; i++)
                {
                    cleaningOBject(objectone.transform.GetChild(i));
                }
            }
          
        }
       

    }
    public static void cleaningOBject(Transform obj)
    {
         GameObjectUtility.RemoveMonoBehavioursWithMissingScript(obj.gameObject);
            
    }
}