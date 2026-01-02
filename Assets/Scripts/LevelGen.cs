using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGen : MonoBehaviour
{
    public static LevelGen instance;

    public enum RoomType
    {
        CenterRoom,
        LeftEnd,
        RightEnd
    }

    public enum ExitType
    {
        Left,
        Right
    }

    public enum Direction
    {
        Left,
        Right
    }

    [Header("Prefabs")]
    public List<GameObject> centerRoomPrefabs;

    public List<GameObject> leftEndRoomPrefabs;
    public List<GameObject> rightEndRoomPrefabs;

    public List<GameObject> leftExitRoomPrefabs;
    public List<GameObject> rightExitRoomPrefabs;

    public GameObject startingRoomPrefab;

    [Header("Parameters")]
    public int width;

    private Direction direction = Direction.Left;
    private Direction lastDirection;

    private bool startingRoom = true;

    private void Start()
    {
        instance = this;

        for (int i = 0; i < 5; i++)
        {
            CreateFloor();
        }
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    CreateFloor();
        //}
    }

    public void CreateFloor()
    {
        if(lastDirection == Direction.Left)
        {
            direction = Direction.Right;
            lastDirection = direction;
        }
        else if(lastDirection == Direction.Right)
        {
            direction = Direction.Left;
            lastDirection = direction;
        }

        for (int i = 0; i < width; i++)
        {
            if(i == 0)
            {
                if(direction == Direction.Right)
                {
                    CreateRandomRoom(transform.position, RoomType.LeftEnd);
                }
                else if(direction == Direction.Left)
                {
                    CreateRandomRoom(transform.position, RoomType.RightEnd);
                }
            }
            else if(i == width - 1)
            {
                if(direction == Direction.Right)
                {
                    CreateExitRoom(transform.position, ExitType.Right);
                }
                else if(direction == Direction.Left)
                {
                    CreateExitRoom(transform.position, ExitType.Left);
                }
            }
            else
            {
                CreateRandomRoom(transform.position, RoomType.CenterRoom);
            }

            if(direction == Direction.Right)
            {
                transform.position = new Vector2(transform.position.x + 16, transform.position.y);
            }
            else if(direction == Direction.Left)
            {
                transform.position = new Vector2(transform.position.x - 16, transform.position.y);
            }
        }

        ShiftToNextFloor();
    }

    private void CreateRandomRoom(Vector2 position, RoomType roomType)
    {
        if (!startingRoom)
        {
            switch (roomType)
            {
                case RoomType.CenterRoom:
                    Instantiate(centerRoomPrefabs[Random.Range(0, centerRoomPrefabs.Count)], position, Quaternion.identity);
                    break;

                case RoomType.LeftEnd:
                    Instantiate(leftEndRoomPrefabs[Random.Range(0, leftEndRoomPrefabs.Count)], position, Quaternion.identity);
                    break;

                case RoomType.RightEnd:
                    Instantiate(rightEndRoomPrefabs[Random.Range(0, rightEndRoomPrefabs.Count)], position, Quaternion.identity);
                    break;
            }
        }
        else
        {
            Instantiate(startingRoomPrefab, position, Quaternion.identity);
            startingRoom = false;
        }
    }

    private void CreateExitRoom(Vector2 position, ExitType exitType)
    {
        switch (exitType)
        {
            case ExitType.Left:
                Instantiate(leftExitRoomPrefabs[Random.Range(0, leftExitRoomPrefabs.Count)], position, Quaternion.identity);
                break;

            case ExitType.Right:
                Instantiate(rightExitRoomPrefabs[Random.Range(0, rightExitRoomPrefabs.Count)], position, Quaternion.identity);
                break;
        }
    }

    private void ShiftToNextFloor()
    {
        if(direction == Direction.Left)
        {
            transform.position = new Vector2(transform.position.x + 16, transform.position.y + 8);
        }
        else if(direction == Direction.Right)
        {
            transform.position = new Vector2(transform.position.x - 16, transform.position.y + 8);

        }
    }
}