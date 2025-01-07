using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public enum HotKeyCode { 
    A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z,
}
public class HotKey : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject keyName;
    private SpriteRenderer keySr;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float keyNameLocalPositionUp;
    [SerializeField] private float keyNameLocalPositionDown;
    [SerializeField] private HotKeyCode keyCode;
    private string spriteSheetPath = "Sprites\\×ÖÄ¸";
    void Start()
    {
        keyName = transform.Find("KeyName").gameObject;
        keyName.transform.localScale = new Vector3(2, 2, 1);
        this.transform.localScale = new Vector3(1.5f, 1.5f, 1);
        keySr = keyName.GetComponent<SpriteRenderer>();
        sprites = Resources.LoadAll<Sprite>(spriteSheetPath);
    }

    // Update is called once per frame
    void Update()
    {
        switch (keyCode)
        {
            case HotKeyCode.A:
                keySr.sprite = sprites[0];
                break;
            case HotKeyCode.B:
                keySr.sprite = sprites[1];
                break;
            case HotKeyCode.C:
                keySr.sprite = sprites[2];
                break;
            case HotKeyCode.D:
                keySr.sprite = sprites[3];
                break;
            case HotKeyCode.E:
                keySr.sprite = sprites[4];
                break;
            case HotKeyCode.F:
                keySr.sprite = sprites[5];
                break;
            case HotKeyCode.G:
                keySr.sprite = sprites[6];
                break;
            case HotKeyCode.H:
                keySr.sprite = sprites[7];
                break;
            case HotKeyCode.I:
                keySr.sprite = sprites[8];
                break;
            case HotKeyCode.J:
                keySr.sprite = sprites[9];
                break;
            case HotKeyCode.K:
                keySr.sprite = sprites[10];
                break;
            case HotKeyCode.L:
                keySr.sprite = sprites[11];
                break;
            case HotKeyCode.M:
                keySr.sprite = sprites[12];
                break;
            case HotKeyCode.N:
                keySr.sprite = sprites[13];
                break;
            case HotKeyCode.O:
                keySr.sprite = sprites[14];
                break;
            case HotKeyCode.P:
                keySr.sprite = sprites[15];
                break;
            case HotKeyCode.Q:
                keySr.sprite = sprites[16];
                break;
            case HotKeyCode.R:
                keySr.sprite = sprites[17];
                break;
            case HotKeyCode.S:
                keySr.sprite = sprites[18];
                break;
            case HotKeyCode.T:
                keySr.sprite = sprites[19];
                break;
            case HotKeyCode.U:
                keySr.sprite = sprites[20];
                break;
            case HotKeyCode.V:
                keySr.sprite = sprites[21];
                break;
            case HotKeyCode.W:
                keySr.sprite = sprites[22];
                break;
            case HotKeyCode.X:
                keySr.sprite = sprites[23];
                break;
            case HotKeyCode.Y:
                keySr.sprite = sprites[24];
                break;
            case HotKeyCode.Z:
                keySr.sprite = sprites[25];
                break;
        }
    }

    private void KeyNameDown() {
        if (keyName.GetComponent<SpriteRenderer>() != null)
        {
            keyName.transform.localPosition = new Vector3(keyName.transform.localPosition.x, keyNameLocalPositionDown);
        }
    }

    private void KeyNameUP()
    {
        if (keyName.GetComponent<SpriteRenderer>() != null)
        {
            keyName.transform.localPosition = new Vector3(keyName.transform.localPosition.x, keyNameLocalPositionUp);
        }
    }

    public void SetHotKey(HotKeyCode keyCode) {
        this.keyCode = keyCode;
    }

    public KeyCode SwitchHotKeyCodeToKeyCode(){
        switch (keyCode)
        {
            case HotKeyCode.A:
                return KeyCode.A;
            case HotKeyCode.B:
                return KeyCode.B;
            case HotKeyCode.C:
                return KeyCode.C;
            case HotKeyCode.D:
                return KeyCode.D;
            case HotKeyCode.E:
                return KeyCode.E;
            case HotKeyCode.F:
                return KeyCode.F;
            case HotKeyCode.G:
                return KeyCode.G;
            case HotKeyCode.H:
                return KeyCode.H;
            case HotKeyCode.I:
                return KeyCode.I;
            case HotKeyCode.J:
                return KeyCode.J;
            case HotKeyCode.K:
                return KeyCode.K;
            case HotKeyCode.L:
                return KeyCode.L;
            case HotKeyCode.M:
                return KeyCode.M;
            case HotKeyCode.N:
                return KeyCode.N;
            case HotKeyCode.O:
                return KeyCode.O;
            case HotKeyCode.P:
                return KeyCode.P;
            case HotKeyCode.Q:
                return KeyCode.Q;
            case HotKeyCode.R:
                return KeyCode.R;
            case HotKeyCode.S:
                return KeyCode.S;
            case HotKeyCode.T:
                return KeyCode.T;
            case HotKeyCode.U:
                return KeyCode.U;
            case HotKeyCode.V:
                return KeyCode.V;
            case HotKeyCode.W:
                return KeyCode.W;
            case HotKeyCode.X:
                return KeyCode.X;
            case HotKeyCode.Y:
                return KeyCode.Y;
            case HotKeyCode.Z:
                return KeyCode.Z;
        }
        return KeyCode.Alpha0;
    }
}
    