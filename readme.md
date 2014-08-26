### What is LightMessageBus?
A simple, light-weight application level **event aggregator** which facilitates the **exchange of messages** between objects **without direct references**. 

### Use-cases and examples?

> A soundComponent needs to be notified whenever a user clicks on any button.

```
MessageBus.Default.FromAny().Where<ButtonClickMessage>.Notify(soundComponent)
```

> A taxCalculator needs to be notified whenever shopping cart is changed.

```
MessageBus.Default.From(shoppingCart).Notify(taxCalculator);
``` 

### How is this achieved?
LightMessageBus uses a simple publisher/subscriber pattern.

* **Subscribers** use the MessageBus to **subscribe to messages** they wish to be notified about. They may wish to filter messages by the source, type of message or some other predicate.
* **Publishers** use the MessageBus to **publish messages** to the potential subscribers
* Neither subscribers nor publishers have any references to each other. They simply use a **common communication channel** - the MessageBus

### How does that help?
The pub/sub pattern enables you to **promote decoupled architecture** in your application. Since there is no need for various objects (views, models, controllers, ...) to know of each other existence - you are free to develop them independently in isolation. Such architecture enables **testing**, **refactoring** and **limits the propagation of changes** through the system.

### Why not use delegates/events?
Using delegates and/or events requires a reference between the publisher and the subscriber (subscribers reference publishers or vice-versa). Any change to the referenced class will affect the other. Such change may even propagate through the system resulting in **significant changes due to a small cause**. 

### How is it used?
LightMessageBus uses LINQ-like, **fluent syntax** for maximum **readability** and **flexibility**. The basic building blocks of a LightMessageBus registration expression are: `From`, `Where` and `Notify`.

First we define the publishers:

* `From` - specifies publisher(s) of interest to subscriber.
* `FromAny` - specifies any publisher.

Next we optionally define the messages:

* `Where` - specifies the types of messages of interest to subscriber.

Lastly we define the subscriber:

* `Notify` - specifies the subscriber.

