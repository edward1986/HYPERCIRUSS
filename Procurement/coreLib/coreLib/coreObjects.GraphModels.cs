using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;

namespace coreLib.Objects.GraphModels.JqueryFlot
{
    public class GraphModel
    {
        public List<SeriesItem> Series;
        public List<List<object>> XAxisLabels;
        public List<List<object>> XAxisData;

        public string SerializedData()
        {
            return JsonConvert.SerializeObject(Series);
        }

        public string SerializedXAxisLabels()
        {
            return JsonConvert.SerializeObject(XAxisLabels);
        }

        public string SerializedXAxisData()
        {
            return JsonConvert.SerializeObject(XAxisData);
        }
    }

    public class SeriesItem
    {
        public string label;
        public List<List<double>> data;
    }

    public class PieModel
    {
        public List<PieSeriesItem> Series;

        public string SerializedData()
        {
            return JsonConvert.SerializeObject(Series);
        }
    }
    
    public class PieSeriesItem
    {
        public string label;
        public object data;
    }
}
