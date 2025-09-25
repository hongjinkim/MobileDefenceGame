// OdinDummyAttributes.cs

#if !ODIN_INSPECTOR
using System;
using UnityEngine;

namespace Sirenix.OdinInspector
{

    [AttributeUsage(AttributeTargets.All)]
    public class ShowInInspectorAttribute : Attribute { }


    [AttributeUsage(AttributeTargets.All)]
    public class ReadOnlyAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.All)]
    public class ButtonAttribute : Attribute
    {
        public ButtonAttribute() { }
        public ButtonAttribute(string name) { }
        public ButtonAttribute(ButtonSizes size) { }
        public ButtonAttribute(string name, ButtonSizes size) { }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class BoxGroupAttribute : Attribute
    {
        public BoxGroupAttribute(string group) { }
        public BoxGroupAttribute(string group, bool showLabel) { }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class FoldoutGroupAttribute : Attribute
    {
        public FoldoutGroupAttribute(string groupName) { }
        public FoldoutGroupAttribute(string groupName, bool expanded) { }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class TabGroupAttribute : Attribute
    {
        public TabGroupAttribute(string group) { }
        public TabGroupAttribute(string group, string tab) { }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class TitleAttribute : Attribute
    {
        public TitleAttribute(string title) { }
        public TitleAttribute(string title, string subtitle) { }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class InfoBoxAttribute : Attribute
    {
        public InfoBoxAttribute(string message) { }
        public InfoBoxAttribute(string message, InfoMessageType messageType) { }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class RequiredAttribute : Attribute
    {
        public RequiredAttribute() { }
        public RequiredAttribute(string errorMessage) { }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class ValidateInputAttribute : Attribute
    {
        public ValidateInputAttribute(string memberName) { }
        public ValidateInputAttribute(string memberName, string defaultMessage) { }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class PropertySpaceAttribute : Attribute
    {
        public PropertySpaceAttribute() { }
        public PropertySpaceAttribute(float spaceBefore) { }
        public PropertySpaceAttribute(float spaceBefore, float spaceAfter) { }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class OnValueChangedAttribute : Attribute
    {
        public OnValueChangedAttribute(string methodName) { }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class ShowIfAttribute : Attribute
    {
        public ShowIfAttribute(string condition) { }
        public ShowIfAttribute(string condition, object optionalValue) { }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class HideIfAttribute : Attribute
    {
        public HideIfAttribute(string condition) { }
        public HideIfAttribute(string condition, object optionalValue) { }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class EnableIfAttribute : Attribute
    {
        public EnableIfAttribute(string condition) { }
        public EnableIfAttribute(string condition, object optionalValue) { }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class DisableIfAttribute : Attribute
    {
        public DisableIfAttribute(string condition) { }
        public DisableIfAttribute(string condition, object optionalValue) { }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class MinValueAttribute : Attribute
    {
        public MinValueAttribute(float minValue) { }
        public MinValueAttribute(int minValue) { }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class MaxValueAttribute : Attribute
    {
        public MaxValueAttribute(float maxValue) { }
        public MaxValueAttribute(int maxValue) { }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class MinMaxSliderAttribute : Attribute
    {
        public MinMaxSliderAttribute(float minValue, float maxValue) { }
        public MinMaxSliderAttribute(string minValueGetter, float maxValue) { }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class PropertyRangeAttribute : Attribute
    {
        public PropertyRangeAttribute(float min, float max) { }
        public PropertyRangeAttribute(int min, int max) { }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class PropertyOrderAttribute : Attribute
    {
        public PropertyOrderAttribute(int order) { }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class LabelTextAttribute : Attribute
    {
        public LabelTextAttribute(string text) { }
    }


    [AttributeUsage(AttributeTargets.All)]
    public class TableListAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.All)]
    public class InlineEditorAttribute : Attribute
    {
        public InlineEditorAttribute() { }
        public InlineEditorAttribute(InlineEditorModes mode) { }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class PreviewFieldAttribute : Attribute
    {
        public PreviewFieldAttribute() { }
        public PreviewFieldAttribute(float height) { }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class AssetSelectorAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.All)]
    public class ValueDropdownAttribute : Attribute
    {
        public ValueDropdownAttribute(string valuesGetter) { }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class SceneObjectsOnlyAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.All)]
    public class AssetsOnlyAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.All)]
    public class FilepathAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.All)]
    public class FolderPathAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.All)]
    public class HorizontalGroupAttribute : Attribute
    {
        public HorizontalGroupAttribute(string groupID) { }
        public HorizontalGroupAttribute(string groupID, float width) { }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class VerticalGroupAttribute : Attribute
    {
        public VerticalGroupAttribute(string groupID) { }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class SearchableAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.All)]
    public class EnumPagingAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.All)]
    public class EnumToggleButtonsAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.All)]
    public class InlinePropertyAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.All)]
    public class LabelWidthAttribute : Attribute
    {
        public LabelWidthAttribute(float width) { }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class ToggleGroupAttribute : Attribute
    {
        public ToggleGroupAttribute(string toggleMemberName) { }
        public ToggleGroupAttribute(string toggleMemberName, string groupTitle) { }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class ToggleLeftAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.All)]
    public class HideLabelAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.All)]
    public class DisableInEditorModeAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.All)]
    public class DisableInPlayModeAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.All)]
    public class DisplayAsStringAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.All)]
    public class CustomValueDrawerAttribute : Attribute
    {
        public CustomValueDrawerAttribute(string methodName) { }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class DetailedInfoBoxAttribute : Attribute
    {
        public DetailedInfoBoxAttribute(string message, string details) { }
    }


    [AttributeUsage(AttributeTargets.All)]
    public class SuffixLabelAttribute : Attribute
    {
        public SuffixLabelAttribute(string label) { }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class PrefixLabelAttribute : Attribute
    {
        public PrefixLabelAttribute(string label) { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class ListDrawerSettings : Attribute
    {
        public bool IsReadOnly { get; set; }
        public bool HideAddButton { get; set; }
        public bool HideRemoveButton { get; set; }
        public bool DraggableItems { get; set; }
        public bool Expanded { get; set; }
        public int NumberOfItemsPerPage { get; set; }

        public ListDrawerSettings() { }
    }

    [AttributeUsage(AttributeTargets.All)]
    public class DictionaryDrawerSettings : Attribute
    {
        public DictionaryDisplayOptions DisplayMode { get; set; }
        public string KeyLabel { get; set; }
        public string ValueLabel { get; set; }
        public bool IsReadOnly { get; set; }
        public bool ShowFoldout { get; set; }

        public DictionaryDrawerSettings() { }
    }

    public enum DictionaryDisplayOptions
    {
        Foldout = 0,
        OneLine = 1,
        TwoLine = 2,
        Tree = 3,
        Grid = 4,
        ExpandedFoldout = 5
    }

    // Enums
    public enum ButtonSizes
    {
        Small,
        Medium,
        Large,
        Gigantic
    }

    public enum InfoMessageType
    {
        None,
        Info,
        Warning,
        Error
    }

    public enum InlineEditorModes
    {
        GUIOnly,
        GUIAndHeader,
        GUIAndPreview,
        FullEditor
    }
}

// SerializedMonoBehaviour와 SerializedScriptableObject를 위한 더미 클래스
namespace Sirenix.OdinInspector
{
    public class SerializedMonoBehaviour : MonoBehaviour { }
    public class SerializedScriptableObject : ScriptableObject { }
}

namespace Sirenix.Serialization
{
}

#endif