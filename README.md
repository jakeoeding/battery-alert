# BatteryAlert

Simple script that fetches your battery level and alerts you via SMS message if your battery level is too low or full.

## Dependencies
- Windows only (uses `SystemInformation` from `System.Windows.Forms` for checking battery status)
- Twilio. Add the package via NuGet. You'll also need:
    - an account with funds to cover cost of sending messages
    - a verified phone number that you can send messages to
    - a reserved phone number from Twilio for sending messages from

## Customization
- You can set what battery levels trigger notifications by changing `LOW_THRESHOLD` and `HIGH_THRESHOLD`
- `MINUTES_DELAY` corresponds to the time interval between battery status checks
- `MAX_CHECK_COUNT` provides a way for you to limit the number of checks and effectively gives you an upper bound on the amount of time the program will run (`MINUTES_DELAY` * `MAX_CHECK_COUNT`)