using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HC_BehaviourTree
{
    public class RobberBehaviour : MonoBehaviour
    {
        BehaviourTree tree;
        void Start()
        {
            tree = new BehaviourTree();
            Node steal = new Node("Steal Something");
            Node gotoDiamond = new Node("Go to Diamond");
            Node gotoVan = new Node("Go to Van");

            steal.AddChild(gotoDiamond);
            steal.AddChild(gotoVan);
            tree.AddChild(steal);

            tree.PrintTree();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}