using UnityEngine;
using System.Collections;
using System.Linq;
using System;
using System.Runtime.InteropServices;

public class BHV_CameraControl : MonoBehaviour
{
    float CamSpeedFast = 0.15f;
    float CamSpeedMed = 0.09f;
    float CamSpeedSlow = 0.05f;
    float CamSpeedSlowMoa = 0.01f;

    float CamSpeed = 0;

    float GUIsizeSlow = 90;
    float GUIsizeMed = 60;
    float GUIsizeFast = 25;

    private Transform thisTransform;
    public Transform thePlayer;
    private Vector3 thePlayerPosition;

    Rect recdownFast;
    Rect recupFast;
    Rect recleftFast;
    Rect recrightFast;

    Rect recdownMed;
    Rect recupMed;
    Rect recleftMed;
    Rect recrightMed;

    Rect recdownSlow;
    Rect recupSlow;
    Rect recleftSlow;
    Rect recrightSlow;

    void Start()
    {
        thisTransform = this.transform;

        recdownFast = new Rect(0, 0, Screen.width, GUIsizeFast);
        recupFast = new Rect(0, Screen.height - GUIsizeFast, Screen.width, GUIsizeFast);
        recleftFast = new Rect(0, 0, GUIsizeFast, Screen.height);
        recrightFast = new Rect(Screen.width - GUIsizeFast, 0, GUIsizeFast, Screen.height);

        recdownMed = new Rect(0, 0, Screen.width, GUIsizeMed);
        recupMed = new Rect(0, Screen.height - GUIsizeMed, Screen.width, GUIsizeMed);
        recleftMed = new Rect(0, 0, GUIsizeMed, Screen.height);
        recrightMed = new Rect(Screen.width - GUIsizeMed, 0, GUIsizeMed, Screen.height);

        recdownSlow = new Rect(0, 0, Screen.width, GUIsizeSlow);
        recupSlow = new Rect(0, Screen.height - GUIsizeSlow, Screen.width, GUIsizeSlow);
        recleftSlow = new Rect(0, 0, GUIsizeSlow, Screen.height);
        recrightSlow = new Rect(Screen.width - GUIsizeSlow, 0, GUIsizeSlow, Screen.height);
    }

    void Update()
    {
        if (!Input.GetKey(KeyCode.Space))
        {
            thisTransform.parent = null;

            if (recdownSlow.Contains(Input.mousePosition) || recdownMed.Contains(Input.mousePosition) || recdownFast.Contains(Input.mousePosition))
            {
                if (!isTooFarFromPlayer){
                
                    if (recdownFast.Contains(Input.mousePosition))
                    {
                        CamSpeed = CamSpeedFast;
                    }
                    else
                        if (recdownMed.Contains(Input.mousePosition))
                        {
                            CamSpeed = CamSpeedMed;
                        }
                        else
                        {
                            CamSpeed = CamSpeedSlow;
                        }

                }
                else
                {
                    CamSpeed = CamSpeedSlowMoa;
                }
                    thisTransform.Translate(0, 0, CamSpeed, Space.World);
            }

            if (recupSlow.Contains(Input.mousePosition) || recupMed.Contains(Input.mousePosition) || recupFast.Contains(Input.mousePosition))
            {
                if (!isTooFarFromPlayer)
                {
                    if (recupFast.Contains(Input.mousePosition))
                    {
                        CamSpeed = CamSpeedFast;
                    }
                    else
                        if (recupMed.Contains(Input.mousePosition))
                        {
                            CamSpeed = CamSpeedMed;
                        }
                        else
                        {
                            CamSpeed = CamSpeedSlow;
                        }
                }
                else
                {
                    CamSpeed = CamSpeedSlowMoa;
                }
                    thisTransform.Translate(0, 0, -CamSpeed, Space.World);
            }
            if (recleftSlow.Contains(Input.mousePosition) || recleftMed.Contains(Input.mousePosition) || recleftFast.Contains(Input.mousePosition))
            {
                if (!isTooFarFromPlayer)
                {
                    if (recleftFast.Contains(Input.mousePosition))
                    {
                        CamSpeed = CamSpeedFast;
                    }
                    else
                        if (recleftMed.Contains(Input.mousePosition))
                        {
                            CamSpeed = CamSpeedMed;
                        }
                        else
                        {
                            CamSpeed = CamSpeedSlow;
                        }
                }
                else
                {
                    CamSpeed = CamSpeedSlowMoa;
                }
                    thisTransform.Translate(CamSpeed, 0, 0, Space.World);
            }
            if (recrightSlow.Contains(Input.mousePosition) || recrightMed.Contains(Input.mousePosition) || recrightFast.Contains(Input.mousePosition))
            {
                if (!isTooFarFromPlayer)
                {
                    if (recrightFast.Contains(Input.mousePosition))
                    {
                        CamSpeed = CamSpeedFast;
                    }
                    else
                        if (recrightMed.Contains(Input.mousePosition))
                        {
                            CamSpeed = CamSpeedMed;
                        }
                        else
                        {
                            CamSpeed = CamSpeedSlow;
                        }
                }
                else {
                    CamSpeed = CamSpeedSlowMoa;
                }
                    thisTransform.Translate(-CamSpeed, 0, 0, Space.World);
            }
        }
        
    }

    void LateUpdate() {
        if (thePlayerPosition != thePlayer.transform.position)
        {
            thePlayerPosition = thePlayer.transform.position;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            thisTransform.position = new Vector3(thePlayerPosition.x, thePlayerPosition.y + 13, thePlayerPosition.z + 6.5f);
        }
    }

    bool isTooFarFromPlayer = false;
    void OnTriggerStay(Collider obj) {
        if (obj.name == "CameraBoundaries") {
            isTooFarFromPlayer = false;
        }
    }
    void OnTriggerExit(Collider obj)
    {
        if (obj.name == "CameraBoundaries")
        {
            isTooFarFromPlayer = true;
        }
    }
}
