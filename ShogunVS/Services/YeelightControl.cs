using System;
using System.Threading.Tasks;
using YeelightAPI;
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
            var progressReporter = new Progress<Device>(OnDeviceFound);

            await DeviceLocator.DiscoverAsync(progressReporter);
        }

        private async void OnDeviceFound(Device device)
        {
            _device = device;

            await device.Connect();
            await device.SetPower();
            await device.SetBrightness(30);
            await device.SetColorTemperature(6474);

            SetLight();
        }

        public async void SetLight()
        {
            ColorFlow flow = new ColorFlow(0, ColorFlowEndAction.Restore);
            flow.Add(new ColorFlowRGBExpression(84, 232, 255, 100, 3500)); // color : red / brightness : 1% / duration : 500
            flow.Add(new ColorFlowTemperatureExpression(6474, 100, 500)); // color temperature : 2700k / brightness : 100 / duration : 500

            await _device.StartColorFlow(flow); // start

            await Task.Delay(3500);
            await _device.StopColorFlow(); // stop the color flow
        }

        public async void StartBlueLight(int time)
        {
            if (_device == null)
                return;
            await _device?.SetRGBColor(84, 232, 255);
            await Task.Delay(time);
            await _device?.SetColorTemperature(6474);
        }
        #endregion

    }
}
