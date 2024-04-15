using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float speed;
    public Renderer Renderer;
    public List<GameObject> Elements;

    private GameObject currentElement;
    private bool elementPresent = false;

    void FixedUpdate()
    {
        Renderer.material.mainTextureOffset += new Vector2(0, speed * Time.deltaTime);
        SpawnElement();
        if (currentElement != null)
        {
            currentElement.transform.position += new Vector3(0, -speed * 10 * Time.deltaTime, 0);
            if(currentElement.transform.position.y < -10)
            {
                Destroy(currentElement);
                elementPresent = false;
            }
        }
    }

    private void SpawnElement()
    {
        if (!elementPresent)
        {
            elementPresent = true;
            int pick = Random.Range(0, Elements.Count);
            currentElement = GameObject.Instantiate(Elements[pick]);
            float xSpawn = Random.Range(-9f, 9f);
            currentElement.transform.position = new Vector3(xSpawn, 10f, 5);
            currentElement.SetActive(true);
        }
    }
}
