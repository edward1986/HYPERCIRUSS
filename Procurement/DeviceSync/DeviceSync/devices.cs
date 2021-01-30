
using Devices;
using Module.DB;
using Module.DB.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSync
{
    public class DeviceWrapper
    {
        public List<deviceItem> devices = new List<deviceItem>();

        public DeviceWrapper()
        {
            using (dalDataContext context = new dalDataContext())
            {
                foreach (tblDevice d in context.tblDevices.Where(x => !x.ManagedByKiosk))
                {
                    deviceItem item = new deviceItem
                    {
                        tblDevice = d
                    };
                    
                    if (d.DeviceModel == "INO1-A")
                    {
                        item.clDevice = new INO1_A
                        {
                            _iMachineNumber = d.TerminalId,
                            IP = d.IP,
                            Port = d.Port,
                            Username = d.Username,
                            Password = d.Password
                        };
                    }
                    else if (d.DeviceModel == "INO1-A-LEGACY")
                    {
                        item.clDevice = new INO1_A_Legacy
                        {
                            _iMachineNumber = d.TerminalId,
                            IP = d.IP,
                            Port = d.Port,
                            Username = d.Username,
                            Password = d.Password
                        };
                    }
                    else if (d.DeviceModel == "GRANDING")
                    {
                        item.clDevice = new GRANDING
                        {
                            _iMachineNumber = d.TerminalId,
                            IP = d.IP,
                            Port = d.Port,
                            Username = d.Username,
                            Password = d.Password
                        };
                    }
                    else if (d.DeviceModel == "ZKTECO_F8")
                    {
                        item.clDevice = new ZKTECO_F8
                        {
                            IP = d.IP,
                            Port = d.Port,
                            Username = d.Username,
                            Password = d.Password
                        };
                    }

                    item.clDevice.OnScanComplete += ScanComplete;

                    devices.Add(item);
                }
            }
        }

        public void Connect()
        {
            foreach (deviceItem item in devices)
            {
                item.clDevice.Connect();
                Logger.WriteLog(string.Format("device: [{0}] {1} is {2}", item.tblDevice.DeviceId, item.tblDevice.DeviceName, item.clDevice.IsConnected ? "connected" : "disconnected"));
            }
        }

        public void Disconnect()
        {
            foreach (deviceItem item in devices)
            {
                while (item.locked) { }
                item.clDevice.Disconnect();
                Logger.WriteLog(string.Format("device: [{0}] {1} is {2}", item.tblDevice.DeviceId, item.tblDevice.DeviceName, item.clDevice.IsConnected ? "connected" : "disconnected"));
            }
        }

        public void DownloadLogs(DateTime startDate, DateTime endDate)
        {
            foreach (deviceItem item in devices)
            {
                item.locked = true;

                TmpUploadResult result = item.clDevice.DownloadLogs(startDate, endDate);

                List<tblDeviceLog> newLogs = saveLogs(item.tblDevice, result);

                DateTime d = DateTime.Now;

                Logger.WriteLog(string.Format("download data from device [{0}] {1}:\t{2}\t{3} items", item.tblDevice.DeviceId, item.tblDevice.DeviceName, newLogs.Any() ? "successful" : "failed", newLogs.Count), d);
                Logger.WriteLog(result.Exceptions.Select(x => string.Format("download data error: {0}", x.Message)).ToArray(), d);

                item.locked = false;
            }
        }

        public void RelateLogs(DateTime startDate, DateTime endDate)
        {
            foreach (deviceItem item in devices)
            {
                item.locked = true;

                using (dalDataContext context = new dalDataContext())
                {
                    List<tblEmployee_TimeLog> logs = context.vwDeviceLogs
                                                        .Where(x => x.DeviceId == item.tblDevice.DeviceId && x.LogDate >= startDate && x.LogDate <= endDate)
                                                        .ToList()
                                                        .Where(x =>
                                                            !context.tblEmployee_TimeLogs.Any(y => y.EmployeeId == x.EmployeeId && y.TimeLog == x.LogDate && y.DeviceReference == x.GenerateReference())
                                                            )
                                                        .Select(x => new tblEmployee_TimeLog
                                                        {
                                                            EmployeeId = x.EmployeeId,
                                                            EntryType = (int)TimeLogEntryType.Auto,
                                                            TimeLog = x.LogDate,
                                                            Mode = x.Mode,
                                                            DeviceLogId = x.Id,
                                                            DeviceReference = x.GenerateReference()
                                                        })
                                                        .ToList();

                    context.tblEmployee_TimeLogs.InsertAllOnSubmit(logs);
                    context.SubmitChanges();

                    DateTime d = DateTime.Now;

                    Logger.WriteLog(string.Format("linked data from device [{0}] {1}:\t{2}\t{3} items", item.tblDevice.DeviceId, item.tblDevice.DeviceName, logs.Any() ? "successful" : "failed", logs.Count), d);

                }

                item.locked = false;
            }
        }

        private List<tblDeviceLog> saveLogs(tblDevice tblDevice, TmpUploadResult res)
        {
            using (dalDataContext context = new dalDataContext())
            {
                List<tblDeviceLog> logs = new List<tblDeviceLog>();
                List<tblDeviceLog> dbLogs = context.tblDeviceLogs.Where(x => x.DeviceId == tblDevice.DeviceId).ToList();

                var tmp = res.LogItems.Where(x => x.Exception == null).ToList();
                foreach (clLogItem logItem in tmp)
                {
                    int mode = Devices.Common.getMode(logItem.Mode);

                    logs.Add(new tblDeviceLog
                    {
                        DeviceId = tblDevice.DeviceId,
                        IdNo = int.Parse(logItem.IdNo),
                        LogDate = DateTime.Parse(logItem.LogDate),
                        Mode = mode
                    });
                }

                List<tblDeviceLog> newLogs = logs.Where(x => !dbLogs.Any(y => y.IdNo == x.IdNo && y.LogDate == x.LogDate)).ToList();

                context.tblDeviceLogs.InsertAllOnSubmit(newLogs.Distinct());
                context.SubmitChanges();

                foreach (tblDeviceLog log in newLogs)
                {
                    Logger.WriteTimeLog(log, tblDevice);
                }

                return newLogs;
            }
        }

        private void ScanComplete(int deviceIndex, int enrollNo, DateTime timeLog, int mode)
        {
            deviceItem device = devices.Where(x => x.tblDevice.TerminalId == deviceIndex).Single();
            device.locked = true;

            tblDeviceLog log = new tblDeviceLog
            {
                DeviceId = device.tblDevice.DeviceId,
                IdNo = enrollNo,
                LogDate = timeLog,
                Mode = mode
            };

            using (dalDataContext context = new dalDataContext())
            {
                if (!context.tblDeviceLogs.Any(x => x.DeviceId == log.DeviceId && x.IdNo == log.IdNo && x.LogDate == log.LogDate && x.Mode == log.Mode))
                {
                    context.tblDeviceLogs.InsertOnSubmit(log);
                    context.SubmitChanges();

                    Logger.WriteTimeLog(log, device.tblDevice);
                }

                List<tblEmployee_DeviceIdNo> employees = context.tblEmployee_DeviceIdNos.Where(x => x.DeviceId == log.DeviceId && x.IdNo == log.IdNo).ToList();

                List<tblEmployee_TimeLog> timelogs = new List<tblEmployee_TimeLog>();
                foreach (var item in employees)
                {
                    if (!context.tblEmployee_TimeLogs.Where(x =>
                        x.EmployeeId == item.EmployeeId &&
                        x.EntryType == (int)TimeLogEntryType.Auto &&
                        x.TimeLog == log.LogDate && 
                        x.DeviceReference == log.GenerateReference()
                    ).Any())
                    {
                        timelogs.Add(new tblEmployee_TimeLog
                        {
                            DeviceLogId = log.Id,
                            EmployeeId = item.EmployeeId,
                            EntryType = (int)TimeLogEntryType.Auto,
                            TimeLog = log.LogDate,
                            Mode = log.Mode,
                            DeviceReference = log.GenerateReference()
                        });
                    }
                }

                context.tblEmployee_TimeLogs.InsertAllOnSubmit(timelogs.Distinct());
                context.SubmitChanges();
            }

            device.locked = false;
        }
    }

    public class deviceItem
    {
        public bool locked { get; set; }
        public tblDevice tblDevice { get; set; }
        public clDevice clDevice { get; set; }
    }
}