using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Timeline : MonoBehaviour {

    public float interval = 0.1f;
    int index = 0;
    List<Vector3> storedPos = new List<Vector3>();
    List<Vector3> storedRot = new List<Vector3>();
    Vector3 lastPos, lastRot;
    Rigidbody2D rb;
    float cooldown = 0;
    private float buttonPressedTime = 0;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        lastPos = transform.position;
        lastRot = transform.rotation.eulerAngles;
        storedPos.Add(lastPos);
        storedRot.Add(lastRot);
        InvokeRepeating("StoreInfo", 0, interval);
        rb.gravityScale = 1;
    }

    void Update() {
        if(Input.GetMouseButton(0)) {
            buttonPressedTime += Time.deltaTime;
            if(index < storedPos.Count) {
                if(buttonPressedTime > 0.2f) {
                    rb.isKinematic = true;
                    RewindTime();
                    transform.position = Vector3.Lerp(transform.position, storedPos[storedPos.Count - index], interval);
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(storedRot[storedRot.Count - index]), interval);
                    cooldown -= Time.deltaTime;
                }
            }
        } else if(Input.GetMouseButtonUp(0) && buttonPressedTime > 0.2f) {
            rb.isKinematic = false;
            buttonPressedTime = 0;
            index = storedPos.Count;
            //storedPos.Clear();
            //storedRot.Clear();
        }
        Debug.Log("Count: " + storedPos.Count + " Index: " + index);
    }

    void RewindTime() {

        if(cooldown <= 0 && buttonPressedTime > 0.2f) {
            cooldown = interval;
            index++;
        }
    }

    void StoreInfo() {
        Vector3 pos = transform.position;
        Vector3 rot = transform.rotation.eulerAngles;

        if(!Input.GetMouseButton(0) && buttonPressedTime < 0.2f) {
            storedPos.Add(pos);
            storedRot.Add(rot);
            //Debug.Log(storedPos[storedPos.Count - 1]);
            //Debug.Log(storedRot[storedRot.LastIndexOf(transform.rotation.eulerAngles)]);
            lastPos = pos;
            lastRot = rot;
        }
    }
}

