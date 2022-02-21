using ApiService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiService.DataService
{
    public class DataProvider
    {
        private static readonly string _directory = Path.Combine(AppContext.BaseDirectory, "Files");
        static DataProvider()
        {
            if (!Directory.Exists(_directory))
                Directory.CreateDirectory(_directory);
        }
        public static async Task<bool> AddNewCardAsync(InfoCard infoCard)
        {
            if (infoCard == null) return false;
            if (string.IsNullOrEmpty(infoCard.Id))
            {
                infoCard.Id = DateTime.Now.ToFileTime().ToString();
            }
            string path = Path.Combine(_directory, $"{infoCard.Id}.json");
            if (File.Exists(path)) return false;
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    await JsonSerializer.SerializeAsync<InfoCard>(fs, infoCard);
                }
            }
            catch { return false; }
            return true;
        }

        public static async Task<bool> UpdateCardAsync(InfoCard infoCard)
        {
            if (infoCard == null) return false;
            if (infoCard.Id == String.Empty)
            {
                return false;
            }
            string path = Path.Combine(_directory, $"{infoCard.Id}.json");
            if (File.Exists(path))
            {
                try
                {
                    using (FileStream fs = new FileStream(path, FileMode.Create))
                    {
                        await JsonSerializer.SerializeAsync<InfoCard>(fs, infoCard);
                    }
                }
                catch { return false; }
            }
            return true;
        }

        public static bool RemoveCard(string id)
        {
            if (id == String.Empty) return false;
            string path = Path.Combine(_directory, $"{id}.json");
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                }
                catch { return false; }
            }
            return true;
        }

        public static async Task<IEnumerable<InfoCard>> GetCardsAsync()
        {
            string[] files;
            try
            {
                files = Directory.GetFiles(_directory, "*.json");
            }
            catch { return Enumerable.Empty<InfoCard>(); }

            var list = new List<InfoCard>();
            foreach (var file in files)
            {
                try
                {
                    using (FileStream fs = new FileStream(file, FileMode.Open))
                    {
                        fs.Position = 0;
                        var card = await JsonSerializer.DeserializeAsync<InfoCard>(fs);
                        list.Add(card);
                    }
                }
                catch { }
            }
            return list;
        }
    }
}

