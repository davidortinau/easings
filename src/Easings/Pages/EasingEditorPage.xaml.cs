using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
        private bool _isPlaying = false;
        private uint _duration = 3000;
        private bool _isLooping;
        
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

        public bool IsPlaying
        {
            get => _isPlaying;
            set { _isPlaying = value; NotifyPropertyChanged(); }
        }

        public uint Duration
        {
            get => _duration;
            set { _duration = value; NotifyPropertyChanged();}
        }

        public uint Rate
        {
            get => _rate;
            set { _rate = value; NotifyPropertyChanged();}
        }

        public bool IsLooping
        {
            get => _isLooping;
            set { _isLooping = value; NotifyPropertyChanged();}
        }

        public EasingEditorPage()
        {
            BindingContext = this;
            InitializeComponent();
        }

        

        protected override void OnAppearing()
        {
            base.OnAppearing();

            EasingPath.Data = ((Path) Card.Content).Data;
            CalculateScale();
            
        }

        void CalculateScale()
        {
            double padding = 15.0 * 2;
            var pathRatio = EasingPath.Width / EasingPath.Height;
            var gridRatio = PathContainer.Width / PathContainer.Height;
            
            Debug.WriteLine($"PathRatio: {pathRatio}, GridRatio: {gridRatio}");

            var size = (gridRatio > pathRatio)
                ? new Size(EasingPath.Width * ((PathContainer.Height-padding) / EasingPath.Height), PathContainer.Height-padding)
                : new Size(PathContainer.Width - padding, EasingPath.Height * ((PathContainer.Width-padding) / EasingPath.Width));

            ScaleFactor = (gridRatio > pathRatio)
                ? (PathContainer.Height / EasingPath.Height)
                : (PathContainer.Width / EasingPath.Width);
            
            NotifyPropertyChanged(nameof(ScaleFactor));
            
            Debug.WriteLine($"ScaleFactor: {ScaleFactor}");
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private async void PlayBtn_OnClicked(object sender, EventArgs e)
        {
            if (_isPlaying)
            {
                IsPlaying = false;
                this.AbortAnimation("Tween");
            }
            else
            {
                IsPlaying = true;
                Animate();
            }
            
        }
        
        private void Animate()
        {
            // Box.TranslationX = 0;
            
            var a = new Animation (v => Box.TranslationX = v, 0, (AnimationTrack.Width - Box.Width - AnimationTrack.Padding.Left - AnimationTrack.Padding.Right));
            a.Commit(this, "Tween", _rate, _duration, Card.EasingStyle, (v, c) =>
            {
                if(!_isLooping)
                    IsPlaying = false;
            }, ()=>IsLooping);
        }

        private void ResetBtn_OnClicked(object sender, EventArgs e)
        {
            if (_isPlaying)
            {
                this.AbortAnimation("Tween");
                IsPlaying = false;
            }

            Box.TranslationX = 0;
        }
        
        public List<string> Easings = new List<string>()
        {
            Easing.Linear.ToString(),
            Easing.BounceIn.ToString(),
            Easing.BounceOut.ToString(),
            Easing.CubicIn.ToString(),
            Easing.CubicOut.ToString(),
            Easing.CubicInOut.ToString(),
            Easing.Linear.ToString(),
        };

        private uint _rate = 16;
    }
}