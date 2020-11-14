using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace Easings.Pages.Controls
{
    public class EasingCard : ContentView
    {
        public static readonly BindableProperty CardTitleProperty = BindableProperty.Create(nameof(CardTitle), typeof(string), typeof(EasingCard), string.Empty);
        public static readonly BindableProperty CardDescriptionProperty = BindableProperty.Create(nameof(CardDescription), typeof(string), typeof(EasingCard), string.Empty);
        public static readonly BindableProperty EasingStyleProperty = BindableProperty.Create(nameof(EasingStyle), typeof(Easing), typeof(EasingCard), Easing.Linear);
        
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
        
        
        public Easing EasingStyle
        {
            get => (Easing)GetValue(EasingStyleProperty);
            set => SetValue(EasingStyleProperty, value);
        }

        public EasingCard()
        {
            this.GestureRecognizers.Add(new TapGestureRecognizer(OnDblTap){ NumberOfTapsRequired = 2});
            this.GestureRecognizers.Add(new TapGestureRecognizer(OnTap)
            {
                NumberOfTapsRequired = 1
            });
        }

        private void OnDblTap(View arg1, object arg2)
        {
            _cancelTapToken.Cancel();
            Debug.WriteLine("You DOUBLE tapped");
            Navigation.PushAsync(new EasingEditorPage(), true);
        }

        private async void OnTap(View arg1, object arg2)
        {
            await DebouncedTap().ConfigureAwait(false);
        }

        private CancellationTokenSource _cancelTapToken = new CancellationTokenSource();

        private async Task DebouncedTap()
        {
            try
            {
                Interlocked.Exchange(ref _cancelTapToken, new CancellationTokenSource()).Cancel();

                //NOTE THE 500 HERE - WHICH IS THE TIME TO WAIT
                await Task.Delay(TimeSpan.FromMilliseconds(300), _cancelTapToken.Token)

                    //NOTICE THE "ACTUAL" SEARCH METHOD HERE
                    .ContinueWith(async task =>
                        {
                            Debug.WriteLine("You tapped");
                            var ag = (Grid) this.GetTemplateChild("AxisGrid");
                            ag.Opacity = (ag.Opacity == 0) ? 1 : 0;

                            if (ag.Opacity != 0)
                            {
                                var b = (BoxView) this.GetTemplateChild("Box");
                                b.TranslationY = 0;// make sure it's reset
                                await Task.Delay(500);
                                await b.TranslateTo(b.TranslationX, ag.Height*-1, 1000, EasingStyle);
                            }
                        },
                        CancellationToken.None,
                        TaskContinuationOptions.OnlyOnRanToCompletion,
                        TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch
            {
                //Ignore any Threading errors
            }
            
            
        }
    }
}
