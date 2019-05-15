using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooting : MonoBehaviour
{

    public enum ShipType
    {
        NORMAL, POWER, HEAVY
    }

    public ShipType shipType;

    public enum Factions
    {
        USA, URSS, ALIEN
    }

    public Factions faction;

    public GameObject bullet;
    public GameObject reference1;
    public GameObject reference2;

    public GameObject arms;
    public GameObject enemyTarget;
    private Vector3 enemyPos;


    public float shootingPower;
    bool shootingTurn;
    float turnTime;
    int normalBulletsNumber = 5;
    int powerBulletsNumber = 3;
    int heavyBulletsNumber = 2;

    // Use this for initialization
    void Start()
    {
       //attack();

    }

    private void Update()
    {

        if(shootingTurn)turnTime += Time.deltaTime;
        if (shootingTurn)
        {

            if (turnTime != 0)
            {

                arms.transform.Rotate(0, Time.deltaTime * 120 / 0.5f, 0);

            }
        }
    }

    /*
    public void shoot()
    {
        if (shipType == ShipType.NORMAL) {

            GameObject newBullet = Instantiate(bullet, reference1.transform.position, new Quaternion());
            newBullet.transform.rotation = reference1.transform.rotation;
            newBullet.GetComponent<Rigidbody>().velocity = reference1.transform.up.normalized * shootingPower;
            newBullet.GetComponent<Bullet>().target = enemyTarget;
            newBullet.GetComponent<Bullet>().dmg = GetComponentInParent<Unit>().Damage;
            newBullet.GetComponent<Bullet>().targetPos = enemyPos;

            GameObject newBullet2 = Instantiate(bullet, reference2.transform.position, new Quaternion());
            newBullet2.transform.rotation = reference2.transform.rotation;
            newBullet2.GetComponent<Rigidbody>().velocity = reference2.transform.up.normalized * shootingPower;
            newBullet2.GetComponent<Bullet>().target = enemyTarget;
            newBullet2.GetComponent<Bullet>().dmg = GetComponentInParent<Unit>().Damage;
            newBullet2.GetComponent<Bullet>().targetPos = enemyPos;
        }

        if (shipType == ShipType.POWER)
        {
            shootingTurn = true;
            Invoke("turnFinish", 0.5f);
            GameObject newBullet = Instantiate(bullet, reference1.transform.position, new Quaternion());
            newBullet.transform.rotation = reference1.transform.rotation;
            newBullet.GetComponent<Rigidbody>().velocity = reference1.transform.up.normalized * shootingPower;
            newBullet.GetComponent<Bullet>().target = enemyTarget;
            newBullet.GetComponent<Bullet>().setDistanceMultiplier(); //estas balas necesitan que la distancia a la que deben ser destruidas sea mayor, porque vuelan más alto
            newBullet.GetComponent<Bullet>().dmg = GetComponentInParent<Unit>().Damage;
            newBullet.GetComponent<Bullet>().targetPos = enemyPos;
        }

        if (shipType == ShipType.HEAVY)
        {

            GameObject newBullet = Instantiate(bullet, reference1.transform.position, new Quaternion());
            newBullet.transform.rotation = reference1.transform.rotation;
            newBullet.GetComponent<Rigidbody>().velocity = reference1.transform.up.normalized * shootingPower;
            newBullet.GetComponent<Bullet>().target = enemyTarget;
            newBullet.GetComponent<Bullet>().dmg = GetComponentInParent<Unit>().Damage;
            newBullet.GetComponent<Bullet>().targetPos = enemyPos;
        }

    }
    */
    void cancelAllInvokes()
    {
        shootingTurn = false;
        turnTime = 0f;
        //timerGap = 0f;
        CancelInvoke();
    }
    void turnFinish()
    {
        shootingTurn = false;
        turnTime = 0f;
    }

    IEnumerator shoot ()
    {
        yield return new WaitForSeconds(0.5f);
        switch (shipType)
        {            
            case ShipType.NORMAL:
                for (int i = 0; i < normalBulletsNumber; i++)
                {
                    yield return new WaitForSeconds(0.25f);
                    GameObject newBullet = Instantiate(bullet, reference1.transform.position, new Quaternion());
                    newBullet.transform.rotation = reference1.transform.rotation;
                    newBullet.GetComponent<Rigidbody>().velocity = reference1.transform.up.normalized * shootingPower;
                    newBullet.GetComponent<Bullet>().target = enemyTarget;
                    newBullet.GetComponent<Bullet>().dmg = GetComponentInParent<Unit>().Damage;
                    newBullet.GetComponent<Bullet>().targetPos = enemyPos;
                    if (i == normalBulletsNumber - 1)
                    {
                        newBullet.GetComponent<Bullet>().shtng = this;
                        newBullet.GetComponent<Bullet>().amITheLastBullet = true;
                    }

                    GameObject newBullet2 = Instantiate(bullet, reference2.transform.position, new Quaternion());
                    newBullet2.transform.rotation = reference2.transform.rotation;
                    newBullet2.GetComponent<Rigidbody>().velocity = reference2.transform.up.normalized * shootingPower;
                    newBullet2.GetComponent<Bullet>().target = enemyTarget;
                    newBullet2.GetComponent<Bullet>().dmg = GetComponentInParent<Unit>().Damage;
                    newBullet2.GetComponent<Bullet>().targetPos = enemyPos;
                }
                break;
            case ShipType.POWER:
                for (int i = 0; i < powerBulletsNumber; i++)
                {
                    yield return new WaitForSeconds(0.5f);
                    shootingTurn = true;
                    Invoke("turnFinish", 0.5f);
                    GameObject newBullet = Instantiate(bullet, reference1.transform.position, new Quaternion());
                    newBullet.transform.rotation = reference1.transform.rotation;
                    newBullet.GetComponent<Rigidbody>().velocity = reference1.transform.up.normalized * shootingPower;
                    newBullet.GetComponent<Bullet>().target = enemyTarget;
                    newBullet.GetComponent<Bullet>().setDistanceMultiplier(); //estas balas necesitan que la distancia a la que deben ser destruidas sea mayor, porque vuelan más alto
                    newBullet.GetComponent<Bullet>().dmg = GetComponentInParent<Unit>().Damage;
                    newBullet.GetComponent<Bullet>().targetPos = enemyPos;
                    if (i == powerBulletsNumber - 1)
                    {
                        newBullet.GetComponent<Bullet>().shtng = this;
                        newBullet.GetComponent<Bullet>().amITheLastBullet = true;
                    }
                }
                break;
            case ShipType.HEAVY:
                for (int i = 0; i < heavyBulletsNumber; i++)
                {
                    yield return new WaitForSeconds(1f);
                    GameObject newBullet = Instantiate(bullet, reference1.transform.position, new Quaternion());
                    newBullet.transform.rotation = reference1.transform.rotation;
                    newBullet.GetComponent<Rigidbody>().velocity = reference1.transform.up.normalized * shootingPower;
                    newBullet.GetComponent<Bullet>().target = enemyTarget;
                    newBullet.GetComponent<Bullet>().dmg = GetComponentInParent<Unit>().Damage;
                    newBullet.GetComponent<Bullet>().targetPos = enemyPos;
                    if (i == heavyBulletsNumber - 1)
                    {
                        newBullet.GetComponent<Bullet>().shtng = this;
                        newBullet.GetComponent<Bullet>().amITheLastBullet = true;
                    }
                }
                break;            
        }
    }

    public void attack(GameObject target)
    {
        enemyTarget = target;
        //Nos guardamos por separado la posición porque habrá balas que salgan cuando la nave enemiga ya esté destruida y necesitamos esa posición
        //para que las balas se destruyan
        enemyPos = target.transform.position;
        shootingTurn = false;
        turnTime = 0f;
        StartCoroutine(shoot());
        /*
        if (shipType == ShipType.NORMAL) InvokeRepeating("shoot", 0.5f, 0.25f);
        else if (shipType == ShipType.POWER) InvokeRepeating("shoot", 0.5f, 0.5f);
        else if (shipType == ShipType.HEAVY) InvokeRepeating("shoot", 0.5f, 1f);
        Invoke("cancelAllInvokes", 2f);        
        */        
    }    
}
