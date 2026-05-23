## Project setup

1. **Install MS SQL Server to Docker**
```bash
docker pull mcr.microsoft.com/mssql/server:2025-latest
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=<password>" -e "MSSQL_PID=Evaluation" -p 1433:1433  --name sql2025 --hostname sql2025 -d mcr.microsoft.com/mssql/server:2025-latest
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=SampleStore;User Id=sa;Password=<password>;TrustServerCertificate=True;" --project SampleStore.Host
```

2. **Configure SendGrid**
  1. Go to SendGrid (https://sendgrid.com/) and create an account (free tier allows 100 emails/day)
  2. Complete email verification and account setup
  3. Navigate to **Settings → API Keys**
  4. Click **Create API Key**
  5. Give it a name (e.g. SampleStore)
  6. Select **Restricted Access**, then enable **Mail Send → Full Access** (minimum required)
  7. Click **Create & View** — copy the key immediately (it won't be shown again)

For the sender email, you also need to verify it:
  1. Go to **Settings → Sender Authentication**
  2. Choose **Single Sender Verification** (simpler) or **Domain Authentication** (recommended for production)
  3. Follow the steps to verify your sender email address

Then add ApiKey and SenderEmail to user secrets:
```bash
dotnet user-secrets set "SendGrid:ApiKey" "SG.your-api-key" --project SampleStore.Host
dotnet user-secrets set "SendGrid:SenderEmail" "your-verified-sender@example.com" --project SampleStore.Host
```

3. **(Optional) Configure Google authentication:**
  1. Go to Google Cloud Console (https://console.cloud.google.com/)
  2. Create a new project (or select an existing one)
  3. Navigate to APIs & Services → Credentials
  4. Click Create Credentials → OAuth client ID
  5. If prompted, configure the OAuth consent screen first (set app name, user support email, etc.)
  6. For application type, select Web application
  7. Under Authorized redirect URIs, add: https://localhost:<port>/identity/signin-google
  8. Click Create — you'll get your ClientId and ClientSecret

Then add ClientId and ClientSecret to user secrets:
```bash
dotnet user-secrets set "Authentication:Google:ClientId" "your-client-id" --project SampleStore.Host
dotnet user-secrets set "Authentication:Google:ClientSecret" "your-secret" --project SampleStore.Host
```

4. **(Optional) Setup Facebook authentication**
  1. Go to Meta for Developers (https://developers.facebook.com/)
  2. Click My Apps → Create App
  3. Select Allow people to log in with their Facebook account, click Next
  4. Fill in the app name and contact email, click Create app
  5. On the app dashboard, find Facebook Login and click Set up
  6. Choose Web, enter your site URL (e.g. https://localhost:<port>)
  7. Go to Facebook Login → Settings in the left sidebar
  8. Under Valid OAuth Redirect URIs, add: https://localhost:<port>/identity/signin-facebook
  9. Save changes
  10. Go to App Settings → Basic — your App ID is the ClientId and App Secret is the ClientSecret

Then add ClientId and ClientSecret to user secrets:
```bash
dotnet user-secrets set "Authentication:Facebook:ClientId" "your-app-id" --project SampleStore.Host
dotnet user-secrets set "Authentication:Facebook:ClientSecret" "your-app-secret" --project SampleStore.Host
```
**Note:** Facebook apps are in **Development mode** by default, which restricts login to app admins/testers only. To allow any user to log in, you must switch to **Live mode** — which requires the app to have a privacy policy URL.

6. **(Optional) Configure Microsoft authentication**
1. Go to Azure Portal (https://portal.azure.com/)
  2. Search for App registrations and click New registration
  3. Fill in the app name
  4. Under Supported account types, select Accounts in any organizational directory and personal Microsoft accounts (for broadest support)
  5. Under Redirect URI, select Web and enter: https://localhost:<port>/identity/signin-microsoft
  6. Click Register
  7. On the app overview page, copy the Application (client) ID — this is your ClientId
  8. Go to Certificates & secrets → New client secret
  9. Add a description, choose expiry, click Add — copy the Value immediately (it won't be shown again), this is your ClientSecret

Then add ClientId and ClientSecret to user secrets:
```bash
dotnet user-secrets set "Authentication:Microsoft:ClientId" "your-client-id" --project SampleStore.Host
dotnet user-secrets set "Authentication:Microsoft:ClientSecret" "your-client-secret" --project SampleStore.Host
```
Note: Client secrets have an expiry date (up to 24 months). Set a reminder to rotate it before it expires, or your auth will silently break.

7. ***Build and run***
```bash
cd SampleStore.Host
dotnet build
dotnet run
```