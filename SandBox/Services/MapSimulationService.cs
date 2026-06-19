using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using CsvHelper;

namespace SandBox
{
    internal class MapSimulationService
    {
        public void Load(PlayListViewModel playList)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using Stream stream = assembly.GetManifestResourceStream("SandBox.Resources.Countries.csv");
            using var reader = new StreamReader(stream);
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<Country>();
                foreach (var record in records)
                {
                    Geometry.Parse(record.Data);
                    playList.Tracks.Add(record);
                    playList.Stacks.Add(record);

                }
            }        
        }
    }

    public class Country : Notification
    {
        public string? Name { get; set; } = string.Empty;

        public string? ISO2 { get; set; } = string.Empty;

        public double? Width { get; set; }
        public double? Height { get; set; }
        public double? Left { get; set; }
        public double? Top { get; set; }

        public double? Skew { get; set; }
        public double? Rotate { get; set; }
        public double? Translate_X { get; set; }
        public double? Translate_Y { get; set; }

        public string? Data { get; set; } = string.Empty;
    }
}
