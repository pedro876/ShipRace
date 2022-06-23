using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    Transform player;

    [SerializeField] GameObject sectionPrefab;
    [SerializeField] int numOfSections = 3;
    [SerializeField] int initialSectionWithoutObstacles = 3;
    [SerializeField] float distanceToReplaceLast = 60f;
    Queue<TunnelSection> sections;
    TunnelSection lastSection;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>().transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        sections = new Queue<TunnelSection>();
        for (int i = 0; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).gameObject);
        for(int i = 0; i < numOfSections; i++)
        {
            var section = Instantiate(sectionPrefab, transform);
            var tunnelSection = section.GetComponent<TunnelSection>();
            tunnelSection.Place(new Vector3(0f, 0f, i * tunnelSection.Length), Quaternion.identity, i >= initialSectionWithoutObstacles);
            sections.Enqueue(tunnelSection);
            if(i == numOfSections-1)
                lastSection = tunnelSection;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var currentSection = sections.Peek();

        if(player.transform.position.z > currentSection.transform.position.z + distanceToReplaceLast)
        {
            ReplaceFirstToLast();
        }
    }

    private void ReplaceFirstToLast()
    {
        //var first = sections.First;
        var first = sections.Dequeue();
        first.Place(
            lastSection.transform.localPosition + Vector3.forward * (lastSection.Length / 2f + first.Length / 2f),
            Quaternion.identity);
        lastSection = first;
        sections.Enqueue(first);
    }
}
