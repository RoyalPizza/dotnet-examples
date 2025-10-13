# MEC

<img width="512" height="384" alt="App" src="https://github.com/user-attachments/assets/5a0a3f5b-407a-4567-b37d-b3c10fd04379" />

# Architecture
- Application is designed to run standalone with out interaction of an external system. (ex. plc configuration coming from a shared )
- This project is designed as a standalone, under the assumption that shared libraries and abstractions are not wanted.
- .. TODO

## libplctag
**NOT IMPLEMENTED YET**
libplctag is a dependable open source library for plc communication. This project uses the .net wrapper as its PLC interface.
https://github.com/libplctag/libplctag.NET

## Generic Host Builder vs Custom Host
Because WPF is an older tech stack, and the host builder is a new tech, I do not force it into WPF. Instead, a mock host builder with just a singleton and some functions that mimic a service locator is a nice in between. The custom host does not manage lifetime of the application, but it will hold lifetime of the services and configuration and such.

While technically it is possible, I often find issues with it as I start scaling the application. Instead, a service locator is at least some seperation of concerns seeing as true DI is not possible with WPF controls.

## PLC Interface
### Tags
R = Read, W = Write, P = Polled

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
	- This is the PLC telling the app that the bag is in the propper conditions to hit dispatch.
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