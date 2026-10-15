using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace MiniSupermarket.WinForms
{
    /// <summary>
    /// Giữ thông tin phiên đăng nhập dùng chung trong suốt thời gian ứng dụng chạy.
    /// </summary>
    public static class SessionManager
    {
        public static string JwtToken { get; private set; } = string.Empty;
        public static string CurrentUsername { get; private set; } = string.Empty;
        public static string CurrentFullName { get; private set; } = string.Empty;
        public static string CurrentRole { get; private set; } = string.Empty;
        public static bool IsLoggedIn => !string.IsNullOrWhiteSpace(JwtToken);

        internal static void StartSession(
            string token,
            string username,
            string fullName,
            string role)
        {
            JwtToken = token;
            CurrentUsername = username;
            CurrentFullName = fullName;
            CurrentRole = role;

            ApiClientService.SetBearerToken(token);
        }

        /// <summary>Quên toàn bộ thông tin phiên và xóa Bearer Token khỏi HttpClient dùng chung.</summary>
        public static void Clear()
        {
            JwtToken = string.Empty;
            CurrentUsername = string.Empty;
            CurrentFullName = string.Empty;
            CurrentRole = string.Empty;

            ApiClientService.ClearBearerToken();
        }
    }

    /// <summary>
    /// HttpClient dùng chung để đăng nhập và gọi API cần JWT.
    /// </summary>
    public static class ApiClientService
    {
        public static HttpClient Client { get; } = new HttpClient
        {
            // Đồng bộ với cổng HTTPS trong launchSettings.json của MiniSupermarket.API.
            BaseAddress = new Uri("https://localhost:7158/api/")
        };

        public static async Task<bool> LoginAsync(string username, string password)
        {
            SessionManager.Clear();

            using var response = await Client.PostAsJsonAsync("auth/login", new
            {
                Username = username,
                Password = password
            });

            if (!response.IsSuccessStatusCode)
                return false;

            using var document = JsonDocument.Parse(
                await response.Content.ReadAsStringAsync());

            var payload = document.RootElement;
            var token = payload.GetProperty("token").GetString() ?? string.Empty;
            var resolvedUsername = payload.TryGetProperty("username", out var usernameValue)
                ? usernameValue.GetString() ?? username
                : username;
            var fullName = payload.TryGetProperty("fullName", out var fullNameValue)
                ? fullNameValue.GetString() ?? resolvedUsername
                : resolvedUsername;
            var role = payload.GetProperty("role").GetString() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(role))
                return false;

            SessionManager.StartSession(token, resolvedUsername, fullName, role);
            return true;
        }

        public static void SetBearerToken(string token)
        {
            Client.DefaultRequestHeaders.Authorization =
                string.IsNullOrWhiteSpace(token)
                    ? null
                    : new AuthenticationHeaderValue("Bearer", token);
        }

        public static void ClearBearerToken()
        {
            Client.DefaultRequestHeaders.Authorization = null;
        }

        public static async Task<string> GetDataWithTokenAsync(string endpoint)
        {
            SetBearerToken(SessionManager.JwtToken);

            using var response = await Client.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new Exception("Phiên làm việc hết hạn hoặc chưa đăng nhập!");

            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                throw new Exception("Bạn không có quyền truy cập tài nguyên này!");

            throw new Exception("Lỗi khi gọi dữ liệu từ Server.");
        }
    }
}
