using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace MathExamWeb.Data
{
    public class LocalStorageService
    {
        private readonly IJSRuntime _js;
        public LocalStorageService(IJSRuntime js) { _js = js; }

        public async Task<string?> GetAsync(string key)
        {
            return await _js.InvokeAsync<string?>("appLocalStorage.get", key);
        }

        public async Task<bool> SetAsync(string key, string value)
        {
            return await _js.InvokeAsync<bool>("appLocalStorage.set", key, value);
        }

        public async Task<bool> RemoveAsync(string key)
        {
            return await _js.InvokeAsync<bool>("appLocalStorage.remove", key);
        }
    }
}
