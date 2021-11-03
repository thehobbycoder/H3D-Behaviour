 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HC_BehaviourTree
{
    public class Worker : BTAgent
    {
        GameObject patron;
        public GameObject office;
        public override void Start()
        {
            base.Start();

            Leaf stillWaiting = new Leaf("Is waiting", PatronWaiting);
            Leaf allocatePatron = new Leaf("Allocate Patron", AllocatePatron);
            Leaf gotoPatron = new Leaf("Go To Patron", GoToPatron);
            Leaf gotoOffice = new Leaf("Go To Office", GoToOffice);

            Sequence getPatron = new Sequence("Find patron");
            getPatron.AddChild(allocatePatron);


            BehaviourTree waiting = new BehaviourTree();
            waiting.AddChild(stillWaiting);

            DepSequence moveToPatron = new DepSequence("Moving to Patron", waiting, agent);
            moveToPatron.AddChild(gotoPatron);

            getPatron.AddChild(moveToPatron);

            Selector beWorker = new Selector("Be a Worker");
            beWorker.AddChild(getPatron );
            beWorker.AddChild(gotoOffice);

            tree.AddChild(beWorker);
        }

        public Node.Status PatronWaiting()
        {
            if (patron == null) return Node.Status.FAILURE;

            if(patron.GetComponent<PatronBehavior>().isWaiting)
            {
              return  Node.Status.SUCCESS;
            }

            return Node.Status.FAILURE;
        }
        public Node.Status AllocatePatron()
        {
            if (Blackboard.Instance.patrons.Count == 0)
            {
                return Node.Status.FAILURE;
            }
            patron = Blackboard.Instance.patrons.Pop();
            if(patron == null)
            {
                return Node.Status.FAILURE;
            }
            return Node.Status.SUCCESS;
        }

            public Node.Status GoToPatron()
        {
           if(patron == null) return Node.Status.FAILURE;
            Node.Status s = GoToLocation(patron.transform.position);
            if (s == Node.Status.SUCCESS)
            {
                patron.GetComponent<PatronBehavior>().ticket = true;
                patron = null;
            
            }

            return s;
        }

        public Node.Status GoToOffice()
        {
            Node.Status s = GoToLocation(office.transform.position);
            patron = null;
            return s;
        }
    }
}