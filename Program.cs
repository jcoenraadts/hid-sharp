using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HIDInterface;

namespace HID_Demo
{
    class Program
    {
        /*------------------------------------------------------------------------------------------
         *                      HID Interface class demo code
         *                      
         * This demo code returns the details of all connected HID devices, then selects one of them
         * and connects to it. A synchronous read / write operation is performed and the device is 
         * closed.
         * Two methods are included to show the synchronous and asynchronous operations of this software.
         * 
         * The intention of this code is to provide a demonstration which can be run in a debug
         * environment, and the sample methods can be cut and pasted into your program
         * ----------------------------------------------------------------------------------------*/

        static void Main(string[] args)
        {
            HID_demo demo = new HID_demo();
            
            //call one or other of these methods to demonstrate each type of operation - sync and async
            demo.startAsyncOperation();             
            //demo.useSynchronousOperation();
        }
    }

    public class HID_demo
    {

        // Apologies for the repeated code, however i feel it provides a better demonstration
        // of the functionality of this code.
        public void useSynchronousOperation()
        {
            //Get the details of all connected USB HID devices
            HIDDevice.interfaceDetails[] devices = HIDDevice.getConnectedDevices();

            //Arbitrarily select one of the devices which we found in the previous step
            //record the details of this device to be used in the class constructor
            int selectedDeviceIndex = 0;
            ushort VID = devices[selectedDeviceIndex].VID;
            ushort PID = devices[selectedDeviceIndex].PID;
            int SN = devices[selectedDeviceIndex].serialNumber;
            string devicePath = devices[selectedDeviceIndex].devicePath;

            //create a handle to the device by calling the constructor of the HID class
            //This can be done using either the VID/PID/Serialnumber, or the device path (string) 
            //all of these details are available from the HIDDevice.interfaceDetails[] struct array created above
            //The "false" boolean in the constructor tells the class we only want synchronous operation
            HIDDevice device = new HIDDevice(devicePath, false);
            //OR, the normal usage when you know the VID and PID of the device
            //HIDDevice device = new HIDDevice(VID, PID, (ushort)SN, false);

            //Write some data to the device (the write method throws an exception if the data is longer than the report length
            //specified by the device, this length can be found in the HIDDevice.interfaceDetails struct)
            byte[] writeData = { 0x00, 0x01, 0x02, 0x03, 0x04 };
            device.write(writeData);    //Its that easy!!

            //Read some data synchronously from the device. This method blocks the calling thread until the data
            //is returned. This takes 1-20ms for most HID devices
            byte[] readData = device.read();    //again, that easy!

            //close the device to release all handles etc
            device.close();
        }

        public void startAsyncOperation()
        {
            //Get the details of all connected USB HID devices
            HIDDevice.interfaceDetails[] devices = HIDDevice.getConnectedDevices();

            //Arbitrarily select one of the devices which we found in the previous step
            //record the details of this device to be used in the class constructor
            int selectedDeviceIndex = 0;
            ushort VID = devices[selectedDeviceIndex].VID;
            ushort PID = devices[selectedDeviceIndex].PID;
            int SN = devices[selectedDeviceIndex].serialNumber;
            string devicePath = devices[selectedDeviceIndex].devicePath;

            //create a handle to the device by calling the constructor of the HID class
            //This can be done using either the VID/PID/Serialnumber, or the device path (string) 
            //all of these details are available from the HIDDevice.interfaceDetails[] struct array created above
            //The "true" boolean in the constructor tells the class we want asynchronous operation this time
            HIDDevice device = new HIDDevice(devicePath, true);
            //OR, the normal usage when you know the VID and PID of the device
            //HIDDevice device = new HIDDevice(VID, PID, (ushort)SN, true);
            
            //next create the event handler for the incoming reports
            device.dataReceived += new HIDDevice.dataReceivedEvent(device_dataReceived);

            //Write some data to the device (the write method throws an exception if the data is longer than the report length
            //specified by the device, this length can be found in the HIDDevice.interfaceDetails struct)
            //The write method is identical to the synchronous mode of operation
            byte[] writeData = { 0x00, 0x01, 0x02, 0x03, 0x04 };
            device.write(writeData);    //Its that easy!!

            //Send your program off to do other stuff here, wait for UI events etc
            //When a report occurs, the device_dataReceived(byte[] message) method will be called
            System.Threading.Thread.Sleep(100);

            //close the device to release all handles etc
            device.close();
        }

        //Whenever a report comes in, this method will be called and the data will be available! Like magic...
        void device_dataReceived(byte[] message)
        {
            //Do something with the data here...
        }
    }
}
