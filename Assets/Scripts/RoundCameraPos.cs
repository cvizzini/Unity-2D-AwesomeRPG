using UnityEngine;
// 1
using Cinemachine;
// 2
public class RoundCameraPos : CinemachineExtension
{
    // 3
    public float PixelsPerUnit = 32;
    // 4
    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage,
                                                      ref CameraState state, float deltaTime)
    {
        // 5
        if (stage == CinemachineCore.Stage.Body)
        {
            // 6
            Vector3 pos = state.FinalPosition;
            // 7
            Vector3 pos2 = new Vector3(Round(pos.x), Round(pos.y), pos.z);

            // 8
            state.PositionCorrection += pos2 - pos;
        }
    }
    // 9
    float Round(float x)
    {
        return Mathf.Round(x * PixelsPerUnit) / PixelsPerUnit;
    }
}