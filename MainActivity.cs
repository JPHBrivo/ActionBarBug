using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Support.V7.App;
using System.Linq;
using System.Collections.Generic;

namespace ActionBarBug
{
  [Activity (MainLauncher = true, Icon = "@mipmap/icon", Theme="@style/Theme")]
  public class MainActivity : AppCompatActivity
  {
    Spinner _spinner;
    ImageView _logo;
    Button _randomizeButton;
    Button _toggleButton;
    Random _randomGen;
    Random RandomGen {
      get {
        if (_randomGen == null) {
          _randomGen = new Random ();
        }
        return _randomGen;
      }
    }
    ArrayAdapter _adapter;

    protected override void OnCreate (Bundle savedInstanceState)
    {
      base.OnCreate (savedInstanceState);

      // Set our view from the "main" layout resource
      SetContentView (Resource.Layout.Main);

      var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar> (Resource.Id.toolbar);
      SetSupportActionBar (toolbar);
      SupportActionBar.Title = String.Empty;

      _spinner = FindViewById<Spinner> (Resource.Id.spinner);
      _logo = FindViewById<ImageView> (Resource.Id.logo);
      _randomizeButton = FindViewById<Button> (Resource.Id.randomizeButton);
      _randomizeButton.Click += RandomSpinnerOptions;
      _toggleButton = FindViewById<Button> (Resource.Id.toggleButton);
      _toggleButton.Click += ToggleOptions;
    }

    protected override void OnDestroy ()
    {
      _randomizeButton.Click -= RandomSpinnerOptions;
      _toggleButton.Click -= ToggleOptions;
      base.OnDestroy ();
    }

    public override bool OnCreateOptionsMenu (Android.Views.IMenu menu)
    {
      MenuInflater.Inflate (Resource.Menu.MainMenu, menu);
      return true;
    }

    void RandomSpinnerOptions (object sender, EventArgs e)
    {
      var numItems = randomLength();
      var randList = new List<string> ();
      for (var i = 0; i < numItems; i++) {
        randList.Add (RandomString (randomLength()));
      }

      if (_adapter == null) {
        _adapter = new ArrayAdapter (this, Resource.Layout.support_simple_spinner_dropdown_item, randList);
        _spinner.Adapter = _adapter;
      }
      else {
        _adapter.Clear ();
        _adapter.AddAll (randList);
        _adapter.NotifyDataSetChanged ();
      }
    }

    void ToggleOptions (object sender, EventArgs e)
    {
      if (_spinner.Visibility == Android.Views.ViewStates.Gone) {
        _spinner.Visibility = Android.Views.ViewStates.Visible;
        _logo.Visibility = Android.Views.ViewStates.Gone;
      } else {
        _spinner.Visibility = Android.Views.ViewStates.Gone;
        _logo.Visibility = Android.Views.ViewStates.Visible;
      }
    }

    int randomLength ()
    {
      return RandomGen.Next (2, 10);
    }

    static string RandomString (int length)
    {
      const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
      var random = new Random ();
      return new string (Enumerable.Repeat (chars, length)
        .Select (s => s [random.Next (s.Length)]).ToArray ());
    }
  }
}


