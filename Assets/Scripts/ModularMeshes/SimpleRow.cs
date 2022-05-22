using UnityEngine;

namespace ModularMeshes {
    public class SimpleRow : Shape {
        private int number;
        private GameObject[] prefabs;
        private Vector3 direction;

        public void Initialize(int number, GameObject[] prefabs, Vector3 dir=new Vector3()) {
            this.number=number;
            this.prefabs=prefabs;
            direction = dir.magnitude!=0 ? dir : new Vector3(0, 0, 1);
        }

        protected override void Execute() {

            if (number<=0)
                return;
            for (var i=0;i<number;i++) {	// spawn the prefabs, randomly chosen
                var index = RandomInt(prefabs.Length); // choose a random prefab index
                SpawnPrefab(prefabs[index],
                    direction * (i - (number-1)/2f), // position offset from center
                    Quaternion.identity			// no rotation
                );
            }
        }
    }
}