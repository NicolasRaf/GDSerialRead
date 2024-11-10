# GDSerialRead

A plugin for serial data reading in Godot Engine 4.3, aimed at bridging communication between devices like Arduino and Godot to create dynamic interactions in games and applications.

## Table of Contents
- [About the Project](#about-the-project)
- [Requirements](#requirements)
- [Installation](#installation)
- [Usage](#usage)
- [Examples](#examples)
- [License](#license)

## About the Project

`GDSerialRead` offers a straightforward solution for reading serial data directly in Godot, making it ideal for integrating external devices like Arduino with the engine. This project was initially developed to simulate and capture data from an Arduino Esplora in a Linux environment, enabling real-world hardware and software interactions in a game environment.

## Requirements

- [**Godot Engine - .NET**](https://godotengine.org/download/windows/) 4.3 or higher.
- [SDK do .NET 8](https://dotnet.microsoft.com/pt-br/download) version usade in project.
- Build System.IO.Ports in project paste: ```dotnet add package System.IO.Ports```

  
## Installation


### 1. In the project editor in godot initialize the environment for C# following the path below:
``` Project --> Tools --> C# --> Creat C# solution ```
<img src="image.png">

### 2. After initializing the environment, open a terminal with the path to the main folder of the Godot project.
<img src="image-2.png">

### 3. Clone the main branch of the repository into the project folder 
``` 
git clone https://github.com/NicolasRaf/GDSerialRead.git
```
<img src="image-3.png">

### 4. Still in the terminal, install the “System.IO.Ports” package in your project's main folder
```
dotnet add package System.IO.Ports
````
<img src="image-4.png">

### 5. Now inside Godot open the scene called “DataReceiver.tscn” inside the cloned folder of the plugin and assign the script “DataReceiver.gd” to the node.
<img src="image-5.png">

### 6. After that, the program is up and running and you can receive and manipulate data from the “DataReciever.gd” node.
<img src="image-6.png">