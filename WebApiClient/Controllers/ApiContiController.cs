using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;
using WebApiClient.Handler;

namespace WebApiClient.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //controller para hacer los request de prueba 
    public class ApiContiController : ControllerBase
    {
        private readonly ILogger<ApiContiController> _logger;
        public ApiContiController(ILogger<ApiContiController> logger)
        {
            _logger = logger;
            //_clientFactory = clientFactory;
        }
        [HttpPost]
        [Route("RequestConti")]
        public async Task<string> Post()
        {
            var headers = new Dictionary<string, string>()//headers para conti
            {
                { "Grant-Type", "client_credentials" },
                { "Scope", "profile" },
                { "subscription-key","c7932c4fd9144e27a0cc904bd2acd037" },
                { "Client-Id", "punto-courier" },
                { "Client-Secret", "f2e75bb8-4c19-4b40-85e1-51ff4b522fc1" },
                { "Host", "api-sandbox.bancontinental.com.py" },
                { "Connection", "keep-alive" },
                { "User-Agent", "Other" },
            };
            var password = "";//ingresar aqui la clave del certificado
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;//protocolos de seguridad
            ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            var certificate = new X509Certificate2(@"PuntoaPunto-sandbox.pfx", password);

            string nombre = certificate.GetNameInfo(X509NameType.DnsName, false);
            
            bool esValido = certificate.Verify();
            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(certificate);//
            handler.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13;
            handler.AllowAutoRedirect = false;
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
           
            handler.MaxAutomaticRedirections = 1;
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            esValido = certificate.Verify();//verifica validez de certificado
            using (var httpClient = new HttpClient(new LoggingHandler(handler)))
            {
                foreach (var item in headers)
                {
                    httpClient.DefaultRequestHeaders.Remove(item.Key);
                    httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
                using (var response = await httpClient.PostAsync("https://api-sandbox.bancontinental.com.py/autenticarServicio/v1/realms/externo", new StringContent("")))
                {
                    var returnValue = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(returnValue);
                    return string.Format("{0}-{1}",response.StatusCode, response.ReasonPhrase);
                }

            }
            
        }

        [HttpPost]
        [Route("RequestTest")]
        public async Task<string> RequestTest()//request de prueba a server futura tracker.sandbox
        {
            var password = "";
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            var certificate = new X509Certificate2(@"trackerSandbox.pfx", password);
            string nombre = certificate.GetNameInfo(X509NameType.DnsName, false);

            bool esValido = certificate.Verify();
            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(certificate);
            handler.SslProtocols = SslProtocols.Tls12;
            handler.AllowAutoRedirect = false;
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;

            handler.MaxAutomaticRedirections = 1;
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            esValido = certificate.Verify();
            using (var httpClient = new HttpClient(handler))
            {
                using (var response = await httpClient.GetAsync("https://tracker.sandbox.futura.software/test/TestApi/Numeros"))
                {
                    var returnValue = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(string.Format("{0}-{1}", response.StatusCode, response.ReasonPhrase));
                    return returnValue;
                }
            }
           
        }

    }
}
