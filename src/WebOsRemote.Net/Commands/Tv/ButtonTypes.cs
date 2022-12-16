using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WebOsRemote.Net.Commands.Tv
{
    public class ButtonType
    {
        public static readonly ButtonType Left = new ButtonType("LEFT", "Left");
        public static readonly ButtonType Right = new ButtonType("RIGHT", "Right");
        public static readonly ButtonType Down = new ButtonType("DOWN", "Down");
        public static readonly ButtonType Up = new ButtonType("UP", "Up");
        public static readonly ButtonType Home = new ButtonType("HOME", "Home");
        public static readonly ButtonType Back = new ButtonType("BACK", "Back");
        public static readonly ButtonType Menu = new ButtonType("MENU", "Menu");
        public static readonly ButtonType Enter = new ButtonType("ENTER", "Enter");
        public static readonly ButtonType Dash = new ButtonType("DASH", "Dash");
        public static readonly ButtonType Info = new ButtonType("INFO", "Info");
        public static readonly ButtonType One = new ButtonType("1", "1");
        public static readonly ButtonType Two = new ButtonType("2", "2");
        public static readonly ButtonType Three = new ButtonType("3", "3");
        public static readonly ButtonType Four = new ButtonType("4", "4");
        public static readonly ButtonType Five = new ButtonType("5", "5");
        public static readonly ButtonType Six = new ButtonType("6", "6");
        public static readonly ButtonType Seven = new ButtonType("7", "7");
        public static readonly ButtonType Eight = new ButtonType("8", "8");
        public static readonly ButtonType Nine = new ButtonType("9", "9");
        public static readonly ButtonType Zero = new ButtonType("0", "0");
        public static readonly ButtonType Asterisk = new ButtonType("ASTERISK", "Asterisk");
        public static readonly ButtonType Cc = new ButtonType("CC", "Closed Captioning");
        public static readonly ButtonType Exit = new ButtonType("EXIT", "Exit");
        public static readonly ButtonType Mute = new ButtonType("MUTE", "Mute");
        public static readonly ButtonType Red = new ButtonType("RED", "Red");
        public static readonly ButtonType Green = new ButtonType("GREEN", "Green");
        public static readonly ButtonType Yellow = new ButtonType("YELLOW", "Yellow");
        public static readonly ButtonType Blue = new ButtonType("BLUE", "Blue");
        public static readonly ButtonType VolumeUp = new ButtonType("VOLUMEUP", "Volume Up");
        public static readonly ButtonType VolumeDown = new ButtonType("VOLUMEDOWN", "VolumeD own");
        public static readonly ButtonType ChannelUp = new ButtonType("CHANNELUP", "Channel Up");
        public static readonly ButtonType ChannelDown = new ButtonType("CHANNELDOWN", "Channel Down");
        public static readonly ButtonType Play = new ButtonType("PLAY", "Play");
        public static readonly ButtonType Pause = new ButtonType("PAUSE", "Pause");
        public static readonly ButtonType Stop = new ButtonType("STOP", "Stop");
        public static readonly ButtonType Rewind = new ButtonType("REWIND", "Rewind");
        public static readonly ButtonType FastForward = new ButtonType("FASTFORWARD", "Fast Forward");

        public static readonly ButtonType QMenu = new ButtonType("QMENU", "Quick Select Menu");
        public static readonly ButtonType LiveZoom = new ButtonType("LIVE_ZOOM", "Live Zoom");
        public static readonly ButtonType Favorites = new ButtonType("FAVORITES", "Favorites");
        public static readonly ButtonType GoToPrev = new ButtonType("GOTOPREV", "Go to Previous");
        public static readonly ButtonType GoToNext = new ButtonType("GOTONEXT", "Go to Next");
        public static readonly ButtonType AspectRatio = new ButtonType("ASPECT_RATIO", "Aspect Ratio");
        public static readonly ButtonType MyApps = new ButtonType("MYAPPS", "My Apps");
        public static readonly ButtonType Recent = new ButtonType("RECENT", "Recent");

        public static ButtonType[] All => new[]
        {
            Left,
            Right,
            Down,
            Up,
            Home,
            Back,
            Menu,
            Enter,
            Dash,
            Info,
            One,
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine,
            Zero,
            Asterisk,
            Cc,
            Exit,
            Mute,
            Red,
            Green,
            Yellow,
            Blue,
            VolumeUp,
            VolumeDown,
            ChannelUp,
            ChannelDown,
            Play,
            Pause,
            Stop,
            Rewind,
            FastForward,

            QMenu,
            LiveZoom,
            Favorites,
            GoToPrev,
            GoToNext,
            AspectRatio,
            MyApps,
            Recent
        };

        public string ButtonCode { get; }
        public string DisplayName { get; }

        public static ButtonType GetByDisplayName(string displayName)
        {
            DisplayNameLookup.TryGetValue(displayName, out var buttonType);
            return buttonType;
        }

        private ButtonType(string buttonCode, string displayName)
        {
            ButtonCode = buttonCode;
            DisplayName = displayName;
        }

        public override string ToString()
        {
            return DisplayName;
        }

        private static readonly Dictionary<string, ButtonType> DisplayNameLookup = All.ToDictionary(b => b.DisplayName);
    }
}