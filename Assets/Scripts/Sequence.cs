using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HC_BehaviourTree
{
    public class Sequence : Node
    {
        public Sequence(string n)
        {
            name = n;
        }

        public override Status Process()
        {
           
            Status childStatus = children[currentChild].Process();
           
            if (childStatus == Status.RUNNING) return Status.RUNNING;

            if (childStatus == Status.FAILURE)
            {
                return Status.FAILURE;
            }

            currentChild++;
            if (currentChild >= children.Count)
            {
                return Status.SUCCESS;
            }

            return Status.RUNNING;
        }
    }
}