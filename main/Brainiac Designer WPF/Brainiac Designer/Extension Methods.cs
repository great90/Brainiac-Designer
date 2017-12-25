using System.Windows;
using System.Windows.Media;

namespace Brainiac.Design
{
	public static class ExtensionMethods
	{
		public static void DrawTriangle(this DrawingContext dc, Point[] points, Brush brush, Pen pen)
		{
			Debug.Check(points.Length ==3);

			PathFigure pathFigure= new PathFigure();
			pathFigure.StartPoint= points[0];
			pathFigure.Segments.Add( new LineSegment(points[1], true) );
			pathFigure.Segments.Add( new LineSegment(points[2], true) );

			PathGeometry pathGeometry= new PathGeometry();
			pathGeometry.Figures.Add(pathFigure);

			dc.DrawGeometry(brush, pen, pathGeometry);
		}
	}
}
