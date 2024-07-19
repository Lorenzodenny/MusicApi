using MusicApi.DTO.RequestDTO;
using Newtonsoft.Json;

namespace MusicApi.Service.HTTP_Client
{
    public class FakeApiService
    {
        private readonly HttpClient _httpClient;

        public FakeApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TodoItem> GetFakeDataAsync()
        {
            var response = await _httpClient.GetAsync("https://jsonplaceholder.typicode.com/todos/1");
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TodoItem>(data);
        }
    }

}
