using UnityEngine;

namespace Demo
{
    public class Stack : Shape
    {
        public GameObject prefab;
        public int heightRemaining;

        private void Initialize(GameObject pPrefab, int pHeightRemaining)
        {
            prefab = pPrefab;
            heightRemaining = pHeightRemaining;
        }

        protected override void Execute()
        {
            // Spawn the (box) prefab as child of this game object:
            // (Optional parameters: localPosition, localRotation, alternative parent)
            var box = SpawnPrefab(prefab);

            // Example: fat box:
            //box.transform.localScale=new Vector3(7, 1, 1);

            if (heightRemaining <= 0) return;
            
            //SimpleStack();

            //ScalingStack();

            //RotationStack();

            //RotationAndScalingStack();

            ChildStacks();
        }

        private void SimpleStack()
        {
            // Simple stack:
            // Spawn a smaller stack on top of this:
            var newStack = CreateSymbol<Stack>("stack", new Vector3(0, 1, 0));
            newStack.Initialize(prefab, heightRemaining - 1);
            newStack.Generate(0.1f);
        }

        private void ScalingStack()
        {
            // Scaling:
            // Every new stack gets a bit smaller:
            var newStack = CreateSymbol<Stack>("stack", new Vector3(0, 1, 0));
            newStack.Initialize(prefab, heightRemaining-1);
            newStack.transform.localScale=new Vector3(0.9f, 0.9f, 0.9f);
            newStack.Generate(0.1f); 
        }

        private void RotationStack()
        {
            // Rotation:
            // Every new stack rotates by 30 degrees around the y-axis:
            var newStack = CreateSymbol<Stack>("stack", new Vector3(0, 1, 0));
            newStack.Initialize(prefab, heightRemaining-1);
            newStack.transform.localRotation = Quaternion.Euler(0, 30, 0);
            newStack.Generate(0.1f); 
        }

        private void RotationAndScalingStack()
        {
            // Rotation & scaling:
            // Every new stack rotates by 45 degrees around the z-axis, and becomes a bit smaller:
            var newStack = CreateSymbol<Stack>("stack", new Vector3(-0.25f, 1.25f, 0));
            newStack.Initialize(prefab, heightRemaining-1);
            var newStackTransform= newStack.transform;
            (newStackTransform ).localRotation = Quaternion.Euler(0, 0, 45);
            newStackTransform.localScale=new Vector3(0.707f, 0.707f, 0.707f);
            newStack.Generate(0.1f); 
        }

        private void ChildStacks()
        {
            // Two smaller child stacks, spawned with an offset:
            // **** WARNING: don't do this with HeighRemaining values larger than about 8! ****
            //      (exponential growth breaks computers!)
            if (heightRemaining>8) {
                heightRemaining=8;
            }
            for (var i = 0; i<2; i++) {
                var newStack = CreateSymbol<Stack>("stack", new Vector3(i-0.5f, 1, 0));
                newStack.Initialize(prefab, heightRemaining-1);
                newStack.transform.localScale=new Vector3(0.5f, 0.5f, 0.5f);
                newStack.Generate(0.1f);
            }
        }
    }
}