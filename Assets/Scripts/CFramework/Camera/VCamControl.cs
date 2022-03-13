using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using Pathfinding;
public class VCamControl : MonoBehaviour
{
    public static VCamControl _instance;
    public AstarPath _path;
    public Seeker _seeker;
    public UnitObject unit;
    CinemachineVirtualCamera _VCam;
    CinemachineFramingTransposer _FramingTransposer;
    [Header("相机距离")]
    public float CamDistance;
    [Header("移动速度")]
    public float MoveSpeed;
    [Header("旋转速度")]
    public float RotateXSpeed;
    public float RotateYSpeed;
    [Header("X轴旋转范围")]
    public float MinX;
    public float MaxX;

    public LineRenderer _line;
    List<Vector3> LinePosList;
    /// <summary>
    /// 锁定摄像机
    /// </summary>
    public static bool CameraLock;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _VCam = GetComponent<CinemachineVirtualCamera>();
        CinemachineComponentBase componentBase = _VCam.GetCinemachineComponent(CinemachineCore.Stage.Body);
        if (componentBase is CinemachineFramingTransposer)
        {
            _FramingTransposer = (componentBase as CinemachineFramingTransposer);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            RotateCamera(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit = new RaycastHit();
            if (Physics.Raycast(ray, out raycastHit))
            {
                Vector3 pos = raycastHit.point;
                pos.x = Mathf.RoundToInt(pos.x);
                pos.z = Mathf.RoundToInt(pos.z);
                _seeker.StartPath(_seeker.transform.position, pos, unit.StartMove);
            }
        }
        ZoomCamera(Input.GetAxis("Mouse ScrollWheel"));
    }

    void DrawLine(List<Vector3> _path)
    {
        Vector3[] line = new Vector3[_path.Count + 1];
        line[0] = unit.transform.position;
        _path.CopyTo(line, 1);
        _line.positionCount = line.Length;
        _line.SetPositions(line);
    }

    private void FixedUpdate()
    {
        MoveCamera(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));
        if (unit != null)
        {
            DrawLine(unit.PathList);
        }
    }

    /// <summary>
    /// 旋转视角
    /// </summary>
    void RotateCamera(float dx, float dy)
    {
        if (CameraLock)
            return;

        float delta_rotation_x, delta_rotation_y;
        delta_rotation_x = dx * RotateXSpeed;
        delta_rotation_y = -dy * RotateYSpeed;

        Vector3 rot = transform.rotation.eulerAngles;
        if (rot.x < MinX && delta_rotation_y < 0)
        {
            delta_rotation_y = 0;
        }
        if (rot.x > MaxX && delta_rotation_y > 0)
        {
            delta_rotation_y = 0;
        }

        transform.Rotate(0, delta_rotation_x, 0, Space.World);
        transform.Rotate(delta_rotation_y, 0, 0);
    }
    /// <summary>
    /// 移动视角
    /// </summary>
    void MoveCamera(float dx, float dy)
    {
        if (CameraLock)
            return;
        float height = 0;
        if (Input.GetKey(KeyCode.Q))
            height = 1;
        if (Input.GetKey(KeyCode.E))
            height = -1;
        Vector3 pos = _VCam.Follow.transform.position;
        pos += (transform.forward.normalized * dx + transform.right.normalized * dy) * MoveSpeed;
        pos.y = _VCam.Follow.transform.position.y + height * MoveSpeed;
        _VCam.Follow.transform.position = pos;
    }

    public void ZoomCamera(float distance)
    {
        _FramingTransposer.m_CameraDistance += distance;
    }

    public void CameraFocus(Transform target)
    {
        _VCam.Follow.DOMove(target.position, 1).SetSpeedBased();
    }
    public void CameraFocus(Vector3 target)
    {
        _VCam.Follow.DOMove(target, 1).SetSpeedBased();
    }

    public void SelectTarget(UnitObject obj)
    {
        unit = obj;
    }
}
