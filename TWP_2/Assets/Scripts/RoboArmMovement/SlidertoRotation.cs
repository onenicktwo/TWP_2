using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlidertoRotation : MonoBehaviour
{
    public Slider Slider1;
    public Slider Slider2;
    public Slider Slider3;
    public Slider Slider4;
    public Slider Slider5;
    public Slider Slider6;

    public float multiplyingValue;

    public bool xRot1;
    public bool yRot2;
    public bool zRot3;
    
    

    public GameObject basething;
    public GameObject leg;
    public GameObject middle;
    public GameObject neck;
    public GameObject head;
    public GameObject gripper1;
    public GameObject gripper2;

    


    
//for reference: www.youtube.com/watch?v=bxheIjLAXLE&ab_channel=ThirteeNovCodingVlog 

    void Start()
    {   
        Slider1.onValueChanged.AddListener(delegate{
            RotateY(Slider1,basething);
        });
        Slider2.onValueChanged.AddListener(delegate{
            RotateX(Slider2, leg);
        });
        Slider3.onValueChanged.AddListener(delegate{
            RotateX(Slider3,middle);
        });
        Slider4.onValueChanged.AddListener(delegate{
            RotateX(Slider4, neck);
        });
        Slider5.onValueChanged.AddListener(delegate{
            RotateZ(Slider5, head);
        });
        Slider6.onValueChanged.AddListener(delegate{
            MoveMe(Slider6, gripper1, gripper2);
        }); 
        
    }

public void RotateX(Slider mySlider, GameObject gm)
{
        gm.transform.localEulerAngles = new Vector3(mySlider.value * multiplyingValue,gm.transform.localEulerAngles.y,gm.transform.localEulerAngles.z);

}
public void RotateY(Slider mySlider, GameObject gm)
{


     gm.transform.localEulerAngles = new Vector3(gm.transform.localEulerAngles.x, mySlider.value * multiplyingValue,gm.transform.localEulerAngles.z);


}
public void RotateZ(Slider mySlider, GameObject gm)
{
    gm.transform.localEulerAngles = new Vector3(gm.transform.localEulerAngles.x,gm.transform.localEulerAngles.y,mySlider.value * multiplyingValue);
    

}

public void MoveMe(Slider mySlider, GameObject gm, GameObject gm2)
{
    gm.transform.localPosition = new Vector3(gm.transform.localPosition.x, mySlider.value * -.8f, gm.transform.localPosition.z);
    gm2.transform.localPosition = new Vector3(gm2.transform.localPosition.x, mySlider.value * .8f, gm2.transform.localPosition.z);


}
}
