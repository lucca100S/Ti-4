using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStartAnimationHandler : MonoBehaviour
{

    [SerializeField] private float _animationDuration = 2f;
    private float _animationTime = 0f;

    private bool _isPlaying = false;

    [SerializeField] private GameObject _playerGameplay;
    [SerializeField] private InputActionReference _transformAction;

    [Header("Cameras")]
    [SerializeField] private GameObject _panCamera;
    [SerializeField] private GameObject _startCamera;

    [Header("Animations Models")]
    [SerializeField] private GameObject _mudStill;
    [SerializeField] private GameObject _solidStill;
    [SerializeField] private PlayerTransformationGroundVFX _transformationVFX;

    [Header("UI")]
    [SerializeField] private GameObject _gameplayCanvas;
    [SerializeField] private GameObject _startCanvas;


    #region Properties

    public float AnimationDuration { get { return _animationDuration; } }
    public float AnimationTime { get { return _animationTime; } }
    public float AnimationProgress { get { return _animationTime/_animationDuration; } }

    #endregion

    private void Awake()
    {
        _playerGameplay.SetActive(false);

        _mudStill.SetActive(true);
        _solidStill.SetActive(false);

        _transformationVFX.gameObject.SetActive(false);

        _startCamera.SetActive(true);
        _panCamera.SetActive(false);

        _startCanvas.SetActive(true);
        _gameplayCanvas.SetActive(false);


        _playerGameplay.transform.position = _solidStill.transform.position;
    }

    public void StartAnimation()
    {
        if (_isPlaying)
            return;
        _isPlaying = true;
        StartCoroutine(HandleAnimation());
    }

    private IEnumerator HandleAnimation()
    {
        _startCamera.SetActive(false);
        _panCamera.SetActive(true);

        _mudStill.SetActive(false);
        _solidStill.SetActive(true);

        _transformationVFX.gameObject.SetActive(true);
        _transformationVFX.Play();

        while (_animationTime < _animationDuration)
        {
            _animationTime += Time.deltaTime;
            yield return null;
        }

        _solidStill.SetActive(false);
        _mudStill.SetActive(false);

        _panCamera.SetActive(false);
        _playerGameplay.SetActive(true);

        _startCanvas.SetActive(false);
        _gameplayCanvas.SetActive(true);
    }
}
