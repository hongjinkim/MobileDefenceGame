namespace Gpm.AssetManagement.AssetMap
{
    public class AssetMapLink
    {
        public string guid;

        private AssetMapData data;

        public AssetMapLink()
        {

        }

        public AssetMapLink(string guid)
        {
            this.guid = guid;
        }

        public AssetMapLink(AssetMapData data)
        {
            this.data = data;
            this.guid = data.guid;
        }

        public bool Equals(string guid)
        {
            return this.guid.Equals(guid);
        }

        public bool Equals(AssetMapData node)
        {
            return this.guid.Equals(node.guid);
        }

        public AssetMapData GetData()
        {
            if (data == null)
            {
                data = GpmAssetManagementManager.GetAssetDataFromGUID(guid);
            }

            return data;
        }

        public string GetPath()
        {
            if (data == null)
            {
                data = GetData();
            }

            if (data != null)
            {
                return data.GetPath();
            }

            return "";
        }
    }
}