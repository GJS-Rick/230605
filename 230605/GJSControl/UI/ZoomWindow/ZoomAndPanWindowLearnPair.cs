﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using Emgu.CV;
using System.Windows.Forms;

namespace nsUI
{
    class ZoomAndPanWindowLearnPair
    {
        enum SIDE { Center, Up, Down, Left, Right, LeftUp, LeftDown, RightUp, RightDown, Nowhere };

        public RectangleF PatternRoi;
        public RectangleF PatternOldRoi;

        private Point _PtMouseDown;
        private bool _IsMouseDown = false;
        private int _DragMode = (int)SIDE.Nowhere;
        private Cursor[] _AdjustCursor = { Cursors.SizeAll, Cursors.SizeNS, Cursors.SizeNS, Cursors.SizeWE, Cursors.SizeWE, Cursors.SizeNWSE, Cursors.SizeNESW, Cursors.SizeNESW, Cursors.SizeNWSE, Cursors.Arrow };
        private ZoomAndPanWindow _OwnerWindow;

        public bool IsInRoi;
        public bool IsAdjustStatus;
        public int MaxSideLength;
        public int MinSideLength;

        public ZoomAndPanWindowLearnPair(Rectangle roi, ZoomAndPanWindow ownerWindow)
        {
            MaxSideLength = 900;
            MinSideLength = 5;
            PatternRoi = roi;
            PatternOldRoi = roi;
            _OwnerWindow = ownerWindow;
        }
        public ZoomAndPanWindowLearnPair(Rectangle roi, ZoomAndPanWindow ownerWindow, int maxSideLength, int minSideLength)
        {
            MaxSideLength = maxSideLength;
            MinSideLength = minSideLength;
            PatternRoi = roi;
            PatternOldRoi = roi;
            _OwnerWindow = ownerWindow;
        }

        public void PatternRoiMouseUp()
        {
            PatternOldRoi = PatternRoi;
            _DragMode = (int)SIDE.Nowhere;
            IsAdjustStatus = false;
            _IsMouseDown = false;
        }

        public void PatternRoiMouseDown(MouseEventArgs e)
        {
            _PtMouseDown = e.Location;
            _IsMouseDown = true;
        }
        public void PatternRoiMouseMove(MouseEventArgs e, Point aftZoomRoiLeftUpPt, int edgeWidth, int edgeHeight, float zoomRatio)
        {
            SIDE side;
            side = SIDE.Nowhere;

            Rectangle patternRoiAftZoom = new Rectangle((int)(PatternRoi.X * zoomRatio), (int)(PatternRoi.Y * zoomRatio)
                , (int)(PatternRoi.Width * zoomRatio), (int)(PatternRoi.Height * zoomRatio));

            AdjustRect(e, patternRoiAftZoom, zoomRatio, edgeWidth, edgeHeight);

            if (IsAdjustStatus) return;
            for (int s = (int)SIDE.Center; s < (int)SIDE.Nowhere; s++) //new a rectangle to adjust PatternRoi and decide which side to select
                if (RectAdjustArea(patternRoiAftZoom, (SIDE)s).Contains(e.X + aftZoomRoiLeftUpPt.X, e.Y + aftZoomRoiLeftUpPt.Y))
                    side = (SIDE)s;

            if ((int)side >= (int)SIDE.Center && (int)side < (int)SIDE.Nowhere)  //change cursor
                _OwnerWindow.Cursor = _AdjustCursor[(int)side];

            for (int s = (int)SIDE.Center; s < (int)SIDE.Nowhere + 1; s++)  //select dragMode
                if (side == (SIDE)s && _IsMouseDown)
                    _DragMode = (int)side;

            if (side != SIDE.Nowhere) IsInRoi = true;
            if (side == SIDE.Nowhere) IsInRoi = false;
            if (_DragMode != (int)SIDE.Nowhere) IsAdjustStatus = true;
        }

        private Rectangle RectAdjustArea(Rectangle roi, SIDE side)
        {
            int oneOfTenWidth = roi.Width / 10;
            int oneOfTenHeight = roi.Height / 10;
            switch (side)
            {
                case SIDE.Center:
                    return Rectangle.FromLTRB(roi.Left + oneOfTenWidth, roi.Top + oneOfTenHeight, roi.Right - oneOfTenWidth, roi.Bottom - oneOfTenHeight);
                case SIDE.Up:
                    return Rectangle.FromLTRB(roi.Left + oneOfTenWidth, roi.Top - oneOfTenHeight, roi.Right - oneOfTenWidth, roi.Top + oneOfTenHeight);
                case SIDE.Down:
                    return Rectangle.FromLTRB(roi.Left + oneOfTenWidth, roi.Bottom - oneOfTenHeight, roi.Right - oneOfTenWidth, roi.Bottom + oneOfTenHeight);
                case SIDE.Left:
                    return Rectangle.FromLTRB(roi.Left - oneOfTenWidth, roi.Top + oneOfTenHeight, roi.Left + oneOfTenWidth, roi.Bottom - oneOfTenHeight);
                case SIDE.Right:
                    return Rectangle.FromLTRB(roi.Right - oneOfTenWidth, roi.Top + oneOfTenHeight, roi.Right + oneOfTenWidth, roi.Bottom - oneOfTenWidth);
                case SIDE.LeftUp:
                    return Rectangle.FromLTRB(roi.Left - oneOfTenWidth, roi.Top - oneOfTenHeight, roi.Left + oneOfTenWidth, roi.Top + oneOfTenHeight);
                case SIDE.LeftDown:
                    return Rectangle.FromLTRB(roi.Left - oneOfTenWidth, roi.Bottom - oneOfTenHeight, roi.Left + oneOfTenWidth, roi.Bottom + oneOfTenHeight);
                case SIDE.RightUp:
                    return Rectangle.FromLTRB(roi.Right - oneOfTenWidth, roi.Top - oneOfTenHeight, roi.Right + oneOfTenWidth, roi.Top + oneOfTenHeight);
                case SIDE.RightDown:
                    return Rectangle.FromLTRB(roi.Right - oneOfTenWidth, roi.Bottom - oneOfTenHeight, roi.Right + oneOfTenWidth, roi.Bottom + oneOfTenHeight);
                case SIDE.Nowhere:
                    return Rectangle.Empty;
                default:
                    return Rectangle.Empty;
            }
        }

        private void AdjustRect(MouseEventArgs e, Rectangle patternRoiAftZoom, float zoomRatio, int edgeRight, int edgeBottom)
        {

            float dx = (e.X - _PtMouseDown.X) / zoomRatio;
            float dy = (e.Y - _PtMouseDown.Y) / zoomRatio;

            if (PatternRoi.Width < MinSideLength)
            {
                PatternRoi.Width += 1;
                _DragMode = (int)SIDE.Nowhere;
                return;
            }
            if (PatternRoi.Height < MinSideLength)
            {
                PatternRoi.Height += 1;
                _DragMode = (int)SIDE.Nowhere;
                return;
            }
            if (PatternRoi.X < 0)
            {
                PatternRoi.X += 1;
                _DragMode = (int)SIDE.Nowhere;
                return;
            }
            if (PatternRoi.Y < 0)
            {
                PatternRoi.Y += 1;
                _DragMode = (int)SIDE.Nowhere;
                return;
            }


            /*
            if (PatternRoi.Width >= edgeRight)
            {
                PatternRoi.Width -= 1;
                DragMode = (int)SIDE.nowhere;
                return;
            }
            if (PatternRoi.Height >= edgeBottom)
            {
                PatternRoi.Height -= 1;
                DragMode = (int)SIDE.nowhere;
                return;
            }*/

            switch (_DragMode)
            {
                case (int)SIDE.Center:
                    PatternRoi.X = dx + PatternOldRoi.X;
                    PatternRoi.Y = dy + PatternOldRoi.Y;
                    break;
                case (int)SIDE.Up:
                    PatternRoi = RectangleF.FromLTRB(PatternOldRoi.Left, PatternOldRoi.Top + dy, PatternOldRoi.Right, PatternOldRoi.Bottom);
                    break;
                case (int)SIDE.Down:
                    PatternRoi = RectangleF.FromLTRB(PatternOldRoi.Left, PatternOldRoi.Top, PatternOldRoi.Right, dy + PatternOldRoi.Bottom);
                    break;
                case (int)SIDE.Left:
                    PatternRoi = RectangleF.FromLTRB(PatternOldRoi.Left + dx, PatternOldRoi.Top, PatternOldRoi.Right, PatternOldRoi.Bottom);
                    break;
                case (int)SIDE.Right:
                    PatternRoi = RectangleF.FromLTRB(PatternOldRoi.Left, PatternOldRoi.Top, PatternOldRoi.Right + dx, PatternOldRoi.Bottom);
                    break;
                case (int)SIDE.LeftUp:
                    PatternRoi = RectangleF.FromLTRB(PatternOldRoi.Left + dx, PatternOldRoi.Top + dy, PatternOldRoi.Right, PatternOldRoi.Bottom);
                    break;
                case (int)SIDE.LeftDown:
                    PatternRoi = RectangleF.FromLTRB(PatternOldRoi.Left + dx, PatternOldRoi.Top, PatternOldRoi.Right, PatternOldRoi.Bottom + dy);
                    break;
                case (int)SIDE.RightUp:
                    PatternRoi = RectangleF.FromLTRB(PatternOldRoi.Left, PatternOldRoi.Top + dy, PatternOldRoi.Right + dx, PatternOldRoi.Bottom);
                    break;
                case (int)SIDE.RightDown:
                    PatternRoi = RectangleF.FromLTRB(PatternOldRoi.Left, PatternOldRoi.Top, PatternOldRoi.Right + dx, PatternOldRoi.Bottom + dy);
                    break;
            }

            if (_DragMode != (int)SIDE.Nowhere)  //avoid rect fail
            {
                if (PatternRoi.X <= 0) PatternRoi.X = 0;
                if (PatternRoi.Y <= 0) PatternRoi.Y = 0;
                if (PatternRoi.X + PatternRoi.Width >= edgeRight) PatternRoi.X = edgeRight - PatternRoi.Width - 1;
                if (PatternRoi.Y + PatternRoi.Height >= edgeBottom) PatternRoi.Y = edgeBottom - PatternRoi.Height - 1;
            }

        }

        public UMat DrawPatternRectangle(UMat image, float zoomRatio, Point aftZoomRoiLeftUpPt, Emgu.CV.Structure.MCvScalar color)
        {
            RectangleF patternRoiAftZoomF;
            Rectangle patternRoiAftZoom;
            Rectangle patternRoiInWindow;
            patternRoiAftZoomF = new RectangleF(PatternRoi.X * zoomRatio, PatternRoi.Y * zoomRatio,
                                PatternRoi.Width * zoomRatio, PatternRoi.Height * zoomRatio);
            patternRoiAftZoom = Rectangle.Round(patternRoiAftZoomF);
            patternRoiInWindow = new Rectangle(patternRoiAftZoom.X - aftZoomRoiLeftUpPt.X, patternRoiAftZoom.Y - aftZoomRoiLeftUpPt.Y
                                , patternRoiAftZoom.Width, patternRoiAftZoom.Height);

            if (new Rectangle(aftZoomRoiLeftUpPt, _OwnerWindow.ClientSize).IntersectsWith(patternRoiAftZoom))
            {
                CvInvoke.Rectangle(image, patternRoiInWindow, color, 1);
                CvInvoke.Line(image, new Point(patternRoiInWindow.Left, patternRoiInWindow.Top + patternRoiInWindow.Height / 2)
                    , new Point(patternRoiInWindow.Right, patternRoiInWindow.Top + patternRoiInWindow.Height / 2), color, 1);
                CvInvoke.Line(image, new Point(patternRoiInWindow.Left + patternRoiInWindow.Width / 2, patternRoiInWindow.Top)
                    , new Point(patternRoiInWindow.Right - patternRoiInWindow.Width / 2, patternRoiInWindow.Bottom), color, 1);
                CvInvoke.Line(image, new Point(patternRoiInWindow.Left, patternRoiInWindow.Top)
                    , new Point(patternRoiInWindow.Right, patternRoiInWindow.Bottom), color, 1);
                CvInvoke.Line(image, new Point(patternRoiInWindow.Left, patternRoiInWindow.Bottom)
                    , new Point(patternRoiInWindow.Right, patternRoiInWindow.Top), color, 1);

                image = DrawScale(image, new Point(patternRoiInWindow.Left, patternRoiInWindow.Top), patternRoiInWindow.Width, patternRoiInWindow.Width / 30, 11, color, true);
                image = DrawScale(image, new Point(patternRoiInWindow.Left, patternRoiInWindow.Bottom), patternRoiInWindow.Width, patternRoiInWindow.Width / 30, 11, color, true);
                image = DrawScale(image, new Point(patternRoiInWindow.Left, patternRoiInWindow.Top), patternRoiInWindow.Height, patternRoiInWindow.Height / 30, 11, color, false);
                image = DrawScale(image, new Point(patternRoiInWindow.Right, patternRoiInWindow.Top), patternRoiInWindow.Height, patternRoiInWindow.Height / 30, 11, color, false);

            }
            return image;
        }

        private UMat DrawScale(UMat image, Point startPt, int sideLength, int ScaleLength, int quantity, Emgu.CV.Structure.MCvScalar color, bool IsHorizontalLine)
        {

            quantity++;
            if (IsHorizontalLine)
            {
                for (int i = 0; i < quantity + 1; i++)
                {
                    if (i % 3 != 0)
                        CvInvoke.Line(image, new Point(startPt.X + sideLength * i / quantity, startPt.Y - ScaleLength),
                            new Point(startPt.X + sideLength * i / quantity, startPt.Y + ScaleLength), color, 1);
                    else
                        CvInvoke.Line(image, new Point(startPt.X + sideLength * i / quantity, (int)(startPt.Y - ScaleLength * 1.25)),
                            new Point(startPt.X + sideLength * i / quantity, (int)(startPt.Y + ScaleLength * 1.25)), color, 2);
                }
            }
            if (!IsHorizontalLine)
            {
                for (int i = 0; i < quantity + 1; i++)
                {
                    if (i % 3 != 0)
                        CvInvoke.Line(image, new Point(startPt.X - ScaleLength, startPt.Y + sideLength * i / quantity),
                            new Point(startPt.X + ScaleLength, startPt.Y + sideLength * i / quantity), color, 1);
                    else
                        CvInvoke.Line(image, new Point((int)(startPt.X - ScaleLength * 1.25), startPt.Y + sideLength * i / quantity),
                            new Point((int)(startPt.X + ScaleLength * 1.25), startPt.Y + sideLength * i / quantity), color, 2);
                }
            }
            return image;
        }
    }
}
