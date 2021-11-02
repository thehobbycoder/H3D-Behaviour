using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace HC_BehaviourTree
{
    public class PatronBehavior : BTAgent
    {

        public GameObject[] art;
        public GameObject frontDoor;
        public GameObject homeBase;


        [Range(0,1000)]
        public int boredome = 0;

        public override void Start()
        {
            base.Start();

            RSelector selectObject = new RSelector("Select Art to View");
            for (int i = 0; i < art.Length; i++)
            {
                Leaf gta = new Leaf("Go to " + art[i].name, i, GoToArt);
                selectObject.AddChild(gta);
            }
            Leaf gotoFrontDoor = new Leaf("Go To Front Door", GoToFrontDoor);
            Leaf gotoHome = new Leaf("Go To Home", GoToHome);
            Leaf isBored = new Leaf("Is Bored", IsBored);

            Sequence viewArt = new Sequence("View Art");
            viewArt.AddChild(isBored);
            viewArt.AddChild(gotoFrontDoor);
            viewArt.AddChild(selectObject);
            viewArt.AddChild(gotoHome);

            Selector bePatron = new Selector("Be An Art Patron");
            bePatron.AddChild(viewArt);

            tree.AddChild(bePatron);

            StartCoroutine(IncreaseBoredome());
        }

        IEnumerator IncreaseBoredome()
        {
            while (true)
            {
                boredome = Mathf.Clamp(boredome + 20, 0, 1000);
                yield return new WaitForSeconds(Random.Range(1, 5));
            }
        }
        public Node.Status GoToArt(int i)
        {
            if (!art[i].activeSelf) return Node.Status.FAILURE;
            Node.Status s = GoToLocation(art[i].transform.position);
            if(s == Node.Status.SUCCESS)
            {
                boredome = Mathf.Clamp(boredome - 500, 0, 1000);
            }
            return s;
        }

        public Node.Status GoToFrontDoor()
        {
            Node.Status s = GoToDoor(frontDoor);
            return s;
        }

        public Node.Status GoToHome()
        {
            Node.Status s = GoToLocation(homeBase.transform.position);
            
            return s;
        }

        public Node.Status IsBored()
        {

            if(boredome < 100)
            {
                return Node.Status.FAILURE;
            }
            else
            {
                return Node.Status.SUCCESS;
            }

        }

    }
}