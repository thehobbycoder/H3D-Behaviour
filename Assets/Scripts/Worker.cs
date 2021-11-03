 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HC_BehaviourTree
{
    public class Worker : BTAgent
    {
        public GameObject office;
        public override void Start()
        {
            base.Start();

            Leaf gotoPatron = new Leaf("Go To Patron", GoToPatron);
            Leaf gotoOffice = new Leaf("Go To Office", GoToOffice);

            Selector beWorker = new Selector("Be a Worker");
            beWorker.AddChild(gotoPatron);
            beWorker.AddChild(gotoOffice);

            tree.AddChild(beWorker);
        }

        public Node.Status GoToPatron()
        {
            if (Blackboard.Instance.patron == null) return Node.Status.FAILURE;
            Node.Status s = GoToLocation(Blackboard.Instance.patron.transform.position);
            if (s == Node.Status.SUCCESS)
            {
                Blackboard.Instance.patron.GetComponent<PatronBehavior>().ticket = true;
                Blackboard.Instance.DeregisterPatron();
            }

            return s;
        }

        public Node.Status GoToOffice()
        {
            Node.Status s = GoToLocation(office.transform.position);
            return s;
        }
    }
}