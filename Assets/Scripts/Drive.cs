//Taken from https://docs.unity3d.com/ScriptReference/Input.GetAxis.html
using UnityEngine;
namespace HC_BehaviourTree
{
    public class Drive : MonoBehaviour
    {
        public float speed = 10.0f;
        public float rotationSpeed = 100.0f;

        void Update()
        {
            // Get the horizontal and vertical axis.
            // By default they are mapped to the arrow keys.
            // The value is in the range -1 to 1
            float translation = Input.GetAxis("Vertical") * speed;
            float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

            // Make it move 10 meters per second instead of 10 meters per frame...
            translation *= Time.deltaTime;
            rotation *= Time.deltaTime;

            // Move translation along the object's z-axis
            transform.Translate(0, 0, translation);

            // Rotate around our y-axis
            transform.Rotate(0, rotation, 0);
        }
    }
}