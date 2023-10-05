using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    int[,] levelMap = 
    { 
        {1,2,2,2,2,2,2,2,2,2,2,2,2,7}, 
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

    [SerializeField] private GameObject OriginMap;
    [SerializeField] private GameObject OutsideCorner;
    [SerializeField] private GameObject OutsideWall;
    [SerializeField] private GameObject InsideCorner;
    [SerializeField] private GameObject InsideWall;
    [SerializeField] private GameObject TJunction;
    [SerializeField] private GameObject pellet;
    [SerializeField] private GameObject powerPellet;

    [SerializeField] private Transform parent;
    
    void Start()
    {
        OriginMap.SetActive(false);
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        GenerateTraverseTable();
        GenerateQuadrant();
        GenerateOtherQuadrant();
    }

    private void GenerateOtherQuadrant()
    {
        int posX = levelMap.GetLength(1) * 2 - 1;
        int posY = -(levelMap.GetLength(0) * 2 - 1);
        
        GameObject parent2Mirror = Instantiate(parent.gameObject, transform.position, Quaternion.identity);
        parent2Mirror.transform.localScale = new Vector3(-1, 1, 1);
        parent2Mirror.transform.position = new Vector3(posX, 0, 0);
        
        GameObject parent3Mirror = Instantiate(parent.gameObject, transform.position, Quaternion.identity);
        parent3Mirror.transform.localScale = new Vector3(1, -1, 1);
        parent3Mirror.transform.position = new Vector3(0, posY, 0);
        
        GameObject parent4Mirror = Instantiate(parent2Mirror, transform.position, Quaternion.identity);
        parent4Mirror.transform.localScale = new Vector3(-1, -1, 1);
        parent4Mirror.transform.position = new Vector3(posX, posY, 0);
        
    }

    #region Generate map

    void GenerateQuadrant()
    {
        for (int i = 0; i < levelMap.GetLength(0); i++)
        {
            for (int j = 0; j < levelMap.GetLength(1); j++)
            {
                if (table[i, j]) continue;
                
                if (corner.Contains(levelMap[i, j]))
                {
                    if (!TraverseInLoop(i, j, i, j, Direction.None))
                    {
                        TraverseLine(i, j, Direction.None);
                    }
                }
                else
                {
                    GeneratePellet(i, j);
                }
            }
        }
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
    
    private void TraverseLine(int i, int j, Direction fromDirection)
    {
        if (!CanTravel(i, j)) return;
        
        table[i, j] = true;

        Direction toDirection = Direction.None;
        if (corner.Contains(levelMap[i, j]) || levelMap[i, j] == 7) 
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
                if (CanTravel(i, j - 1))
                {
                    TraverseLine(i, j - 1, Direction.LeftWard);
                    toDirection = Direction.LeftWard;
                }
                else
                {
                    TraverseLine(i, j + 1, Direction.RightWard);
                    toDirection = Direction.RightWard;
                }
            }
            else
            {
                TraverseLine(i + 1, j, Direction.DownWard);
                toDirection = Direction.DownWard;
            }

            if (levelMap[i, j] == 7)
            {
                switch (toDirection)
                {
                    case Direction.DownWard:
                        toDirection = Direction.UpWard;
                        break;
                    case Direction.UpWard:
                        toDirection = Direction.DownWard;
                        break;
                    case Direction.LeftWard:
                        toDirection = Direction.RightWard;
                        break;
                    case Direction.RightWard:
                        toDirection = Direction.LeftWard;
                        break;
                    
                }
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
        
        GenerateBlock(i, j, fromDirection, toDirection);
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
        else if (levelMap[i, j] == 7)
        {
            List<Direction> goDirection = new List<Direction>() { Direction.UpWard , Direction.DownWard, Direction.LeftWard, Direction.RightWard};
            if (TraverseInLoop(startI, startJ, i, j + 1, Direction.RightWard))
            {
                success = true;
                goDirection.Remove(Direction.RightWard);
            }
            if (TraverseInLoop(startI, startJ, i + 1, j, Direction.DownWard))
            {
                success = true;
                goDirection.Remove(Direction.DownWard);
            }
            if (TraverseInLoop(startI, startJ, i - 1, j, Direction.UpWard))
            {
                success = true;
                goDirection.Remove(Direction.UpWard);
            }
            if (TraverseInLoop(startI, startJ, i, j - 1, Direction.LeftWard))
            {
                success = true;
                goDirection.Remove(Direction.LeftWard);
            }

            toDirection = goDirection[0];
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
            GenerateBlock(i, j, fromDirection, toDirection);
        }
        
        return success;
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
    

    void GeneratePellet(int i, int j)
    {
        if (table[i, j]) return;
        table[i, j] = true;
        
        GenerateBlock(i, j, Direction.None, Direction.None);
    }
        

    void GenerateBlock(int i, int j, Direction fromDirection, Direction toDirection = Direction.None)
    {
        Vector2 spawnPos = new Vector2(j, -i);
        switch (levelMap[i, j])
        {
            case 1:
                Instantiate(OutsideCorner, spawnPos, RotateCorner(fromDirection, toDirection)).transform.parent = parent;
                break;
            case 2:
                Instantiate(OutsideWall, spawnPos, RotateWall(fromDirection)).transform.parent = parent;
                break;
            case 3:
                Instantiate(InsideCorner, spawnPos, RotateCorner(fromDirection, toDirection)).transform.parent = parent;
                break;
            case 4:
                Instantiate(InsideWall, spawnPos, RotateWall(fromDirection)).transform.parent = parent;
                break;
            case 5:
                Instantiate(pellet, spawnPos, Quaternion.identity).transform.parent = parent;
                break;
            case 6:
                Instantiate(powerPellet, spawnPos, Quaternion.identity).transform.parent = parent;
                break;
            case 7:
                Instantiate(TJunction, spawnPos, RotateTJoin(fromDirection, toDirection)).transform.parent = parent;
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
        else if (fromDirection == Direction.UpWard)
        {
            if (toDirection == Direction.RightWard) return Quaternion.identity;
            return Quaternion.Euler(0, 0, 180);
        }
        else 
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

    Quaternion RotateTJoin(Direction fromDirection, Direction notToDirection)
    {
        if (notToDirection == Direction.LeftWard) return Quaternion.Euler(0, 0, 270);
        if (notToDirection == Direction.RightWard) return Quaternion.Euler(0, 0, 90);
        if (notToDirection == Direction.DownWard) return Quaternion.Euler(0, 0, 0);
        return Quaternion.Euler(0, 0, 180);
    }

#endregion
}
