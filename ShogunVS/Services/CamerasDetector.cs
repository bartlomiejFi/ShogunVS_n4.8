using DirectShowLib;
using ShogunVS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShogunVS.Services
{
    public static class CamerasDetector
    {
        public static List<CameraDevice> CameraDevices()
        {
            var cameras = new List<CameraDevice>();
            var videoInputDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);

            int openCvId = 0;
            return videoInputDevices.Select(v => new CameraDevice()
            {
                DeviceId = v.DevicePath,
                Name = v.Name,
                OpenCvId = openCvId++
            }).ToList();
        }
    }
}
