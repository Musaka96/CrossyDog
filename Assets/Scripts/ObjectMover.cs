using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectMover : MonoBehaviour
{
    [SerializeField]
    float speed = 1;

    bool shouldMove = false;

    [SerializeField]
    MovementEnum direction = MovementEnum.Down;

    Vector3 selectedDirectionalVector;

    private void Awake() {
        GameManager.onGamePlay += () => this.shouldMove = true; ;
        GameManager.onGamePause += () => this.shouldMove = false; ;

        this.SetDirection();
    }

    public void Start() {
        this.shouldMove = true;
    }

    private void Update() {
        if (this.shouldMove) {
            this.transform.Translate(this.selectedDirectionalVector * Time.deltaTime * this.speed, Space.World);
        }

        if (this.transform.position.z > GameManager.horizontalSize ||
            this.transform.position.z < -GameManager.horizontalSize) {
            this.Reset();
        }
    }

    private void Reset() {
        Vector3 tempPos = this.transform.position;

        switch (this.direction) {
            case MovementEnum.Left:
                tempPos.z = -GameManager.horizontalSize;
                break;
            case MovementEnum.Right:
                tempPos.z = GameManager.horizontalSize;
                break;
            default:
                tempPos.z = 0;
                break;
        }

        this.transform.position = tempPos;
    }

    private void SetDirection() {
        float degrees = 0;

        switch (this.direction) {
            case MovementEnum.Left:
                this.selectedDirectionalVector = Vector3.forward;
                degrees = -90;
                break;

            case MovementEnum.Right:
                this.selectedDirectionalVector = Vector3.back;
                degrees = 90;
                break;

            case MovementEnum.Both:
                int rand = Random.Range(0, 2);
                if (rand == 0) {
                    this.selectedDirectionalVector = Vector3.forward;
                    this.direction = MovementEnum.Left;
                    degrees = -90;
                    break;
                }

                this.selectedDirectionalVector = Vector3.back;
                this.direction = MovementEnum.Right;
                degrees = 90;
                break;

            case MovementEnum.Up:
                this.selectedDirectionalVector = Vector3.up;
                break;

            case MovementEnum.Down:
                this.selectedDirectionalVector = Vector3.down;
                break;

            default:
                this.selectedDirectionalVector = Vector3.zero;
                break;
        }

        //Vector3 to = new Vector3(degrees, 0, 0);
        //this.transform.eulerAngles = Vector3.Lerp(this.transform.rotation.eulerAngles, to, Time.deltaTime);
        this.transform.Rotate(0, degrees, 0, Space.Self);
    }
}