using LagoVista.Core.PlatformSupport;
using LagoVista.IoT.Logging.Loggers;
using System;
using System.Collections.Generic;
using System.Text;
using LagoVista.IoT.Logging;

namespace LagoVista.IoT.Verifiers.Utils
{
    public class VerifierLogger : IInstanceLogger
    {
        private String _instanceInstrumentationKey;
        private String _deviceInstrumentationKey;
        private String _hostId;
        private String _instanceId;

        public VerifierLogger(String hostId, string instanceId, string instanceInstrumentionKey, string deviceInstrumentationKey)
        {
            _instanceInstrumentationKey = InstanceInstrumentationKey;
            _deviceInstrumentationKey = deviceInstrumentationKey;
            _hostId = hostId;
            _instanceId = instanceId;
        }

        public bool DebugMode { get; set; }

        public string InstanceInstrumentationKey => _instanceInstrumentationKey;

        public string DeviceInstrumentationKey => _deviceInstrumentationKey;

        public string HostId => _hostId;

        public string InstanceId => _instanceId;

        public void AddConfigurationError(string tag, string message, params KeyValuePair<string, string>[] args)
        {

        }

        public void AddCustomEvent(LogLevel level, string tag, string customEvent, params KeyValuePair<string, string>[] args)
        {

        }

        public void AddError(string tag, string message, params KeyValuePair<string, string>[] args)
        {

        }

        public void AddError(ErrorCode errorCode, params KeyValuePair<string, string>[] args)
        {

        }

        public void AddException(string tag, Exception ex, params KeyValuePair<string, string>[] args)
        {

        }

        public void AddKVPs(params KeyValuePair<string, string>[] args)
        {
            throw new NotImplementedException();
        }

        public void AddMetric(string measure, double duration)
        {

        }

        public void AddMetric(string measure, TimeSpan duration)
        {

        }

        public void AddMetric(string measure, int count)
        {

        }

        public void EndTimedEvent(TimedEvent evt)
        {
        }

        public void SetUserId(string userId)
        {
          
        }

        public void Start()
        {

        }

        public TimedEvent StartTimedEvent(string area, string description)
        {
            return new TimedEvent(area, description);
        }

        public void Stop()
        {

        }


        public void Trace(string message, params KeyValuePair<string, string>[] args)
        {
        }

        public void TrackEvent(string message, Dictionary<string, string> parameters)
        {
            
        }

        public void TrackMetric(string kind, string name, MetricType metricType, double count, params KeyValuePair<string, string>[] args)
        {
        }

        public void TrackMetric(string kind, string name, MetricType metricType, int count, params KeyValuePair<string, string>[] args)
        {
        }
    }
}
