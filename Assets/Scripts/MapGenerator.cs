using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    private const Single UNDERGOUND_Y = -2.72f;

    private List<Single> _activeLevels = new List<Single>();

    public Transform trMapParent;

    public GameObject objGroundPrefab;
    public GameObject objCablePrefab;
    public PowerableActivationNode objPowerableActivationNodePrefab;
    public List<PowerableObject> _powerableObjects = new List<PowerableObject>();

    private Single _lastGroundX = 0;
    private Single _thisSegmentCableX = -8.19f;


    private List<GameObject> _generatedUnderGrounds = new List<GameObject>();
    private List<GameObject> _generatedCables = new List<GameObject>();
    private List<GameObject> _generatedPowerables = new List<GameObject>();

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

        Single thisGenerationSegmentStartXBefore = _lastGroundX;
        Single totalUndergroundSegmentWidth = GenerateUndergrounds();
        thisGenerationSegmentStartXBefore -= (totalUndergroundSegmentWidth / 2f);
        GenerateCables();
        GeneratePowerables(thisGenerationSegmentStartXBefore, totalUndergroundSegmentWidth);

        yield return new WaitForSeconds(_generationsCount > 3 ? 2.5f : 0f);

        StartCoroutine(MapGeneratorCor());
    }

    private void GeneratePowerables(Single beforeStartX, Single totalSegmentWidth)
    {
        Int32 powerablesInThisSegment = Random.Range(1, 4);

        for (Int32 i = 0; i < powerablesInThisSegment; i++)
        {
            PowerableObject powerable = Instantiate(_powerableObjects[0], trMapParent);
            Single thisPowerableX = beforeStartX + ((totalSegmentWidth / (powerablesInThisSegment + 1)) * (i + 1));
            powerable.transform.localPosition = new Vector3(thisPowerableX,
                powerable.PositionY, 0f);

            PowerableActivationNode[] nodes = new PowerableActivationNode[1];
            for(Int32 n = 0; n < 1; n++)
            {
                PowerableActivationNode newNode = Instantiate(objPowerableActivationNodePrefab, trMapParent);
                newNode.transform.localPosition = new Vector3(thisPowerableX, _activeLevels[Random.Range(0, _activeLevels.Count)]);
                nodes[n] = newNode;
            }

            powerable.AttachPowerableActivationNodes(nodes);

            _generatedPowerables.Add(powerable.gameObject);
        }
    }

    private void GenerateCables()
    {
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
    }

    private Single GenerateUndergrounds()
    {
        GameObject newUnderGround = Instantiate(objGroundPrefab, trMapParent);
        newUnderGround.transform.localPosition = new Vector3(_lastGroundX, UNDERGOUND_Y, 0);
        Single totalSegmentWidth = newUnderGround.GetComponent<RectTransform>().sizeDelta.x;
        _lastGroundX += totalSegmentWidth;

        _generatedUnderGrounds.Add(newUnderGround);

        return totalSegmentWidth;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
