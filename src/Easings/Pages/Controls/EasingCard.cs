using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace Easings.Pages.Controls
{
    public class EasingCard : ContentView
    {
        public static readonly BindableProperty CardTitleProperty = BindableProperty.Create(nameof(CardTitle), typeof(string), typeof(EasingCard), string.Empty);
        public static readonly BindableProperty CardDescriptionProperty = BindableProperty.Create(nameof(CardDescription), typeof(string), typeof(EasingCard), string.Empty);
        // public static readonly BindableProperty EasingPathProperty = BindableProperty.Create(nameof(EasingPath), typeof(Geometry), typeof(EasingCard), string.Empty);
        
        public string CardTitle
        {
            get => (string)GetValue(CardTitleProperty);
            set => SetValue(CardTitleProperty, value);
        }

        public string CardDescription
        {
            get => (string)GetValue(CardDescriptionProperty);
            set => SetValue(CardDescriptionProperty, value);
        }
        
        
        // public Geometry EasingPath
        // {
        //     get => (Geometry)GetValue(EasingPathProperty);
        //     set => SetValue(EasingPathProperty, value);
        // }

        public EasingCard()
        {
            this.GestureRecognizers.Add(new TapGestureRecognizer(OnTap));
        }

        private void OnTap(View arg1, object arg2)
        {
            var ag = (Grid) this.GetTemplateChild("AxisGrid");
            ag.IsVisible = (ag.IsVisible) ? false : true;
        }
    }
}
