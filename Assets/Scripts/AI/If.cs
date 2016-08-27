using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BAD
{
    /// <summary>
    /// Runs it's child node if a System.Func<bool> method returns true.
    /// </summary>
    public class If : Decorator
    {

        ComponentMethodLookup method;
        
        public override void Apply (object[] args)
        {
            this.method = (ComponentMethodLookup)args[0];
        }
            
        public override IEnumerator<NodeResult> NodeTask ()
        {
            if (ChildIsMissing ()) {
                yield return NodeResult.Failure;
            }
            var result = (bool)method.Invoke ();
            if (result) {
                var task = children [0].GetNodeTask ();
                while (task.MoveNext()) {
                    yield return task.Current;
                }
            } else {
                yield return NodeResult.Failure;
            }
        }

        public override string ToString ()
        {
            return string.Format ("IF ({0})", method);
        }

    }

    /// <summary>
    /// Runs either child[0] the failure or child[1] the success given the condition.
    /// </summary>
    public class Decision : Decorator
    {

        ComponentMethodLookup method;

        public override void Apply(object[] args)
        {
            this.method = (ComponentMethodLookup)args[0];
        }

        public override IEnumerator<NodeResult> NodeTask()
        {
            if (ChildIsMissing() || children.Count < 2)
            {
                yield return NodeResult.Failure;
            }
            var result = (bool)method.Invoke();
            if (result)
            {
                // do the success function
                var task = children[1].GetNodeTask();
                while (task.MoveNext())
                {
                    yield return task.Current;
                }
            }
            else
            {
                // do the failure function
                var task = children[0].GetNodeTask();
                while (task.MoveNext())
                {
                    yield return task.Current;
                }
            }
        }

        public override string ToString()
        {
            return string.Format("DECISION ({0})", method);
        }
    }
}