# ScreenLogicConnect

This is a C# 7.1 / .NET Core 2.0 library for interfacing with Pentair ScreenLogic systems over your local network. It requires a Pentair ScreenLogic device on the same network (a network which supports UDP broadcasts).

It was created to be able to connect to my pool equipment via a Raspberry Pi. But then I discovered that .NET Core doesn't support armv6, so my RPi Zero wasn't going to support it after all. I may convert to regular .NET in order to take advantage of Mono which I hear _does_ support the Zero.

## Usage

See Program.cs inside the Test directory for an example of interfacing with the library. Broadly, you'll want to await `ScreenLogicConnect.FindUnits.Find()` to get a list of available controllers on your network, create a connection with
`new ScreenLogicConnect.UnitConnection()` and await `connection.ConnectTo(controller)`. Once connected, there are a few methods implemented such as `GetPoolStatus()` that should show up via Intellisense.

## Notes

Contributions welcome. There are lots of available messages supported by ScreenLogic that the app doesn't support yet, but can be added pretty easily as needed.
