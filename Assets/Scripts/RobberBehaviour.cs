using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace HC_BehaviourTree
{
    public class RobberBehaviour : BTAgent
    {
       
        public GameObject frontDoor;
        public GameObject backdoor;
        public GameObject diamond;
        public GameObject painting;
        public GameObject van;
       
        [Range(0, 1000)]
        public int money = 800;

      new void Start()
        {

            base.Start();
            Sequence steal = new Sequence("Steal Something");
            Leaf hasGotMoney = new Leaf("Has Got Money", HasMoney);
            Leaf gotoFrontDoor = new Leaf("Go to Door", GoToFronDoor);
            Leaf gotoDiamond = new Leaf("Go to Diamond", GoToDiamond);
            Leaf gotoPainting = new Leaf("Go to Diamond", GoToPainting);
            Leaf gotoBackDoor= new Leaf("Go to Door", GoToBackDoor);
            Leaf gotoVan = new Leaf("Go to Van", GoToVan);
          
            Inverter invertMoney = new Inverter("Invert Money");
            invertMoney.AddChild(hasGotMoney);


            Selector openDoor = new Selector("Open Door");
            Selector selectObjectToSteal = new Selector("Select Object To Steal");

            openDoor.AddChild(gotoFrontDoor);
            openDoor.AddChild(gotoBackDoor);


            steal.AddChild(invertMoney);
            steal.AddChild(openDoor);

            selectObjectToSteal.AddChild(gotoDiamond);
            selectObjectToSteal.AddChild(gotoPainting);

            steal.AddChild(selectObjectToSteal);
            // steal.AddChild(gotoBackDoor);
            steal.AddChild(gotoVan);
            tree.AddChild(steal);

            tree.PrintTree();

         
          
        }

        public Node.Status HasMoney()
        {
            if(money < 500)
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

        public Node.Status GoToPainting()
        {
            Node.Status s = GoToLocation(painting.transform.position);
            if (s == Node.Status.SUCCESS)
            {
                painting.SetActive(false);
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
                    door.GetComponent<NavMeshObstacle>().enabled = false;
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

   

    }
}