using System.ComponentModel;

namespace NexoRecruiter.Domain.Helpers
{
    /// <summary>
    /// Define las rutas de la aplicación de manera centralizada y evita hardcodear strings en toda la aplicación.
    /// </summary>
    public static class AppRoutes
    {
        // dashboard
        public const string Dashboard = "/dashboard";
        public const string Jobs = "/dashboard/jobs";
        public const string Applications = "/dashboard/applications";
        public const string Users = "/dashboard/users";
        public const string Candidates = "/dashboard/candidates";
        public const string AIEvaluations = "/dashboard/ai-evaluations";
        public const string Settings = "/dashboard/settings";
        public const string PublicPortal = "/";


        // authentication...
        public const string Login = "/auth/login";
        public const string Logout = "/auth/logout";

        // variantes de autenticación con controller de API.
        public const string ApiLogin = "/auth-controller/login";
        public const string ApiLogout = "/auth-controller/logout";
        public const string RequestResetPassword = "/auth/request-reset-password";
        public static string ResetPassword(string email, string token) => $"https://localhost:5110/auth/reset-password?email={email}&token={token}";
        public const string ChangePassword = "/auth/change-password";
        public const string Account = "/auth/account";
        public static string ConfirmEmail(string userId, string token) => $"https://localhost:5110/auth/confirm-email?userId={userId}&token={token}";


        // Rutas con parámetros: Para ejemplo...
        // public static string JobDetail(int id) => $"/jobs/{id}";
    }
}