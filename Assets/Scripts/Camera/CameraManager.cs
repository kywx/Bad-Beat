using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [SerializeField] private CinemachineCamera[] _allCameras;

    [Header("Controls for lerping the Y Damping during player jump and fall")]
    [SerializeField] private float _fallPanAmount = 0.25f;
    [SerializeField] private float _fallYPanTime = 0.35f;
    public float _fallSpeedYDampingChangeThreshold = -15f;

    public bool isLerpingYDamping {get; private set;}
    public bool LerpedFromPlayerFalling {get; set;}

    private Coroutine _lerpYPanCoroutine;
    
    private CinemachineCamera _currentCamera;
    private CinemachinePositionComposer _framingTransposer;

    private float _normYPanAmount;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        for (int i = 0; i < _allCameras.Length; i++)
        {
            if (_allCameras[i].enabled)
            {
                // set the current active camera
                _currentCamera = _allCameras[i];

                // set the framing transposer
                _framingTransposer = _currentCamera.GetComponent<CinemachinePositionComposer>();
            }
        }
            
        // set YDamping amount to inspector value
        _normYPanAmount = _framingTransposer.Damping.y;
    }

    #region Lerp the Y Damping

    public void LerpYDamping(bool isPlayerFalling)
    {
        _lerpYPanCoroutine = StartCoroutine(LerpYAction(isPlayerFalling));
    }

    private IEnumerator LerpYAction(bool isPlayerFalling)
    {
        isLerpingYDamping = true;

        // grab the starting damp amount
        float startDampAmount = _framingTransposer.Damping.y;
        float endDampAmount = 0f;

        // determine end damp amount
        if (isPlayerFalling)
        {
            endDampAmount = _fallPanAmount;
            LerpedFromPlayerFalling = true;
        }
        else
        {
            endDampAmount = _normYPanAmount;
        }

        // lerp the pan amount
        float elapsedTime = 0f;
        while(elapsedTime < _fallYPanTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedPanAmount = Mathf.Lerp(startDampAmount, endDampAmount, (elapsedTime / _fallYPanTime));

            yield return null;
        }

        isLerpingYDamping = false;
    }


    #endregion
   
}
