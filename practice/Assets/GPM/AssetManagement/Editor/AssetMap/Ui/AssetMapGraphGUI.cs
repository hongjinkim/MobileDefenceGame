using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Gpm.AssetManagement.AssetMap.Ui
{
    using Gpm.AssetManagement.Const;

    public class AssetMapGraphGUI
    {
        private AssetMapGUI assetMap;

        public List<AssetMapGraphNode> nodes = new List<AssetMapGraphNode>();
        public List<AssetMapGraphConntection> connections = new List<AssetMapGraphConntection>();

        private float windowWidth = 180;
        private float windowHeight = 100;
        private float GapX = 90;
        private float GapY = 30;

        public Object selectObj;

        private Rect ViewRT;

        private Vector2 focusTarget = new Vector2();
        private float rate = 1;

        private Vector2 focusPos = new Vector2();

        private Rect focusRect = new Rect();
        private Rect windowGroupRect = new Rect();

        private float zoom = 1.0f;

        private bool bFirst = true;

        public AssetMapGraphGUI(AssetMapGUI assetMap)
        {
            this.assetMap = assetMap;
        }

        public void SetRootObject(Object select, bool reposition = true)
        {
            if(selectObj != select)
            {
                selectObj = select;
                
                nodes.Clear();
                connections.Clear();

                //if(reposition)
                {
                    if (selectObj != null)
                    {
                        string guid;
                        long localId;
                        if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(selectObj, out guid, out localId) == true)
                        {
                            Setting(guid);

                            focusPos = Vector2.zero;
                        }
                    }
                }
            }
        }

        public void Clear()
        {
            nodes.Clear();
            connections.Clear();
        }

        public void Setting(string guid)
        {
            ReadyNode(guid, 0, 0);

            Ready_ParentNode(guid, 0, 0);

            Ready_ChildNode(guid, 0, 0);
        }

        public void OnGUI(Rect rt)
        {
            if (selectObj != null)
            {
                string guid;
                long localId;
                if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(selectObj, out guid, out localId) == true)
                {
                    Setting(guid);
                }
            }

            if (rate < 1)
            {
                rate += Time.deltaTime;

                if (rate > 1)
                {
                    rate = 1;
                }

                focusPos = Vector2.Lerp(focusPos, focusTarget, rate);

                bFirst = false;

                assetMap.window.Repaint();
            }

            if (Event.current.type == EventType.ScrollWheel)
            {
                float beforeZoom = zoom;
                zoom -= Event.current.delta.y * Time.deltaTime;
                zoom = Mathf.Clamp(zoom, 0.5f, 1);

                float zoomChangeRate = zoom / beforeZoom;
 
                focusPos *= zoomChangeRate;

                focusRect = new Rect(focusPos.x - rt.width * 0.5f, focusPos.y - rt.height * 0.5f, rt.width, rt.height);
                focusRect.width -= GUI.skin.verticalScrollbar.fixedWidth;
                focusRect.height -= GUI.skin.horizontalScrollbar.fixedHeight;

                windowGroupRect.x *= zoomChangeRate;
                windowGroupRect.y *= zoomChangeRate;
                windowGroupRect.width *= zoomChangeRate;
                windowGroupRect.height *= zoomChangeRate;
                ViewRT = MaximumRect(focusRect, windowGroupRect);

                Event.current.Use();

                assetMap.window.Repaint();
            }


            if (Event.current.type == EventType.Repaint)
            {
                focusRect = new Rect(focusPos.x - rt.width * 0.5f, focusPos.y - rt.height * 0.5f, rt.width, rt.height);
                focusRect.width -= GUI.skin.verticalScrollbar.fixedWidth;
                focusRect.height -= GUI.skin.horizontalScrollbar.fixedHeight;
            }

            Vector2 scrollPos = new Vector2();
            if (bFirst == false)
            {
                scrollPos.x = focusRect.xMin - ViewRT.xMin;
                scrollPos.y = focusRect.yMin - ViewRT.yMin;
            }

            using (GUI.ScrollViewScope scroll = new GUI.ScrollViewScope(rt, scrollPos, ViewRT, true, true))
            {
                scroll.handleScrollWheel = false;

                for (int i = 0; i < connections.Count; i++)
                {
                    connections[i].Draw();
                }

                Rect windowRect = focusRect;


                bool bFirstGroup = true;
                Rect newWindowGroupRect = Rect.zero;

                assetMap.window.BeginWindows();

                for (int i = 0; i < nodes.Count; i++)
                {
                    if (nodes[i].bInit == false)
                    {
                        nodes[i].Init();
                    }

                    nodes[i].SetZoom(zoom);

                    if (nodes[i].obj == selectObj)
                    {
                        nodes[i].ZoomRect = GUI.Window(i, nodes[i].ZoomRect, DrawNodeWindow, Ui.GetString(nodes[i].title));
                    }
                    else
                    {
                        nodes[i].ZoomRect = GUI.Window(i, nodes[i].ZoomRect, DrawNodeWindow, "", UiStyle.SubWindow);
                    }

                    if (Event.current.type != EventType.Layout)
                    {
                        if (bFirstGroup == true)
                        {
                            newWindowGroupRect = nodes[i].ZoomRect;
                            bFirstGroup = false;
                        }
                        else
                        {
                            newWindowGroupRect = MaximumRect(newWindowGroupRect, nodes[i].ZoomRect);
                        }


                    }
                }

                assetMap.window.EndWindows();

                for (int i = 0; i < nodes.Count; i++)
                {
                    nodes[i].ConnectDraw();
                }

                if (Event.current.type == EventType.Used)
                {
                    Vector2 changePos = scroll.scrollPosition - scrollPos;
                    focusPos.x += changePos.x;
                    focusPos.y += changePos.y;
                }

                if (Event.current.type != EventType.Layout)
                {
                    windowGroupRect = newWindowGroupRect;

                    ViewRT = MaximumRect(focusRect, windowGroupRect);

                    bFirst = false;
                }
            }

            

            ProcessEvents(Event.current);
        }

        private Rect MaximumRect(Rect srcRect, Rect destRect)
        {
            Rect returnRect = new Rect(srcRect);
            if (returnRect.xMin > destRect.xMin)
            {
                returnRect.xMin = destRect.xMin;
            }

            if (returnRect.xMax < destRect.xMax)
            {
                returnRect.xMax = destRect.xMax;
            }

            if (returnRect.yMin > destRect.yMin)
            {
                returnRect.yMin = destRect.yMin;
            }

            if (returnRect.yMax < destRect.yMax)
            {
                returnRect.yMax = destRect.yMax;
            }

            return returnRect;
        }

        public AssetMapGraphNode GetNode(string guid)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].guid.Equals(guid) == true)
                {
                    return nodes[i];
                }
            }

            return null;
        }


        private AssetMapGraphNode ReadyNode(string guid, float x, float y)
        {
            var node = GetNode(guid);
            if (node == null)
            {
                x -= windowWidth * 0.5f;
                y -= windowHeight * 0.5f;

                node = new AssetMapGraphNode(assetMap, guid, new Vector2(x, y), windowWidth, windowHeight);
                nodes.Add(node);

                windowGroupRect = MaximumRect(windowGroupRect, node.ZoomRect);
            }
            return node;
        }

        private AssetMapGraphNode Ready_ParentNode(string guid, float x, float y)
        {
            var dependency = GpmAssetManagementManager.GetAssetDataFromGUID(guid);

            float yCenter = (dependency.referenceLinks.Count - 1) * (windowHeight + GapY) * 0.5f;

            var node = ReadyNode(guid, x, y);

            int count = 0;
            foreach (var parent in dependency.referenceLinks)
            {
                if (dependency.dependencyLinks.Exists(child => child.guid.Equals(parent.guid)))
                {
                    continue;
                }
                
                var parentToolNode = GetNode(parent.guid);
                if (parentToolNode == null)
                {
                    parentToolNode = Ready_ParentNode(parent.guid, x - (windowWidth + GapX), y + (count * (windowHeight + GapY)) - yCenter);
                }

                var connection = GetConnection(parentToolNode, node);
                if (connection == null)
                {
                    connections.Add(new AssetMapGraphConntection(parentToolNode, node));
                }

                count++;
            }

            return node;
        }

        private AssetMapGraphNode Ready_ChildNode(string guid, float x, float y)
        {
            var dependency = GpmAssetManagementManager.GetAssetDataFromGUID(guid);

            float yCenter = (dependency.dependencyLinks.Count - 1) * (windowHeight + GapY) * 0.5f;

            var node = ReadyNode(guid, x, y);

            int count = 0;
            foreach (var child in dependency.dependencyLinks)
            {
                var childToolNode = GetNode(child.guid);
                if (childToolNode == null)
                {
                    childToolNode = Ready_ChildNode(child.guid, x + (windowWidth + GapX), y + (count * (windowHeight + GapY)) - yCenter);
                }


                var connection = GetConnection(node, childToolNode);
                if (connection == null)
                {
                    connections.Add(new AssetMapGraphConntection(node, childToolNode));
                }

                count++;
            }

            return node;
        }

        private AssetMapGraphConntection GetConnection(AssetMapGraphNode left, AssetMapGraphNode right)
        {
            for (int i = 0; i < connections.Count; i++)
            {
                if (connections[i].leftNode == left &&
                    connections[i].rightNode == right)
                {
                    return connections[i];
                }
            }

            return null;
        }

        private void DrawNodeWindow(int id)
        {
            nodes[id].Draw();
            GUI.DragWindow();
        }

        public void FocusPostion(Vector2 pos)
        {
            focusTarget = pos;
            rate = 0;
        }

        public void ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    {

                    }
                    break;

                case EventType.MouseUp:
                    {

                    }
                    break;

                case EventType.MouseDrag:
                    {
                        if (e.button == 2)
                        {
                            focusPos -= e.delta;

                            e.Use();
                            GUI.changed = true;
                        }
                    }
                    break;
            }
        }
    }
}