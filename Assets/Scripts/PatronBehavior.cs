using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace HC_BehaviourTree
{
    public class PatronBehavior : BTAgent
    {
        Animator anim;
        public GameObject[] art;
        public GameObject frontDoor;
        public GameObject homeBase;


        [Range(0,1000)]
        public int boredome = 0;

        public bool ticket = false;

        public override void Start()
        {
            base.Start();
            anim = GetComponent<Animator>();
            RSelector selectObject = new RSelector("Select Art to View");
            for (int i = 0; i < art.Length; i++)
            {
                Leaf gta = new Leaf("Go to " + art[i].name, i, GoToArt);
                selectObject.AddChild(gta);
            }
            Leaf gotoFrontDoor = new Leaf("Go To Front Door", GoToFrontDoor);
            Leaf gotoHome = new Leaf("Go To Home", GoToHome);
            Leaf isBored = new Leaf("Is Bored", IsBored);
            Leaf isOpen = new Leaf("Is Open", IsOpen);

            Sequence viewArt = new Sequence("View Art");
            viewArt.AddChild(isOpen);
            viewArt.AddChild(isBored);
            viewArt.AddChild(gotoFrontDoor);

            Leaf noTicket = new Leaf("Wait for Ticket", NoTicket);
            Leaf isWaiting = new Leaf("Waiting for Worker", IsWaiting);

            BehaviourTree waitforTicket = new BehaviourTree("wait for Ticket");
            waitforTicket.AddChild(noTicket);

            Loop getTicket = new Loop("Ticket", waitforTicket);
            getTicket.AddChild(isWaiting);

            viewArt.AddChild(getTicket);

            BehaviourTree whileBored = new BehaviourTree();
             whileBored.AddChild(isBored);

            Loop lookAtPaintings = new Loop("Look", whileBored);
            lookAtPaintings.AddChild(selectObject);
            viewArt.AddChild(lookAtPaintings);


            viewArt.AddChild(gotoHome);

            BehaviourTree galleryOpenCondition = new BehaviourTree();
            galleryOpenCondition.AddChild(isOpen);

            DepSequence bePatron = new DepSequence("Be An Art Patron", galleryOpenCondition, agent);
            bePatron.AddChild(viewArt);

            Selector viewArtWithFallback = new Selector("View art with fallback");
            viewArtWithFallback.AddChild(bePatron);
            viewArtWithFallback.AddChild(gotoHome);

            tree.AddChild(viewArtWithFallback);

            StartCoroutine(IncreaseBoredome());
        }


        private Vector3 previousPosition;
        public float curSpeed;
        void Update()
        {
            float finalSpeed;
            Vector3 curMove = transform.position - previousPosition;
            curSpeed = curMove.magnitude / Time.deltaTime;
            previousPosition = transform.position;

            finalSpeed = Mathf.Clamp(curSpeed, 0, 1);
            if(anim != null)
            {
                anim.SetFloat("speed", finalSpeed);
            }
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
                boredome = Mathf.Clamp(boredome - 10, 0, 1000);
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

        public Node.Status NoTicket()
        {
            if(ticket || IsOpen() == Node.Status.FAILURE)
            {
                return Node.Status.FAILURE;
            }
            else
            {
                return Node.Status.SUCCESS;
            }
        }

        public Node.Status IsWaiting()
        {
            if(Blackboard.Instance.RegisterPatron(this.gameObject) == this.gameObject){
                return Node.Status.SUCCESS;
            }
            else
            {
                return Node.Status.FAILURE;
            }
        }
   

    }
}