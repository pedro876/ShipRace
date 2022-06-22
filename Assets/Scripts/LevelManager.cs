using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject sectionPrefab;
    [SerializeField] int numOfSections = 3;
    [SerializeField] float sectionLength = 20f;

    //LinkedList<GameObject> sections;

    // Start is called before the first frame update
    void Start()
    {
        //sections = new LinkedList<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).gameObject);
        for(int i = 0; i < numOfSections; i++)
        {
            var section = Instantiate(sectionPrefab, transform);
            section.transform.localPosition = new Vector3(0f, 0f, i * sectionLength);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ReplaceFirstToLast()
    {
        //var first = sections.First;
    }
}
