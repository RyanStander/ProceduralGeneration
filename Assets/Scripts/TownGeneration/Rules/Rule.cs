using UnityEngine;

namespace TownGeneration.Rules
{
    [CreateAssetMenu(menuName = "ScriptableObject/TownGeneration/Rule")]
    public class Rule : ScriptableObject
    {
        public string letter;
        [SerializeField] private string[] results=null;

        public string GetResult()
        {
            return results[0];
        }
    }
}
