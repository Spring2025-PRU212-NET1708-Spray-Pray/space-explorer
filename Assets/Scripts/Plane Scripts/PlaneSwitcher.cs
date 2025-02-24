using UnityEngine;
using UnityEngine.UIElements;

public class PlaneSwitcher : MonoBehaviour
{
    public GameObject spaceShip1;
    public GameObject spaceShip2;

    private GameObject currentShip;
    private Vector3 shipPosition;
    private Quaternion shipRotation;
    private int bulletLevel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shipPosition = new Vector3(0, -3.8f, 0);
        shipRotation = Quaternion.identity;
        currentShip = Instantiate(spaceShip1, shipPosition, shipRotation);   // Rotate the ship 180 degrees
        StoreBulletLevel(currentShip);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Right click
        {
            SwapShip();
        }
    }

    private void SwapShip()
    {
        // Store the current ship's bullet level
        StoreBulletLevel(currentShip);
        // Store the current ship's position
        shipPosition = currentShip.transform.position;
        // Store the current ship's rotation
        Quaternion shipRotation = currentShip.transform.rotation;
        // Destroy the current ship
        Destroy(currentShip);
        // Switch the ship
        if (currentShip.CompareTag("Ship1"))
        {
            currentShip = Instantiate(spaceShip2, shipPosition, shipRotation);   
            //currentShip.tag = "Ship2";
        }
        else
        {
            currentShip = Instantiate(spaceShip1, shipPosition, shipRotation);  
            //currentShip.tag = "Ship1";
        }

        // Set the bullet level of the new ship
        currentShip.GetComponent<Plane>().SetBulletLevel(bulletLevel);
    }

    private void StoreBulletLevel(GameObject ship)
    {
        bulletLevel = ship.GetComponent<Plane>().GetBulletLevel();
    }

    public string GetCurrentShipTag()
    {
        return currentShip.tag;
    }

}
