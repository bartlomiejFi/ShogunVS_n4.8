using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeelightAPI;
using YeelightAPI.Models;
using YeelightAPI.Models.ColorFlow;

namespace ShogunVS.Services
{
    public class YeelightControl
    {

        #region Fields

        private Device _device;

        #endregion

        #region Constructors

        public YeelightControl()
        {
            GetDevicesAsync();
        }

        #endregion

        #region Properties

        

        #endregion

        #region Methods

        private async Task GetDevicesAsync()
        {
            // Await the asynchronous call to the static API
          //  IEnumerable<Device> discoveredDevices = await DeviceLocator.DiscoverAsync();

            // Initialize the instance of Progress<T> with a callback to handle a discovered device
            var progressReporter = new Progress<Device>(OnDeviceFound);

            // Await the asynchronous call to the static API
            await DeviceLocator.DiscoverAsync(progressReporter);
        }

        // Define the callback for the progress reporter
        private async void OnDeviceFound(Device device)
        {
            _device = device;

             await device.Connect();
            await device.SetPower();
            await device.SetBrightness(30);
            await device.SetColorTemperature(6474);

            //SetLight();
        }

        public async void SetLight()
        {
            ColorFlow flow = new ColorFlow(0, ColorFlowEndAction.Restore);
            flow.Add(new ColorFlowRGBExpression(84, 232, 255, 100, 3500)); // color : red / brightness : 1% / duration : 500
            flow.Add(new ColorFlowTemperatureExpression(6474, 100, 500)); // color temperature : 2700k / brightness : 100 / duration : 500

            await _device.StartColorFlow(flow); // start

            /* Do Some amazing stuff ... */
            await Task.Delay(3500);
            await _device.StopColorFlow(); // stop the color flow
        }

        public async void Aziz()
        {
            if (_device == null)
                return;
            await _device?.SetRGBColor(84, 232, 255);
            await Task.Delay(4000);
            await _device?.SetColorTemperature(6474);
        }
        #endregion

    }
}
