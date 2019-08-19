using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Persistence.MongoDB;
using Convey.Tracing.Jaeger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyApplication.Messaging;
using MyApplication.Mongo;

namespace MyApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            return ConveyBuilder
                .Create(services)
                .AddRabbitMq<CorrelationContext>()
                .AddMongo()
                .AddMongoRepository<MessageDocument, Guid>("messages")
                .AddJaeger()
                .Build();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IMongoRepository<MessageDocument, Guid> repo)
        {
            app
                .UseRabbitMq()
                .Subscribe<Message>(async (sp, message, ctx) =>
                {
                    await repo.AddAsync(new MessageDocument
                    {
                        Id = Guid.NewGuid(),
                        Value = message.Value
                    });
                });
            app.UseMvc();
        }
    }
}
