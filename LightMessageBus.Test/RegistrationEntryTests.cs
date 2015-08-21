using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using LightMessageBus.Test.TestClasses;
using NUnit.Framework;

namespace LightMessageBus.Test
{
    [TestFixture]
    public class RegistrationEntryTests
    {
        //registration entry
        [Test]
        public void GivenRepositoryWithRegistrationEntry_FindByReference_ReturnsTrue()
        {
            var registrationEntry = new RegistrationEntry();
            var repo = new RegistrationEntryRepository();
            repo.Add(registrationEntry);

            var result = repo.HasRegistered(registrationEntry);

            Assert.IsTrue(result);
        }

        [Test]
        public void GivenRepositoryWithRegistrationEntry_FindByNull_ReturnsFalse()
        {
            var registrationEntry = new RegistrationEntry();
            var repo = new RegistrationEntryRepository();
            repo.Add(registrationEntry);

            var result = repo.HasRegistered(null);

            Assert.IsFalse(result);
        }

        //publisher
        [Test]
        public void GivenRepositoryWithRegistrationEntry_FindByPublisherReference_ReturnsTrue()
        {
            var repo = new RegistrationEntryRepository();
            var publisher = new object();
            var registrationEntry = repo.CreateRegistrationEntry(publisher, null);
            repo.Add(registrationEntry);

            var repoQuery = new RegistrationEntry {Publisher = new RegistrationEntryItem { Reference = new WeakReference(publisher)}};
            var result = repo.HasRegistered(repoQuery);

            Assert.IsTrue(result);
        }

        [Test]
        public void GivenRepositoryWithRegistrationEntry_FindByOtherSourceReference_ReturnsFalse()
        {
            var repo = new RegistrationEntryRepository();
            var publisher = new object();
            var registrationEntry = repo.CreateRegistrationEntry(publisher, null);
            repo.Add(registrationEntry);

            var repoQuery = new RegistrationEntry {Publisher = new RegistrationEntryItem { Reference = new WeakReference(new object())}};
            var result = repo.HasRegistered(repoQuery);

            Assert.IsFalse(result);
        }

        [Test]
        public void GivenRepositoryWithRegistrationEntry_FindByPublisherType_ReturnsTrue()
        {
            var repo = new RegistrationEntryRepository();
            var publisher = new object();
            var registrationEntry = repo.CreateRegistrationEntry(publisher, null);
            repo.Add(registrationEntry);

            var repoQuery = new RegistrationEntry { Publisher = new RegistrationEntryItem { ItemType = publisher.GetType() } };
            var result = repo.HasRegistered(repoQuery);

            Assert.IsTrue(result);
        }

        [Test]
        public void GivenRepositoryWithRegistrationEntry_FindByOtherPublisherType_ReturnsFalse()
        {
            var repo = new RegistrationEntryRepository();
            var publisher = new TestPublisher();
            var registrationEntry = repo.CreateRegistrationEntry(publisher, null);
            repo.Add(registrationEntry);

            var repoQuery = new RegistrationEntry { Publisher = new RegistrationEntryItem { ItemType = typeof(TestOtherPublisher) } };
            var result = repo.HasRegistered(repoQuery);

            Assert.IsFalse(result);
        }


        //subscriber
        [Test]
        public void GivenRepositoryWithRegistrationEntry_FindBySubscriberReference_ReturnsTrue()
        {
            var repo = new RegistrationEntryRepository();
            var subscriber = new object();
            var registrationEntry = repo.CreateRegistrationEntry(null, subscriber);
            repo.Add(registrationEntry);

            var repoQuery = new RegistrationEntry { Subscriber = new RegistrationEntryItem { Reference = new WeakReference(subscriber)}};
            var result = repo.HasRegistered(repoQuery);

            Assert.IsTrue(result);
        }

        [Test]
        public void GivenRepositoryWithRegistrationEntry_FindByOtherSubscriberReference_ReturnsFalse()
        {
            var repo = new RegistrationEntryRepository();
            var subscriber = new object();
            var registrationEntry = repo.CreateRegistrationEntry(null, subscriber);
            repo.Add(registrationEntry);

            var repoQuery = new RegistrationEntry { Subscriber = new RegistrationEntryItem { Reference = new WeakReference( new object()) } };
            var result = repo.HasRegistered(repoQuery);

            Assert.IsFalse(result);
        }

        [Test]
        public void GivenRepositoryWithRegistrationEntry_FindBySubscriberType_ReturnsTrue()
        {
            var repo = new RegistrationEntryRepository();
            var subscriber = new object();
            var registrationEntry = repo.CreateRegistrationEntry(null, subscriber);
            repo.Add(registrationEntry);

            var repoQuery = new RegistrationEntry { Subscriber = new RegistrationEntryItem { ItemType = subscriber.GetType() } };
            var result = repo.HasRegistered(repoQuery);

            Assert.IsTrue(result);
        }

        [Test]
        public void GivenRepositoryWithRegistrationEntry_FindByOtherSubscriberType_ReturnsFalse()
        {
            var repo = new RegistrationEntryRepository();
            var subscriber = new TestSubscriber();
            var registrationEntry = repo.CreateRegistrationEntry(null, subscriber);
            repo.Add(registrationEntry);

            var repoQuery = new RegistrationEntry { Subscriber = new RegistrationEntryItem { ItemType = typeof(TestOtherSubscriber) } };
            var result = repo.HasRegistered(repoQuery);

            Assert.IsFalse(result);
        }


        [Test]
        public void GivenRepositoryWithRegistrationEntry_SourceIsReferenced()
        {
            var source = new object();
            var repo = new RegistrationEntryRepository();
            WeakReference sourceWeakReference = null;
            new Action(() =>
            {
                sourceWeakReference = new WeakReference(source);
                var registrationEntry = new RegistrationEntry { Publisher = new RegistrationEntryItem { Reference = sourceWeakReference }};
                repo.Add(registrationEntry);
            })();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Assert.IsTrue(sourceWeakReference.IsAlive);
        }

        [Test]
        public void GivenRepositoryWithRegistrationEntry_WhenEntryIsDisposed_SourceNotReferenced()
        {
            var repo = new RegistrationEntryRepository();
            WeakReference sourceWeakReference = null;
            new Action(() =>
            {
                var source = new object();
                sourceWeakReference = new WeakReference(source);
                var registrationEntry = new RegistrationEntry { Publisher = new RegistrationEntryItem { Reference = sourceWeakReference } };
                repo.Add(registrationEntry);
            })();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Assert.IsFalse(sourceWeakReference.IsAlive);
        }
    }

    public class TestPublisher{}
    public class TestOtherPublisher { }

    public class TestSubscriber { }
    public class TestOtherSubscriber { }


    public class RegistrationEntry
    {
        public RegistrationEntryItem Publisher { get; set; }
        public RegistrationEntryItem Subscriber { get; set; }
    }

    public class RegistrationEntryItem
    {
        public WeakReference Reference { get; set; }
        public Type ItemType { get; set; }
    }
}
