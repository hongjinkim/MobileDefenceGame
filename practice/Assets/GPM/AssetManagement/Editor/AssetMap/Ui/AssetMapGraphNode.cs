using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Gpm.AssetManagement.AssetMap.Ui
{
    using Gpm.AssetManagement.Const;
    using Gpm.AssetManagement.AssetFind.Ui;

    public class AssetMapGraphNode
    {
        private AssetMapGUI assetMap;

        private Rect rect;
        private Rect zoom_rect;
        public string title;

        public AssetMapData dependency;
        public string guid;
        public string path;
        public Object obj;

        public float zoom = 1;

        public bool bInit = false;

        private GUIStyle ping_style;

        private float ping_alpha = 0;
        public AssetMapGraphNode(AssetMapGUI assetMap, string guid, Vector2 position, float width, float height)
        {
            this.assetMap = assetMap;

            this.guid = guid;
            
            rect = new Rect(position.x, position.y, width, height);
            zoom_rect = rect;
            zoom = 1;
            title = Strings.KEY_ROOT_ASSET;

            Init();
            
        }

        public void Init()
        {
            this.dependency = GpmAssetManagementManager.GetAssetDataFromGUID(guid);
            this.path = AssetDatabase.GUIDToAssetPath(guid);
            this.obj = AssetDatabase.LoadMainAssetAtPath(path);

            bInit = true;
        }

        public void SetZoom(float zoom)
        {
            this.zoom = zoom;
            zoom_rect = new Rect(rect.x * zoom, rect.y * zoom, rect.width * zoom, rect.height * zoom);
        }

        public void Drag(Vector2 delta)
        {
            rect.position += delta;
        }

        public Rect ZoomRect
        {
            get
            {
                return zoom_rect;
            }

            set
            {
                zoom_rect = value;
                rect = new Rect(zoom_rect.x / zoom, zoom_rect.y / zoom, zoom_rect.width / zoom, zoom_rect.height / zoom);
            }
        }

        public void Draw()
        {
            if(zoom == 1)
            {
                if (assetMap.rootObject != obj)
                {
                    if (Ui.Button(Strings.KEY_SELECT_ROOT) == true)
                    {
                        assetMap.SetRootObject(obj);

                        GUI.changed = true;
                    }
                }

                EditorGUILayout.ObjectField(obj, typeof(UnityEngine.Object), false);

                if( dependency.hasMissing == AssetMapData.MissingState.UNKNOWN)
                {
                    dependency.ReImport(true);
                }
                using (new EditorGUILayout.VerticalScope())
                {
                    if (dependency.missingCount > 0)
                    {
                        GUIContent icon = new GUIContent(UiContent.WarnIconContents);
                        icon.text = Ui.GetString(Strings.KEY_HAS_ISSUE);

                        if (GUILayout.Button(icon) == true)
                        {
                            AssetIssue.Ui.AssetIssueWindow.Open(obj);
                        }
                    }
                    else
                    {
                        if (dependency.hasMissing != AssetMapData.MissingState.OK)
                        {
                            dependency.hasMissing = AssetMapData.MissingState.OK;
                            GpmAssetManagementManager.cache.bDirty = true;
                        }
                    }

                    if (Ui.Button(Strings.KEY_REFERENCE_FIND) == true)
                    {
                        AssetFindWindow.Find(obj);
                    }
                }
            }
            else
            {
                if(assetMap.rootObject != obj)
                {
                    if (Ui.Button(Strings.KEY_SELECT_ROOT) == true)
                    {
                        assetMap.SetRootObject(obj);

                        GUI.changed = true;
                    }
                }

                EditorGUILayout.ObjectField(obj, typeof(UnityEngine.Object), false);
            }

            if (GpmAssetManagementManager.cache.bDirty == true)
            {
                GpmAssetManagementManager.cache.Save();
            }
        }

        public Vector2 LeftPoint
        {
            get
            {
                return new Vector2(zoom_rect.xMin-20, zoom_rect.center.y);
            }
        }

        public Vector2 RightPoint
        {
            get
            {
                return new Vector2(zoom_rect.xMax + 20, zoom_rect.center.y);
            }
        }

        public void ConnectDraw()
        {
            float width = 20;
            float height = 20;

            Rect leftRect = new Rect(zoom_rect.xMin - width, zoom_rect.center.y - height * 0.5f, 20, 20);
            Rect rightRect = new Rect(zoom_rect.xMax, zoom_rect.center.y - height * 0.5f, 20, 20);


            if (dependency.referenceLinks.Count > 0)
            {
                EditorGUI.TextArea(leftRect, dependency.referenceLinks.Count.ToString());
            }

            if (dependency.dependencyLinks.Count > 0)
            {
                EditorGUI.TextArea(rightRect, dependency.dependencyLinks.Count.ToString());
            }

            if (Event.current.type == EventType.Repaint)
            {
                if (ping_alpha > 0)
                {
                    GUI.color = new Color(1, 1, 1, ping_alpha);
                    if(ping_style == null)
                    {
                        ping_style = GUI.skin.FindStyle(Constants.GUI_STYLE_HIGHLIGHT);
                    }
                    ping_style.Draw(ZoomRect, false, false, false, false);
                    GUI.color = new Color(1, 1, 1, 1);

                    ping_alpha -= Time.deltaTime;
                    GUI.changed = true;
                }
            }
        }

        public void Ping()
        {
            ping_alpha = 2;
        }

        public virtual bool ProcessEvents(Event e)
        {
            return false;
        }
    }
}