using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Gpm.AssetManagement.AssetFind
{
    using Gpm.AssetManagement.Const;

    public class ObjectFinderModule : FindModule
    {
        public Object rootObject;

        public ObjectFinderModule(Object rootObject)
        {
            this.rootObject = rootObject;

            if (EditorUtility.IsPersistent(rootObject) == true)
            {
                long localid;
                if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(rootObject, out guid, out localid) == false)
                {
                    Common.Log.GpmLogger.Warn(string.Format(Constants.FORMAT_IS_NOT_FILE, rootObject.name),
                        Constants.SERVICE_NAME, typeof(ObjectFinderModule), "ObjectFinderModule");
                }

                isPersistent = true;
            }
            else
            {
                isPersistent = false;
            }
        }

        public override bool IsValid()
        {
            return rootObject != null;
        }

        public override string GetName()
        {
            if (rootObject != null)
            {
                return rootObject.name;
            }

            return Strings.NAME_NULL;
        }

        public override int GetID()
        {
            return rootObject.GetInstanceID();
        }


        public override Texture2D GetTypeIcon()
        {
            return AssetPreview.GetMiniThumbnail(rootObject);
        }

        protected override bool IsChildObject(Object objectReferenceValue)
        {
            if (isPersistent == true)
            {
                string guid;
                long localid;
                if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(objectReferenceValue, out guid, out localid) == true)
                {
                    if (guid.Equals(this.guid) == true)
                    {
                        return true;
                    }
                }
            }

            if (EditorUtility.IsPersistent(objectReferenceValue) == false)
            {
                return true;
            }

            return false;
        }

        public override void Find(bool bForce = false)
        {
            FindProperty(rootObject);
        }
        public override bool CheckFindAble()
        {
            return true;
        }
    }
}