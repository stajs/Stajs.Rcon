# Stajs.Rcon

## Overview

A playground for exploring the RCON (Remote Connection) protocol used to administer servers for the Valve game Half-Life 2, and in particular the [Team Fortress 2](http://www.teamfortress.com/) (TF2) mod.

## Disclaimer

Only tested against:

1. TF2. RCON is used to administer a wide range of game servers, however, I'm only interested in TF2 at this point in time.
2. Windows servers.

## Prerequisites

Obviously, you will need RCON access to a server. If you don't have access to a public server, you can [download and install](http://wiki.teamfortress.com/wiki/Windows_dedicated_server) your own.

### Building

Visual Studio 2012

## Features

### Commands

A sprinkling of classes for authenticating and issuing a few basic commands (status, users, say) as well as a raw command.

### Responses

Handles stitching together responses that span multiple packets.

## API

There ain't one!

Seriously, what do you expect? This is just a playground. There is one Test() method currently that issues a few commands, collates responses, and prints out the packets being sent and received in the Output Window in VS.

## Usage

Uh, there is a command line app that calls the aforementioned API. Add the IP address and RCON password for your server to the App.config.

## Roadmap

Who knows. This is just for fun. I would like to swap out the currently blocking socket read/writes for the async versions. And write a purty WPF front end.