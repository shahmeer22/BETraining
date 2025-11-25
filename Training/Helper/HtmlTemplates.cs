namespace Training.Helper
{
    public static class HtmlTemplates
    {
        public static string GetSetPasswordPage(string userId, string token, string actionUrl)
        {
            return $@"
            <html>
                <body>
                    <h2>Set Your Password</h2>
                    <form method='post' action='{actionUrl}'>
                        <input type='hidden' name='userId' value='{userId}' />
                        <input type='hidden' name='token' value='{token}' />
                        <label>Password:</label><input type='password' name='newPassword' /><br/>
                        <label>Confirm Password:</label><input type='password' name='confirmPassword' /><br/>
                        <button type='submit'>Set Password</button>
                    </form>
                </body>
            </html>";
        }

        public static string GetEmailBody(string name, string confirmationLink)
        {
            return $"<p>Hello {name},</p>" +
                   $"<p>Please set your password by clicking the link below:</p>" +
                   $"<p><a href='{confirmationLink}'>Set Password</a></p>";
        }
    }
}
