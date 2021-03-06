﻿using System;
using Weapsy.Cqrs.Domain;
using Weapsy.Cqrs.Examples.Domain.Events;

namespace Weapsy.Cqrs.Examples.Domain
{
    public class Product : AggregateRoot
    {
        public string Title { get; private set; }

        public Product()
        {            
        }

        public Product(Guid id, string title) : base(id)
        {
            if (string.IsNullOrEmpty(title))
                throw new ApplicationException("Product title is required.");

            AddEvent(new ProductCreated
            {
                AggregateRootId = Id,
                Title = title
            });
        }

        public void UpdateTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
                throw new ApplicationException("Product title is required.");

            AddEvent(new ProductTitleUpdated
            {
                AggregateRootId = Id,
                Title = title
            });
        }

        private void Apply(ProductCreated @event)
        {
            Id = @event.AggregateRootId;
            Title = @event.Title;
        }

        private void Apply(ProductTitleUpdated @event)
        {
            Title = @event.Title;
        }
    }
}
