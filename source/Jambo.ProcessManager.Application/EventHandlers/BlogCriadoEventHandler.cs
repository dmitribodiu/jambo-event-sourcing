﻿using System;
using Jambo.Domain.AggregatesModel.BlogAggregate;
using Jambo.Domain.Events;
using Jambo.ProcessManager.Application.Events;
using MediatR;

namespace Jambo.ProcessManager.Application.IntegrationEventHandlers
{
    public class BlogCriadoEventHandler : IRequestHandler<IEventRequest<BlogCriadoDomainEvent>>
    {
        private readonly IBlogReadOnlyRepository _blogReadOnlyRepository;
        private readonly IBlogWriteOnlyRepository _blogWriteOnlyRepository;

        public BlogCriadoEventHandler(
            IBlogReadOnlyRepository blogReadOnlyRepository,
            IBlogWriteOnlyRepository blogWriteOnlyRepository)
        {
            _blogReadOnlyRepository = blogReadOnlyRepository ??
                throw new ArgumentNullException(nameof(blogReadOnlyRepository));
            _blogWriteOnlyRepository = blogWriteOnlyRepository ??
                throw new ArgumentNullException(nameof(blogWriteOnlyRepository));
        }
        public async void Handle(IEventRequest<BlogCriadoDomainEvent> message)
        {
            Blog blog = new Blog(message.Event.AggregateRootId);
            blog.Version = message.Event.Version;

            await _blogWriteOnlyRepository.Add(blog);

            await _blogWriteOnlyRepository
                .UnitOfWork
                .SaveChanges();
        }
    }
}
