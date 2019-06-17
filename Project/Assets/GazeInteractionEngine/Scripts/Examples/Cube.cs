using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{

    private MeshRenderer _meshRenderer;
    private Vector3 startPosition;
    public bool isMoving = false;

    // Start is called before the first frame update
    private void Start()
    {
        startPosition = transform.position;
        _meshRenderer = GetComponent<MeshRenderer>();
    }




    public void PlaySound()
    {
        this.GetComponent<AudioSource>().Play();
        _meshRenderer.material.color = Color.green;
    }

    public void OnEndInteraction()
    {
        _meshRenderer.material.color = Color.red;
    }

    public void ChangeColor()
    {
        _meshRenderer.material.color = new Color(Mathf.Sin(Time.frameCount*0.01f), Mathf.Cos(Time.frameCount*0.01f), 1.0f);
       
    }

    public void Update()
    {
        if (isMoving)
            transform.position = new Vector3(startPosition.x, startPosition.y + Mathf.Sin(Time.frameCount * 0.05f) * 2, startPosition.z);
    }
}
