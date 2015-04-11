using UnityEngine;
using System.Collections;

public class BHV_BasicAnimations : MonoBehaviour {

    Stats_BaseStats Stats;
    
    public Animation thisModelAnimation;

    bool animation_flag;
    bool isIdle;
    bool isWalking;

    public string curentAnimation;

    void Start() {
        PlayAnimation(0);

    }

    int lastAnimation = 1000;
    public void PlayAnimation(int animation) {
        if (lastAnimation != animation)
        {
            lastAnimation = animation;
            // Animaition Idle
            if (animation == 0)
            {
                isIdle = true;
            }
            // Animation Walk
            else if (animation == 1)
            {
                isWalking = true;
            }
            // Animation Pickup
            else if (animation == 17){
                Debug.Log("animation - pickup.");
            }
        }
        else 
        {
            return;   
        }
    }

	// Update is called once per frame
	void FixedUpdate () {
        // If no animation runs.
        if (!animation_flag)
        {

        }
        else 
        {

        }

        if (isWalking)
        {
            thisModelAnimation["Walk"].speed = 1.4f;
            thisModelAnimation.CrossFade("Walk");
            isWalking = false;
        }

        if (isIdle)
        {
            thisModelAnimation.CrossFade("Idle");
            isIdle = false;
        }
	}
}

