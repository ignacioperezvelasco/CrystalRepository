using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TurretEnemy : MonoBehaviour
{
    #region VARIABLES
    [Header("PLAYER")]
    public Transform player;

    [Header("HEADER")]
    public Transform head;    
    public Transform eyeTurret;

    [Header("CROSSHAIR")]
    public Transform crosshair;
    public float followSpeed;
    public bool isInside = false;

    [Header("Shooting")]
    public float fireRate = 3;
    public Transform shootSpawner;
    public GameObject projectile;
    [Header("Particles Efect")]
    public GameObject chargeParticles;
    public float restartParticles = 0.3f;

    LineRenderer line;
    float shootTimer;
    #endregion

    #region UPDATE
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }
    #endregion

    #region UPDATE
    void Update()
    {
        //Pintamos la linea laser
        line.SetPosition(0, eyeTurret.position);
        line.SetPosition(1, crosshair.position);
        
        if (isInside)
        {
            //Actualizamos la posicion de la cabeza y crosshair
            crosshair.DOMove(new Vector3(player.position.x, crosshair.position.y, player.position.z) ,followSpeed);
            head.DOLookAt(new Vector3(player.position.x, crosshair.position.y - 1.5f, player.position.z), followSpeed);

            shootTimer += Time.deltaTime;
            if (shootTimer >= fireRate)
            {
                shootTimer = 0;
                Shoot();
            }
        }
    }
    #endregion

    #region SHOOT
    void Shoot()
    {
        chargeParticles.SetActive(false);
        Invoke("RestartParticles", restartParticles);

        //Instanciamos el proyectil
        GameObject go = Instantiate(projectile, shootSpawner.position, shootSpawner.rotation) as GameObject;
        //Guardamos el bullet script
        EnemyBullet bullet = go.GetComponent<EnemyBullet>();

        //Creamos la direccion del disparo
        Vector3 direction = crosshair.position - eyeTurret.position;


        //bullet.SetVelocity(direction.normalized);
    }
    #endregion

    #region RESTART PARTICLES
    void RestartParticles()
    {
        chargeParticles.SetActive(true);
    }
    #endregion

    #region TRIGGER ENTER
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //encendemos las particulas
            chargeParticles.SetActive(true);

            isInside = true;
        }
    }
    #endregion

    #region TRIGGER EXIT
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Paramos las particulas
            chargeParticles.SetActive(false);

            //reiniciamos las cosas
            shootTimer = 0;
            isInside = false;
        }
    }
    #endregion

}
