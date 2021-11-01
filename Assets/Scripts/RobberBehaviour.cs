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

        public GameObject[] art;

        GameObject pickup;
       
        [Range(0, 1000)]
        public int money = 800;

      new void Start()
        {

            base.Start();
            Sequence steal = new Sequence("Steal Something");
            Leaf goToDiamond = new Leaf("Go To Diamond", GoToDiamond, 1);
            Leaf goToPainting = new Leaf("Go To Painting", GoToPainting, 2);
            Leaf hasGotMoney = new Leaf("Has Got Money", HasMoney);

            Leaf gotoArt1 = new Leaf("Go To Art 1", GoToArt1);
            Leaf gotoArt2= new Leaf("Go To Art 2", GoToArt2);
            Leaf gotoArt3 = new Leaf("Go To Art 3", GoToArt3);
          


            Leaf goToBackDoor = new Leaf("Go To Backdoor", GoToBackDoor, 2);
            Leaf goToFrontDoor = new Leaf("Go To Frontdoor", GoToFrontDoor, 1);
            Leaf goToVan = new Leaf("Go To Van", GoToVan);
            pSelector opendoor = new pSelector("Open Door");
            RSelector selectObject = new RSelector("Select Object to Steal");

            Inverter invertMoney = new Inverter("Invert Money");
            invertMoney.AddChild(hasGotMoney);

            opendoor.AddChild(goToFrontDoor);
            opendoor.AddChild(goToBackDoor);

            steal.AddChild(invertMoney);
            steal.AddChild(opendoor);

           
            selectObject.AddChild(gotoArt1);
            selectObject.AddChild(gotoArt2);
            selectObject.AddChild(gotoArt3);

            steal.AddChild(selectObject);

          
            steal.AddChild(goToVan);
            tree.AddChild(steal);

            tree.PrintTree();



        }

        public Node.Status HasMoney()
        {
            if (money < 500)
                return Node.Status.FAILURE;
            return Node.Status.SUCCESS;
        }

        public Node.Status GoToDiamond()
        {
            if (!diamond.activeSelf) return Node.Status.FAILURE;
            Node.Status s = GoToLocation(diamond.transform.position);
            if (s == Node.Status.SUCCESS)
            {
                diamond.transform.parent = this.gameObject.transform;
                pickup = diamond;
            }
            return s;
        }


        public Node.Status GoToArt1()
        {
            if (!art[0].activeSelf) return Node.Status.FAILURE;
            Node.Status s = GoToLocation(art[0].transform.position);
            if (s == Node.Status.SUCCESS)
            {
                art[0].transform.parent = this.gameObject.transform;
                pickup = art[0];
            }
            return s;
        }

        public Node.Status GoToArt2()
        {
            if (!art[1].activeSelf) return Node.Status.FAILURE;
            Node.Status s = GoToLocation(art[1].transform.position);
            if (s == Node.Status.SUCCESS)
            {
                art[1].transform.parent = this.gameObject.transform;
                pickup = art[1];
            }
            return s;
        }


        public Node.Status GoToArt3()
        {
            if (!art[2].activeSelf) return Node.Status.FAILURE;
            Node.Status s = GoToLocation(art[2].transform.position);
            if (s == Node.Status.SUCCESS)
            {
                art[2].transform.parent = this.gameObject.transform;
                pickup = art[2];
            }
            return s;
        }


        public Node.Status GoToPainting()
        {
            if (!painting.activeSelf) return Node.Status.FAILURE;
            Node.Status s = GoToLocation(painting.transform.position);
            if (s == Node.Status.SUCCESS)
            {
                painting.transform.parent = this.gameObject.transform;
                pickup = painting;
            }
            return s;
        }

        public Node.Status GoToBackDoor()
        {
            return GoToDoor(backdoor);
        }

        public Node.Status GoToFrontDoor()
        {
            return GoToDoor(frontDoor);
        }

        public Node.Status GoToVan()
        {
            Node.Status s = GoToLocation(van.transform.position);
            if (s == Node.Status.SUCCESS)
            {               
                money += 300;
                pickup.SetActive(false);
            }
            return s;
        }

        public Node.Status GoToDoor(GameObject door)
        {
            Node.Status s = GoToLocation(door.transform.position);
            if (s == Node.Status.SUCCESS)
            {
                if (!door.GetComponent<Lock>().isLocked)
                {
                    door.GetComponent<NavMeshObstacle>().enabled = false;
                    return Node.Status.SUCCESS;
                }
                return Node.Status.FAILURE;
            }
            else
                return s;
        }


    }
}