﻿using Autofac;
using Jambo.Application.Queries;
using Jambo.Domain.AggregatesModel.BlogAggregate;
using Jambo.Domain.SeedWork;
using Jambo.Infrastructure;
using Jambo.Infrastructure.Repositories;

namespace Jambo.API.IoC
{
    public class ApplicationModule : Module
    {
        public readonly string _connectionString;
        public readonly string _database;

        public ApplicationModule(string connectionString, string database)
        {
            _connectionString = connectionString;
            _database = database;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new BlogQueries(_connectionString, _database))
                .As<IBlogQueries>();

            builder.Register(c => new MongoContext(_connectionString, _database))
                .As<MongoContext>().SingleInstance();

            builder.RegisterType<BlogReadOnlyRepository>()
                .As<IBlogReadOnlyRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<BlogWriteOnlyRepository>()
                .As<IBlogWriteOnlyRepository>()
                .InstancePerLifetimeScope();
        }
    }
}
