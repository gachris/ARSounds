using IdentityModel.OidcClient;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public static class OidcClientExtensions
    {
        public static async Task<LoginResult> LoginAsync(this OidcClient client, LoginRequest request)
        {
            string redirectUri = string.Format(request.RedirectUri);
            Debug.Log("UnityAuthClient::redirect URI: " + redirectUri);

            // create an HttpListener to listen for requests on that redirect URI.
            using HttpListener http = new HttpListener();
            http.Prefixes.Add(redirectUri);
            Debug.Log("UnityAuthClient::Listening..");
            http.Start();
            var parameters = new IdentityModel.Client.Parameters() { { "response_mode", "form_post" } };
            var state = await client.PrepareLoginAsync(parameters);

            Debug.Log($"UnityAuthClient::Start URL: {state.StartUrl}");

            // open system browser to start authentication
            Application.OpenURL(state.StartUrl);

            // wait for the authorization response.
            var context = await http.GetContextAsync();

            var formData = GetRequestPostData(context.Request);

            // sends an HTTP response to the browser.
            using (HttpListenerResponse response = context.Response)
            {
                string responseString = string.Format($"<html><head><meta http-equiv='refresh' content='10;url={request.CallbackUri}'></head><body>Please return to the app.</body></html>");
                var buffer = Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                using Stream responseOutput = response.OutputStream;
                await responseOutput.WriteAsync(buffer, 0, buffer.Length);
            }

            Debug.Log($"UnityAuthClient::Form Data: {formData}");
            var result = await client.ProcessResponseAsync(formData, state);

            http.Stop();

            return result;
        }

        public static async Task<LogoutResult> LogoutAsync(this OidcClient client, LogoutRequest request)
        {
            try
            {
                var startUrl = await client.PrepareLogoutAsync(new IdentityModel.OidcClient.LogoutRequest()
                {
                    IdTokenHint = request.IdTokenHint
                });

                Debug.Log($"UnityAuthClient::HttpWebRequest: Prepare web request...");
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(startUrl);
                myRequest.Method = "GET";
                var resp = (HttpWebResponse)myRequest.GetResponse();
                return resp.StatusCode == HttpStatusCode.OK
                    ? new LogoutResult() { Response = "Success" }
                    : throw new Exception(resp.StatusCode.ToString());
            }
            catch (Exception ex)
            {
                return new LogoutResult(ex.Message);
            }
        }

        private static string GetRequestPostData(HttpListenerRequest request)
        {
            if (!request.HasEntityBody)
            {
                return null;
            }

            using var body = request.InputStream;
            using var reader = new StreamReader(body, request.ContentEncoding);
            return reader.ReadToEnd();
        }
    }
}
