using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CertificateData", menuName = "Custom/CertificateData", order = 1)]
public class CertificateData : ScriptableObject
{
    public List<FeatureRequirement> features = new List<FeatureRequirement>();
}

[System.Serializable]
public class FeatureRequirement
{
    public Feature.Type type;
    public int count = 2;
}


