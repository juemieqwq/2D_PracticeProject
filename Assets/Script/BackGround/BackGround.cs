using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    private GameObject _camera = null;
    private float _moveDistanceX = 0;
    private Vector3 _beginPosition;
    private float _moveDistanceY = 0;
    private float _length = 0;
    private Player _player;
    private float _dvalue = 0;
    [SerializeField]
    private float _parallaxEffectX = 0;
    [SerializeField]
    private float _parallaxEffectY = 0;
    void Start()
    {
        if (_camera == null)
            _camera = GameObject.Find("Virtual Camera");
        _beginPosition = transform.position;
        _length = (int)GetComponentInChildren<SpriteRenderer>().bounds.size.x + 1;
        //Debug.Log("_length" + _length);
        _player = GameObject.FindAnyObjectByType<Player>();
        //Debug.Log("Playaer" + _player);
        _dvalue = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (_camera != null && _parallaxEffectX > 0)
            _moveDistanceX = _camera.transform.position.x * _parallaxEffectX;
        else
            Debug.Log(this + "的相机为空");

        if (_camera != null && _parallaxEffectY > 0)
            _moveDistanceY = _camera.transform.position.y * _parallaxEffectY;
        else
            Debug.Log(this + "的相机为空");

        if (_moveDistanceX != 0)
            transform.position = new Vector3(_beginPosition.x + _moveDistanceX + _dvalue, transform.position.y);
        if (_moveDistanceY != 0)
            transform.position = new Vector3(transform.position.x, _beginPosition.y + _moveDistanceY);

        //使背景进行无限循环X轴上
        if (transform.position.x - _camera.transform.position.x > _length / 2)
        {
            _dvalue -= _length;
            Debug.Log("以往左移");
        }
        else if (transform.position.x - _camera.transform.position.x < -_length / 2)
        {
            _dvalue += _length;
            Debug.Log("以往右移");
        }
    }
}
