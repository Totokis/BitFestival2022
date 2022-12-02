using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private const Single UNDERGOUND_Y = -2.72f;

    private List<Single> _activeLevels = new List<Single>();

    public Transform trMapParent;

    public GameObject objGroundPrefab;
    public GameObject objCablePrefab;

    private Single _lastGroundX = 0;
    private Single _thisSegmentCableX = -8.19f;

    private List<GameObject> _generatedUnderGrounds = new List<GameObject>();

    private List<GameObject> _generatedCables = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        _activeLevels = new List<float>()
        {
            -0.5f,
            -2f,
            -3.5f,
        };

        StartCoroutine(MapGeneratorCor());
    }


    private Int32 _generationsCount = 0;
    private IEnumerator MapGeneratorCor()
    {
        _generationsCount++;

        GameObject newUnderGround = Instantiate(objGroundPrefab, trMapParent);
        newUnderGround.transform.localPosition = new Vector3(_lastGroundX, UNDERGOUND_Y, 0);

        _lastGroundX += newUnderGround.GetComponent<RectTransform>().sizeDelta.x;

        _generatedUnderGrounds.Add(newUnderGround);

        Int32 cableSegmentsInBacgkround = 11;
        Single cableWidthTakingScale = objCablePrefab.GetComponent<RectTransform>().sizeDelta.x * objCablePrefab.transform.localScale.x;
        for (Int32 i = 0; i < cableSegmentsInBacgkround; i++)
        {
            foreach (Single level in _activeLevels)
            {
                GameObject cable = Instantiate(objCablePrefab, trMapParent);
                cable.transform.localPosition = new Vector3(_thisSegmentCableX + i * cableWidthTakingScale, level, 0);
                _generatedCables.Add(cable);
            }
        }

        Single totalCablesWidthThisSegment = cableSegmentsInBacgkround * cableWidthTakingScale;
        _thisSegmentCableX += totalCablesWidthThisSegment;

        yield return new WaitForSeconds(_generationsCount > 3 ? 2.5f : 0f);

        StartCoroutine(MapGeneratorCor());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
