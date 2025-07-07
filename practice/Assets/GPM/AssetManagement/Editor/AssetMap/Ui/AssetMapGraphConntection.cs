using UnityEditor;
using UnityEngine;

namespace Gpm.AssetManagement.AssetMap.Ui
{
    public class AssetMapGraphConntection
    {
        public AssetMapGraphNode leftNode;
        public AssetMapGraphNode rightNode;

        public AssetMapGraphConntection(AssetMapGraphNode leftNode, AssetMapGraphNode rightNode)
        {
            this.leftNode = leftNode;
            this.rightNode = rightNode;
        }

        public void Draw()
        {
            Vector2 leftPostion = leftNode.RightPoint;
            Vector2 rightPostion = rightNode.LeftPoint;

            Vector2 leftTangent = leftPostion;
            Vector2 rightTangent = rightPostion;

            float tangent = 0;
            if (Vector2.Distance(leftPostion, rightPostion) > 200)
            {
                tangent = 0.4f * Vector2.Distance(leftPostion, rightPostion);

                leftTangent.x = leftPostion.x + tangent;
                leftTangent.y = (leftPostion.y + rightPostion.y) * 0.5f;


                rightTangent.x = rightPostion.x - tangent;
                rightTangent.y = (leftPostion.y + rightPostion.y) * 0.5f;
            }

            Handles.DrawBezier(
                leftPostion,
                rightPostion,
                leftTangent,
                rightTangent,
                Color.white,
                null,
                2f
            );
        }
    }
}