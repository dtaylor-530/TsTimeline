using System;
using System.Collections.Generic;
using System.Text;
using TsTimeline;

namespace SandBox
{
    public class ChartSimulationService
    {
        public void Load(ChartViewModel chartViewModel)
        {
            var chartSeries = new ChartSeries();
            for (int i = 0; i < 10; i++)
            {
                chartSeries.Values.Add(new ChartPoint (i*10, i * 20));
            }
            chartViewModel.Series = chartSeries;
        }
    }
}
