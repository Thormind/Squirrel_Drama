using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormColliderController : MonoBehaviour
{
    public GameObject wormModel;
    public GameObject capsule;
    public GameObject wormHead;
    public Vector3 holePosition;
    public Vector3 headBonePosition;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Transform[] allBones = wormModel.GetComponentsInChildren<Transform>();
        //FindHighestBone(allBones);
        capsule.transform.SetPositionAndRotation(wormHead.GetComponent<Transform>().position, Quaternion.identity);
    }

    public void FindHighestBone(Transform[] allBonesToCheck)
    {
        float HighestDistance = 0;

        foreach (Transform boneTransform in allBonesToCheck)
        {
            Vector3 bonePosition = boneTransform.position;
            float checkedDistance = Vector3.Distance(bonePosition, holePosition);


            //Debug.Log(checkedDistance);

            if (checkedDistance > HighestDistance)
            {
                Debug.Log("distance: " + checkedDistance);
                headBonePosition = bonePosition;
                HighestDistance = checkedDistance;
                //Debug.Log(highestBoneTemp);
                //Debug.Log(HighestDistance);
            }
        }
    }



}
