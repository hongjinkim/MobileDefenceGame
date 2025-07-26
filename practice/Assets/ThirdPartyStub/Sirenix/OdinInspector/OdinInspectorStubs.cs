#if !ODIN_INSPECTOR
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sirenix.OdinInspector
{
    [AttributeUsage(AttributeTargets.All)]
    public class AssetListAttribute : Attribute
    {
        public AssetListAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class AssetSelectorAttribute : Attribute
    {
        public AssetSelectorAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class AssetsOnlyAttribute : Attribute
    {
        public AssetsOnlyAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class BoxGroupAttribute : Attribute
    {
        public BoxGroupAttribute(string group, bool showLabel, bool centerLabel, float order) { }
        public BoxGroupAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class ButtonAttribute : Attribute
    {
        public ButtonAttribute() { }
        public ButtonAttribute(ButtonSizes size) { }
        public ButtonAttribute(int buttonSize) { }
        public ButtonAttribute(string name) { }
        public ButtonAttribute(string name, ButtonSizes buttonSize) { }
        public ButtonAttribute(string name, int buttonSize) { }
        public ButtonAttribute(ButtonStyle parameterBtnStyle) { }
        public ButtonAttribute(int buttonSize, ButtonStyle parameterBtnStyle) { }
        public ButtonAttribute(ButtonSizes size, ButtonStyle parameterBtnStyle) { }
        public ButtonAttribute(string name, ButtonStyle parameterBtnStyle) { }
        public ButtonAttribute(string name, ButtonSizes buttonSize, ButtonStyle parameterBtnStyle) { }
        public ButtonAttribute(string name, int buttonSize, ButtonStyle parameterBtnStyle) { }
        public ButtonAttribute(SdfIconType icon, IconAlignment iconAlignment) { }
        public ButtonAttribute(SdfIconType icon) { }
        public ButtonAttribute(SdfIconType icon, string name) { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class ButtonGroupAttribute : Attribute
    {
        public ButtonGroupAttribute(string group, float order) { }
        public ButtonGroupAttribute() { }
    }
    public enum ButtonStyle
    {
        CompactBox,
        FoldoutButton,
        Box,
    }
    [AttributeUsage(AttributeTargets.All)]
    public class ChildGameObjectsOnlyAttribute : Attribute
    {
        public ChildGameObjectsOnlyAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class ColorPaletteAttribute : Attribute
    {
        public ColorPaletteAttribute() { }
        public ColorPaletteAttribute(string paletteName) { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class CustomContextMenuAttribute : Attribute
    {
        public CustomContextMenuAttribute(string menuItem, string action) { }
        public CustomContextMenuAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class CustomValueDrawerAttribute : Attribute
    {
        public CustomValueDrawerAttribute(string action) { }
        public CustomValueDrawerAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class DelayedPropertyAttribute : Attribute
    {
        public DelayedPropertyAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class DetailedInfoBoxAttribute : Attribute
    {
        public DetailedInfoBoxAttribute(string message, string details, InfoMessageType infoMessageType, string visibleIf) { }
        public DetailedInfoBoxAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class DictionaryDrawerSettings : Attribute
    {
        public DictionaryDrawerSettings() { }
        public DictionaryDrawerSettings(DictionaryDisplayOptions DisplayMode, string KeyLabel, string ValueLabel) { }

        public DictionaryDisplayOptions DisplayMode { get; set; }
        public string KeyLabel { get; set; }
        public string ValueLabel { get; set; }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class DisableContextMenuAttribute : Attribute
    {
        public DisableContextMenuAttribute(bool disableForMember, bool disableCollectionElements) { }
        public DisableContextMenuAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class DisableIfAttribute : Attribute
    {
        public DisableIfAttribute(string condition) { }
        public DisableIfAttribute(string condition, Object optionalValue) { }
        public DisableIfAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class DisableInAttribute : Attribute
    {
        public DisableInAttribute(PrefabKind prefabKind) { }
        public DisableInAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class DisableInEditorModeAttribute : Attribute
    {
        public DisableInEditorModeAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class DisableInInlineEditorsAttribute : Attribute
    {
        public DisableInInlineEditorsAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class DisableInNonPrefabsAttribute : Attribute
    {
        public DisableInNonPrefabsAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class DisableInPlayModeAttribute : Attribute
    {
        public DisableInPlayModeAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class DisableInPrefabAssetsAttribute : Attribute
    {
        public DisableInPrefabAssetsAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class DisableInPrefabInstancesAttribute : Attribute
    {
        public DisableInPrefabInstancesAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class DisableInPrefabsAttribute : Attribute
    {
        public DisableInPrefabsAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class DisallowModificationsInAttribute : Attribute
    {
        public DisallowModificationsInAttribute(PrefabKind kind) { }
        public DisallowModificationsInAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class DisplayAsStringAttribute : Attribute
    {
        public DisplayAsStringAttribute() { }
        public DisplayAsStringAttribute(bool overflow) { }
        public DisplayAsStringAttribute(TextAlignment alignment) { }
        public DisplayAsStringAttribute(int fontSize) { }
        public DisplayAsStringAttribute(bool overflow, TextAlignment alignment) { }
        public DisplayAsStringAttribute(bool overflow, int fontSize) { }
        public DisplayAsStringAttribute(int fontSize, TextAlignment alignment) { }
        public DisplayAsStringAttribute(bool overflow, int fontSize, TextAlignment alignment) { }
        public DisplayAsStringAttribute(TextAlignment alignment, bool enableRichText) { }
        public DisplayAsStringAttribute(int fontSize, bool enableRichText) { }
        public DisplayAsStringAttribute(bool overflow, TextAlignment alignment, bool enableRichText) { }
        public DisplayAsStringAttribute(bool overflow, int fontSize, bool enableRichText) { }
        public DisplayAsStringAttribute(int fontSize, TextAlignment alignment, bool enableRichText) { }
        public DisplayAsStringAttribute(bool overflow, int fontSize, TextAlignment alignment, bool enableRichText) { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class DoNotDrawAsReferenceAttribute : Attribute
    {
        public DoNotDrawAsReferenceAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class DontApplyToListElementsAttribute : Attribute
    {
        public DontApplyToListElementsAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class DontValidateAttribute : Attribute
    {
        public DontValidateAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class DrawWithUnityAttribute : Attribute
    {
        public DrawWithUnityAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class DrawWithVisualElementsAttribute : Attribute
    {
        public DrawWithVisualElementsAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class EnableGUIAttribute : Attribute
    {
        public EnableGUIAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class EnableIfAttribute : Attribute
    {
        public EnableIfAttribute(string condition) { }
        public EnableIfAttribute(string condition, Object optionalValue) { }
        public EnableIfAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class EnableInAttribute : Attribute
    {
        public EnableInAttribute(PrefabKind prefabKind) { }
        public EnableInAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class EnumPagingAttribute : Attribute
    {
        public EnumPagingAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class EnumToggleButtonsAttribute : Attribute
    {
        public EnumToggleButtonsAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class FilePathAttribute : Attribute
    {
        public FilePathAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class FolderPathAttribute : Attribute
    {
        public FolderPathAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class FoldoutGroupAttribute : Attribute
    {
        public FoldoutGroupAttribute(string groupName, float order) { }
        public FoldoutGroupAttribute(string groupName, bool expanded, float order) { }
        public FoldoutGroupAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class GUIColorAttribute : Attribute
    {
        public GUIColorAttribute(float r, float g, float b, float a) { }
        public GUIColorAttribute(string getColor) { }
        public GUIColorAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class HideDuplicateReferenceBoxAttribute : Attribute
    {
        public HideDuplicateReferenceBoxAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class HideIfAttribute : Attribute
    {
        public HideIfAttribute(string condition, bool animate) { }
        public HideIfAttribute(string condition, Object optionalValue, bool animate) { }
        public HideIfAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class HideIfGroupAttribute : Attribute
    {
        public HideIfGroupAttribute(string path, bool animate) { }
        public HideIfGroupAttribute(string path, Object value, bool animate) { }
        public HideIfGroupAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class HideInAttribute : Attribute
    {
        public HideInAttribute(PrefabKind prefabKind) { }
        public HideInAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class HideInEditorModeAttribute : Attribute
    {
        public HideInEditorModeAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class HideInInlineEditorsAttribute : Attribute
    {
        public HideInInlineEditorsAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class HideInNonPrefabsAttribute : Attribute
    {
        public HideInNonPrefabsAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class HideInPlayModeAttribute : Attribute
    {
        public HideInPlayModeAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class HideInPrefabAssetsAttribute : Attribute
    {
        public HideInPrefabAssetsAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class HideInPrefabInstancesAttribute : Attribute
    {
        public HideInPrefabInstancesAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class HideInPrefabsAttribute : Attribute
    {
        public HideInPrefabsAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class HideInTablesAttribute : Attribute
    {
        public HideInTablesAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class HideLabelAttribute : Attribute
    {
        public HideLabelAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class HideMonoScriptAttribute : Attribute
    {
        public HideMonoScriptAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class HideNetworkBehaviourFieldsAttribute : Attribute
    {
        public HideNetworkBehaviourFieldsAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class HideReferenceObjectPickerAttribute : Attribute
    {
        public HideReferenceObjectPickerAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class HorizontalGroupAttribute : Attribute
    {
        public HorizontalGroupAttribute(string group, float width, int marginLeft, int marginRight, float order) { }
        public HorizontalGroupAttribute(float width, int marginLeft, int marginRight, float order) { }
        public HorizontalGroupAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class IndentAttribute : Attribute
    {
        public IndentAttribute(int indentLevel) { }
        public IndentAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class InfoBoxAttribute : Attribute
    {
        public InfoBoxAttribute(string message, InfoMessageType infoMessageType, string visibleIfMemberName) { }
        public InfoBoxAttribute(string message, string visibleIfMemberName) { }
        public InfoBoxAttribute(string message, SdfIconType icon, string visibleIfMemberName) { }
        public InfoBoxAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class InlineButtonAttribute : Attribute
    {
        public InlineButtonAttribute(string action, string label) { }
        public InlineButtonAttribute(string action, SdfIconType icon, string label) { }
        public InlineButtonAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class InlineEditorAttribute : Attribute
    {
        public InlineEditorAttribute(InlineEditorModes inlineEditorMode, InlineEditorObjectFieldModes objectFieldMode) { }
        public InlineEditorAttribute(InlineEditorObjectFieldModes objectFieldMode) { }
        public InlineEditorAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class InlinePropertyAttribute : Attribute
    {
        public InlinePropertyAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class LabelTextAttribute : Attribute
    {
        public LabelTextAttribute(string text) { }
        public LabelTextAttribute(SdfIconType icon) { }
        public LabelTextAttribute(string text, bool nicifyText) { }
        public LabelTextAttribute(string text, SdfIconType icon) { }
        public LabelTextAttribute(string text, bool nicifyText, SdfIconType icon) { }
        public LabelTextAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class LabelWidthAttribute : Attribute
    {
        public LabelWidthAttribute(float width) { }
        public LabelWidthAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class ListDrawerSettingsAttribute : Attribute
    {
        public ListDrawerSettingsAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class MaxValueAttribute : Attribute
    {
        public MaxValueAttribute(Double maxValue) { }
        public MaxValueAttribute(string expression) { }
        public MaxValueAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class MinMaxSliderAttribute : Attribute
    {
        public MinMaxSliderAttribute(float minValue, float maxValue, bool showFields) { }
        public MinMaxSliderAttribute(string minValueGetter, float maxValue, bool showFields) { }
        public MinMaxSliderAttribute(float minValue, string maxValueGetter, bool showFields) { }
        public MinMaxSliderAttribute(string minValueGetter, string maxValueGetter, bool showFields) { }
        public MinMaxSliderAttribute(string minMaxValueGetter, bool showFields) { }
        public MinMaxSliderAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class MinValueAttribute : Attribute
    {
        public MinValueAttribute(Double minValue) { }
        public MinValueAttribute(string expression) { }
        public MinValueAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class MultiLinePropertyAttribute : Attribute
    {
        public MultiLinePropertyAttribute(int lines) { }
        public MultiLinePropertyAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class OnCollectionChangedAttribute : Attribute
    {
        public OnCollectionChangedAttribute() { }
        public OnCollectionChangedAttribute(string after) { }
        public OnCollectionChangedAttribute(string before, string after) { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class OnInspectorDisposeAttribute : Attribute
    {
        public OnInspectorDisposeAttribute() { }
        public OnInspectorDisposeAttribute(string action) { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class OnInspectorGUIAttribute : Attribute
    {
        public OnInspectorGUIAttribute() { }
        public OnInspectorGUIAttribute(string action, bool append) { }
        public OnInspectorGUIAttribute(string prepend, string append) { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class OnInspectorInitAttribute : Attribute
    {
        public OnInspectorInitAttribute() { }
        public OnInspectorInitAttribute(string action) { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class OnStateUpdateAttribute : Attribute
    {
        public OnStateUpdateAttribute(string action) { }
        public OnStateUpdateAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class OnValueChangedAttribute : Attribute
    {
        public OnValueChangedAttribute(string action, bool includeChildren) { }
        public OnValueChangedAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class OptionalAttribute : Attribute
    {
        public OptionalAttribute() { }
    }
    public enum NonDefaultConstructorPreference
    {
        Exclude,
        ConstructIdeal,
        PreferUninitialized,
        LogWarning,
    }
    [AttributeUsage(AttributeTargets.All)]
    public class PolymorphicDrawerSettingsAttribute : Attribute
    {
        public PolymorphicDrawerSettingsAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class PreviewFieldAttribute : Attribute
    {
        public PreviewFieldAttribute() { }
        public PreviewFieldAttribute(float height) { }
        public PreviewFieldAttribute(string previewGetter, FilterMode filterMode) { }
        public PreviewFieldAttribute(string previewGetter, float height, FilterMode filterMode) { }
        public PreviewFieldAttribute(float height, ObjectFieldAlignment alignment) { }
        public PreviewFieldAttribute(string previewGetter, ObjectFieldAlignment alignment, FilterMode filterMode) { }
        public PreviewFieldAttribute(string previewGetter, float height, ObjectFieldAlignment alignment, FilterMode filterMode) { }
        public PreviewFieldAttribute(ObjectFieldAlignment alignment) { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class ProgressBarAttribute : Attribute
    {
        public ProgressBarAttribute(Double min, Double max, float r, float g, float b) { }
        public ProgressBarAttribute(string minGetter, Double max, float r, float g, float b) { }
        public ProgressBarAttribute(Double min, string maxGetter, float r, float g, float b) { }
        public ProgressBarAttribute(string minGetter, string maxGetter, float r, float g, float b) { }
        public ProgressBarAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class PropertyGroupAttribute : Attribute
    {
        public PropertyGroupAttribute(string groupId, float order) { }
        public PropertyGroupAttribute(string groupId) { }
        public PropertyGroupAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class PropertyOrderAttribute : Attribute
    {
        public PropertyOrderAttribute() { }
        public PropertyOrderAttribute(float order) { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class PropertyRangeAttribute : Attribute
    {
        public PropertyRangeAttribute(Double min, Double max) { }
        public PropertyRangeAttribute(string minGetter, Double max) { }
        public PropertyRangeAttribute(Double min, string maxGetter) { }
        public PropertyRangeAttribute(string minGetter, string maxGetter) { }
        public PropertyRangeAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class PropertySpaceAttribute : Attribute
    {
        public PropertySpaceAttribute() { }
        public PropertySpaceAttribute(float spaceBefore) { }
        public PropertySpaceAttribute(float spaceBefore, float spaceAfter) { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class PropertyTooltipAttribute : Attribute
    {
        public PropertyTooltipAttribute(string tooltip) { }
        public PropertyTooltipAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class ReadOnlyAttribute : Attribute
    {
        public ReadOnlyAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class RequiredAttribute : Attribute
    {
        public RequiredAttribute() { }
        public RequiredAttribute(string errorMessage, InfoMessageType messageType) { }
        public RequiredAttribute(string errorMessage) { }
        public RequiredAttribute(InfoMessageType messageType) { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class RequiredInAttribute : Attribute
    {
        public RequiredInAttribute(PrefabKind kind) { }
        public RequiredInAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class RequiredInPrefabAssetsAttribute : Attribute
    {
        public RequiredInPrefabAssetsAttribute() { }
        public RequiredInPrefabAssetsAttribute(string errorMessage, InfoMessageType messageType) { }
        public RequiredInPrefabAssetsAttribute(string errorMessage) { }
        public RequiredInPrefabAssetsAttribute(InfoMessageType messageType) { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class RequiredInPrefabInstancesAttribute : Attribute
    {
        public RequiredInPrefabInstancesAttribute() { }
        public RequiredInPrefabInstancesAttribute(string errorMessage, InfoMessageType messageType) { }
        public RequiredInPrefabInstancesAttribute(string errorMessage) { }
        public RequiredInPrefabInstancesAttribute(InfoMessageType messageType) { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class RequiredListLengthAttribute : Attribute
    {
        public RequiredListLengthAttribute() { }
        public RequiredListLengthAttribute(int fixedLength) { }
        public RequiredListLengthAttribute(int minLength, int maxLength) { }
        public RequiredListLengthAttribute(int minLength, string maxLengthGetter) { }
        public RequiredListLengthAttribute(string fixedLengthGetter) { }
        public RequiredListLengthAttribute(string minLengthGetter, string maxLengthGetter) { }
        public RequiredListLengthAttribute(string minLengthGetter, int maxLength) { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class ResponsiveButtonGroupAttribute : Attribute
    {
        public ResponsiveButtonGroupAttribute(string group) { }
        public ResponsiveButtonGroupAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class SceneObjectsOnlyAttribute : Attribute
    {
        public SceneObjectsOnlyAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class SearchableAttribute : Attribute
    {
        public SearchableAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class ShowDrawerChainAttribute : Attribute
    {
        public ShowDrawerChainAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class ShowIfAttribute : Attribute
    {
        public ShowIfAttribute(string condition, bool animate) { }
        public ShowIfAttribute(string condition, Object optionalValue, bool animate) { }
        public ShowIfAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class ShowIfGroupAttribute : Attribute
    {
        public ShowIfGroupAttribute(string path, bool animate) { }
        public ShowIfGroupAttribute(string path, Object value, bool animate) { }
        public ShowIfGroupAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class ShowInAttribute : Attribute
    {
        public ShowInAttribute(PrefabKind prefabKind) { }
        public ShowInAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class ShowInInlineEditorsAttribute : Attribute
    {
        public ShowInInlineEditorsAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class ShowInInspectorAttribute : Attribute
    {
        public ShowInInspectorAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class ShowOdinSerializedPropertiesInInspectorAttribute : Attribute
    {
        public ShowOdinSerializedPropertiesInInspectorAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class ShowPropertyResolverAttribute : Attribute
    {
        public ShowPropertyResolverAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class SuffixLabelAttribute : Attribute
    {
        public SuffixLabelAttribute(string label, bool overlay) { }
        public SuffixLabelAttribute(string label, SdfIconType icon, bool overlay) { }
        public SuffixLabelAttribute(SdfIconType icon) { }
        public SuffixLabelAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class SuppressInvalidAttributeErrorAttribute : Attribute
    {
        public SuppressInvalidAttributeErrorAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class TabGroupAttribute : Attribute
    {
        public TabGroupAttribute(string tab, string label) { }
        public TabGroupAttribute(string tab, bool useFixedHeight, float order) { }
        public TabGroupAttribute(string group, string tab, bool useFixedHeight, float order) { }
        public TabGroupAttribute(string group, string tab, SdfIconType icon, bool useFixedHeight, float order) { }
        public TabGroupAttribute() { }
    }
    public enum TabLayouting
    {
        MultiRow,
        Shrink,
    }
    [AttributeUsage(AttributeTargets.All)]
    public class TableColumnWidthAttribute : Attribute
    {
        public TableColumnWidthAttribute(int width, bool resizable) { }
        public TableColumnWidthAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class TableListAttribute : Attribute
    {
        public TableListAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class TableMatrixAttribute : Attribute
    {
        public TableMatrixAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class TitleAttribute : Attribute
    {
        public TitleAttribute(string title, string subtitle, TitleAlignments titleAlignment, bool horizontalLine, bool bold) { }
        public TitleAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class TitleGroupAttribute : Attribute
    {
        public TitleGroupAttribute(string title, string subtitle, TitleAlignments alignment, bool horizontalLine, bool boldTitle, bool indent, float order) { }
        public TitleGroupAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class ToggleAttribute : Attribute
    {
        public ToggleAttribute(string toggleMemberName) { }
        public ToggleAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class ToggleGroupAttribute : Attribute
    {
        public ToggleGroupAttribute(string toggleMemberName, float order, string groupTitle) { }
        public ToggleGroupAttribute(string toggleMemberName, string groupTitle) { }
        public ToggleGroupAttribute(string toggleMemberName, float order, string groupTitle, string titleStringMemberName) { }
        public ToggleGroupAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class ToggleLeftAttribute : Attribute
    {
        public ToggleLeftAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class TypeDrawerSettingsAttribute : Attribute
    {
        public TypeDrawerSettingsAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class TypeFilterAttribute : Attribute
    {
        public TypeFilterAttribute(string filterGetter) { }
        public TypeFilterAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class TypeInfoBoxAttribute : Attribute
    {
        public TypeInfoBoxAttribute(string message) { }
        public TypeInfoBoxAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class TypeRegistryItemAttribute : Attribute
    {
        public TypeRegistryItemAttribute(string name, string categoryPath, SdfIconType icon, float lightIconColorR, float lightIconColorG, float lightIconColorB, float lightIconColorA, float darkIconColorR, float darkIconColorG, float darkIconColorB, float darkIconColorA, int priority) { }
        public TypeRegistryItemAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class TypeSelectorSettingsAttribute : Attribute
    {
        public TypeSelectorSettingsAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class UnitAttribute : Attribute
    {
        public UnitAttribute(Units unit) { }
        public UnitAttribute(string unit) { }
        public UnitAttribute(Units _base, Units display) { }
        public UnitAttribute(Units _base, string display) { }
        public UnitAttribute(string _base, Units display) { }
        public UnitAttribute(string _base, string display) { }
        public UnitAttribute() { }
    }
    public enum Units
    {
        Nanometer,
        Micrometer,
        Millimeter,
        Centimeter,
        Meter,
        Kilometer,
        Inch,
        Feet,
        Mile,
        Yard,
        NauticalMile,
        LightYear,
        Parsec,
        AstronomicalUnit,
        CubicMeter,
        CubicKilometer,
        CubicCentimeter,
        CubicMillimeter,
        Liter,
        Milliliter,
        Centiliter,
        Deciliter,
        Hectoliter,
        CubicInch,
        CubicFeet,
        CubicYard,
        AcreFeet,
        BarrelOil,
        TeaspoonUS,
        TablespoonUS,
        CupUS,
        GillUS,
        PintUS,
        QuartUS,
        GallonUS,
        BarrelUS,
        FluidOunceUS,
        BarrelUK,
        FluidOunceUK,
        TeaspoonUK,
        TablespoonUK,
        CupUK,
        GillUK,
        PintUK,
        QuartUK,
        GallonUK,
        SquareMeter,
        SquareKilometer,
        SquareCentimeter,
        SquareMillimeter,
        SquareMicrometer,
        SquareInch,
        SquareFeet,
        SquareYard,
        SquareMile,
        Hectare,
        Acre,
        Are,
        Joule,
        Kilojoule,
        WattHour,
        KilowattHour,
        HorsepowerHour,
        Newton,
        Kilonewton,
        Meganewton,
        Giganewton,
        Teranewton,
        Centinewton,
        Millinewton,
        JouleMeter,
        JouleCentimeter,
        GramForce,
        KilogramForce,
        TonForce,
        PoundForce,
        KilopoundForce,
        OunceForce,
        MetersPerSecond,
        MetersPerMinute,
        MetersPerHour,
        KilometersPerSecond,
        KilometersPerMinute,
        KilometersPerHour,
        CentimetersPerSecond,
        CentimetersPerMinute,
        CentimetersPerHour,
        MillimetersPerSecond,
        MillimetersPerMinute,
        MillimetersPerHour,
        FeetPerSecond,
        FeetPerMinute,
        FeetPerHour,
        YardsPerSecond,
        YardsPerMinute,
        YardsPerHour,
        MilesPerSecond,
        MilesPerMinute,
        MilesPerHour,
        Knot,
        KnotUK,
        SpeedOfLight,
        Bit,
        Kilobit,
        Megabit,
        Gigabit,
        Terabit,
        Petabit,
        Byte,
        Kilobyte,
        Kibibyte,
        Megabyte,
        Mebibyte,
        Gigabyte,
        Gibibyte,
        Terabyte,
        Tebibyte,
        Petabyte,
        Pebibyte,
        Kilogram,
        Hectogram,
        Dekagram,
        Gram,
        Decigram,
        Centigram,
        Milligram,
        MetricTon,
        Pounds,
        ShortTon,
        LongTon,
        Ounce,
        StoneUS,
        StoneUK,
        QuarterUS,
        QuarterUK,
        Slug,
        Grain,
        Celsius,
        Fahrenheit,
        Kelvin,
        Pascal,
        Decipascal,
        Centipascal,
        Millipascal,
        Micropascal,
        Kilopascal,
        Megapascal,
        Gigapascal,
        Bar,
        Millibar,
        Microbar,
        PSI,
        KSI,
        StandardAtmosphere,
        Watt,
        Kilowatt,
        Megawatt,
        Gigawatt,
        Terawatt,
        Horsepower,
        JouleSecond,
        JouleMinute,
        JouleHour,
        KilojouleSecond,
        KilojouleMinute,
        KilojouleHour,
        Second,
        Millisecond,
        Microsecond,
        Nanosecond,
        Minute,
        Hour,
        Day,
        Week,
        Radian,
        Degree,
        Turn,
        Grad,
        SecondsOfAngle,
        MinutesOfAngle,
        Mil,
        MetersPerSecondSquared,
        DecimetersPerSecondSquared,
        CentimetersPerSecondSquared,
        MillimetersPerSecondSquared,
        MicrometersPerSecondSquared,
        DekametersPerSecondSquared,
        HectometersPerSecondSquared,
        KilometersPerSecondSquared,
        MilePerSecondSquared,
        YardPerSecondSquared,
        FeetPerSecondSquared,
        InchPerSecondSquared,
        GForce,
        NewtonMeter,
        NewtonCentimeter,
        NewtonMillimeter,
        KilonewtonMeter,
        KilogramForceMeter,
        KilogramForceCentimeter,
        KilogramForceMillimeter,
        GramForceMeter,
        GramForceCentimeter,
        GramForceMillimeter,
        PoundFeet,
        PoundInch,
        OuncecFeet,
        OuncecInch,
        RadiansPerSecond,
        RadiansPerMinute,
        RadiansPerHour,
        RadiansPerDay,
        DegreesPerSecond,
        DegreesPerMinute,
        DegreesPerHour,
        DegreesPerDay,
        RevolutionsPerSecond,
        RevolutionsPerMinute,
        RevolutionsPerHour,
        RevolutionsPerDay,
        Hertz,
        Kilohertz,
        Megahertz,
        Gigahertz,
        PercentMultiplier,
        Percent,
        Permille,
        Permyriad,
        Unset,
    }
    [AttributeUsage(AttributeTargets.All)]
    public class ValidateInputAttribute : Attribute
    {
        public ValidateInputAttribute(string condition, string defaultMessage, InfoMessageType messageType) { }
        public ValidateInputAttribute(string condition, string message, InfoMessageType messageType, bool rejectedInvalidInput) { }
        public ValidateInputAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class ValueDropdownAttribute : Attribute
    {
        public ValueDropdownAttribute(string valuesGetter) { }
        public ValueDropdownAttribute() { }
    }
    public class ValueDropdownList { }
    public struct ValueDropdownItem { }
    [AttributeUsage(AttributeTargets.All)]
    public class VerticalGroupAttribute : Attribute
    {
        public VerticalGroupAttribute(string groupId, float order) { }
        public VerticalGroupAttribute(float order) { }
        public VerticalGroupAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class WrapAttribute : Attribute
    {
        public WrapAttribute(Double min, Double max) { }
        public WrapAttribute() { }
    }
    public class AttributeTargetFlags { }
    public enum ButtonSizes
    {
        Small,
        Medium,
        Large,
        Gigantic,
    }
    public enum DictionaryDisplayOptions
    {
        OneLine,
        Foldout,
        CollapsedFoldout,
        ExpandedFoldout,
    }
    public enum IconAlignment
    {
        LeftOfText,
        RightOfText,
        LeftEdge,
        RightEdge,
    }
    [AttributeUsage(AttributeTargets.All)]
    public class IncludeMyAttributesAttribute : Attribute
    {
        public IncludeMyAttributesAttribute() { }
    }
    public enum InfoMessageType
    {
        None,
        Info,
        Warning,
        Error,
    }
    public enum InlineEditorModes
    {
        GUIOnly,
        GUIAndHeader,
        GUIAndPreview,
        SmallPreview,
        LargePreview,
        FullEditor,
    }
    public enum InlineEditorObjectFieldModes
    {
        Boxed,
        Foldout,
        Hidden,
        CompletelyHidden,
    }
    public enum ValidatorSeverity
    {
        Error,
        Warning,
        Ignore,
    }
    public class SelfValidationResultItemExtensions { }
    public class SelfValidationResult { }
    public class SelfMetaData { }
    public struct SelfFix { }
    public enum ObjectFieldAlignment
    {
        Left,
        Center,
        Right,
    }
    [AttributeUsage(AttributeTargets.All)]
    public class OdinRegisterAttributeAttribute : Attribute
    {
        public OdinRegisterAttributeAttribute(Type attributeType, string category, string description, bool isEnterprise) { }
        public OdinRegisterAttributeAttribute(Type attributeType, string category, string description, bool isEnterprise, string url) { }
        public OdinRegisterAttributeAttribute() { }
    }
    public enum PrefabKind
    {
        None,
        InstanceInScene,
        InstanceInPrefab,
        PrefabInstance,
        Regular,
        Variant,
        PrefabAsset,
        NonPrefabInstance,
        PrefabInstanceAndNonPrefabInstance,
        All,
    }
    public enum PreviewAlignment
    {
        Left,
        Right,
        Top,
        Bottom,
    }
    public enum SdfIconType
    {
        None,
        AlarmFill,
        Alarm,
        AlignBottom,
        AlignCenter,
        AlignEnd,
        AlignMiddle,
        AlignStart,
        AlignTop,
        Alt,
        AppIndicator,
        App,
        ArchiveFill,
        Archive,
        Arrow90degDown,
        Arrow90degLeft,
        Arrow90degRight,
        Arrow90degUp,
        ArrowBarDown,
        ArrowBarLeft,
        ArrowBarRight,
        ArrowBarUp,
        ArrowClockwise,
        ArrowCounterclockwise,
        ArrowDownCircleFill,
        ArrowDownCircle,
        ArrowDownLeftCircleFill,
        ArrowDownLeftCircle,
        ArrowDownLeftSquareFill,
        ArrowDownLeftSquare,
        ArrowDownLeft,
        ArrowDownRightCircleFill,
        ArrowDownRightCircle,
        ArrowDownRightSquareFill,
        ArrowDownRightSquare,
        ArrowDownRight,
        ArrowDownShort,
        ArrowDownSquareFill,
        ArrowDownSquare,
        ArrowDownUp,
        ArrowDown,
        ArrowLeftCircleFill,
        ArrowLeftCircle,
        ArrowLeftRight,
        ArrowLeftShort,
        ArrowLeftSquareFill,
        ArrowLeftSquare,
        ArrowLeft,
        ArrowRepeat,
        ArrowReturnLeft,
        ArrowReturnRight,
        ArrowRightCircleFill,
        ArrowRightCircle,
        ArrowRightShort,
        ArrowRightSquareFill,
        ArrowRightSquare,
        ArrowRight,
        ArrowUpCircleFill,
        ArrowUpCircle,
        ArrowUpLeftCircleFill,
        ArrowUpLeftCircle,
        ArrowUpLeftSquareFill,
        ArrowUpLeftSquare,
        ArrowUpLeft,
        ArrowUpRightCircleFill,
        ArrowUpRightCircle,
        ArrowUpRightSquareFill,
        ArrowUpRightSquare,
        ArrowUpRight,
        ArrowUpShort,
        ArrowUpSquareFill,
        ArrowUpSquare,
        ArrowUp,
        ArrowsAngleContract,
        ArrowsAngleExpand,
        ArrowsCollapse,
        ArrowsExpand,
        ArrowsFullscreen,
        ArrowsMove,
        AspectRatioFill,
        AspectRatio,
        Asterisk,
        At,
        AwardFill,
        Award,
        Back,
        BackspaceFill,
        BackspaceReverseFill,
        BackspaceReverse,
        Backspace,
        Badge3dFill,
        Badge3d,
        Badge4kFill,
        Badge4k,
        Badge8kFill,
        Badge8k,
        BadgeAdFill,
        BadgeAd,
        BadgeArFill,
        BadgeAr,
        BadgeCcFill,
        BadgeCc,
        BadgeHdFill,
        BadgeHd,
        BadgeTmFill,
        BadgeTm,
        BadgeVoFill,
        BadgeVo,
        BadgeVrFill,
        BadgeVr,
        BadgeWcFill,
        BadgeWc,
        BagCheckFill,
        BagCheck,
        BagDashFill,
        BagDash,
        BagFill,
        BagPlusFill,
        BagPlus,
        BagXFill,
        BagX,
        Bag,
        BarChartFill,
        BarChartLineFill,
        BarChartLine,
        BarChartSteps,
        BarChart,
        BasketFill,
        Basket,
        Basket2Fill,
        Basket2,
        Basket3Fill,
        Basket3,
        BatteryCharging,
        BatteryFull,
        BatteryHalf,
        Battery,
        BellFill,
        Bell,
        Bezier,
        Bezier2,
        Bicycle,
        BinocularsFill,
        Binoculars,
        BlockquoteLeft,
        BlockquoteRight,
        BookFill,
        BookHalf,
        Book,
        BookmarkCheckFill,
        BookmarkCheck,
        BookmarkDashFill,
        BookmarkDash,
        BookmarkFill,
        BookmarkHeartFill,
        BookmarkHeart,
        BookmarkPlusFill,
        BookmarkPlus,
        BookmarkStarFill,
        BookmarkStar,
        BookmarkXFill,
        BookmarkX,
        Bookmark,
        BookmarksFill,
        Bookmarks,
        Bookshelf,
        BootstrapFill,
        BootstrapReboot,
        Bootstrap,
        BorderAll,
        BorderBottom,
        BorderCenter,
        BorderInner,
        BorderLeft,
        BorderMiddle,
        BorderOuter,
        BorderRight,
        BorderStyle,
        BorderTop,
        BorderWidth,
        Border,
        BoundingBoxCircles,
        BoundingBox,
        BoxArrowDownLeft,
        BoxArrowDownRight,
        BoxArrowDown,
        BoxArrowInDownLeft,
        BoxArrowInDownRight,
        BoxArrowInDown,
        BoxArrowInLeft,
        BoxArrowInRight,
        BoxArrowInUpLeft,
        BoxArrowInUpRight,
        BoxArrowInUp,
        BoxArrowLeft,
        BoxArrowRight,
        BoxArrowUpLeft,
        BoxArrowUpRight,
        BoxArrowUp,
        BoxSeam,
        Box,
        Braces,
        Bricks,
        BriefcaseFill,
        Briefcase,
        BrightnessAltHighFill,
        BrightnessAltHigh,
        BrightnessAltLowFill,
        BrightnessAltLow,
        BrightnessHighFill,
        BrightnessHigh,
        BrightnessLowFill,
        BrightnessLow,
        BroadcastPin,
        Broadcast,
        BrushFill,
        Brush,
        BucketFill,
        Bucket,
        BugFill,
        Bug,
        Building,
        Bullseye,
        CalculatorFill,
        Calculator,
        CalendarCheckFill,
        CalendarCheck,
        CalendarDateFill,
        CalendarDate,
        CalendarDayFill,
        CalendarDay,
        CalendarEventFill,
        CalendarEvent,
        CalendarFill,
        CalendarMinusFill,
        CalendarMinus,
        CalendarMonthFill,
        CalendarMonth,
        CalendarPlusFill,
        CalendarPlus,
        CalendarRangeFill,
        CalendarRange,
        CalendarWeekFill,
        CalendarWeek,
        CalendarXFill,
        CalendarX,
        Calendar,
        Calendar2CheckFill,
        Calendar2Check,
        Calendar2DateFill,
        Calendar2Date,
        Calendar2DayFill,
        Calendar2Day,
        Calendar2EventFill,
        Calendar2Event,
        Calendar2Fill,
        Calendar2MinusFill,
        Calendar2Minus,
        Calendar2MonthFill,
        Calendar2Month,
        Calendar2PlusFill,
        Calendar2Plus,
        Calendar2RangeFill,
        Calendar2Range,
        Calendar2WeekFill,
        Calendar2Week,
        Calendar2XFill,
        Calendar2X,
        Calendar2,
        Calendar3EventFill,
        Calendar3Event,
        Calendar3Fill,
        Calendar3RangeFill,
        Calendar3Range,
        Calendar3WeekFill,
        Calendar3Week,
        Calendar3,
        Calendar4Event,
        Calendar4Range,
        Calendar4Week,
        Calendar4,
        CameraFill,
        CameraReelsFill,
        CameraReels,
        CameraVideoFill,
        CameraVideoOffFill,
        CameraVideoOff,
        CameraVideo,
        Camera,
        Camera2,
        CapslockFill,
        Capslock,
        CardChecklist,
        CardHeading,
        CardImage,
        CardList,
        CardText,
        CaretDownFill,
        CaretDownSquareFill,
        CaretDownSquare,
        CaretDown,
        CaretLeftFill,
        CaretLeftSquareFill,
        CaretLeftSquare,
        CaretLeft,
        CaretRightFill,
        CaretRightSquareFill,
        CaretRightSquare,
        CaretRight,
        CaretUpFill,
        CaretUpSquareFill,
        CaretUpSquare,
        CaretUp,
        CartCheckFill,
        CartCheck,
        CartDashFill,
        CartDash,
        CartFill,
        CartPlusFill,
        CartPlus,
        CartXFill,
        CartX,
        Cart,
        Cart2,
        Cart3,
        Cart4,
        CashStack,
        Cash,
        Cast,
        ChatDotsFill,
        ChatDots,
        ChatFill,
        ChatLeftDotsFill,
        ChatLeftDots,
        ChatLeftFill,
        ChatLeftQuoteFill,
        ChatLeftQuote,
        ChatLeftTextFill,
        ChatLeftText,
        ChatLeft,
        ChatQuoteFill,
        ChatQuote,
        ChatRightDotsFill,
        ChatRightDots,
        ChatRightFill,
        ChatRightQuoteFill,
        ChatRightQuote,
        ChatRightTextFill,
        ChatRightText,
        ChatRight,
        ChatSquareDotsFill,
        ChatSquareDots,
        ChatSquareFill,
        ChatSquareQuoteFill,
        ChatSquareQuote,
        ChatSquareTextFill,
        ChatSquareText,
        ChatSquare,
        ChatTextFill,
        ChatText,
        Chat,
        CheckAll,
        CheckCircleFill,
        CheckCircle,
        CheckSquareFill,
        CheckSquare,
        Check,
        Check2All,
        Check2Circle,
        Check2Square,
        Check2,
        ChevronBarContract,
        ChevronBarDown,
        ChevronBarExpand,
        ChevronBarLeft,
        ChevronBarRight,
        ChevronBarUp,
        ChevronCompactDown,
        ChevronCompactLeft,
        ChevronCompactRight,
        ChevronCompactUp,
        ChevronContract,
        ChevronDoubleDown,
        ChevronDoubleLeft,
        ChevronDoubleRight,
        ChevronDoubleUp,
        ChevronDown,
        ChevronExpand,
        ChevronLeft,
        ChevronRight,
        ChevronUp,
        CircleFill,
        CircleHalf,
        CircleSquare,
        Circle,
        ClipboardCheck,
        ClipboardData,
        ClipboardMinus,
        ClipboardPlus,
        ClipboardX,
        Clipboard,
        ClockFill,
        ClockHistory,
        Clock,
        CloudArrowDownFill,
        CloudArrowDown,
        CloudArrowUpFill,
        CloudArrowUp,
        CloudCheckFill,
        CloudCheck,
        CloudDownloadFill,
        CloudDownload,
        CloudDrizzleFill,
        CloudDrizzle,
        CloudFill,
        CloudFogFill,
        CloudFog,
        CloudFog2Fill,
        CloudFog2,
        CloudHailFill,
        CloudHail,
        CloudHaze1,
        CloudHazeFill,
        CloudHaze,
        CloudHaze2Fill,
        CloudLightningFill,
        CloudLightningRainFill,
        CloudLightningRain,
        CloudLightning,
        CloudMinusFill,
        CloudMinus,
        CloudMoonFill,
        CloudMoon,
        CloudPlusFill,
        CloudPlus,
        CloudRainFill,
        CloudRainHeavyFill,
        CloudRainHeavy,
        CloudRain,
        CloudSlashFill,
        CloudSlash,
        CloudSleetFill,
        CloudSleet,
        CloudSnowFill,
        CloudSnow,
        CloudSunFill,
        CloudSun,
        CloudUploadFill,
        CloudUpload,
        Cloud,
        CloudsFill,
        Clouds,
        CloudyFill,
        Cloudy,
        CodeSlash,
        CodeSquare,
        Code,
        CollectionFill,
        CollectionPlayFill,
        CollectionPlay,
        Collection,
        ColumnsGap,
        Columns,
        Command,
        CompassFill,
        Compass,
        ConeStriped,
        Cone,
        Controller,
        CpuFill,
        Cpu,
        CreditCard2BackFill,
        CreditCard2Back,
        CreditCard2FrontFill,
        CreditCard2Front,
        CreditCardFill,
        CreditCard,
        Crop,
        CupFill,
        CupStraw,
        Cup,
        CursorFill,
        CursorText,
        Cursor,
        DashCircleDotted,
        DashCircleFill,
        DashCircle,
        DashSquareDotted,
        DashSquareFill,
        DashSquare,
        Dash,
        Diagram2Fill,
        Diagram2,
        Diagram3Fill,
        Diagram3,
        DiamondFill,
        DiamondHalf,
        Diamond,
        Dice1Fill,
        Dice1,
        Dice2Fill,
        Dice2,
        Dice3Fill,
        Dice3,
        Dice4Fill,
        Dice4,
        Dice5Fill,
        Dice5,
        Dice6Fill,
        Dice6,
        DiscFill,
        Disc,
        Discord,
        DisplayFill,
        Display,
        DistributeHorizontal,
        DistributeVertical,
        DoorClosedFill,
        DoorClosed,
        DoorOpenFill,
        DoorOpen,
        Dot,
        Download,
        DropletFill,
        DropletHalf,
        Droplet,
        Earbuds,
        EaselFill,
        Easel,
        EggFill,
        EggFried,
        Egg,
        EjectFill,
        Eject,
        EmojiAngryFill,
        EmojiAngry,
        EmojiDizzyFill,
        EmojiDizzy,
        EmojiExpressionlessFill,
        EmojiExpressionless,
        EmojiFrownFill,
        EmojiFrown,
        EmojiHeartEyesFill,
        EmojiHeartEyes,
        EmojiLaughingFill,
        EmojiLaughing,
        EmojiNeutralFill,
        EmojiNeutral,
        EmojiSmileFill,
        EmojiSmileUpsideDownFill,
        EmojiSmileUpsideDown,
        EmojiSmile,
        EmojiSunglassesFill,
        EmojiSunglasses,
        EmojiWinkFill,
        EmojiWink,
        EnvelopeFill,
        EnvelopeOpenFill,
        EnvelopeOpen,
        Envelope,
        EraserFill,
        Eraser,
        ExclamationCircleFill,
        ExclamationCircle,
        ExclamationDiamondFill,
        ExclamationDiamond,
        ExclamationOctagonFill,
        ExclamationOctagon,
        ExclamationSquareFill,
        ExclamationSquare,
        ExclamationTriangleFill,
        ExclamationTriangle,
        Exclamation,
        Exclude,
        EyeFill,
        EyeSlashFill,
        EyeSlash,
        Eye,
        Eyedropper,
        Eyeglasses,
        Facebook,
        FileArrowDownFill,
        FileArrowDown,
        FileArrowUpFill,
        FileArrowUp,
        FileBarGraphFill,
        FileBarGraph,
        FileBinaryFill,
        FileBinary,
        FileBreakFill,
        FileBreak,
        FileCheckFill,
        FileCheck,
        FileCodeFill,
        FileCode,
        FileDiffFill,
        FileDiff,
        FileEarmarkArrowDownFill,
        FileEarmarkArrowDown,
        FileEarmarkArrowUpFill,
        FileEarmarkArrowUp,
        FileEarmarkBarGraphFill,
        FileEarmarkBarGraph,
        FileEarmarkBinaryFill,
        FileEarmarkBinary,
        FileEarmarkBreakFill,
        FileEarmarkBreak,
        FileEarmarkCheckFill,
        FileEarmarkCheck,
        FileEarmarkCodeFill,
        FileEarmarkCode,
        FileEarmarkDiffFill,
        FileEarmarkDiff,
        FileEarmarkEaselFill,
        FileEarmarkEasel,
        FileEarmarkExcelFill,
        FileEarmarkExcel,
        FileEarmarkFill,
        FileEarmarkFontFill,
        FileEarmarkFont,
        FileEarmarkImageFill,
        FileEarmarkImage,
        FileEarmarkLockFill,
        FileEarmarkLock,
        FileEarmarkLock2Fill,
        FileEarmarkLock2,
        FileEarmarkMedicalFill,
        FileEarmarkMedical,
        FileEarmarkMinusFill,
        FileEarmarkMinus,
        FileEarmarkMusicFill,
        FileEarmarkMusic,
        FileEarmarkPersonFill,
        FileEarmarkPerson,
        FileEarmarkPlayFill,
        FileEarmarkPlay,
        FileEarmarkPlusFill,
        FileEarmarkPlus,
        FileEarmarkPostFill,
        FileEarmarkPost,
        FileEarmarkPptFill,
        FileEarmarkPpt,
        FileEarmarkRichtextFill,
        FileEarmarkRichtext,
        FileEarmarkRuledFill,
        FileEarmarkRuled,
        FileEarmarkSlidesFill,
        FileEarmarkSlides,
        FileEarmarkSpreadsheetFill,
        FileEarmarkSpreadsheet,
        FileEarmarkTextFill,
        FileEarmarkText,
        FileEarmarkWordFill,
        FileEarmarkWord,
        FileEarmarkXFill,
        FileEarmarkX,
        FileEarmarkZipFill,
        FileEarmarkZip,
        FileEarmark,
        FileEaselFill,
        FileEasel,
        FileExcelFill,
        FileExcel,
        FileFill,
        FileFontFill,
        FileFont,
        FileImageFill,
        FileImage,
        FileLockFill,
        FileLock,
        FileLock2Fill,
        FileLock2,
        FileMedicalFill,
        FileMedical,
        FileMinusFill,
        FileMinus,
        FileMusicFill,
        FileMusic,
        FilePersonFill,
        FilePerson,
        FilePlayFill,
        FilePlay,
        FilePlusFill,
        FilePlus,
        FilePostFill,
        FilePost,
        FilePptFill,
        FilePpt,
        FileRichtextFill,
        FileRichtext,
        FileRuledFill,
        FileRuled,
        FileSlidesFill,
        FileSlides,
        FileSpreadsheetFill,
        FileSpreadsheet,
        FileTextFill,
        FileText,
        FileWordFill,
        FileWord,
        FileXFill,
        FileX,
        FileZipFill,
        FileZip,
        File,
        FilesAlt,
        Files,
        Film,
        FilterCircleFill,
        FilterCircle,
        FilterLeft,
        FilterRight,
        FilterSquareFill,
        FilterSquare,
        Filter,
        FlagFill,
        Flag,
        Flower1,
        Flower2,
        Flower3,
        FolderCheck,
        FolderFill,
        FolderMinus,
        FolderPlus,
        FolderSymlinkFill,
        FolderSymlink,
        FolderX,
        Folder,
        Folder2Open,
        Folder2,
        Fonts,
        ForwardFill,
        Forward,
        Front,
        FullscreenExit,
        Fullscreen,
        FunnelFill,
        Funnel,
        GearFill,
        GearWideConnected,
        GearWide,
        Gear,
        Gem,
        GeoAltFill,
        GeoAlt,
        GeoFill,
        Geo,
        GiftFill,
        Gift,
        Github,
        Globe,
        Globe2,
        Google,
        GraphDown,
        GraphUp,
        Grid1x2Fill,
        Grid1x2,
        Grid3x2GapFill,
        Grid3x2Gap,
        Grid3x2,
        Grid3x3GapFill,
        Grid3x3Gap,
        Grid3x3,
        GridFill,
        Grid,
        GripHorizontal,
        GripVertical,
        Hammer,
        HandIndexFill,
        HandIndexThumbFill,
        HandIndexThumb,
        HandIndex,
        HandThumbsDownFill,
        HandThumbsDown,
        HandThumbsUpFill,
        HandThumbsUp,
        HandbagFill,
        Handbag,
        Hash,
        HddFill,
        HddNetworkFill,
        HddNetwork,
        HddRackFill,
        HddRack,
        HddStackFill,
        HddStack,
        Hdd,
        Headphones,
        Headset,
        HeartFill,
        HeartHalf,
        Heart,
        HeptagonFill,
        HeptagonHalf,
        Heptagon,
        HexagonFill,
        HexagonHalf,
        Hexagon,
        HourglassBottom,
        HourglassSplit,
        HourglassTop,
        Hourglass,
        HouseDoorFill,
        HouseDoor,
        HouseFill,
        House,
        Hr,
        Hurricane,
        ImageAlt,
        ImageFill,
        Image,
        Images,
        InboxFill,
        Inbox,
        InboxesFill,
        Inboxes,
        InfoCircleFill,
        InfoCircle,
        InfoSquareFill,
        InfoSquare,
        Info,
        InputCursorText,
        InputCursor,
        Instagram,
        Intersect,
        JournalAlbum,
        JournalArrowDown,
        JournalArrowUp,
        JournalBookmarkFill,
        JournalBookmark,
        JournalCheck,
        JournalCode,
        JournalMedical,
        JournalMinus,
        JournalPlus,
        JournalRichtext,
        JournalText,
        JournalX,
        Journal,
        Journals,
        Joystick,
        JustifyLeft,
        JustifyRight,
        Justify,
        KanbanFill,
        Kanban,
        KeyFill,
        Key,
        KeyboardFill,
        Keyboard,
        Ladder,
        LampFill,
        Lamp,
        LaptopFill,
        Laptop,
        LayerBackward,
        LayerForward,
        LayersFill,
        LayersHalf,
        Layers,
        LayoutSidebarInsetReverse,
        LayoutSidebarInset,
        LayoutSidebarReverse,
        LayoutSidebar,
        LayoutSplit,
        LayoutTextSidebarReverse,
        LayoutTextSidebar,
        LayoutTextWindowReverse,
        LayoutTextWindow,
        LayoutThreeColumns,
        LayoutWtf,
        LifePreserver,
        LightbulbFill,
        LightbulbOffFill,
        LightbulbOff,
        Lightbulb,
        LightningChargeFill,
        LightningCharge,
        LightningFill,
        Lightning,
        Link45deg,
        Link,
        Linkedin,
        ListCheck,
        ListNested,
        ListOl,
        ListStars,
        ListTask,
        ListUl,
        List,
        LockFill,
        Lock,
        Mailbox,
        Mailbox2,
        MapFill,
        Map,
        MarkdownFill,
        Markdown,
        Mask,
        MegaphoneFill,
        Megaphone,
        MenuAppFill,
        MenuApp,
        MenuButtonFill,
        MenuButtonWideFill,
        MenuButtonWide,
        MenuButton,
        MenuDown,
        MenuUp,
        MicFill,
        MicMuteFill,
        MicMute,
        Mic,
        MinecartLoaded,
        Minecart,
        Moisture,
        MoonFill,
        MoonStarsFill,
        MoonStars,
        Moon,
        MouseFill,
        Mouse,
        Mouse2Fill,
        Mouse2,
        Mouse3Fill,
        Mouse3,
        MusicNoteBeamed,
        MusicNoteList,
        MusicNote,
        MusicPlayerFill,
        MusicPlayer,
        Newspaper,
        NodeMinusFill,
        NodeMinus,
        NodePlusFill,
        NodePlus,
        NutFill,
        Nut,
        OctagonFill,
        OctagonHalf,
        Octagon,
        Option,
        Outlet,
        PaintBucket,
        PaletteFill,
        Palette,
        Palette2,
        Paperclip,
        Paragraph,
        PatchCheckFill,
        PatchCheck,
        PatchExclamationFill,
        PatchExclamation,
        PatchMinusFill,
        PatchMinus,
        PatchPlusFill,
        PatchPlus,
        PatchQuestionFill,
        PatchQuestion,
        PauseBtnFill,
        PauseBtn,
        PauseCircleFill,
        PauseCircle,
        PauseFill,
        Pause,
        PeaceFill,
        Peace,
        PenFill,
        Pen,
        PencilFill,
        PencilSquare,
        Pencil,
        PentagonFill,
        PentagonHalf,
        Pentagon,
        PeopleFill,
        People,
        Percent,
        PersonBadgeFill,
        PersonBadge,
        PersonBoundingBox,
        PersonCheckFill,
        PersonCheck,
        PersonCircle,
        PersonDashFill,
        PersonDash,
        PersonFill,
        PersonLinesFill,
        PersonPlusFill,
        PersonPlus,
        PersonSquare,
        PersonXFill,
        PersonX,
        Person,
        PhoneFill,
        PhoneLandscapeFill,
        PhoneLandscape,
        PhoneVibrateFill,
        PhoneVibrate,
        Phone,
        PieChartFill,
        PieChart,
        PinAngleFill,
        PinAngle,
        PinFill,
        Pin,
        PipFill,
        Pip,
        PlayBtnFill,
        PlayBtn,
        PlayCircleFill,
        PlayCircle,
        PlayFill,
        Play,
        PlugFill,
        Plug,
        PlusCircleDotted,
        PlusCircleFill,
        PlusCircle,
        PlusSquareDotted,
        PlusSquareFill,
        PlusSquare,
        Plus,
        Power,
        PrinterFill,
        Printer,
        PuzzleFill,
        Puzzle,
        QuestionCircleFill,
        QuestionCircle,
        QuestionDiamondFill,
        QuestionDiamond,
        QuestionOctagonFill,
        QuestionOctagon,
        QuestionSquareFill,
        QuestionSquare,
        Question,
        Rainbow,
        ReceiptCutoff,
        Receipt,
        Reception0,
        Reception1,
        Reception2,
        Reception3,
        Reception4,
        RecordBtnFill,
        RecordBtn,
        RecordCircleFill,
        RecordCircle,
        RecordFill,
        Record,
        Record2Fill,
        Record2,
        ReplyAllFill,
        ReplyAll,
        ReplyFill,
        Reply,
        RssFill,
        Rss,
        Rulers,
        SaveFill,
        Save,
        Save2Fill,
        Save2,
        Scissors,
        Screwdriver,
        Search,
        SegmentedNav,
        Server,
        ShareFill,
        Share,
        ShieldCheck,
        ShieldExclamation,
        ShieldFillCheck,
        ShieldFillExclamation,
        ShieldFillMinus,
        ShieldFillPlus,
        ShieldFillX,
        ShieldFill,
        ShieldLockFill,
        ShieldLock,
        ShieldMinus,
        ShieldPlus,
        ShieldShaded,
        ShieldSlashFill,
        ShieldSlash,
        ShieldX,
        Shield,
        ShiftFill,
        Shift,
        ShopWindow,
        Shop,
        Shuffle,
        Signpost2Fill,
        Signpost2,
        SignpostFill,
        SignpostSplitFill,
        SignpostSplit,
        Signpost,
        SimFill,
        Sim,
        SkipBackwardBtnFill,
        SkipBackwardBtn,
        SkipBackwardCircleFill,
        SkipBackwardCircle,
        SkipBackwardFill,
        SkipBackward,
        SkipEndBtnFill,
        SkipEndBtn,
        SkipEndCircleFill,
        SkipEndCircle,
        SkipEndFill,
        SkipEnd,
        SkipForwardBtnFill,
        SkipForwardBtn,
        SkipForwardCircleFill,
        SkipForwardCircle,
        SkipForwardFill,
        SkipForward,
        SkipStartBtnFill,
        SkipStartBtn,
        SkipStartCircleFill,
        SkipStartCircle,
        SkipStartFill,
        SkipStart,
        Slack,
        SlashCircleFill,
        SlashCircle,
        SlashSquareFill,
        SlashSquare,
        Slash,
        Sliders,
        Smartwatch,
        Snow,
        Snow2,
        Snow3,
        SortAlphaDownAlt,
        SortAlphaDown,
        SortAlphaUpAlt,
        SortAlphaUp,
        SortDownAlt,
        SortDown,
        SortNumericDownAlt,
        SortNumericDown,
        SortNumericUpAlt,
        SortNumericUp,
        SortUpAlt,
        SortUp,
        Soundwave,
        SpeakerFill,
        Speaker,
        Speedometer,
        Speedometer2,
        Spellcheck,
        SquareFill,
        SquareHalf,
        Square,
        Stack,
        StarFill,
        StarHalf,
        Star,
        Stars,
        StickiesFill,
        Stickies,
        StickyFill,
        Sticky,
        StopBtnFill,
        StopBtn,
        StopCircleFill,
        StopCircle,
        StopFill,
        Stop,
        StoplightsFill,
        Stoplights,
        StopwatchFill,
        Stopwatch,
        Subtract,
        SuitClubFill,
        SuitClub,
        SuitDiamondFill,
        SuitDiamond,
        SuitHeartFill,
        SuitHeart,
        SuitSpadeFill,
        SuitSpade,
        SunFill,
        Sun,
        Sunglasses,
        SunriseFill,
        Sunrise,
        SunsetFill,
        Sunset,
        SymmetryHorizontal,
        SymmetryVertical,
        Table,
        TabletFill,
        TabletLandscapeFill,
        TabletLandscape,
        Tablet,
        TagFill,
        Tag,
        TagsFill,
        Tags,
        Telegram,
        TelephoneFill,
        TelephoneForwardFill,
        TelephoneForward,
        TelephoneInboundFill,
        TelephoneInbound,
        TelephoneMinusFill,
        TelephoneMinus,
        TelephoneOutboundFill,
        TelephoneOutbound,
        TelephonePlusFill,
        TelephonePlus,
        TelephoneXFill,
        TelephoneX,
        Telephone,
        TerminalFill,
        Terminal,
        TextCenter,
        TextIndentLeft,
        TextIndentRight,
        TextLeft,
        TextParagraph,
        TextRight,
        TextareaResize,
        TextareaT,
        Textarea,
        ThermometerHalf,
        ThermometerHigh,
        ThermometerLow,
        ThermometerSnow,
        ThermometerSun,
        Thermometer,
        ThreeDotsVertical,
        ThreeDots,
        ToggleOff,
        ToggleOn,
        Toggle2Off,
        Toggle2On,
        Toggles,
        Toggles2,
        Tools,
        Tornado,
        TrashFill,
        Trash,
        Trash2Fill,
        Trash2,
        TreeFill,
        Tree,
        TriangleFill,
        TriangleHalf,
        Triangle,
        TrophyFill,
        Trophy,
        TropicalStorm,
        TruckFlatbed,
        Truck,
        Tsunami,
        TvFill,
        Tv,
        Twitch,
        Twitter,
        TypeBold,
        TypeH1,
        TypeH2,
        TypeH3,
        TypeItalic,
        TypeStrikethrough,
        TypeUnderline,
        Type,
        UiChecksGrid,
        UiChecks,
        UiRadiosGrid,
        UiRadios,
        UmbrellaFill,
        Umbrella,
        Union,
        UnlockFill,
        Unlock,
        UpcScan,
        Upc,
        Upload,
        VectorPen,
        ViewList,
        ViewStacked,
        VinylFill,
        Vinyl,
        Voicemail,
        VolumeDownFill,
        VolumeDown,
        VolumeMuteFill,
        VolumeMute,
        VolumeOffFill,
        VolumeOff,
        VolumeUpFill,
        VolumeUp,
        Vr,
        WalletFill,
        Wallet,
        Wallet2,
        Watch,
        Water,
        Whatsapp,
        Wifi1,
        Wifi2,
        WifiOff,
        Wifi,
        Wind,
        WindowDock,
        WindowSidebar,
        Window,
        Wrench,
        XCircleFill,
        XCircle,
        XDiamondFill,
        XDiamond,
        XOctagonFill,
        XOctagon,
        XSquareFill,
        XSquare,
        X,
        Youtube,
        ZoomIn,
        ZoomOut,
        Bank,
        Bank2,
        BellSlashFill,
        BellSlash,
        CashCoin,
        CheckLg,
        Coin,
        CurrencyBitcoin,
        CurrencyDollar,
        CurrencyEuro,
        CurrencyExchange,
        CurrencyPound,
        CurrencyYen,
        DashLg,
        ExclamationLg,
        FileEarmarkPdfFill,
        FileEarmarkPdf,
        FilePdfFill,
        FilePdf,
        GenderAmbiguous,
        GenderFemale,
        GenderMale,
        GenderTrans,
        HeadsetVr,
        InfoLg,
        Mastodon,
        Messenger,
        PiggyBankFill,
        PiggyBank,
        PinMapFill,
        PinMap,
        PlusLg,
        QuestionLg,
        Recycle,
        Reddit,
        SafeFill,
        Safe2Fill,
        Safe2,
        SdCardFill,
        SdCard,
        Skype,
        SlashLg,
        Translate,
        XLg,
        Safe,
        Apple,
        Microsoft,
        Windows,
        Behance,
        Dribbble,
        Line,
        Medium,
        Paypal,
        Pinterest,
        Signal,
        Snapchat,
        Spotify,
        StackOverflow,
        Strava,
        Wordpress,
        Vimeo,
        Activity,
        Easel2Fill,
        Easel2,
        Easel3Fill,
        Easel3,
        Fan,
        Fingerprint,
        GraphDownArrow,
        GraphUpArrow,
        Hypnotize,
        Magic,
        PersonRolodex,
        PersonVideo,
        PersonVideo2,
        PersonVideo3,
        PersonWorkspace,
        Radioactive,
        WebcamFill,
        Webcam,
        YinYang,
        BandaidFill,
        Bandaid,
        Bluetooth,
        BodyText,
        Boombox,
        Boxes,
        DpadFill,
        Dpad,
        EarFill,
        Ear,
        EnvelopeCheck1,
        EnvelopeCheckFill,
        EnvelopeCheck,
        EnvelopeDash1,
        EnvelopeDashFill,
        EnvelopeDash,
        EnvelopeExclamation1,
        EnvelopeExclamationFill,
        EnvelopeExclamation,
        EnvelopePlusFill,
        EnvelopePlus,
        EnvelopeSlash1,
        EnvelopeSlashFill,
        EnvelopeSlash,
        EnvelopeX1,
        EnvelopeXFill,
        EnvelopeX,
        ExplicitFill,
        Explicit,
        Git,
        Infinity,
        ListColumnsReverse,
        ListColumns,
        Meta,
        MortorboardFill,
        Mortorboard,
        NintendoSwitch,
        PcDisplayHorizontal,
        PcDisplay,
        PcHorizontal,
        Pc,
        Playstation,
        PlusSlashMinus,
        ProjectorFill,
        Projector,
        QrCodeScan,
        QrCode,
        Quora,
        Quote,
        Robot,
        SendCheckFill,
        SendCheck,
        SendDashFill,
        SendDash,
        SendExclamation1,
        SendExclamationFill,
        SendExclamation,
        SendFill,
        SendPlusFill,
        SendPlus,
        SendSlashFill,
        SendSlash,
        SendXFill,
        SendX,
        Send,
        Steam,
        TerminalDash1,
        TerminalDash,
        TerminalPlus,
        TerminalSplit,
        TicketDetailedFill,
        TicketDetailed,
        TicketFill,
        TicketPerforatedFill,
        TicketPerforated,
        Ticket,
        Tiktok,
        WindowDash,
        WindowDesktop,
        WindowFullscreen,
        WindowPlus,
        WindowSplit,
        WindowStack,
        WindowX,
        Xbox,
        Ethernet,
        HdmiFill,
        Hdmi,
        UsbCFill,
        UsbC,
        UsbFill,
        UsbPlugFill,
        UsbPlug,
        UsbSymbol,
        Usb,
        BoomboxFill,
        Displayport1,
        Displayport,
        GpuCard,
        Memory,
        ModemFill,
        Modem,
        MotherboardFill,
        Motherboard,
        OpticalAudioFill,
        OpticalAudio,
        PciCard,
        RouterFill,
        Router,
        SsdFill,
        Ssd,
        ThunderboltFill,
        Thunderbolt,
        UsbDriveFill,
        UsbDrive,
        UsbMicroFill,
        UsbMicro,
        UsbMiniFill,
        UsbMini,
        CloudHaze2,
        DeviceHddFill,
        DeviceHdd,
        DeviceSsdFill,
        DeviceSsd,
        DisplayportFill,
        MortarboardFill,
        Mortarboard,
        TerminalX,
    }
    public enum SearchFilterOptions
    {
        PropertyName,
        PropertyNiceName,
        TypeOfValue,
        ValueToString,
        ISearchFilterableInterface,
        All,
    }
    public enum TableAxis
    {
        X,
        Y,
    }
    public enum LabelDirection
    {
        LeftToRight,
        TopToBottom,
        BottomToTop,
    }
    public enum TitleAlignments
    {
        Left,
        Centered,
        Right,
        Split,
    }
    public enum TypeInclusionFilter
    {
        None,
        IncludeConcreteTypes,
        IncludeGenerics,
        IncludeAbstracts,
        IncludeInterfaces,
        IncludeAll,
    }
    public class EditorOnlyModeConfigUtility { }
    public class SerializedBehaviour { }
    public class SerializedComponent { }
    public class SerializedMonoBehaviour { }
    public class SerializedScriptableObject { }
    public class SerializedStateMachineBehaviour { }
    public class SerializedUnityObject { }
}

namespace Sirenix.Serialization
{
}
#endif
