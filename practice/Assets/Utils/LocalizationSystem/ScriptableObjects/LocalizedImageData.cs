
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Scriptables/Skin/LocalizedImage")]
public class LocalizedImageData : ScriptableObject
{
    [PreviewField] public Sprite Korean_Image;
    [PreviewField] public Sprite English_Image;
    [PreviewField] public Sprite ChineseSimplified_Image;
    [PreviewField] public Sprite ChineseTraditional_Image;
    [PreviewField] public Sprite Japanese_Image;
    [PreviewField] public Sprite Spanish_Image;
    [PreviewField] public Sprite French_Image;
    [PreviewField] public Sprite German_Image;
    [PreviewField] public Sprite Portuguese_Image;
    [PreviewField] public Sprite Italian_Image;
}
