using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Brush : MonoBehaviour
{
    private float _createLineColInterval = 0.01f;
    private float _lineWidth = 0.1f;
    private Color _lineColor = Color.yellow;
    private LineRenderer _curLine;
    private Vector2 _previousPoint;
    private InputReceiver _inputReceiver;
    private List<LineRenderer> _lines;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Init()
    {
        _inputReceiver = new InputReceiver(FindObjectOfType<MyInput>(),this,_createLineColInterval);
        _lines = new List<LineRenderer>();
    }

    private void Destory()
    {
        if (_inputReceiver != null)
        {
            _inputReceiver?.Destroy();
            _inputReceiver = null;
        }

        _lines.Clear();
        _lines = null;
    }

    private void BeginCreateLine(Vector2 pos)
    {
        _curLine = new GameObject("Line").AddComponent<LineRenderer>();
        _curLine.material = new Material(Shader.Find("Sprites/Default")) { color = _lineColor };
        _curLine.widthMultiplier = _lineWidth;
        _curLine.useWorldSpace = false;
        _curLine.positionCount = 1;
        _curLine.SetPosition(0,pos);

        _curLine.transform.SetParent(transform);
        _previousPoint = pos;
    }

    private void RefreshLineData(Vector2 pos, bool shooldCreateCol)
    {
        if (_previousPoint != null)
        {
            _curLine.positionCount++;
            _curLine.SetPosition(_curLine.positionCount-1, pos);
            if (shooldCreateCol)
            {
                BoxCollider2D col = new GameObject(string.Format("Col_{0}_{1}", _curLine.positionCount - 1, _curLine.transform.childCount + 1)).AddComponent<BoxCollider2D>();
                col.transform.SetParent(_curLine.transform);
                col.transform.position = (_previousPoint + pos) / 2f;
                Vector2 vec = pos - _previousPoint;
                float angle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
                col.transform.eulerAngles = Vector3.forward * angle;
                col.size = new Vector2(vec.magnitude, _lineWidth);
                _previousPoint = pos;
            }
        }
    }

    private void EndCreateLine(Vector2 pos)
    {
        if (_curLine.transform.childCount > 0)
        {
            _curLine.gameObject.AddComponent<Rigidbody2D>().useAutoMass = true;
            _lines.Add(_curLine);
            _curLine.gameObject.name = "Line_" + _lines.Count;
            _curLine = null;
        }
        else
        {
            Destroy(_curLine.gameObject);
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (_inputReceiver != null)
        {
            _inputReceiver.Update();
        }
    }

    private class InputReceiver
    {
        private readonly float CreateLineColInterval;
        private MyInput _myInput;
        private Brush _brush;
        private float _timer;
        private bool ShouldCreateLineCol { get { return _timer <= 0; } }

        public InputReceiver(MyInput myInput, Brush brush, float createLineColInterval)
        {
            _myInput = myInput;
            _myInput.PointerDownEvent += OnPointerDown;
            _myInput.DragEvent += OnDrag;
            _myInput.PointerUpEvent += OnPointerUp;

            _brush = brush;
            CreateLineColInterval = createLineColInterval;
            _timer = 0;
        }

        public void Destroy()
        {
            if (_myInput != null)
            {
                _myInput.PointerDownEvent -= OnPointerDown;
                _myInput.DragEvent -= OnDrag;
                _myInput.PointerUpEvent -= OnPointerUp;
                _myInput = null;
            }

            if (_brush != null)
            {
                _brush = null;
            }
        }

        public void Update()
        {
            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
            }
        }
        private void OnPointerDown(Vector2 pos)
        {
            Debug.Log("OnPointerDown");
            _timer = 0;
            _brush.BeginCreateLine(ScreenToWorldPos(pos));
        }

        private void OnDrag(Vector2 pos)
        {
            Debug.Log("OnDrag");
            _brush.RefreshLineData(ScreenToWorldPos(pos), ShouldCreateLineCol);
            if (ShouldCreateLineCol)
            {
                _timer = CreateLineColInterval;
            }
        }

        private void OnPointerUp(Vector2 pos)
        {
            Debug.Log("OnPointerUp");
            _brush.EndCreateLine(ScreenToWorldPos(pos));
            _timer = 0;
        }

        private Vector2 ScreenToWorldPos(Vector2 pos)
        {
            return Camera.main.ScreenToWorldPoint(pos);
        }
    }
}
