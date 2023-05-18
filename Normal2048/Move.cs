using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace Normal2048
{
    //internal class Move
    //{
    //    //private Map map;
    //    private readonly List<Rectangle> rects;
    //    public Move(List<Rectangle> rects, KeyEventArgs e, Map map)
    //    {
    //        this.rects = rects;
    //        this.map = map;
    //        switch (e.Key)
    //        {
    //            case Key.Left:
    //                for (int j = 0; j < 5; j++)
    //                {
                        
    //                    foreach (var rect in rects)
    //                    {
    //                        int col = Grid.GetColumn(rect);
    //                        int row = Grid.GetRow(rect);
    //                        if (col > j && CanMove(rect, row, j))
    //                        {
    //                            Grid.SetColumn(rect, j);
                                
    //                        }
    //                    }
                      
    //                }
    //                break;

    //            case Key.Right:
    //                for (int j = 5; j >= 0; j--)
    //                {
    //                    foreach (var rect in rects)
    //                    {
    //                        int col = Grid.GetColumn(rect);
    //                        int row = Grid.GetRow(rect);
    //                        if (col < j && CanMove(rect, row, j))
    //                        {
    //                            Grid.SetColumn(rect, j);
    //                        }
    //                    }
    //                }
    //                break;

    //            case Key.Up:
    //                for (int j = 5; j >= 0; j--)
    //                {
    //                    var sortedItems = rects.Where(rect => Grid.GetColumn(rect) == j).OrderBy(rect => Grid.GetRow(rect));
    //                    for (int i = 0 ; i <= 5; i++)
    //                    {
    //                        foreach (var rect in sortedItems)   
    //                        {
    //                            int col = Grid.GetColumn(rect);
    //                            int row = Grid.GetRow(rect);
    //                            if (row < i && CanMove(rect, i, col))
    //                            {
    //                                Grid.SetRow(rect, i);

    //                            }
    //                        }
    //                    }
    //                }
    //                break;

    //            case Key.Down:
    //                for (int i = 4; i >= 0; i--)
    //                {
                        
    //                    foreach (var rect in rects)
    //                    {
    //                        int col = Grid.GetColumn(rect);
    //                        int row = Grid.GetRow(rect);
    //                        if (row < i && CanMove(rect, i, col))
    //                        {
    //                            Grid.SetRow(rect, i);
                               
    //                        }
    //                    } 
    //                }
    //                break;
    //        }
    //    }

    //    private bool CanMove(Rectangle rect, int row, int col)
    //    {
    //        double rectLeft = col * 100;
    //        double rectTop = row * 100;
    //        double rectRight = rectLeft + rect.ActualWidth;
    //        double rectBottom = rectTop + rect.ActualHeight;

    //        foreach (var other in rects)
    //        {
    //            if (other == rect)
    //            {
    //                continue;
    //            }
    //            int otherRow = Grid.GetRow(other);
    //            int otherCol = Grid.GetColumn(other);

    //            double otherLeft = otherCol * 100;
    //            double otherTop = otherRow * 100;
    //            double otherRight = otherLeft + other.ActualWidth;
    //            double otherBottom = otherTop + other.ActualHeight;

    //            if (rectLeft < otherRight && rectRight > otherLeft &&
    //                rectTop < otherBottom && rectBottom > otherTop)
    //            {
    //                return false;
    //            }
    //        }
    //        return true;
    //    }

    //}
}
