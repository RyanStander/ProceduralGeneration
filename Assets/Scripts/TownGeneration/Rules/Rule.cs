using UnityEngine;
using Random = System.Random;

namespace TownGeneration.Rules
{
    [CreateAssetMenu(menuName = "ScriptableObject/TownGeneration/Rule")]
    public class Rule : ScriptableObject
    {
        public string letter;
        [SerializeField] private string[] results;
        [SerializeField] private bool randomResult;

        public string GetResult()
        {
            if (!randomResult) return results[0];
            
            var randomValues = new Random();
            var randomIndex = randomValues.Next(0, results.Length);
            return results[randomIndex];
        }
    }
}
