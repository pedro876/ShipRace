using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShipDestroy : MonoBehaviour
{
    private bool isReconstructing = false;
    private bool isDestroyed = false;
    [SerializeField] float reconstructTime = 1f;
    [SerializeField] float explodeForce = 1f;
    [SerializeField] float explodeAngularForce = 2f;
    //[SerializeField] PhysicMaterial physicMat;
    Rigidbody[] rbs;
    Vector3[] partPos;
    Vector3[] srcPartPos;
    Quaternion[] partRots;
    Quaternion[] srcPartRots;
    private bool exploded = false;

    public event Action onFinishedReconstruction;
    SmoothFloat anim;

    public void Place(Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
    }

    public void ExplodeParts(Vector3 direction)
    {
        gameObject.SetActive(true);
        if (exploded) return;
        foreach(var rb in rbs)
        {
            rb.isKinematic = false;
            rb.velocity = RandomVec() * explodeForce;
            rb.angularVelocity = RandomVec() * explodeAngularForce;
        }
        isDestroyed = true;
    }

    private Vector3 RandomVec()
    {
        return new Vector3(
            UnityEngine.Random.Range(-1f, 1f),
            UnityEngine.Random.Range(-1f, 1f),
            UnityEngine.Random.Range(-1f, 1f));
    }

    void Awake()
    {
        rbs = new Rigidbody[transform.childCount];
        partPos = new Vector3[transform.childCount];
        partRots = new Quaternion[transform.childCount];
        srcPartPos = new Vector3[transform.childCount];
        srcPartRots = new Quaternion[transform.childCount];
        for(int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            partPos[i] = child.localPosition;
            partRots[i] = child.localRotation;
            rbs[i] = child.gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
            rbs[i].isKinematic = true;
        }
        anim = new SmoothFloat(0f, reconstructTime);
    }

    public void Reconstruct()
    {
        if (!isDestroyed) return;
        if (isReconstructing) return;
        for (int i = 0; i < transform.childCount; i++)
        {
            rbs[i].isKinematic = true;
            rbs[i].velocity = Vector3.zero;
            srcPartPos[i] = rbs[i].transform.localPosition;
            srcPartRots[i] = rbs[i].transform.localRotation;
        }
        anim.SetValueImmediate(0f);
        anim.SetValue(1f);
        isReconstructing = true;
    }

    private void Update()
    {
        if (isReconstructing)
        {
            if (anim.Update(Time.deltaTime))
            {
                for(int i = 0; i< transform.childCount; i++)
                {
                    rbs[i].transform.localPosition = Vector3.Lerp(srcPartPos[i], partPos[i], anim.Value);
                    rbs[i].transform.localRotation = Quaternion.Lerp(srcPartRots[i], partRots[i], anim.Value);
                }
            }
            else
            {
                isReconstructing = false;
                isDestroyed = false;
                onFinishedReconstruction?.Invoke();
            }
        }
        
    }
}
