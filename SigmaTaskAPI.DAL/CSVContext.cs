using CsvHelper;
using SigmaTaskAPI.DAL.Models;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace SigmaTaskAPI.DAL
{
    public class CSVContext 
    {
        private string _csvFilePath;
        public CSVContext(string csvFilePath)
        {
            if (csvFilePath == null) {
                throw new ArgumentNullException("[csvFilePath] can not be null");
            }
            _csvFilePath = csvFilePath;
            Candidates = new List<Candidate>();
            LoadCSVDataAsync();
        }

        public async Task LoadCSVDataAsync()
        {
            var fileDir = Directory.CreateDirectory(Path.GetDirectoryName(_csvFilePath));
            var lines = new List<string>();

            if (fileDir != null && !fileDir.Exists)
            {
                Directory.CreateDirectory(fileDir.FullName);
            }

            if (!File.Exists(_csvFilePath))
            {
                await File.Create(_csvFilePath).DisposeAsync();
            }
            else
            {
                lines = File.ReadAllLines(_csvFilePath).ToList();
            }

            if (lines.Count > 0)
            {
                for (int i = 1; i < lines.Count; i++)
                {
                    var dataRow = lines[i].Split(',');
                    
                    var candidate = new Candidate
                    {
                        Email = dataRow[0],
                        FirstName = dataRow[1],
                        LastName = dataRow[2],
                        PhoneNumber = dataRow[3],
                        TimeInterval = dataRow[3], 
                        LinkedIn = dataRow[5],    
                        GitHub = dataRow[6],
                        Comment = dataRow[7],

                    };

                    Candidates.Add(candidate);
                }
            }
        }

        public async Task<bool> SaveChanges()
        {
            if (Candidates.Any())
            {
                var columnNames = GetCSVHeader();
                var csvFormatValues = Candidates.Select(candidate => $"{candidate.ToCSVString()}").ToList();

                var fullCSVContent = $"{columnNames}\r\n{string.Join("\r\n", csvFormatValues)}";

                using (FileStream fileStream = new FileStream(
                      _csvFilePath,
                       FileMode.Open,
                       FileAccess.ReadWrite,
                       FileShare.ReadWrite))
                {
                    using (StreamWriter streamWriter = new StreamWriter(fileStream))
                    {
                        await streamWriter.WriteAsync(fullCSVContent);
                    }
                }
                return true;
            }
            return false;
        }

        public string GetCSVHeader()
        {
            var propertyInfos = typeof(Candidate).GetProperties();
            return String.Join(",", propertyInfos.Select(x => x.Name));
        }

        public IList<Candidate> Candidates { set; get; }
    }
}