﻿using System.Collections;
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

       public GameObject cop;

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

     

            RSelector selectObject = new RSelector("Select Object to Steal");

            for (int i = 0; i < art.Length; i++)
            {
                Leaf gta = new Leaf("Go to " + art[i].name, i, GoToArt);
                selectObject.AddChild(gta);
            }


            Leaf goToBackDoor = new Leaf("Go To Backdoor", GoToBackDoor, 2);
            Leaf goToFrontDoor = new Leaf("Go To Frontdoor", GoToFrontDoor, 1);
            Leaf goToVan = new Leaf("Go To Van", GoToVan);
            pSelector opendoor = new pSelector("Open Door");
         

            Inverter invertMoney = new Inverter("Invert Money");
            invertMoney.AddChild(hasGotMoney);

            opendoor.AddChild(goToFrontDoor);
            opendoor.AddChild(goToBackDoor);

            steal.AddChild(invertMoney);
            steal.AddChild(opendoor);

            steal.AddChild(selectObject);

          
            steal.AddChild(goToVan);

            Sequence runAway = new Sequence("Run Away");
            Leaf canSee = new Leaf("Can See cop?", CanSeeCop);
            Leaf flee = new Leaf("Flee from cop?", FleeFromCop);
            runAway.AddChild(canSee);
            runAway.AddChild(flee);
            tree.AddChild(runAway); ;

            tree.PrintTree();



        }

        public Node.Status CanSeeCop()
        {
            return CanSee(cop.transform.position, "Cop", 10, 90);
        }

        public Node.Status FleeFromCop()
        {
        
            return Flee(cop.transform.position, 10);
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


        public Node.Status GoToArt(int i)
        {
            if (!art[i].activeSelf) return Node.Status.FAILURE;
            Node.Status s = GoToLocation(art[i].transform.position);
            if (s == Node.Status.SUCCESS)
            {
                art[i].transform.parent = this.gameObject.transform;
                pickup = art[i];
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