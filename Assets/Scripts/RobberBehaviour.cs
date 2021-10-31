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
            agent.SetDestination(diamond.transform.position);
            return Node.Status.SUCCESS;
        }

        public Node.Status GoToVan()
        {
            agent.SetDestination(van.transform.position);
            return Node.Status.SUCCESS;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}