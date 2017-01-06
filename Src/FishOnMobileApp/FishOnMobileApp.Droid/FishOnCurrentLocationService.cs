using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using FishOn.PlatformInterfaces;
using FishOnMobileApp.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

[assembly: Dependency(typeof(FishOnCurrentLocationService))]
namespace FishOnMobileApp.Droid
{
    public class FishOnCurrentLocationService : Java.Lang.Object, IFishOnCurrentLocationService, ILocationListener, IDisposable
    {
        private LocationManager _locationManager;
        private string _locationProvider;
        private GetCurrentPositionCallBackDelegateAsync _callBack;

        public FishOnCurrentLocationService()
        {
            _locationManager = (LocationManager)Forms.Context.GetSystemService(Context.LocationService);
        }

        public void Dispose()
        {
            _locationManager.Dispose();
        }

        public void GetCurrentPosition(GetCurrentPositionCallBackDelegateAsync callBack)
        {
            var criteriaForLocationService = new Criteria { Accuracy = Accuracy.Fine };
            _callBack = callBack;
            try
            {
                _locationProvider = _locationManager.GetBestProvider(criteriaForLocationService, true);
                if (!string.IsNullOrEmpty(_locationProvider) && _locationManager.IsProviderEnabled(_locationProvider))
                {
                    _locationManager.RequestLocationUpdates(_locationProvider, 1000, 1, this);
                }
                else
                {
                    // Do something here to notify the user we can't get the location updates set up properly
                   _callBack?.Invoke(null, "No (enabled) location provider available.");
                }
            }
            catch (Exception ex)
            {
                _callBack?.Invoke(null, ex.Message);
           }
        }
       
        // ILocationListener methods
        public async void OnLocationChanged(Location location)
        {
            if (location != null && location.Latitude != 0)
            {
                _locationManager.RemoveUpdates(this);
                if (_callBack != null)
                {
                    await _callBack(new Position(location.Latitude, location.Longitude), null);
                }
            }
        }

        public void OnStatusChanged(string provider, Availability status, global::Android.OS.Bundle extras){}

        public void OnProviderDisabled(string provider){}

        public void OnProviderEnabled(string provider){}
    }
}







    /*
    [Service(Name = "com.FishOnMobile.CurrentLocationService")]
    public class FishOnCurrentLocationServiceWorker : Service, ILocationListener
    {
        private GetCurrentPositionCallBackDelegate _callBack;
        private LocationProviderUsedEnumeration _locationProviderUsed;
        private readonly LocationManager _locationManager;


        public FishOnCurrentLocationServiceWorker()
        {
            _locationManager = GetSystemService(Context.LocationService) as LocationManager;
        }

        public void GetCurrentPosition(GetCurrentPositionCallBackDelegate callBack)
        {
            _callBack = callBack;
            if (_locationManager == null)
            {
                callBack?.Invoke(LocationProviderUsedEnumeration.None, null, "Unable to get Android Location (System) Service.  Does the device have location service installed?");
            }

            var provider = LocationManager.GpsProvider;
            if (_locationManager.IsProviderEnabled(provider))
            {
                _locationProviderUsed = LocationProviderUsedEnumeration.GPSHardware;
            }
            else
            {
                var locationCriteria = new Criteria() { Accuracy = Accuracy.High, PowerRequirement = Power.Medium };
                provider = _locationManager.GetBestProvider(locationCriteria, true);
                _locationProviderUsed = LocationProviderUsedEnumeration.Other;
            }

            if (provider == null)
            {
                _locationProviderUsed = LocationProviderUsedEnumeration.None;
                callBack?.Invoke(LocationProviderUsedEnumeration.None, null, "No location providers are available.  Does the device have location service installed?");
            }

            _locationManager.RequestLocationUpdates(provider, 1000, 1, this);
        }

        #region Service

        public override void OnCreate()
        {
            // This method is optional, perform any initialization here.
            base.OnCreate();
        }

        public override IBinder OnBind(Intent intent)
        {
           return new FishOnCurrentLocationServiceWorkerBinder(this);
        }

        public override bool OnUnbind(Intent intent)
        {
            // This method is optional
           return base.OnUnbind(intent);
        }

        public override void OnDestroy()
        {
            // This method is optional
            base.OnDestroy();
        }


        #endregion

        #region ILocationListener

        public void Dispose()
        {
            try
            {
                base.Dispose();
                _locationManager.Dispose();
            }
            catch { }
        }

        public void OnProviderEnabled(string provider)
        {

        }

        public void OnProviderDisabled(string provider)
        {
        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {

        }

        public void OnLocationChanged(Android.Locations.Location location)
        {
            var position = new Position(location.Latitude, location.Longitude);
            _callBack?.Invoke(_locationProviderUsed, position, null);
           _locationManager.RemoveUpdates(this);
        }

        #endregion
    }

    public class FishOnCurrentLocationServiceWorkerBinder : Binder
    {
        public FishOnCurrentLocationServiceWorkerBinder(FishOnCurrentLocationServiceWorker service)
        {
            this.Service = service;
        }

        public FishOnCurrentLocationServiceWorker Service
        {
            get; private set; }
    }

    public class FishOnCurrentLocationServiceWorkerConnetion : Java.Lang.Object, IServiceConnection
    {
        public bool IsConnected { get; private set; }
        public FishOnCurrentLocationServiceWorkerBinder Binder { get; private set; }

        public void OnServiceConnected(ComponentName name, IBinder binder)
        {
            Binder = binder as FishOnCurrentLocationServiceWorkerBinder;
            IsConnected = Binder != null;
         }

        public void OnServiceDisconnected(ComponentName name)
        {
            IsConnected = false;
            Binder = null;
        }
    }
    */

