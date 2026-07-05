using CsvHelper;
using CsvHelper.Configuration;

namespace SandBox
{
    internal class MapFactory
    {
        public void Load(Notification master, Notification slaves)
        {
            var assembly = Assembly.GetExecutingAssembly();     
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null
            };
            load(master);
            load(slaves);
            void load(Notification notification)
            {
                using Stream stream = assembly.GetManifestResourceStream("SandBox.Resources.Countries.csv");
                using var reader = new StreamReader(stream);
                using var csv = new CsvReader(reader, config);
                var records = csv.GetRecords<Country>();
                foreach (var record in records)
                {
                    notification.Add(record);
                }
            }
        }
    }
}
