using System;
using System.Collections.Generic;
using System.Linq;

namespace LightMessageBus.Test
{
    public class RegistrationEntryRepository
    {
        private List<RegistrationEntry> _entries = new List<RegistrationEntry>();
        public void Add(RegistrationEntry registrationEntry)
        {
            _entries.Add(registrationEntry);
        }

        public bool HasRegistered(RegistrationEntry registrationEntry)
        {
            if(registrationEntry == null) { return false;}

            var findByReference = _entries.Any(e => e == registrationEntry);
            var findByPublisherReference = FindByPublisher(registrationEntry.Publisher);
            var findBySubscriberReference = FindBySubscriber(registrationEntry.Subscriber);
            
            return findByReference || findByPublisherReference || findBySubscriberReference;
        }
        
        private bool FindByPublisher(RegistrationEntryItem registrationEntryItem)
        {
            if (registrationEntryItem == null) { return false; }

            return _entries.Any(e => FindByReferenceOrType(e.Publisher, registrationEntryItem));
        }

        private bool FindBySubscriber(RegistrationEntryItem registrationEntryItem)
        {
            if (registrationEntryItem == null) { return false; }

            return _entries.Any(e => FindByReferenceOrType(e.Subscriber, registrationEntryItem));
        }

        private bool FindByReferenceOrType(RegistrationEntryItem itemA, RegistrationEntryItem itemB)
        {
            var findByReference = _entries.Any(e => FindByReference(itemA, itemB));
            var findByType = _entries.Any(e => FindByType(itemA, itemB));

            return findByReference || findByType;
        }

        private bool FindByReference(RegistrationEntryItem itemA, RegistrationEntryItem itemB)
        {
            return itemA != null
               && itemA.Reference != null
               && itemB.Reference != null
               && itemA.Reference.Target == itemB.Reference.Target;
        }

        private bool FindByType(RegistrationEntryItem itemA, RegistrationEntryItem itemB)
        {
            return itemA != null
                   && itemA.ItemType != null
                   && itemB != null
                   && itemA.ItemType == itemB.ItemType;
        }



        public RegistrationEntry CreateRegistrationEntry(object publisher, object subscriber)
        {
            return new RegistrationEntry
            {
                Publisher = CreateRegistrationEntryItem(publisher),
                Subscriber = CreateRegistrationEntryItem(subscriber)
            };
        }

        private RegistrationEntryItem CreateRegistrationEntryItem(object item)
        {
            return new RegistrationEntryItem
            {
                Reference = new WeakReference(item),
                ItemType = item?.GetType()
            };
        }
    }


































}