### Brisk Event Source Application Framework 

Using the idea of Event Sourcing, this project aims to develop a reusable approach to building applications.
Modern applications have many demands made of them - be scalable, reselient, secure, etc. To maintain my sanity as a developer, I would also place an important constraint on the system - 
be simple. 
I like DDD, event sourcing, reactive systems, nosql databases, containerization, and open source. 

Command are CreateItem
UpdateItem
    PublishItem
    UnpublishItem
DeleteItem
GetItem

Event
CreateItemResult
UpdateItemResult
DeleteItemResult

ClientLayer
    Angular2 App
ServerLayer
    Node App
    WebSocket
    
    Controllers
    
items/ POST
validate 
persist item
publish ItemCreated event
controller 

items/ GET
subscribe to items changing
publish items.
