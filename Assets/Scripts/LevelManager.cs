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
    Queue<TunnelSection> sectionsToRemove;
    Queue<TunnelSection> sections;
    TunnelSection lastSection;
    int sectionIndex = 0;
    int sectionType = 0;

    public TunnelSection currentSection => sections.Peek();

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>().transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        sections = new Queue<TunnelSection>();
        sectionsToRemove = new Queue<TunnelSection>();
        for (int i = 0; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).gameObject);
        for(int i = 0; i < numOfSections; i++)
        {
            AddSection();
            /*var section = Instantiate(RandomSection(), transform);
            section.name = section.name + i;
            var tunnelSection = section.GetComponent<TunnelSection>();
            if(i == 0)
            {
                tunnelSection.Place(transform.position, transform.rotation, i >= initialSectionWithoutObstacles);
            }
            else
            {
                tunnelSection.Place(lastSection.TunnelEnd.position, lastSection.TunnelEnd.rotation, i >= initialSectionWithoutObstacles);
            }
            
            sections.Enqueue(tunnelSection);
            lastSection = tunnelSection;*/

        }
    }

    private void AddSection()
    {
        GameObject sectionObj;
        TunnelSection section;
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
        section = sectionObj.GetComponent<TunnelSection>();
        section.Place(pos, rot, sectionIndex % sectionsBetweenCurves > initialSectionsWithoutObstacles);
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
