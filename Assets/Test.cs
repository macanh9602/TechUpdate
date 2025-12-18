using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AntiStress.MiniGame
{
        public class CheckUnityPath
        {
                [MenuItem("MCP/Check PATH")]
                public static void ShowPath()
                {
                        var path = System.Environment.GetEnvironmentVariable("PATH");
                        UnityEngine.Debug.Log("UNITY PATH = " + path);
                }

                [MenuItem("MCP/Spawn Red Sphere")]
                public static void SpawnRedSphere()
                {
                        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        sphere.transform.position = Vector3.zero;
                        sphere.transform.localScale = new Vector3(5f, 5f, 5f);
                        sphere.name = "Large Red Sphere";

                        Renderer renderer = sphere.GetComponent<Renderer>();
                        Material material = new Material(Shader.Find("Standard"));
                        material.color = Color.red;
                        renderer.material = material;

                        UnityEngine.Debug.Log("Spawned large red sphere at world origin (0, 0, 0)");
                }
        }
}
