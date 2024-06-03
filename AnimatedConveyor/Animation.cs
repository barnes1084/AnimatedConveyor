using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace AnimatedConveyor
{
    public class Animation
    {
        private double StartX { get; set; }
        private double StartY { get; set; }
        private double EndX { get; set; }
        private double EndY { get; set; }
        private Canvas RootCanvas { get; set; }
        private List<Storyboard> activeAnimations { get; set; } = new List<Storyboard>();

        private const int MinDistanceBetweenObjects = 100;
        private const int MaxActiveAnimations = 6;
        public bool IsAnimationPaused { get; set; } = false;

        public Animation(double startX, double startY, double endX, double endY, Canvas rootCanvas)
        {
            StartX = startX;
            StartY = startY;
            EndX = endX;
            EndY = endY;
            RootCanvas = rootCanvas;
        }

        public void StartConveyorAnimation(Canvas rootCanvas, double startX, double endX, double startY, double endY)
        {

            
                // Don't allow new objects if animations are paused or there is overlap.
                if (IsAnimationPaused) return;
                if (activeAnimations.Count >= MaxActiveAnimations) return;
                //if (activeAnimations.Count > 0 && Canvas.GetLeft(rootCanvas.Children[rootCanvas.Children.Count - 1]) < MinDistanceBetweenObjects) return;

                // Create the parent StackPanel
                StackPanel stackPanel = new StackPanel
                {
                    Orientation = Orientation.Vertical
                };

                // Create the TextBlock
                TextBlock textBlock = new TextBlock
                {
                    Text = "C01L",
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    TextAlignment = TextAlignment.Center
                };
                // Add the TextBlock to the StackPanel
                stackPanel.Children.Add(textBlock);


                // Create the Ellipse (tire)
                Ellipse product = new Ellipse
                {
                    Width = 20,
                    Height = 20,
                    Fill = Brushes.Transparent,
                    Stroke = Brushes.Black,
                    StrokeThickness = 6,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                // Add the Ellipse to the StackPanel
                stackPanel.Children.Add(product);


                // Add StackPanel to the Canvas
                Canvas.SetLeft(stackPanel, 80);
                Canvas.SetTop(stackPanel, 95);
                rootCanvas.Children.Add(stackPanel);

                // Create the animation
                DoubleAnimation animationX = new DoubleAnimation
                {
                    From = startX,
                    To = endX,
                    Duration = new Duration(TimeSpan.FromSeconds(5))
                };

                DoubleAnimation animationY = new DoubleAnimation
                {
                    From = startY,
                    To = endY,
                    Duration = new Duration(TimeSpan.FromSeconds(5))
                };

                // Create the storyboard
                Storyboard storyboard = new Storyboard();
                Storyboard.SetTarget(animationX, stackPanel);
                Storyboard.SetTargetProperty(animationX, new PropertyPath("(Canvas.Left)"));
                Storyboard.SetTarget(animationY, stackPanel);
                Storyboard.SetTargetProperty(animationY, new PropertyPath("(Canvas.Top)"));
                storyboard.Children.Add(animationX);
                storyboard.Children.Add(animationY);

                // Add the new storyboard to the active animations collection
                activeAnimations.Add(storyboard);

                // Subscribe to the Completed event of the storyboard
                storyboard.Completed += (s, ev) =>
                {
                    rootCanvas.Children.Remove(stackPanel);
                    activeAnimations.Remove(storyboard);
                };

                // Start the storyboard
                storyboard.Begin();
                StartPhotoEyeAnimation(rootCanvas);

        }

        public void PauseConveyorAnimation()
        {
            foreach (var animation in activeAnimations)
            {
                animation.Pause();
            }
            IsAnimationPaused = true;
        }

        public void ResumeConveyorAnimation()
        {
            foreach (var animation in activeAnimations)
            {
                animation.Resume();
            }
            IsAnimationPaused = false;
        }

        private void StartPhotoEyeAnimation(Canvas rootCanvas)
        {
            // Create the Ellipse for the photoeye
            Ellipse photoeye = new Ellipse
            {
                Width = 7,
                Height = 7,
                Stroke = Brushes.LightGray,
                StrokeThickness = 1,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            // Position the photoeye on the Canvas
            Canvas.SetLeft(photoeye, 90); // You can adjust the position as needed
            Canvas.SetTop(photoeye, 130);

            // Add the Ellipse to the Canvas
            rootCanvas.Children.Add(photoeye);

            // Create the brush for animation
            SolidColorBrush brush = new SolidColorBrush();
            photoeye.Fill = brush;

            // Create the color animation
            ColorAnimation colorAnimation = new ColorAnimation
            {
                From = Colors.White,
                To = Colors.LimeGreen,
                Duration = new Duration(TimeSpan.FromMilliseconds(1000)),
                AutoReverse = true
            };

            // Start the animation
            brush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
        }


    }
}
