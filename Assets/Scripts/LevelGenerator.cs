using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    int[,] levelMap =
    {
        {1,2,2,2,2,2,2,2,2,2,2,2,2,1}, //todo change 1 to 7
        {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
        {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
        {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
        {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
        {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
        {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
        {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
        {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
        {0,0,0,0,0,2,5,4,4,0,3,4,4,0},
        {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
        {0,0,0,0,0,0,5,0,0,0,4,0,0,0},
    };

    private int[] block = { 1, 2, 3, 4, 7 };
    private int[] wall = { 2, 4 };
    private int[] corner = { 1, 3 };

    private bool[,] table;

    enum Direction
    {
        DownWard,
        UpWard,
        LeftWard,
        RightWard,
        None,
    }

    [SerializeField] private GameObject OutsideCorner;
    [SerializeField] private GameObject OutsideWall;
    [SerializeField] private GameObject InsideCorner;
    [SerializeField] private GameObject InsideWall;
    [SerializeField] private GameObject TJunction;
    
    void Start()
    {
        GenerateTraverseTable();
        GenerateLevel();
    }

    void GenerateTraverseTable()
    {
        table = new bool[levelMap.GetLength(0), levelMap.GetLength(1)];
        for (int i = 0; i < levelMap.GetLength(0); i++)
        {
            for (int j = 0; j < levelMap.GetLength(1); j++)
            {
                table[i, j] = false;
            }
        }
    }

    private void GenerateLevel()
    {
        for (int i = 0; i < levelMap.GetLength(0); i++)
        {
            for (int j = 0; j < levelMap.GetLength(1); j++)
            {
                // GenerateBlock(i, j, new Vector2(j, -i));
                if (!table[i, j] && corner.Contains(levelMap[i, j]))
                {
                    Debug.Log($"{i} + {j}");
                    Traverse(i, j, Direction.None);
                }
            }
        }
    }

    bool IsBlock(int i, int j)
    {
        if (i >= levelMap.GetLength(0) || j >= levelMap.GetLength(1)) return false;

        return block.Contains(levelMap[i,j]);
    }

    private void Traverse(int i, int j, Direction fromDirection)
    {
        if (i >= levelMap.GetLength(0) || j >= levelMap.GetLength(1)) return;
        if (table[i, j]) return;
        
        table[i, j] = true;
        

        if (corner.Contains(levelMap[i, j]))
        {
            if (fromDirection == Direction.None)
            {
                Traverse(i, j + 1, Direction.RightWard);
                Traverse(i + 1, j, Direction.DownWard);
                GenerateBlock(i, j, new Vector2(j, -i), fromDirection);
            }
            else if (fromDirection == Direction.RightWard)
            {
                if (IsBlock(i + 1, j) && !table[i + 1, j])
                {
                    Traverse(i + 1, j, Direction.DownWard);
                    GenerateBlock(i, j, new Vector2(j, -i), fromDirection, Direction.DownWard);
                }
                else
                {
                    Traverse(i - 1, j, Direction.UpWard);
                    GenerateBlock(i, j, new Vector2(j, -i), fromDirection, Direction.UpWard);
                }
            }
            else
            {
                if (IsBlock(i, j + 1) && !table[i, j + 1])
                {
                    Traverse(i, j + 1, Direction.RightWard);
                    GenerateBlock(i, j, new Vector2(j, -i), fromDirection, Direction.RightWard);
                }
                else
                {
                    Traverse(i, j + 1, Direction.LeftWard);
                    GenerateBlock(i, j, new Vector2(j, -i), fromDirection, Direction.LeftWard);
                }
            }
        }
        else
        {
            GenerateBlock(i, j, new Vector2(j, -i), fromDirection);
            switch (fromDirection)
            {
                case Direction.LeftWard:
                    Traverse(i, j + 1, Direction.LeftWard);
                    break;
                case Direction.RightWard:
                    Traverse(i, j + 1, Direction.RightWard);
                    break;
                case Direction.UpWard:
                    Traverse(i - 1, j, Direction.UpWard);
                    break;
                case Direction.DownWard:
                    Traverse(i + 1, j, Direction.DownWard);
                    break;
            }

        }
    }

    void GenerateBlock(int i, int j, Vector2 position, Direction fromDirection, Direction toDirection = Direction.None)
    {
        switch (levelMap[i, j])
        {
            case 1:
                Instantiate(OutsideCorner, position, RotateCorner(fromDirection, toDirection));
                break;
            case 2:
                Instantiate(OutsideWall, position, RotateWall(fromDirection));
                break;
            case 3:
                Instantiate(InsideCorner, position, RotateCorner(fromDirection, toDirection));
                break;
            case 4:
                Instantiate(InsideWall, position, RotateWall(fromDirection));
                break;
            case 7:
                Instantiate(TJunction, position, Quaternion.identity);
                break;
            default:
                return;
        }
        
    }


    Quaternion RotateCorner(Direction fromDirection, Direction toDirection = Direction.None)
    {
        if (toDirection == Direction.None) return Quaternion.identity;

        if (fromDirection == Direction.RightWard)
        {
            if (toDirection == Direction.DownWard) return Quaternion.Euler(0, 0, 270);

            return Quaternion.Euler(0, 0, 270);
        }
        else // only upward
        {
            if (toDirection == Direction.LeftWard) return Quaternion.Euler(0, 0, 180);
            return Quaternion.Euler(0, 0, 90);
        }
    }

    Quaternion RotateWall(Direction direction)
    {
        if (direction == Direction.LeftWard || direction == Direction.RightWard) return Quaternion.identity;
        
        return Quaternion.Euler(0, 0, 90);
    }
}
