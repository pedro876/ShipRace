using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    Transform player;

    [SerializeField] GameObject initSection;
    [SerializeField] GameObject[] sectionPrefabs;
    [SerializeField] GameObject[] curvePrefabs;
    [SerializeField] int numOfSections = 3;
    [SerializeField] int sectionsBetweenCurves = 10;
    [SerializeField] int initialSectionsWithoutObstacles = 3;
    [SerializeField] int delaySectionReplacement = 1;
    //[SerializeField] float distanceToReplaceLast = 60f;
    Queue<Tunnel> sectionsToRemove;
    Queue<Tunnel> sections;
    Tunnel lastSection;
    int sectionIndex = 0;
    int sectionType = 0;

    public Tunnel currentSection => sections.Peek();

    private void Awake()
    {
        player = FindObjectOfType<ShipController>().transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        sections = new Queue<Tunnel>();
        sectionsToRemove = new Queue<Tunnel>();
        for (int i = 0; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).gameObject);
        for(int i = 0; i < numOfSections; i++)
        {
            AddSection();
        }
    }

    private void AddSection()
    {
        GameObject sectionObj;
        Tunnel section;
        Vector3 pos;
        Quaternion rot;
        if(sectionIndex == 0) //Init
        {
            sectionObj = Instantiate(initSection, transform);
            sectionType = Random.Range(0, sectionPrefabs.Length);
            pos = transform.position;
            rot = transform.rotation;
        }
        else
        {
            if (sectionIndex % sectionsBetweenCurves == 0) //Curve
            {
                var curve = RandomSection(curvePrefabs);
                sectionObj = Instantiate(curve, transform);
                sectionType = Random.Range(0, sectionPrefabs.Length);
            }
            else //Normal section
            {
                sectionObj = Instantiate(sectionPrefabs[sectionType], transform);
            }
            pos = lastSection.TunnelEnd.position;
            rot = lastSection.TunnelEnd.rotation;
        }

        sectionObj.name = "Section_" + sectionIndex;
        section = sectionObj.GetComponent<Tunnel>();
        section.Place(pos, rot, sectionIndex > initialSectionsWithoutObstacles && sectionIndex % sectionsBetweenCurves != 1);
        sections.Enqueue(section);
        lastSection = section;
        sectionIndex++;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(currentSection.gameObject.name);
        if(currentSection.TunnelEnd.InverseTransformPoint(player.position).z > 0f)
        {
            RemoveFirstSection();
        }
    }

    private void RemoveFirstSection()
    {
        var first = sections.Dequeue();
        //first.Place(lastSection.TunnelEnd.position, lastSection.TunnelEnd.rotation);
        sectionsToRemove.Enqueue(first);
        if(sectionsToRemove.Count > delaySectionReplacement)
        {
            var sectionToRemove = sectionsToRemove.Dequeue();
            Destroy(sectionToRemove.gameObject);
            AddSection();
            /*sections.Enqueue(sectionToReplace);
            sectionToReplace.Place(lastSection.TunnelEnd.position, lastSection.TunnelEnd.rotation);
            lastSection = sectionToReplace;*/
        }

        //lastSection = first;
        //sections.Enqueue(first);
    }

    private GameObject RandomSection(GameObject[] sectionList)
    {
        int rnd = Random.Range(0, sectionList.Length);
        return sectionList[rnd];
    }
}
