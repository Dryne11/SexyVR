using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRGIN.Controls.Speech;

namespace SexyVR {
    class SexyVoiceCommand : VoiceCommand {

        // XXX copy paste from PlayClub, do these work here?
        public static readonly VoiceCommand NextAnimation = new SexyVoiceCommand("next");
        public static readonly VoiceCommand PreviousAnimation = new SexyVoiceCommand("previous");
        public static readonly VoiceCommand StartAnimation = new SexyVoiceCommand("start");
        public static readonly VoiceCommand StopAnimation = new SexyVoiceCommand("stop");
        public static readonly VoiceCommand Faster = new SexyVoiceCommand("faster", "increase speed");
        public static readonly VoiceCommand Slower = new SexyVoiceCommand("slower", "decrease speed");
        public static readonly VoiceCommand Climax = new SexyVoiceCommand("climax", "come");
        public static readonly VoiceCommand DisableClimax = new SexyVoiceCommand("disable climax", "disable orgasm");
        public static readonly VoiceCommand EnableClimax = new SexyVoiceCommand("enable climax", "enable orgasm");

        protected SexyVoiceCommand(params string[] texts) : base(texts)
        {
        }
    }
}
