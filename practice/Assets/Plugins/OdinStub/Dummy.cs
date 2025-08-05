using System;

namespace Sirenix.OdinInspector
{
#if ODIN_INSPECTOR
    // Odin이 존재하면 아무것도 정의하지 않음
#else
    // 🔹 인스펙터 표시/비표시
    public class ShowInInspector : Attribute { }
    public class HideInInspector : Attribute { }

    // 🔹 라벨 제어
    public class HideLabel : Attribute { }
    public class LabelText : Attribute
    {
        public LabelText(string label) { }
    }

    // 🔹 인라인 표시
    public class InlineProperty : Attribute { }
    public class InlineEditor : Attribute
    {
        public InlineEditor() { }
        public InlineEditor(InlineEditorObjectFieldModes mode) { }
    }

    public enum InlineEditorObjectFieldModes
    {
        Hidden = 0,
        Foldout = 1,
        CompressedFoldout = 2,
        Boxed = 3,
        BoxedAndFoldout = 4,
    }

    // 🔹 ReadOnly
    public class ReadOnlyAttribute : Attribute { public ReadOnlyAttribute() { } }

    // 🔹 GUI 색상
    public class GUIColor : Attribute
    {
        public GUIColor(float r, float g, float b, float a = 1f) { }
    }

    // 🔹 InfoBox
    public class InfoBox : Attribute
    {
        public InfoBox(string message) { }
        public InfoBox(string message, InfoMessageType type) { }
    }

    public enum InfoMessageType
    {
        None = 0,
        Info = 1,
        Warning = 2,
        Error = 3
    }

    // 🔹 Foldout, Box, Tab 그룹
    public class FoldoutGroup : Attribute
    {
        public FoldoutGroup(string name) { }
    }

    public class BoxGroup : Attribute
    {
        public BoxGroup(string name) { }
    }

    public class TabGroup : Attribute
    {
        public TabGroup(string name) { }
        public TabGroup(string group, string tab) { }
    }


    // 🔹 Horizontal, Vertical Group
    public class HorizontalGroup : Attribute
    {
        public HorizontalGroup(string groupName) { }
    }

    public class VerticalGroup : Attribute
    {
        public VerticalGroup(string groupName) { }
    }

    // 🔹 Title
    public class Title : Attribute
    {
        public Title(string title, string subtitle = null, TitleAlignments alignment = TitleAlignments.Left, bool bold = true, bool underline = false) { }
    }

    public enum TitleAlignments
    {
        Left = 0,
        Center = 1
    }

    // 🔹 GUI Wrapper
    public class PropertySpace : Attribute
    {
        public PropertySpace(float top = 0, float bottom = 0) { }
    }

    public class PropertyOrder : Attribute
    {
        public PropertyOrder(int order) { }
    }

    public class PropertyTooltip : Attribute
    {
        public PropertyTooltip(string tooltip) { }
    }

    public class Indent : Attribute
    {
        public Indent(int indentLevel = 1) { }
    }
    // 🔹 조건부 노출
    public class ShowIf : Attribute
    {
        public ShowIf(string condition) { }
        public ShowIf(string memberName, object optionalValue) { }
    }

    public class HideIf : Attribute
    {
        public HideIf(string condition) { }
        public HideIf(string memberName, object optionalValue) { }
    }

    // 🔹 조건부 활성화
    public class EnableIf : Attribute
    {
        public EnableIf(string condition) { }
        public EnableIf(string memberName, object optionalValue) { }
    }

    public class DisableIf : Attribute
    {
        public DisableIf(string condition) { }
        public DisableIf(string memberName, object optionalValue) { }
    }

    // 🔹 조건부 필드 숨김/비활성화
    public class Required : Attribute
    {
        public Required(string errorMessage = null) { }
    }

    public class ValidateInput : Attribute
    {
        public ValidateInput(string condition) { }
        public ValidateInput(string condition, string message) { }
    }

    public class OnValueChanged : Attribute
    {
        public OnValueChanged(string callbackName) { }
    }
    public class Button : Attribute
    {
        public Button() { }
        public Button(ButtonSizes size) { }
        public Button(string name) { }
    }

    public enum ButtonSizes
    {
        Small = 0,
        Medium = 1,
        Large = 2
    }

    // 🔹 툴팁
    public class Tooltip : Attribute
    {
        public Tooltip(string tooltip) { }
    }

    // 🔹 Enum 선택
    public class EnumToggleButtons : Attribute { }

    public class EnumPagingAttribute : Attribute
    {
        public EnumPagingAttribute() { }
    }

    // 🔹 범위 제한
    public class PropertyRange : Attribute
    {
        public PropertyRange(float min, float max) { }
        public PropertyRange(int min, int max) { }
    }
    // 🔹 리스트 표시 설정
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

    // 🔹 딕셔너리 표시 설정
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

    // 🔹 필드 이름 재정의
    public class ValueDropdown : Attribute
    {
        public ValueDropdown(string methodName) { }
    }

    public class AssetsOnly : Attribute { }
    public class SceneObjectsOnly : Attribute { }
    // 🔹 상태 변화에 대한 트리거
    public class OnInspectorInit : Attribute
    {
        public OnInspectorInit() { }
    }

    public class OnInspectorGUI : Attribute
    {
        public OnInspectorGUI(string methodName = null) { }
    }

    public class OnCollectionChanged : Attribute
    {
        public OnCollectionChanged(string methodName = null) { }
    }

    // 🔹 인포 메시지 제한 조건
    public class ShowIfGroup : Attribute
    {
        public ShowIfGroup(string condition) { }
    }

    public class HideIfGroup : Attribute
    {
        public HideIfGroup(string condition) { }
    }

    public class ToggleGroup : Attribute
    {
        public ToggleGroup(string toggleMemberName) { }
    }

    // 🔹 커스텀 컨트롤 계열 (빈 정의만)
    public class CustomValueDrawer : Attribute
    {
        public CustomValueDrawer(string methodName) { }
    }

    public class CustomContextMenu : Attribute
    {
        public CustomContextMenu(string methodName, string displayName = null) { }
    }

    public class AssetList : Attribute
    {
        public AssetList() { }
    }

    public class AssetSelector : Attribute
    {
        public AssetSelector() { }
        public AssetSelector(string path) { }
    }

    // 🔹 정렬
    public class TableList : Attribute
    {
        public TableList() { }
    }

    public class TableColumnWidth : Attribute
    {
        public TableColumnWidth(int width) { }
    }
#endif
}

namespace Sirenix.Serialization
{
#if ODIN_INSPECTOR
    // Odin Serializer가 존재할 경우 생략
#else
    // 🔹 직렬화 관련
    public class OdinSerialize : Attribute { }

    public class NonSerializedInInspector : Attribute { }

    public class HideInInspector : Attribute { } // 충돌 방지

    // ShowInInspector는 Sirenix.OdinInspector만 정의 (충돌 방지 목적)
#endif
}