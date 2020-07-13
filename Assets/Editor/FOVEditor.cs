using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyFOV))]
public class FOVEditor : Editor
{
    private void OnSceneGUI()
    {
        EnemyFOV fov = (EnemyFOV)target;

        Vector3 fromAnglePos = fov.CirclePoint(-fov.viewAngle * 0.5f);

        Handles.color = new Color(1, 1, 1, 0.2f);

        Handles.DrawSolidArc(fov.transform.position,//원 중심 좌표
            Vector3.up,//부채꼴의 노멀 벡터
            fromAnglePos,   //부채꼴 시작 좌표
            fov.viewAngle, //시야각
            fov.viewRange //반지름
            );

        Handles.Label(fov.transform.position + (fov.transform.forward * 2.0f),
            fov.viewAngle.ToString());
    }
}
