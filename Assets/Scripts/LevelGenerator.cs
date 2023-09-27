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
                if (!table[i, j] && corner.Contains(levelMap[i, j]))
                {
                    if (!TraverseInLoop(i, j, i, j, Direction.None))
                    {
                        TraverseLine(i, j, Direction.None);
                    }
                }
            }
        }
    }

    bool IsBlock(int i, int j)
    {
        if (i >= levelMap.GetLength(0) || j >= levelMap.GetLength(1)) return false;

        return block.Contains(levelMap[i,j]);
    }

    bool CanTravel(int i, int j)
    {
        if (i >= levelMap.GetLength(0) || j >= levelMap.GetLength(1) || i < 0 || j < 0) return false;
        if (table[i, j] || !IsBlock(i, j)) return false;

        return true;
    }

    private void TraverseLine(int i, int j, Direction fromDirection)
    {
        if (!CanTravel(i, j)) return;
        
        table[i, j] = true;

        Direction toDirection = Direction.None;
        if (corner.Contains(levelMap[i, j]))
        {
            if (fromDirection == Direction.None)
            {
                TraverseLine(i, j + 1, Direction.RightWard);
                TraverseLine(i + 1, j, Direction.DownWard);
            }
            else if (fromDirection == Direction.RightWard)
            {
                TraverseLine(i + 1, j, Direction.DownWard);
                toDirection = Direction.DownWard;
            }
            else if (fromDirection == Direction.DownWard)
            {
                if (CanTravel(i, j + 1))
                {
                    TraverseLine(i, j + 1, Direction.RightWard);
                    toDirection = Direction.RightWard;
                }
                else
                {
                    TraverseLine(i, j - 1, Direction.LeftWard);
                    toDirection = Direction.LeftWard;
                }
            }
            else
            {
                TraverseLine(i + 1, j, Direction.DownWard);
                toDirection = Direction.DownWard;
            }
        }
        else
        {
            switch (fromDirection)
            {
                case Direction.DownWard:
                    TraverseLine(i + 1, j, Direction.DownWard);
                    break;
                case Direction.RightWard:
                    TraverseLine(i, j + 1, Direction.RightWard);
                    break;
                case Direction.LeftWard:
                    TraverseLine(i, j - 1, Direction.LeftWard);
                    break;
            }
        }
        
        GenerateBlock(i, j, new Vector2(j, -i), fromDirection, toDirection);
    }

    private bool TraverseInLoop(int startI, int startJ, int i, int j, Direction fromDirection)
    {
        if (startI == i && startJ == j && table[i, j]) return true;
        if (!CanTravel(i, j)) return false;
        
        table[i, j] = true;
        
        bool success = false;
        Direction toDirection = Direction.None;
        if (corner.Contains(levelMap[i, j]))
        {
            if (fromDirection == Direction.None)
            {
                success = TraverseInLoop(startI, startJ, i, j + 1, Direction.RightWard);
            }
            else
            {
                if (TraverseInLoop(startI, startJ, i, j + 1, Direction.RightWard))
                {
                    success = true;
                    toDirection = Direction.RightWard;
                }
                else if (TraverseInLoop(startI, startJ, i + 1, j, Direction.DownWard))
                {
                    success = true;
                    toDirection = Direction.DownWard;
                }
                else if (TraverseInLoop(startI, startJ, i - 1, j, Direction.UpWard))
                {
                    success = true;
                    toDirection = Direction.UpWard;
                }
                else if (TraverseInLoop(startI, startJ, i, j - 1, Direction.LeftWard))
                {
                    success = true;
                    toDirection = Direction.LeftWard;
                }
            }
        }
        else
        {
            switch (fromDirection)
            {
                case Direction.DownWard:
                    success = TraverseInLoop(startI, startJ, i + 1, j, Direction.DownWard);
                    break;
                case Direction.RightWard:
                    success = TraverseInLoop(startI, startJ, i, j + 1, Direction.RightWard);
                    break;
                case Direction.LeftWard:
                    success = TraverseInLoop(startI, startJ, i, j - 1, Direction.LeftWard);
                    break;
                case Direction.UpWard:
                    success = TraverseInLoop(startI, startJ, i - 1, j, Direction.UpWard);
                    break;
            }
        }

        table[i, j] = success;
        if (success)
        {
            Debug.Log($"{toDirection} at {i} {j}");
            GenerateBlock(i, j, new Vector2(j, -i), fromDirection, toDirection);
        }
        
        
        return success;
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
        if (fromDirection == Direction.None) return Quaternion.identity;

        if (fromDirection == Direction.RightWard)
        {
            if (toDirection == Direction.UpWard) return Quaternion.Euler(0, 0, 180);
            return Quaternion.Euler(0, 0, 270);
        }
        else if (fromDirection == Direction.DownWard)
        {
            if (toDirection == Direction.RightWard) return Quaternion.Euler(0, 0, 90);
            return Quaternion.Euler(0, 0, 180);
        }
        else // only leftward 
        {
            if (toDirection == Direction.DownWard) return Quaternion.identity;
            return Quaternion.Euler(0, 0, 90);
        }
    }

    Quaternion RotateWall(Direction direction)
    {
        if (direction == Direction.LeftWard || direction == Direction.RightWard) return Quaternion.identity;
        
        return Quaternion.Euler(0, 0, 90);
    }
}
