using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum mobilityType { MOBILE, STATIC, NONE, SEMIMOVIBLE, JUSTPOLE };
public enum iman { POSITIVE, NEGATIVE, NONE };
public enum forceType { ATRACT, REPULSE, NONE };

public class ImanBehavior : MonoBehaviour
{
    [SerializeField] private List<GameObject> nearImantableObjects;

    [Header("CHECKING CHARGES")]
    [SerializeField]SphereCollider mysphereCollider;
    [Header("ELEMENT TYPE")]
    public mobilityType mobility = mobilityType.NONE;
    public iman myPole = iman.NONE;
    public LayerMask whatCanBeImanted;
    private Rigidbody myRB;

    [Header("FORCES")]
    [SerializeField] float force = 3;
    [SerializeField] float timerImanted = 8f;
    [SerializeField] float timerActive = 3f;
    [SerializeField] float timeImanted = 8f;
    [SerializeField] float timeActive = 3f;
    bool applyForce = false;
    Vector3 directionForce;
    Collider[] others;
    GameObject otherGO;
    forceType myForceType = forceType.NONE;
    int numChargesAdded = 0;
    // Start is called before the first frame update

    //OUTLINE
    private Outline outline;

    void Start()
    {
        myRB = this.GetComponent<Rigidbody>();
        
        if (mobility != mobilityType.JUSTPOLE)
        {
            mysphereCollider = this.GetComponentInChildren<SphereCollider>();
            mysphereCollider.radius = 0.5f;
        }
        
        nearImantableObjects = new List<GameObject>();
        timerActive = timeActive;
        timerImanted = timeImanted;

        //OUTLINE SET
        outline = this.GetComponent<Outline>();
        outline.enabled = false;

        //outline.OutlineColor = new Color32(0, 0, 0, 0);
    }

    private void Update()
    {
        if (mobility != mobilityType.JUSTPOLE)
        {
            if (myPole != iman.NONE)
                CalculateDirectionForce();
        }
    }

    private void FixedUpdate()
    {
        if (mobility != mobilityType.JUSTPOLE)
        {
            if (myPole != iman.NONE)
            {
                if (applyForce && mobility == mobilityType.MOBILE)
                {
                    //Debug.Log("ha de palicart la fuerza : " + directionForce * force);
                    myRB.AddForce(directionForce * force, ForceMode.Force);
                    directionForce = new Vector3(0, 0, 0);
                    timerActive -= Time.fixedDeltaTime;
                    if (timerActive <= 0)
                        ResetObject();
                }
                else
                    timerImanted -= Time.fixedDeltaTime;
                if (timerImanted <= 0)
                    ResetObject();
            }
        }
    }

    #region UPDATING ELEMENTS NEAR
    private void OnTriggerEnter(Collider other)
    {        
        if (myPole != iman.NONE)
        {
            if (other.gameObject.transform.parent != null)
            {                
                if (other.gameObject.transform.parent.gameObject.tag == "CanBeHitted")
                    if (!nearImantableObjects.Contains(other.gameObject.transform.parent.gameObject))
                    {
                        nearImantableObjects.Add(other.gameObject.transform.parent.gameObject);
                    }
            }            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            nearImantableObjects.Remove(other.gameObject);
        }
    }
    #endregion

    #region calculate Forces

    void CalculateDirectionForce()
    {
        bool hay = false;
        foreach (GameObject obj in nearImantableObjects)
        {
            
            //comprobamos q se tenga q calcular
            if (obj.GetComponent<ImanBehavior>().myPole != iman.NONE)
            {
                if (obj.GetComponent<ImanBehavior>().myPole == myPole)
                {
                    //Repulsion
                    directionForce += CalculateOneForce(this.gameObject, obj, forceType.REPULSE);
                }
                else if (obj.GetComponent<ImanBehavior>().myPole != myPole)
                {
                    //atraccion
                    directionForce += CalculateOneForce(this.gameObject, obj, forceType.ATRACT);
                }
                hay = true;
            }
        }
        if (hay)
            applyForce = true;
    }

    private Vector3 CalculateOneForce(GameObject myGO, GameObject otherGO, forceType typeOfForce)
    {
        Vector3 finalForce = new Vector3(0, 0, 0);
        //Suma de cargas
        float numChargesSum = (numChargesAdded + otherGO.GetComponent<ImanBehavior>().numChargesAdded)*2f;

        switch (typeOfForce)
        {
            case forceType.ATRACT:
                finalForce = CalculateVectorAB(myGO.transform.position, otherGO.transform.position);
                break;
            case forceType.REPULSE:
                finalForce = CalculateVectorAB(otherGO.transform.position, myGO.transform.position);
                break;
            case forceType.NONE:
                break;
            default:
                break;
        }
        float invertedDistance = (1f / finalForce.magnitude * numChargesSum * force);

        finalForce = finalForce.normalized * invertedDistance;
        //Debug.Log(finalForce);

        return finalForce;
    }

    private Vector3 CalculateVectorAB(Vector3 A, Vector3 B)
    {
        Vector3 result = new Vector3(B.x - A.x, B.y - A.y, B.z - A.z);
        return result;
    }

    #endregion

    public void AddCharge(iman typeIman, int numCharge, Rigidbody bullet)
    {

        if (numCharge > 1)
        {
            //Primero asignamos polo para que no haya problemas en otra parte del codigo
            if (typeIman == iman.POSITIVE)
            {
                myPole = iman.POSITIVE;
                outline.OutlineColor = new Color32(255, 0, 0, 255);
            }
            else
            {
                myPole = iman.NEGATIVE;
                outline.OutlineColor = new Color32(0, 0, 255, 255);
            }

            //ACTIVAMOS SCRIPT OUTLINE
            outline.enabled = true;

            if (mobility != mobilityType.JUSTPOLE)
            {

                numChargesAdded = numCharge;

                //En caso de tener los radius hardcoded aqui. SINO Cambiarlo a las dos lineas del switch
                switch (numCharge)
                {
                    case 1:
                        mysphereCollider.enabled = true;
                        mysphereCollider.radius = 3.5f;
                        break;
                    case 2:
                        mysphereCollider.enabled = true;
                        mysphereCollider.radius = 4.5f;
                        break;
                    case 3:
                        mysphereCollider.enabled = true;
                        mysphereCollider.radius = 8;
                        break;
                    default:
                        break;
                }
                //Reset timers
                timerActive = timeActive;
                timerImanted = timeImanted;
            }
        }
        else
        {
            //Behavior if is no charge bullet
            myRB.AddForce(bullet.velocity.normalized*2, ForceMode.Impulse);
        }
    }

    private void ResetObject()
    {
        this.gameObject.tag = "CanBeHitted";
        nearImantableObjects.Clear();
        myPole = iman.NONE;
        applyForce = false;
        timerActive = timeActive;
        timerImanted = timeImanted;
        mysphereCollider.radius = 0.5f;
        mysphereCollider.enabled = false;

        //OUTLINE
        outline.OutlineColor = new Color32(0, 0, 0, 0);
        outline.enabled = false;
    }

}
