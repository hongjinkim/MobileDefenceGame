using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gpm.AssetManagement.AssetFind.Ui.PropertyTreeView
{
    internal interface IPropertyItemGUI
    {
        bool CanExpanded();

        void OnExpanded(bool expand);

        void OnClick();

        void OnDoubleClick();

        void RowGUI();

        void CellGUI(Rect cellRect, PropertyTreeView.ColumnId column);
    }
}