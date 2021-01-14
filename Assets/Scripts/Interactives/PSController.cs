using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSController : MonoBehaviour
{
    [SerializeField] Transform ImantableObject;
    [SerializeField] ImanBehavior myIB;
    [SerializeField] GameObject ParticleSystemPositive;
    [SerializeField] GameObject ParticleSystemNegative;
    ParticleSystem myPSPositive;
    ParticleSystem myPSNegative;
    bool isemitingNegative = false;
    bool isemitingPositive = false;
    ParticleSystem.ShapeModule psp;
    ParticleSystem.ShapeModule psn;
    // Start is called before the first frame update
    void Start()
    {
        ParticleSystemPositive.transform.position = ImantableObject.position;
        myPSPositive = ParticleSystemPositive.GetComponent<ParticleSystem>();
        myPSNegative = ParticleSystemNegative.GetComponent<ParticleSystem>();
        myPSPositive.enableEmission = false;
        myPSNegative.enableEmission = false;
        psp = ParticleSystemPositive.GetComponent<ParticleSystem>().shape;
        psn = ParticleSystemNegative.GetComponent<ParticleSystem>().shape;
    }

    // Update is called once per frame
    void Update()
    {
        if ((myIB.mobility == mobilityType.MOBILE) || (myIB.mobility == mobilityType.STATIC))
        {
            switch (myIB.myPole)
            {
                case iman.POSITIVE:
                    if (!isemitingPositive)
                    {
                        isemitingPositive = true;
                        if (isemitingNegative)
                        {
                            isemitingNegative = false;
                            myPSNegative.enableEmission = false;
                        }
                        int num = myIB.GetCharges();
                        switch (num)
                        {
                            case 1:
                                psp.radius = 3.5f*2;
                                break;
                            case 2:
                                psp.radius = 4.5f*2;
                                break;
                            case 3:
                                psp.radius = 8*2;
                                Debug.Log("Ha entrado");
                                break;
                            default:
                                break;
                        }
                        myPSPositive.enableEmission = true;
                    }
                    ParticleSystemPositive.transform.position = ImantableObject.position;
                    break;
                case iman.NEGATIVE:
                    if (!isemitingNegative)
                    {
                        if (isemitingPositive)
                        {
                            isemitingPositive = false;
                            myPSPositive.enableEmission = false;
                        }
                        isemitingNegative = true;
                        int num = myIB.GetCharges();
                        switch (num)
                        {
                            case 1:
                                psn.radius = 3.5f * 2;
                                break;
                            case 2:
                                psn.radius = 4.5f * 2;
                                break;
                            case 3:
                                psn.radius = 8 * 2;
                                break;
                            default:
                                break;
                        }
                        myPSNegative.enableEmission = true;
                    }
                    ParticleSystemNegative.transform.position = ImantableObject.position;
                    break;
                case iman.NONE:
                    if (isemitingNegative || isemitingPositive)
                    {
                        isemitingPositive = false;
                        isemitingNegative = false;
                        myPSPositive.enableEmission = false;
                        myPSNegative.enableEmission = false;
                    }
                    break;
                default:
                    break;
            }          
        }
                
    }
}
