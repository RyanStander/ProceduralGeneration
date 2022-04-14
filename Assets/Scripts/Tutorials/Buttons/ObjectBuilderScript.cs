using UnityEngine;

namespace Tutorials.Buttons
{
    public class ObjectBuilderScript : MonoBehaviour 
    {
        public GameObject obj;
        public Vector3 spawnPoint;


        public void BuildObject()
        {
            Instantiate(obj, spawnPoint, Quaternion.identity);
        }
    }
}
