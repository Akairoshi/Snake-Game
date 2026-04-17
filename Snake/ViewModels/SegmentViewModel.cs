using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Snake.ViewModels
{
    public class SegmentViewModel
    {
        public int GridX {  get; }
        public int GridY { get; }
        public double CellSize { get; }
        public double CanvasX { get; }
        public double CanvasY { get; }
        public SegmentViewModel(int gridX, int gridY, double cellSize)
        {
            GridX = gridX;
            GridY = gridY;
            CellSize = cellSize;
            CanvasX = gridX * cellSize;
            CanvasY = gridY * cellSize;
        }
    }
}
