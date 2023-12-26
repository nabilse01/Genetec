using System;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using Newtonsoft.Json;

namespace ConsoleApp3
{
    public class MqttHelper
    {
        public static async Task<string> Connect()
        {
            string message = "Received message on topic 'Testttt'";
            ShowMessageBox(message);
            return await RunMqttClientAsync();
        }

        private static async Task<string> RunMqttClientAsync()
        {
            try
            {
                var factory = new MqttFactory();
                var mqttClient = factory.CreateMqttClient();

                var clientOptions = new MqttClientOptionsBuilder()
                    .WithClientId("saad")
                    .WithTcpServer("192.168.2.120", 1883)
                    .WithCredentials("gateway", "PenguineersGateWay");

                var options = clientOptions.Build();

                mqttClient.UseConnectedHandler(async e =>
                {
                    ShowMessageBox("Connected to MQTT broker");
                    await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder()
                        .WithTopic("pentrack_iq")
                        .Build());
                });

                mqttClient.UseApplicationMessageReceivedHandler(e =>
                {
                    string receivedMessage = $"Received message on topic '{e.ApplicationMessage.Topic}': {e.ApplicationMessage.ConvertPayloadToString()}";
                    dynamic payloads = JsonConvert.DeserializeObject(e.ApplicationMessage.ConvertPayloadToString());

                    if (payloads != null)
                    {
                        foreach (var payload in payloads)
                        {
                            PayloadData payloadData = JsonConvert.DeserializeObject<PayloadData>(payload.ToString());

                            string macAddress = payloadData.Mac;

                            if (payloadData.Mac == "ac233fac84fd" && payloadData.Raw != null)
                            {
                                var raw_data = payloadData.Raw.ToString();

                                if (raw_data.Length >= 20 && raw_data.Substring(20, Math.Min(1, raw_data.Length - 20)) == "5")
                                {
                                    ShowMessageBox("Done");
                                }
                                else
                                {
                                    ShowMessageBox("None");
                                }
                            }
                        }
                    }
                });

                await mqttClient.ConnectAsync(options);

                // Add an infinite loop or delay to keep the application running
                while (true)
                {
                    await Task.Delay(1000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to MQTT broker: {ex.Message}");
                return "Error connecting to MQTT broker";
            }
        }

        private static void ShowMessageBox(string message)
        {
            Console.WriteLine(message);
        }
    }





    public class PayloadData
    {
        public string Mac { get; set; }
        public string Timestamp { get; set; }
        public int No { get; set; }
        public int Rssi { get; set; }
        public string Raw { get; set; }
    }

}
