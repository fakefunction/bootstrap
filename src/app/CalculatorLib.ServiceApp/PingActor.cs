using Akka.Actor;

namespace CalculatorLib.ServiceApp
{
    public class PingActor : ReceiveActor
    {
        public PingActor()
        {
            Receive<string>(m => Sender.Tell(m));
        }
    }
}