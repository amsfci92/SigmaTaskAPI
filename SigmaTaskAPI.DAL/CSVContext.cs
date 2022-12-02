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

            var lines =  File.ReadAllLines(_csvFilePath);


            if (lines.Length > 0)
            {
                for (int i = 1; i < lines.Length; i++)
                {
                    var dataRow = lines[i].Split(',');
                    var availableTimeValid = TimeOnly.TryParse(dataRow[4], out TimeOnly availableTime);

                    var candidate = new Candidate
                    {
                        Email = dataRow[0],
                        FirstName = dataRow[1],
                        LastName = dataRow[2],
                        PhoneNumber = dataRow[3],

                        AvailableContactTime = availableTimeValid ? availableTime : null,    
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

                    //await File.WriteAllTextAsync($"{_csvFilePath}", fullCSVContent);

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