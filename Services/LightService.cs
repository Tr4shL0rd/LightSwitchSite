using System;
using Renci.SshNet;

namespace myWebApp.Services
{
	public class LightService
    {
                //SSH STUFFS\\
        private string ip = null;
        private readonly string user = "HOSTNAME"; // enter hostname here
        private readonly string pass = "PASSWORD"; // enter password here

        private bool TestConnectionStarted = false;

        public void TestConnection()
        {

            if(this.ip != null || TestConnectionStarted == true)
            {

                return;

            }
            Console.WriteLine("CONNECTION: Tesing Connection!");
            TestConnectionStarted = true;
            for(var i = 0; i < 10; i++)
            {
                var ip = $"IP{i}"; //enter ip here (000.000.00.00{i})
                using (var client = new SshClient(new ConnectionInfo(ip, user, new PasswordAuthenticationMethod(user, pass)) {Timeout = TimeSpan.FromSeconds(2)}))
                {
                    try
                    {
                        client.Connect();
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine($"CONNECTION: Could Not Connect To {ip}");
                    }
                    if (client.IsConnected)
                    {
                        this.ip = ip;
                        Console.WriteLine($"CONNECTION: connected to {ip}");
                        TestConnectionStarted = false;
                        client.Disconnect();
                        break;
                    }
                }


            }
        }
        public void resetIP()
        {
            this.ip = null;
        }



                //LIGHT STUFFS\\
        private bool autoLight = false;
       
        private string RunCommand(string input) 
        {
            var result = string.Empty;

            TestConnection();
            if (this.ip == null)
            {
                return result;
            }
            using (var client = new SshClient(ip, user, pass))
            {
                client.Connect();
                var output = client.RunCommand(input);
                client.Disconnect();
                result = output.Result;
                
            }
            return result;
        }

        //turns on/off lights\\
        public void dataSender(bool on)
        {
            RunCommand($"echo {(on ? 3 : 8)} | python lights.py");
        }

        //reads state of light\\
        public bool GetLightState()
        {
            var on = false;
            var result = RunCommand("cat servostate.txt");
            if (result == "3.0")
            {
                on = true;
            }
            else
            {
                on = false;
            }

            //prints true if on 
            return on;
        }
        //turns lights on
        public void TurnOn()
        {
            dataSender(true);
        }
        //turns lights off
        public void TurnOff()
        {
            dataSender(false);
        }
        


        public void SetAutoLight(bool value)
        {
            this.autoLight = value;
        }
        public bool GetAutoLight()
        {
            return this.autoLight;
        }
    
    
    }
}
