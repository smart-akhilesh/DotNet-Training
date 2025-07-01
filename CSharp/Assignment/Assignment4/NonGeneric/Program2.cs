using System;

namespace NonGeneric
{
    public delegate void RingEventHAndlerDelegate();
    class MobilePhone
    {
        public event RingEventHAndlerDelegate OnRingEvent;
        public void ReceiveCall()
        {
            Console.WriteLine("Incoming Call");
            OnRingEvent();
        }

    }

    class Subscriber
    {

        public void RingtonePlayer()
        {
            Console.WriteLine("Playing Ringtone");
        }

        public void VibrationMotor()
        {
            Console.WriteLine("Displaying Caller Information");
        }

        public void ScreenDisplay()
        {
            Console.WriteLine("Phone is vibrating");
        }
    }
    class Program2
    {
        static void Main(string[] args)
        {
            MobilePhone mobile = new MobilePhone();
            Subscriber subscriber = new Subscriber();

            mobile.OnRingEvent += new RingEventHAndlerDelegate(subscriber.RingtonePlayer);
            mobile.OnRingEvent += subscriber.VibrationMotor;
            mobile.OnRingEvent += subscriber.ScreenDisplay;
            mobile.ReceiveCall();

            Console.Read();
        }
    }
}

