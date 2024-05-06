using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimator : MonoBehaviour
{
    [SerializeField]
    public Animator Animator; //public¡Bprivate¡Bprotected

    // Start is called before the first frame update
    void Start()
    {
        updateaAnimatorState();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void updateaAnimatorState()
    {
        Animator.SetBool("New Bool", true);
        //f(x, z, t) = x + 1
    }

}
