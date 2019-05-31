using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Agenda.Services.External
{
    public class WeatherService
    {
        public string Get(string data)
        {
            string previsaoTempo = "";
            string ret = "";
            DateTime outData;
            if (DateTime.TryParse(data, out outData))
            {
                data = $"{outData.Day.ToString("00")}/{outData.Month.ToString("00")}";
            }

            HttpClient http = new HttpClient();
            ret = http.GetAsync("https://api.hgbrasil.com/weather/?format=json&woeid=456570&fields=only_results,temp,city_name,description,forecast,max,min,date&key=9a56eefd").Result.Content.ReadAsStringAsync().Result;
            if (!string.IsNullOrEmpty(ret))
            {
                var _object = JsonConvert.DeserializeObject<Previsao>(ret);
                previsaoTempo = _object.forecast.Where(x => x.date == data).Select(y => y.description).FirstOrDefault();
            }

            return previsaoTempo;
        }
    }

    public class Forecast
    {
        public string date { get; set; }
        public int max { get; set; }
        public int min { get; set; }
        public string description { get; set; }
    }

    public class Previsao
    {
        public int temp { get; set; }
        public string date { get; set; }
        public string description { get; set; }
        public string city_name { get; set; }
        public List<Forecast> forecast { get; set; }
    }
}