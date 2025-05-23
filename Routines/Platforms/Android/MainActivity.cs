﻿using Android.App;
using Android.Content.PM;
using Android.OS;

namespace Routines
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true,
              LaunchMode = LaunchMode.SingleTop,
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation |
                                     ConfigChanges.UiMode | ConfigChanges.ScreenLayout |
                                     ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        // 👉 Esta propiedad te permitirá acceder a la instancia desde cualquier parte
        public static MainActivity Instance { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Instance = this;
        }
    }
}
