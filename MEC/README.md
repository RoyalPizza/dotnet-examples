# MEC
The Manual Encoding Console (MEC) is a standalone industrial automation application designed for baggage handling systems (BHS). It interfaces with a PLC to manage bag processing, allowing operators to manually encode and dispatch bags to their destinations, ensuring efficient sorting in high-demand environments.

This project is a mock MEC designed around its core principles.
- Running in isolation without internet access, nonstop for years.
- Large bulky UI for low resolution and "not" very touch sensitive screens.
- Able to run with communication external servers and independently.

<img width="512" height="384" alt="App" src="https://github.com/user-attachments/assets/5a0a3f5b-407a-4567-b37d-b3c10fd04379" />

# Architecture
## Tech Stack
.NET, C#, WPF, Sqlite, EFCore, ILogger/Serilog
Uses MVVM with some OOP abstractions here and there.

## Generic Host Builder vs Custom Host
Because WPF is an older tech stack, and the host builder is a new tech, I do not force it into WPF. Instead, a mock host builder with just a singleton and some functions that mimic a service locator is a nice in between. The custom host does not manage lifetime of the application, but it will hold lifetime of the services and configuration and such.

While technically it is possible, I often find issues with it as I start scaling the application. Instead, a service locator is at least some seperation of concerns seeing as true DI is not possible with WPF UI.

## .Net 9 Fluent UI vs Default WPF UI
The app can be switched between the two as desired. Some effort has been made to ensure it will work with either.
Fluent UI has had bugs during development/testing, but looks better as a portfolio project. A real deployment would
likely not use it. But it is a nice alternative to WPF UI Templates; which are quite a pain.

Just comment out Fluent UI from App.xaml to switch between.

## Sqlite & EFCore
Sqlite works well here because a full stack RDBMS is not needed for an applcation of this size. Plus it works
well for easy to test github projects. I used EFCore just to showcase it. But this project does not have much need
for relational data storage as most data is cached in memory.

## libplctag (NOT IMPLEMENTED YET)
libplctag is a dependable open source library for plc communication. This project uses the .net wrapper as its PLC interface.
https://github.com/libplctag/libplctag.NET

## PLC Interface
This application is intended to interface with a PLC. 
No matter the provider used, the interface should follow the following flow.

- bag arrives at station
- PLC udpates Pseudo to say bag is there
- app lets operator process the bag
- operator dispatches bag after choosing destination
- app writes destination to PLC
- PLC dispatches the bag and brings another one to the station
- repeat

### Tags
R = Read, W = Write

MEC.HB (W)
MEC.Enabled (W)
MEC.Pseudo (R)
MEC.CanDispatch (R)
MEC.IATA (W)
MEC.Destination (W)
MEC.Dispatch (W)

- HB (W)
	- set by application. 
	- toggles value every 3 seconds there after.
- Enabled (W)
	- set by app on enable/disable
- Pseudo (R)
	- PLC sets value and app reads every x ms. Changing this tag WILL force the app to update no matter the situation. 
	- At most it will simply log a warning if this is change when it should not have.
- CanDispatch (R)
	- PLC sets this value true anytime the bag is detected on the belt.
	- This is the PLC telling the app that the bag is in the proper conditions to hit dispatch.
- IATA (W)
	- App sets this value on dispatch, if it scans one. 
	- It is assumed all bags at a MEC are there because the ATR could not read a tag. Therefore there is no reason to poll this tag.
	- PLC should can clear this value after dispatch is recieved, if desired
- Destination (W)
	- App sets this value on dispatch.
	- Integer that represents a sort destination the PLC can understand. (integer = makeup/pier ID)
	- PLC should can clear this value after dispatch is recieved, if desired
- Disapatch (W)
	- App sets this value on dispatch.

	- PLC must set low after bag has completed the dispatch process.