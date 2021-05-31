using System;
using System.Linq;
using System.Windows.Forms;
using System.Timers;
using Twilio;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;

namespace BatteryAlert
{
    class Program
    {
        const float LOW_THRESHOLD = 0.15f;
        const float HIGH_THRESHOLD = 0.85f;
        const ushort MINUTES_DELAY = 10;
        const ushort MAX_CHECK_COUNT = 24;

        static ushort CheckCount = 0;
        static readonly string ToNumber = Environment.GetEnvironmentVariable("TWILIO_TO_NUMBER");
        static readonly string FromNumber = Environment.GetEnvironmentVariable("TWILIO_FROM_NUMBER");
        static readonly string AccountSID = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
        static readonly string AuthToken = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_AT");

        static void Main()
        {
            string[] requiredStrings = { ToNumber, FromNumber, AccountSID, AuthToken };

            if (requiredStrings.Any(str => str == null))
            {
                Console.WriteLine("Error: required variable is unavailable.");
                Environment.Exit(1);
            }
            else
            {
                var timer = new System.Timers.Timer
                {
                    Interval = TimeSpan.FromMinutes(MINUTES_DELAY).TotalMilliseconds
                };
                timer.Elapsed += TimeElapsed;
                timer.Start();

                while (CheckCount < MAX_CHECK_COUNT)
                {
                    Application.DoEvents();
                }

                Console.WriteLine($"Program exited after {CheckCount} checks.");
            }
        }

        static void TimeElapsed(object sender, ElapsedEventArgs e)
        {
            CheckBatteryStatus();
            CheckCount++;
        }

        static void CheckBatteryStatus()
        {
            float batteryState = SystemInformation.PowerStatus.BatteryLifePercent;
            string alertText = null;

            if (batteryState <= LOW_THRESHOLD)
            {
                alertText = "Plug in your charger.";
            }
            else if (batteryState >= HIGH_THRESHOLD)
            {
                alertText = "Unplug your charger.";
            }

            if (alertText != null)
            {
                SendMessage($"Your battery is {batteryState * 100}% full. {alertText}");
            }
        }

        static void SendMessage(string messageText)
        {
            try
            {
                TwilioClient.Init(AccountSID, AuthToken);

                MessageResource.Create(
                    body: messageText,
                    from: new PhoneNumber(FromNumber),
                    to: new PhoneNumber(ToNumber)
                );
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
