using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Hands;

public class FingerDataPanel : MonoBehaviour
{
    [SerializeField]
    private HandType handType;
    [SerializeField] 
    private List<FingerText> fingerTexts = new List<FingerText>();

    [Button]
    private void InitialInspector()
    {
        fingerTexts.Clear();
        var textMeshPros = FindObjectsOfType<TextMeshProUGUI>().ToList();
        var baseCurl = textMeshPros.Where(x=>x.transform.parent.name == "BaseCurl").ToList();
        var tipCurl = textMeshPros.Where(x=>x.transform.parent.name == "TipCurl").ToList();
        var fullCurl = textMeshPros.Where(x=>x.transform.parent.name == "FullCurl").ToList();
        for (var fingerID = 0; fingerID < Enum.GetValues(typeof(XRHandFingerID)).Length; fingerID++)
        {
            var finger = new FingerText
            {
                finger = (XRHandFingerID) fingerID,
                baseCurl = baseCurl.Find(x=>x.name == ((XRHandFingerID)fingerID).ToString()),
                tipCurl = tipCurl.Find(x=>x.name == ((XRHandFingerID)fingerID).ToString()),
                fullCurl = fullCurl.Find(x=>x.name == ((XRHandFingerID)fingerID).ToString())
            };
            fingerTexts.Add(finger);
        }
    }

    private void Update()
    {
        if(!HandManager.Instance) return;
        foreach (var finger in handType == HandType.Left
                     ? HandManager.Instance.leftHandFingers
                     : HandManager.Instance.rightHandFingers)
        {
            var fingerText = fingerTexts.Find(x => x.finger == finger.fingerType);
            fingerText.baseCurl.text = finger.baseCurl.ToString("F");
            fingerText.tipCurl.text = finger.tipCurl.ToString("F");
            fingerText.fullCurl.text = finger.fullCurl.ToString("F");
        }
    }
}

[Serializable]
public struct FingerText
{
    public XRHandFingerID finger;
    public TextMeshProUGUI baseCurl;
    public TextMeshProUGUI tipCurl;
    public TextMeshProUGUI fullCurl;
}
[Serializable]
public enum HandType
{
    Left,
    Right
}