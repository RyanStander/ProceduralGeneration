using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the starting point for a custom random generator component
///  (seeded pseudorandom).
///  
/// Note that the Shape and BuildTrigger classes call the three methods below.
/// 
/// TODO: Change this class such that whenever ResetRandom is called, a seeded pseudorandom generator is created,
///  using the seed given in the inspector.
///  (Possibly: use a "random" seed whenever seed=0.)
/// </summary>
public class RandomGenerator : MonoBehaviour {
    [SerializeField] private int seed = 0;

    private System.Random rand;
	   
    /// <summary>
    /// Returns a random integer between 0 and maxValue-1 (inclusive).
    /// </summary>
    public int Next(int maxValue) {
        return Rand.Next(maxValue);
    }
    
    /// <summary>
    /// Returns a random integer between 0 and maxValue-1 (inclusive).
    /// </summary>
    public int IntBetweenRangeInclusive(int startValue,int endValue) {
        return Rand.Next(startValue,endValue+1);
    }

    public System.Random Rand {
        get {
            if (rand==null) {
                ResetRandom();
            }
            return rand;
        }
    }

    public void ResetRandom() {
        rand = new System.Random(seed);
    }
    
    public void InitializeRandom() {
        rand = new System.Random(seed);
    }
}