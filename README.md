# hid-sharp

Hid-Sharp is designed to take the pain out of communicating with HID (human interface devices) in Windows. 

For an existing project, all you require is the HIDInterface.cs class. This class is a wrapper for the Win32 API calls required for USB operation. It implements simple read/write functions and generates events on asynchronous transmissions.

The code will build under .NET 2.0, and uses only the APIs available as far back as WinXP. It hsa been tested under XP, Vista and Win7 and Win8

Also included is a well documented demo of the functionality of the class. This is a console app which implements synchronous and asynchonous operation of the class.

## Example
```
 //Get the details of all connected USB HID devices
 HIDDevice.interfaceDetails[] devices = HIDDevice.getConnectedDevices(); 
 
 //Select a device from the available devices
 string devicePath = devices[selectedDeviceIndex].devicePath;
 
 //create a handle to the device by calling the constructor
 HIDDevice device = new HIDDevice(devicePath, false);
 
 //Write a byte array to the device
 byte[] writeData = { 0x00, 0x01, 0x02, 0x03, 0x04 };
 device.write(writeData);    //Its that easy!!
 
 //Read a byte array from the device
 byte[] readData = device.read();    //again, that easy!
 
 //close the device to release all handles etc
 device.close();
```
