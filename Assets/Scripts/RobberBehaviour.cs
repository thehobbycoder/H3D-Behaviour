using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace HC_BehaviourTree
{
    public class RobberBehaviour : MonoBehaviour
    {
        BehaviourTree tree;
        public GameObject diamond;
        public GameObject van;
        NavMeshAgent agent;

        public enum ActionState {  IDLE, WORKING}
        ActionState state = ActionState.IDLE;


        void Start()
        {
            agent = this.GetComponent<NavMeshAgent>();

            tree = new BehaviourTree();
            Node steal = new Node("Steal Something");
            Leaf gotoDiamond = new Leaf("Go to Diamond", GoToDiamond);
            Leaf gotoVan = new Leaf("Go to Van", GoToVan);

            steal.AddChild(gotoDiamond);
            steal.AddChild(gotoVan);
            tree.AddChild(steal);

            tree.PrintTree();

            tree.Process();
          
        }

        public Node.Status GoToDiamond()
        {
            return GoToLocation(diamond.transform.position);
           
        }

        public Node.Status GoToVan()
        {
            return GoToLocation(van.transform.position);
         
        }

        Node.Status GoToLocation(Vector3 destination)
        {
            float distanceToTarget = Vector3.Distance(destination, transform.position);
            if(state == ActionState.IDLE)
            {
                agent.SetDestination(destination);
                state = ActionState.WORKING;
            }else if (Vector3.Distance(agent.pathEndPosition, destination) >= 2)
            {
                state = ActionState.IDLE;
                return Node.Status.FAILURE;
            }
            else if (distanceToTarget < 2)
            {
                state = ActionState.IDLE;
                return Node.Status.SUCCESS;
            }

            return Node.Status.RUNNING;

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}