using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Easings.Pages.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;
using Xamarin.Forms.Xaml;

namespace Easings.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EasingEditorPage : ContentPage, INotifyPropertyChanged
    {
        private Matrix _pathMatrix;
        private double _scaleFactor;
        public EasingCard Card { get; set; }

        public Matrix PathMatrix
        {
            get => _pathMatrix;
            set { _pathMatrix = value; NotifyPropertyChanged(); }
        }

        public double ScaleFactor
        {
            get => _scaleFactor;
            set => _scaleFactor = value;
        }

        public EasingEditorPage()
        {
            BindingContext = this;
            InitializeComponent();
        }

        

        protected override void OnAppearing()
        {
            base.OnAppearing();

            SetMatrix();
            
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            
            Debug.WriteLine("OnMeasure");
            // calc matrix and set it
            // var element = (FrameworkElement)sender;
            // var transform = (MatrixTransform)element.LayoutTransform;

            
            
            return base.OnMeasure(widthConstraint, heightConstraint);
        }

        void SetMatrix()
        {
            var pathRatio = EasingPath.Width / EasingPath.Height;
            var gridRatio = PathContainer.Width / PathContainer.Height;
            
            Debug.WriteLine($"PathRatio: {pathRatio}, GridRatio: {gridRatio}");

            var size = (gridRatio > pathRatio)
                ? new Size(EasingPath.Width * (PathContainer.Height / EasingPath.Height), PathContainer.Height)
                : new Size(PathContainer.Width, EasingPath.Height * (PathContainer.Width / EasingPath.Width));

            ScaleFactor = (gridRatio > pathRatio)
                ? (PathContainer.Height / EasingPath.Height)
                : (PathContainer.Width / EasingPath.Width);
            
            NotifyPropertyChanged(nameof(ScaleFactor));
            
            Debug.WriteLine($"ScaleFactor: {ScaleFactor}");
            
            // var matrix = new Matrix();
            // var scale = (widthConstraint - EasingPath.Width);
            //
            // matrix.Scale(scaleFactor, scaleFactor);
            // transform.Matrix = matrix;
            // PathMatrix = matrix;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}