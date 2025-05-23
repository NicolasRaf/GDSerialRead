# GDSerialRead

A plugin for serial data reading in Godot Engine 4.3, aimed at bridging communication between devices like Arduino and Godot to create dynamic interactions in games and applications.

## Table of Contents

- [About the Project](#about-the-project)
- [Requirements](#requirements)
- [Installation](#installation)
- [Usage](#usage)
- [Examples](#examples)
- [Credits](#credits)

## [About the Project](#about-the-project)

`GDSerialRead` offers a straightforward solution for reading serial data directly in Godot, making it ideal for integrating external devices like Arduino with the engine. This project was initially developed to simulate and capture data from an Arduino Esplora in a Linux environment, enabling real-world hardware and software interactions in a game environment.

## [Requirements](#requirements)

- [**Godot Engine - .NET**](https://godotengine.org/download/windows/) 4.3 or higher.
- [SDK do .NET 8](https://dotnet.microsoft.com/pt-br/download) version usade in project.
- Build System.IO.Ports in project paste: ```dotnet add package System.IO.Ports``

## [Installation](#installation)

### 1. In the project editor in godot initialize the environment for C# following the path below

``` Terminal
Project --> Tools --> C# --> Creat C# solution 
```

<div align="center">
    <img src="https://github.com/user-attachments/assets/78e9ca9d-bc24-4b9a-aede-c418a9fbdb5f" alt="Descrição da imagem"width="800"/>
</div>

---

### 2. After initializing the environment, open a terminal with the path to the main folder of the Godot project

<div align="center">
    <img src="https://github.com/user-attachments/assets/c9d819ce-3819-4b6d-a500-26ea5f6baa0c" alt="Descrição da imagem" width="800"/>
</div>

---

### 3. Clone the main branch of the repository into the project folder

```
git clone https://github.com/NicolasRaf/GDSerialRead.git
```

<div align="center">
    <img src="https://github.com/user-attachments/assets/3985290b-4434-43ef-9c40-6758bba95328" alt="Descrição da imagem" width="800"/>
</div>

---

### 4. Still in the terminal, install the “System.IO.Ports” package in your project's main folder

```
dotnet add package System.IO.Ports
```

<div align="center">
    <img src="https://github.com/user-attachments/assets/d60e16aa-8167-4ba4-aef5-bdc7cdecf5ea" width="800"/>
</div>

---

### 5. Now inside Godot open the scene called “DataReceiver.tscn” and assign the script “DataReceiver.gd” to the node

<div align="center">
    <img src="https://github.com/user-attachments/assets/4394e286-df14-4a34-b18a-42be40294fc5" alt="Descrição da imagem" width="800"/>
</div>

---

### 6. Finally, set the “DataReceiver.tscn” node and the “SerialRead.cs” script to autoload (Global)

<div align="center">
    <img src="https://github.com/user-attachments/assets/fd4253b0-9c55-4375-a7e5-a3d5fd064ab2" alt="Descrição da imagem" width="800"/>
</div>

---

### 7. After that, the program is up and running and you can receive and manipulate data from the “DataReciever.gd” node

<div align="center">
    <img src="https://github.com/user-attachments/assets/06e1b34b-3842-48a5-bd66-67b01dd52315" alt="Descrição da imagem" width="800"/>
</div>

## [Usage](#usage)

The node `DataReceiver` is responsible for receiving and processing the data sent from the `SerialRead` node. To utilize this functionality, ensure that both this scene (`DataReceiver.gd`) and the `SerialRead.cs` script are set as Global (Autoload) in your project.

### To interact with the serial data

- Call `SerialRead.StartReading()` to begin reading data from the serial port.
- Call `SerialRead._ExitTree()` to stop reading data and properly close the connection.

### Key Points

- The script automatically starts reading data when the node is initialized.
- If you stop the reading process, it will not resume until you explicitly call `StartReading()` again.
- The code attempts to connect to a serial port only when data is being sent.
  - If access to the port is unavailable or if no data is received for 3 seconds, the script will attempt to connect to the next available port.

1. **This node serves as a manager for the incoming data, allowing you to process and store it as needed.**
2. **You can create additional variables to hold the parsed or transformed data as necessary.**

### Note

This functionality only works if both this node and the `SerialRead.cs` script are configured as Autoloads in your project settings.

## [Exemple](#exemple)

- Switch to the `example/TestSerialExample` branch.

- It has a project already installed for testing the reception of serial data.  

## [Credits](#credits)

#### This program was developed by [Nícolas Rafael](https://github.com/NicolasRaf)
