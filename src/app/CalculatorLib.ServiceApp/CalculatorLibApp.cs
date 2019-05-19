using System;
using System.IO;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.Routing;
using Akka.Actor;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.StaticFiles;
using Newtonsoft.Json;
using Owin;
using SlickProxyLib;

namespace CalculatorLib.ServiceApp
{
    internal class CalculatorLibApp
    {
        public void Start()
        {
            StartUpFun();
        }

        private void StartUpFun()
        {
            string akkaConfig = @" akka { }";
            AppActorSystem = Akka.Actor.ActorSystem.Create("serverActorSystemName", akkaConfig);
            PingActor = AppActorSystem.ActorOf<PingActor>(nameof(PingActor));
            OwinRef = WebApp.Start("http://*:10080/", (app) =>
            {
                var ui = AppDomain.CurrentDomain.BaseDirectory + "/../../public";
                if (Directory.Exists(ui))
                {
                    app.UseSlickProxy(handle => { });

                    var fileSystem = new PhysicalFileSystem(ui);
                    var options = new FileServerOptions
                    {
                        EnableDirectoryBrowsing = true,
                        FileSystem = fileSystem,
                        EnableDefaultFiles = true
                    };
                    app.UseFileServer(options);
                }

                HttpConfiguration config = new HttpConfiguration();
                config.Services.Replace(typeof(IHttpControllerTypeResolver), new ControllerResolver());

                config.MapHttpAttributeRoutes();
                config.Routes.IgnoreRoute("elmah", "{resource}.axd/{*pathInfo}");

                config.Routes.MapHttpRoute(
                    "DefaultApi post",
                    "apig/{dataaccess}/{controller}/{id}",
                    new {id = RouteParameter.Optional}
                );

                config.Routes.MapHttpRoute(
                    "DefaultApi",
                    "api/{controller}/{action}/{id}",
                    new {id = RouteParameter.Optional}
                );

                config.Routes.MapHttpRoute("FilesRoute", "{*pathInfo}", null, null, new StopRoutingHandler());

                config.Formatters.Remove(config.Formatters.XmlFormatter);

                JsonMediaTypeFormatter jsonFormatter = config.Formatters.JsonFormatter;
                jsonFormatter.UseDataContractJsonSerializer = false; // defaults to false, but no harm done
                jsonFormatter.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                jsonFormatter.SerializerSettings.Formatting = Formatting.None;
                // jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

                app.UseWebApi(config);
            });
            var result = PingActor.Ask<string>("yooo*").Result;

            Console.WriteLine(result);
        }

        public void Stop()
        {
            Task.Run(() => AppActorSystem.Terminate()).RunSynchronously();
        }

        protected IDisposable OwinRef { get; set; }
        public ActorSystem AppActorSystem { get; private set; }
        public IActorRef PingActor { get; private set; }
    }
}