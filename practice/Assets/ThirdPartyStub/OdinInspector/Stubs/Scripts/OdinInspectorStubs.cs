#if !ODIN_INSPECTOR
using System;

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
        public BoxGroupAttribute(String group, Boolean showLabel, Boolean centerLabel, Single order) { }
        public BoxGroupAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class ButtonAttribute : Attribute
    {
        public ButtonAttribute() { }
        public ButtonAttribute(ButtonSizes size) { }
        public ButtonAttribute(Int32 buttonSize) { }
        public ButtonAttribute(String name) { }
        public ButtonAttribute(String name, ButtonSizes buttonSize) { }
        public ButtonAttribute(String name, Int32 buttonSize) { }
        public ButtonAttribute(ButtonStyle parameterBtnStyle) { }
        public ButtonAttribute(Int32 buttonSize, ButtonStyle parameterBtnStyle) { }
        public ButtonAttribute(ButtonSizes size, ButtonStyle parameterBtnStyle) { }
        public ButtonAttribute(String name, ButtonStyle parameterBtnStyle) { }
        public ButtonAttribute(String name, ButtonSizes buttonSize, ButtonStyle parameterBtnStyle) { }
        public ButtonAttribute(String name, Int32 buttonSize, ButtonStyle parameterBtnStyle) { }
        public ButtonAttribute(SdfIconType icon, IconAlignment iconAlignment) { }
        public ButtonAttribute(SdfIconType icon) { }
        public ButtonAttribute(SdfIconType icon, String name) { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class ButtonGroupAttribute : Attribute
    {
        public ButtonGroupAttribute(String group, Single order) { }
        public ButtonGroupAttribute() { }
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
        public ColorPaletteAttribute(String paletteName) { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class CustomContextMenuAttribute : Attribute
    {
        public CustomContextMenuAttribute(String menuItem, String action) { }
        public CustomContextMenuAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class CustomValueDrawerAttribute : Attribute
    {
        public CustomValueDrawerAttribute(String action) { }
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
        public DetailedInfoBoxAttribute(String message, String details, InfoMessageType infoMessageType, String visibleIf) { }
        public DetailedInfoBoxAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class DictionaryDrawerSettings : Attribute
    {
        public DictionaryDrawerSettings() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class DisableContextMenuAttribute : Attribute
    {
        public DisableContextMenuAttribute(Boolean disableForMember, Boolean disableCollectionElements) { }
        public DisableContextMenuAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class DisableIfAttribute : Attribute
    {
        public DisableIfAttribute(String condition) { }
        public DisableIfAttribute(String condition, Object optionalValue) { }
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
        public DisplayAsStringAttribute(Boolean overflow) { }
        public DisplayAsStringAttribute(TextAlignment alignment) { }
        public DisplayAsStringAttribute(Int32 fontSize) { }
        public DisplayAsStringAttribute(Boolean overflow, TextAlignment alignment) { }
        public DisplayAsStringAttribute(Boolean overflow, Int32 fontSize) { }
        public DisplayAsStringAttribute(Int32 fontSize, TextAlignment alignment) { }
        public DisplayAsStringAttribute(Boolean overflow, Int32 fontSize, TextAlignment alignment) { }
        public DisplayAsStringAttribute(TextAlignment alignment, Boolean enableRichText) { }
        public DisplayAsStringAttribute(Int32 fontSize, Boolean enableRichText) { }
        public DisplayAsStringAttribute(Boolean overflow, TextAlignment alignment, Boolean enableRichText) { }
        public DisplayAsStringAttribute(Boolean overflow, Int32 fontSize, Boolean enableRichText) { }
        public DisplayAsStringAttribute(Int32 fontSize, TextAlignment alignment, Boolean enableRichText) { }
        public DisplayAsStringAttribute(Boolean overflow, Int32 fontSize, TextAlignment alignment, Boolean enableRichText) { }
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
        public EnableIfAttribute(String condition) { }
        public EnableIfAttribute(String condition, Object optionalValue) { }
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
        public FoldoutGroupAttribute(String groupName, Single order) { }
        public FoldoutGroupAttribute(String groupName, Boolean expanded, Single order) { }
        public FoldoutGroupAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class GUIColorAttribute : Attribute
    {
        public GUIColorAttribute(Single r, Single g, Single b, Single a) { }
        public GUIColorAttribute(String getColor) { }
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
        public HideIfAttribute(String condition, Boolean animate) { }
        public HideIfAttribute(String condition, Object optionalValue, Boolean animate) { }
        public HideIfAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class HideIfGroupAttribute : Attribute
    {
        public HideIfGroupAttribute(String path, Boolean animate) { }
        public HideIfGroupAttribute(String path, Object value, Boolean animate) { }
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
        public HorizontalGroupAttribute(String group, Single width, Int32 marginLeft, Int32 marginRight, Single order) { }
        public HorizontalGroupAttribute(Single width, Int32 marginLeft, Int32 marginRight, Single order) { }
        public HorizontalGroupAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class IncludeMyAttributesAttribute : Attribute
    {
        public IncludeMyAttributesAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class IndentAttribute : Attribute
    {
        public IndentAttribute(Int32 indentLevel) { }
        public IndentAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class InfoBoxAttribute : Attribute
    {
        public InfoBoxAttribute(String message, InfoMessageType infoMessageType, String visibleIfMemberName) { }
        public InfoBoxAttribute(String message, String visibleIfMemberName) { }
        public InfoBoxAttribute(String message, SdfIconType icon, String visibleIfMemberName) { }
        public InfoBoxAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class InlineButtonAttribute : Attribute
    {
        public InlineButtonAttribute(String action, String label) { }
        public InlineButtonAttribute(String action, SdfIconType icon, String label) { }
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
        public LabelTextAttribute(String text) { }
        public LabelTextAttribute(SdfIconType icon) { }
        public LabelTextAttribute(String text, Boolean nicifyText) { }
        public LabelTextAttribute(String text, SdfIconType icon) { }
        public LabelTextAttribute(String text, Boolean nicifyText, SdfIconType icon) { }
        public LabelTextAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class LabelWidthAttribute : Attribute
    {
        public LabelWidthAttribute(Single width) { }
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
        public MaxValueAttribute(String expression) { }
        public MaxValueAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class MinMaxSliderAttribute : Attribute
    {
        public MinMaxSliderAttribute(Single minValue, Single maxValue, Boolean showFields) { }
        public MinMaxSliderAttribute(String minValueGetter, Single maxValue, Boolean showFields) { }
        public MinMaxSliderAttribute(Single minValue, String maxValueGetter, Boolean showFields) { }
        public MinMaxSliderAttribute(String minValueGetter, String maxValueGetter, Boolean showFields) { }
        public MinMaxSliderAttribute(String minMaxValueGetter, Boolean showFields) { }
        public MinMaxSliderAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class MinValueAttribute : Attribute
    {
        public MinValueAttribute(Double minValue) { }
        public MinValueAttribute(String expression) { }
        public MinValueAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class MultiLinePropertyAttribute : Attribute
    {
        public MultiLinePropertyAttribute(Int32 lines) { }
        public MultiLinePropertyAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class OdinRegisterAttributeAttribute : Attribute
    {
        public OdinRegisterAttributeAttribute(Type attributeType, String category, String description, Boolean isEnterprise) { }
        public OdinRegisterAttributeAttribute(Type attributeType, String category, String description, Boolean isEnterprise, String url) { }
        public OdinRegisterAttributeAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class OnCollectionChangedAttribute : Attribute
    {
        public OnCollectionChangedAttribute() { }
        public OnCollectionChangedAttribute(String after) { }
        public OnCollectionChangedAttribute(String before, String after) { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class OnInspectorDisposeAttribute : Attribute
    {
        public OnInspectorDisposeAttribute() { }
        public OnInspectorDisposeAttribute(String action) { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class OnInspectorGUIAttribute : Attribute
    {
        public OnInspectorGUIAttribute() { }
        public OnInspectorGUIAttribute(String action, Boolean append) { }
        public OnInspectorGUIAttribute(String prepend, String append) { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class OnInspectorInitAttribute : Attribute
    {
        public OnInspectorInitAttribute() { }
        public OnInspectorInitAttribute(String action) { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class OnStateUpdateAttribute : Attribute
    {
        public OnStateUpdateAttribute(String action) { }
        public OnStateUpdateAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class OnValueChangedAttribute : Attribute
    {
        public OnValueChangedAttribute(String action, Boolean includeChildren) { }
        public OnValueChangedAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class OptionalAttribute : Attribute
    {
        public OptionalAttribute() { }
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
        public PreviewFieldAttribute(Single height) { }
        public PreviewFieldAttribute(String previewGetter, FilterMode filterMode) { }
        public PreviewFieldAttribute(String previewGetter, Single height, FilterMode filterMode) { }
        public PreviewFieldAttribute(Single height, ObjectFieldAlignment alignment) { }
        public PreviewFieldAttribute(String previewGetter, ObjectFieldAlignment alignment, FilterMode filterMode) { }
        public PreviewFieldAttribute(String previewGetter, Single height, ObjectFieldAlignment alignment, FilterMode filterMode) { }
        public PreviewFieldAttribute(ObjectFieldAlignment alignment) { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class ProgressBarAttribute : Attribute
    {
        public ProgressBarAttribute(Double min, Double max, Single r, Single g, Single b) { }
        public ProgressBarAttribute(String minGetter, Double max, Single r, Single g, Single b) { }
        public ProgressBarAttribute(Double min, String maxGetter, Single r, Single g, Single b) { }
        public ProgressBarAttribute(String minGetter, String maxGetter, Single r, Single g, Single b) { }
        public ProgressBarAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class PropertyGroupAttribute : Attribute
    {
        public PropertyGroupAttribute(String groupId, Single order) { }
        public PropertyGroupAttribute(String groupId) { }
        public PropertyGroupAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class PropertyOrderAttribute : Attribute
    {
        public PropertyOrderAttribute() { }
        public PropertyOrderAttribute(Single order) { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class PropertyRangeAttribute : Attribute
    {
        public PropertyRangeAttribute(Double min, Double max) { }
        public PropertyRangeAttribute(String minGetter, Double max) { }
        public PropertyRangeAttribute(Double min, String maxGetter) { }
        public PropertyRangeAttribute(String minGetter, String maxGetter) { }
        public PropertyRangeAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class PropertySpaceAttribute : Attribute
    {
        public PropertySpaceAttribute() { }
        public PropertySpaceAttribute(Single spaceBefore) { }
        public PropertySpaceAttribute(Single spaceBefore, Single spaceAfter) { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class PropertyTooltipAttribute : Attribute
    {
        public PropertyTooltipAttribute(String tooltip) { }
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
        public RequiredAttribute(String errorMessage, InfoMessageType messageType) { }
        public RequiredAttribute(String errorMessage) { }
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
        public RequiredInPrefabAssetsAttribute(String errorMessage, InfoMessageType messageType) { }
        public RequiredInPrefabAssetsAttribute(String errorMessage) { }
        public RequiredInPrefabAssetsAttribute(InfoMessageType messageType) { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class RequiredInPrefabInstancesAttribute : Attribute
    {
        public RequiredInPrefabInstancesAttribute() { }
        public RequiredInPrefabInstancesAttribute(String errorMessage, InfoMessageType messageType) { }
        public RequiredInPrefabInstancesAttribute(String errorMessage) { }
        public RequiredInPrefabInstancesAttribute(InfoMessageType messageType) { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class RequiredListLengthAttribute : Attribute
    {
        public RequiredListLengthAttribute() { }
        public RequiredListLengthAttribute(Int32 fixedLength) { }
        public RequiredListLengthAttribute(Int32 minLength, Int32 maxLength) { }
        public RequiredListLengthAttribute(Int32 minLength, String maxLengthGetter) { }
        public RequiredListLengthAttribute(String fixedLengthGetter) { }
        public RequiredListLengthAttribute(String minLengthGetter, String maxLengthGetter) { }
        public RequiredListLengthAttribute(String minLengthGetter, Int32 maxLength) { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class ResponsiveButtonGroupAttribute : Attribute
    {
        public ResponsiveButtonGroupAttribute(String group) { }
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
        public ShowIfAttribute(String condition, Boolean animate) { }
        public ShowIfAttribute(String condition, Object optionalValue, Boolean animate) { }
        public ShowIfAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class ShowIfGroupAttribute : Attribute
    {
        public ShowIfGroupAttribute(String path, Boolean animate) { }
        public ShowIfGroupAttribute(String path, Object value, Boolean animate) { }
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
        public SuffixLabelAttribute(String label, Boolean overlay) { }
        public SuffixLabelAttribute(String label, SdfIconType icon, Boolean overlay) { }
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
        public TabGroupAttribute(String tab, Boolean useFixedHeight, Single order) { }
        public TabGroupAttribute(String group, String tab, Boolean useFixedHeight, Single order) { }
        public TabGroupAttribute(String group, String tab, SdfIconType icon, Boolean useFixedHeight, Single order) { }
        public TabGroupAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class TableColumnWidthAttribute : Attribute
    {
        public TableColumnWidthAttribute(Int32 width, Boolean resizable) { }
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
        public TitleAttribute(String title, String subtitle, TitleAlignments titleAlignment, Boolean horizontalLine, Boolean bold) { }
        public TitleAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class TitleGroupAttribute : Attribute
    {
        public TitleGroupAttribute(String title, String subtitle, TitleAlignments alignment, Boolean horizontalLine, Boolean boldTitle, Boolean indent, Single order) { }
        public TitleGroupAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class ToggleAttribute : Attribute
    {
        public ToggleAttribute(String toggleMemberName) { }
        public ToggleAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class ToggleGroupAttribute : Attribute
    {
        public ToggleGroupAttribute(String toggleMemberName, Single order, String groupTitle) { }
        public ToggleGroupAttribute(String toggleMemberName, String groupTitle) { }
        public ToggleGroupAttribute(String toggleMemberName, Single order, String groupTitle, String titleStringMemberName) { }
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
        public TypeFilterAttribute(String filterGetter) { }
        public TypeFilterAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class TypeInfoBoxAttribute : Attribute
    {
        public TypeInfoBoxAttribute(String message) { }
        public TypeInfoBoxAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class TypeRegistryItemAttribute : Attribute
    {
        public TypeRegistryItemAttribute(String name, String categoryPath, SdfIconType icon, Single lightIconColorR, Single lightIconColorG, Single lightIconColorB, Single lightIconColorA, Single darkIconColorR, Single darkIconColorG, Single darkIconColorB, Single darkIconColorA, Int32 priority) { }
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
        public UnitAttribute(String unit) { }
        public UnitAttribute(Units base, Units display) { }
        public UnitAttribute(Units base, String display) { }
        public UnitAttribute(String base, Units display) { }
        public UnitAttribute(String base, String display) { }
        public UnitAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class ValidateInputAttribute : Attribute
    {
        public ValidateInputAttribute(String condition, String defaultMessage, InfoMessageType messageType) { }
        public ValidateInputAttribute(String condition, String message, InfoMessageType messageType, Boolean rejectedInvalidInput) { }
        public ValidateInputAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class ValueDropdownAttribute : Attribute
    {
        public ValueDropdownAttribute(String valuesGetter) { }
        public ValueDropdownAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class VerticalGroupAttribute : Attribute
    {
        public VerticalGroupAttribute(String groupId, Single order) { }
        public VerticalGroupAttribute(Single order) { }
        public VerticalGroupAttribute() { }
    }
    [AttributeUsage(AttributeTargets.All)]
    public class WrapAttribute : Attribute
    {
        public WrapAttribute(Double min, Double max) { }
        public WrapAttribute() { }
    }
}
#endif
