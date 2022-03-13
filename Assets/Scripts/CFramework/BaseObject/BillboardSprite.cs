using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    Transform t_camera;
    SpriteRenderer _sprite;
    public Sprite[] _pic;

    // Start is called before the first frame update
    void Start()
    {
        t_camera = VCamControl._instance.transform;
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _sprite.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
        _sprite.receiveShadows = true;
    }

    // Update is called once per frame
    void Update()
    {
        SpriteRotate();

        DirectionCheck();
    }

    void DirectionCheck()
    {
        Vector3 delta = _sprite.transform.position - transform.position;
        delta.y = 0;
        //判断前后

        float angle = Vector3.Angle(transform.forward, delta); //求出两向量之间的夹角
        if (Mathf.Abs(90 - angle) > 47)
        {
            if (angle <= 45)
                _sprite.sprite = _pic[0];
            else
                _sprite.sprite = _pic[1];
        }
        else
        {
            if (Vector3.Angle(transform.right, delta) <= 47)
                _sprite.sprite = _pic[2];
            else
                _sprite.sprite = _pic[3];
        }
    }

    void SpriteRotate()
    {
        Vector3 rot = t_camera.rotation.eulerAngles;
        float x = rot.x;
        rot.x = Mathf.Sin(x / 30) * 45;
        _sprite.transform.rotation = Quaternion.Euler(rot);

        Vector3 pos = _sprite.transform.forward;
        pos.y = 0;
        pos = pos.normalized * -0.25f;
        //pos
        //lp.y = 0;
        _sprite.transform.position = pos + transform.position;
    }

    //private void OnGUI()
    //{
    //    GUIStyle style = new GUIStyle();
    //    style.fontSize = 30;
    //    GUI.Label(new Rect(100,100,100,10),an.ToString(),style);
    //}
}
