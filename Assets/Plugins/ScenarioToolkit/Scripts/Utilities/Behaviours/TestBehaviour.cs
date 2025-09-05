using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Scenario.Utilities
{
    public class TestBehaviour : MonoBehaviour
    {
        public void MonoTest()
        {
            Debug.Log(nameof(MonoTest));
        }
        
        public static void StaticTest()
        {
            Debug.Log(nameof(StaticTest));
        }
    }
}