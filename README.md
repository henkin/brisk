brisk
=====
A Domain Event-driven Web Application Framework 

Overview 
--------

The idea is that you use it as a persistence library, but underneath it creates a stream of 
events and a snapshot DB. You can later get at the events, replay them, etc.

Uses MongoDB, SignalR and Autofac

Brisk supports the following concepts:

* Domain Events
* Commands

  

To use library: 

Have you application expose public properties.

IEventer
ICommander


# ICommander

Supports Create, Update, Delete, Query


