using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    //Sonsuz Gidis
    public float speed, camSpeed = 1.1f;
    private float forwardSpeed = 5;
    public GameObject tempCam;
    public GameObject platform;

    private Touch touch;
    private Vector3 touchDown;
    private Vector3 touchUp;

    private bool dragStarted;
    [SerializeField] private bool isMoving = true;
    private bool isDragable = true;



    private void Start()
    {
        // Screen.SetResolution(Screen.currentResolution.width / 100, Screen.currentResolution.height / 100, true);
        StartCoroutine(SpawnLevel());
        StartCoroutine(BeSlow());
    }

    private void Update()
    {
        //Player Acceleration
        transform.Translate(Vector3.forward * Time.deltaTime * forwardSpeed);
        //Camera Acceleration
        tempCam.transform.Translate(0, 0, Time.deltaTime * forwardSpeed, Space.World);
        //Touch Controller
        if (Input.touchCount > 0)
        {
            //Touch Starter
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                dragStarted = true;
                isMoving = true;
                touchDown = touch.position / 10;
                touchUp = touch.position / 10;
            }
        }
        if (dragStarted)
        {
            //Touch Swipe
            if (touch.phase == TouchPhase.Moved)
            {

                touchDown = touch.position / 10;

            }
            //Touch Stability
            if (touch.phase == TouchPhase.Stationary)
                touchUp = touchDown;
            //Touch Finish
            if (touch.phase == TouchPhase.Ended)
            {
                touchDown = touch.position / 10;
                isMoving = false;
                dragStarted = false;
            }
            if (isMoving && isDragable)
                transform.Translate(new Vector3(CalculateDirection().x, 0, 0) * Time.deltaTime * 10);
            //Max and Min Range
            Mathf.Clamp(transform.position.x, -1.5f, 1.5f);
            //gameObject.transform.position = new Vector3(Mathf.Clamp(CalculateDirection().x, -1.5f, 1.5f), 1, transform.position.z);
        }
        //Dragable Positions
        if (transform.position.x >= -1.5f && transform.position.x <= 1.5f)
            isDragable = true;
        else
        {
            //Change Position To Dragable Position
            isDragable = false;
            if (transform.position.x >= -1.5f)
            {
                transform.position = new Vector3(1.5f, 1, transform.position.z);
                touchUp = touchDown;
            }
            else if (transform.position.x <= 1.5f)
            {
                touchUp = touchDown;
                transform.position = new Vector3(-1.5f, 1, transform.position.z);
            }



        }
    }

    Vector3 CalculateDirection()
    {
        Vector3 temp = (touchDown - touchUp) / 15; //1.5 ile -1.5 arasýnda deðer alabilir
        temp.y = 1;
        return temp;
    }

    IEnumerator SpawnLevel()
    {

        Vector3 pos = new Vector3(0, 0, transform.position.z);
        GameObject platformObject = Instantiate(platform, pos, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        StartCoroutine(SpawnLevel());
        StartCoroutine(WaitforDestroy(platformObject));
    }

    IEnumerator WaitforDestroy(GameObject plat)
    {
        yield return new WaitForSeconds(3f);
        Destroy(plat);
    }
    IEnumerator BeSlow()
    {
        touchUp = touchDown;
        yield return new WaitForSeconds(0.1f);

        StartCoroutine(BeSlow());
    }
}
//Written By SloXeN ---------
