using ClientApp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClientApp.DataService
{
    internal class DataService
    {
        private static readonly string _url = ConfigurationManager.AppSettings["ServerUrl"];
        private HttpClient _httpClient;

        public DataService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_url);

            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public IEnumerable<InfoCard> GetInfoCards()
        {
            var task = Task<IEnumerable<InfoCard>>.Factory.StartNew(() =>
            {
                HttpResponseMessage response;
                try
                {
                    response = _httpClient.GetAsync(_url).Result;
                }
                catch
                {
                    response = null;
                }
                if (response != null && response.IsSuccessStatusCode)
                {
                    var dataObjects = response.Content.ReadAsAsync<IEnumerable<InfoCard>>().Result;
                    return dataObjects;
                }
                else
                {
                    MessageBox.Show("Failed to load cards", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                return Enumerable.Empty<InfoCard>();
            });
            return task.Result;
        }

        public void PutInfoCard(InfoCard card)
        {
            var task = Task.Factory.StartNew(() =>
            {
                HttpResponseMessage response;
                try
                {
                    response = _httpClient.PutAsJsonAsync<InfoCard>(_url, card).Result;
                }
                catch
                {
                    response = null;
                }
                if (response != null && !response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Failed to update the card", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }

        public void PostInfoCard(InfoCard card)
        {
            var task = Task.Factory.StartNew(() =>
            {
                HttpResponseMessage response;
                try
                {
                    response = _httpClient.PostAsJsonAsync<InfoCard>(_url, card).Result;
                }
                catch
                {
                    response = null;
                }
                if (response != null && !response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Failed to upload the card", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }

        public void DeleteInfoCard(InfoCard card)
        {
            var task = Task.Factory.StartNew(() =>
            {
                HttpResponseMessage response;
                try
                {
                    response = _httpClient.DeleteAsync($"{_url}/{card.Id}").Result;
                }
                catch
                {
                    response = null;
                }
                if (response != null && !response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Failed to upload the card", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }
    }
}
