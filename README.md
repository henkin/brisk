### Brisk Event Source Application Framework 

Using Event Sourcing - the events are rest based. Two axis - entities and verbs. create commands. 

Command are CreateItem
UpdateItem
    PublishItem
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
