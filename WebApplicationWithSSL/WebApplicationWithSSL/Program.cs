using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WebApplicationWithSSL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(options =>
                {
                    options.AddServerHeader = false;

                    // listener for HTTP
                    options.Listen(IPAddress.Parse("127.0.0.1"), 80);

                    // use certificate file
                    options.Listen(IPAddress.Any, 443, listenOptions =>
                    {
                        listenOptions.UseHttps("c:\\tmp\\localhost.pfx", "123123");
                    });

                    // retrieve certificate from store
                    //using (var store = new X509Store(StoreName.My))
                    //{
                    //    store.Open(OpenFlags.ReadOnly);
                    //    var certs = store.Certificates.Find(X509FindType.FindBySubjectName, "localhost", false);
                    //    if (certs.Count > 0)
                    //    {
                    //        var certificate = certs[0];

                    //        // listen for HTTPS
                    //        options.Listen(IPAddress.Parse("127.0.0.1"), 40001, listenOptions =>
                    //        {
                    //            listenOptions.UseHttps(certificate);
                    //        });
                    //    }
                    //}
                })
                .UseStartup<Startup>()
                .UseUrls("https://localhost:443")
                .Build();
    }
}
