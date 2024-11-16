using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI { 
public class AStar
{
    public struct Pair
    {
        public int first, second;

        public Pair(int x, int y)
        {
            first = x;
            second = y;
        }
    }

    public struct Cell
    {
        public int parent_i, parent_j;
        public double f, g, h;
    }

    private int[,] grid;
    private int ROW, COL;


    public AStar(int[,] grid)
    {
        this.grid = grid;
        ROW = grid.GetLength(0);
        COL = grid.GetLength(1);
    }

    public (int, int) getNextMove(int srcX, int srcY, int destX, int destY)
    {
      
        Pair? p = AStarSearch(new Pair(srcX, srcY), new Pair(destX, destY));

        if (p.HasValue)
        {
            return (p.Value.first, p.Value.second);
        }
        else
        {
          
            return (srcX, srcY);
        }
    }

    public void printGrid()
{
    string gridOutput = "";
    for (int i = 0; i < ROW; i++)
    {
        for (int j = 0; j < COL; j++)
        {
            gridOutput += grid[i, j] + " ";
        }
        gridOutput += "\n"; // Add a newline after each row
    }
    Debug.Log(gridOutput.Trim()); // Log the entire grid at once
}



    private Pair? AStarSearch(Pair src, Pair dest)
    {
        if (!IsValid(src.first, src.second) || !IsValid(dest.first, dest.second))
        {
            Console.WriteLine("Source or destination is invalid");
            return null;
        }

        if (!IsUnBlocked(src.first, src.second) || !IsUnBlocked(dest.first, dest.second))
        {
            Console.WriteLine("Source or the destination is blocked");
            return null;
        }

        if (src.first == dest.first && src.second == dest.second)
        {
            Console.WriteLine("We are already at the destination");
            return src;
        }

        bool[,] closedList = new bool[ROW, COL];
        Cell[,] cellDetails = new Cell[ROW, COL];

        for (int i = 0; i < ROW; i++)
        {
            for (int j = 0; j < COL; j++)
            {
                cellDetails[i, j].f = double.MaxValue;
                cellDetails[i, j].g = double.MaxValue;
                cellDetails[i, j].h = double.MaxValue;
                cellDetails[i, j].parent_i = -1;
                cellDetails[i, j].parent_j = -1;
            }
        }

        int x = src.first, y = src.second;
        cellDetails[x, y].f = 0.0;
        cellDetails[x, y].g = 0.0;
        cellDetails[x, y].h = 0.0;
        cellDetails[x, y].parent_i = x;
        cellDetails[x, y].parent_j = y;

        SortedSet<(double, Pair)> openList = new SortedSet<(double, Pair)>(
            Comparer<(double, Pair)>.Create((a, b) => a.Item1.CompareTo(b.Item1)));

        openList.Add((0.0, new Pair(x, y)));

        while (openList.Count > 0)
        {
            (double f, Pair pair) p = openList.Min;
            openList.Remove(p);

            x = p.pair.first;
            y = p.pair.second;
            closedList[x, y] = true;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                        continue;

                    int newX = x + i;
                    int newY = y + j;

                    if (IsValid(newX, newY))
                    {
                        if (IsDestination(newX, newY, dest))
                        {
                            cellDetails[newX, newY].parent_i = x;
                            cellDetails[newX, newY].parent_j = y;
                            return GetNextPosition(cellDetails, src, new Pair(newX, newY));
                        }

                        if (!closedList[newX, newY] && IsUnBlocked(newX, newY))
                        {
                            double gNew = cellDetails[x, y].g + 1.0;
                            double hNew = CalculateHValue(newX, newY, dest);
                            double fNew = gNew + hNew;

                            if (cellDetails[newX, newY].f == double.MaxValue || cellDetails[newX, newY].f > fNew)
                            {
                                openList.Add((fNew, new Pair(newX, newY)));
                                cellDetails[newX, newY].f = fNew;
                                cellDetails[newX, newY].g = gNew;
                                cellDetails[newX, newY].h = hNew;
                                cellDetails[newX, newY].parent_i = x;
                                cellDetails[newX, newY].parent_j = y;
                            }
                        }
                    }
                }
            }
        }

        Console.WriteLine("Failed to find the Destination Cell");
        return null;
    }

    private Pair? GetNextPosition(Cell[,] cellDetails, Pair src, Pair dest)
    {
        int row = dest.first;
        int col = dest.second;

        while (!(cellDetails[row, col].parent_i == src.first && cellDetails[row, col].parent_j == src.second))
        {
            int temp_row = cellDetails[row, col].parent_i;
            int temp_col = cellDetails[row, col].parent_j;
            row = temp_row;
            col = temp_col;
        }

        return new Pair(row, col);
    }

    private bool IsValid(int row, int col)
    {
        return (row >= 0) && (row < ROW) && (col >= 0) && (col < COL);
    }

    private bool IsUnBlocked(int row, int col)
    {
        return grid[row, col] == 1;
    }

    private bool IsDestination(int row, int col, Pair dest)
    {
        return (row == dest.first && col == dest.second);
    }

    private double CalculateHValue(int row, int col, Pair dest)
    {
        return Math.Sqrt(Math.Pow(row - dest.first, 2) + Math.Pow(col - dest.second, 2));
    }
}
}