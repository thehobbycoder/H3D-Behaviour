using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace HC_BehaviourTree
{
    public class RobberBehaviour : MonoBehaviour
    {
        BehaviourTree tree;
        public GameObject frontDoor;
        public GameObject backdoor;
        public GameObject diamond;
        public GameObject van;
        NavMeshAgent agent;

        public enum ActionState {  IDLE, WORKING}
        ActionState state = ActionState.IDLE;

        Node.Status treeStatus = Node.Status.RUNNING;

        [Range(0, 1000)]
        public int money = 800;

        void Start()
        {
            agent = this.GetComponent<NavMeshAgent>();

            tree = new BehaviourTree();
            Sequence steal = new Sequence("Steal Something");
            Leaf hasGotMoney = new Leaf("Has Got Money", HasMoney);
            Leaf gotoFrontDoor = new Leaf("Go to Door", GoToFronDoor);
            Leaf gotoDiamond = new Leaf("Go to Diamond", GoToDiamond);
            Leaf gotoBackDoor= new Leaf("Go to Door", GoToBackDoor);
            Leaf gotoVan = new Leaf("Go to Van", GoToVan);
            Selector openDoor = new Selector("Open Door");

            openDoor.AddChild(gotoFrontDoor);
            openDoor.AddChild(gotoBackDoor);


            steal.AddChild(hasGotMoney);
            steal.AddChild(openDoor);
            steal.AddChild(gotoDiamond);
           // steal.AddChild(gotoBackDoor);
            steal.AddChild(gotoVan);
            tree.AddChild(steal);

            tree.PrintTree();

         
          
        }

        public Node.Status HasMoney()
        {
            if(money >= 500)
            {
                return Node.Status.FAILURE;
            }
            return Node.Status.SUCCESS;

        }

        public Node.Status GoToFronDoor()
        {
            return GoToDoor(frontDoor);

        }

        public Node.Status GoToBackDoor()
        {
            return GoToDoor(backdoor);

        }

        public Node.Status GoToDiamond()
        {
            Node.Status s = GoToLocation(diamond.transform.position);
            if (s == Node.Status.SUCCESS)
            {
                diamond.SetActive(false);
            }
           
            
            return s;
           

        }

        public Node.Status GoToVan()
        {
            Node.Status s =  GoToLocation(van.transform.position);
            if (s == Node.Status.SUCCESS)
            {
                money += 500;
            }


            return s;

        }

        public Node.Status GoToDoor(GameObject door)
        {
            Node.Status s = GoToLocation(door.transform.position);

            if(s == Node.Status.SUCCESS)
            {
                if (!door.GetComponent< Lock>().isLocked){
                    door.SetActive(false);
                    return Node.Status.SUCCESS;
                }

                return Node.Status.FAILURE;
            }
            else
            {
                return s;
            }
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
            if(treeStatus != Node.Status.SUCCESS)
                treeStatus =  tree.Process();
        }
    }
}