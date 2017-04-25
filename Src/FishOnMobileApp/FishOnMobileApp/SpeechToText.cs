using System;
using Xamarin.Forms;

namespace VoiceRecognitionSystem
{
    //This interface is used for IOS Speech to text.
    //(Android has a cludgy hook, and doesn't use this interface)
	public interface ISpeechToText
	{
		void Start ();
		void Stop ();
		event EventHandler<EventArgsVoiceRecognition> textChanged;
	}

    //The way we hook into android voice to text is very different than how we hook into IOS.
    //The classes and interface in this file, help to encapsulate all of that logic and create a simple way 
    //to hook into the native voice to text
    public class SpeechToText
    {
        public delegate void VoiceConvertedToTextDelegate(string value);
        public delegate void NoMicrophoneIsAvailableDelegate();

        private enum PlatformEnumeration
        {
            IOS,
            Windows,
            Android,
            Unknown
        };

        private PlatformEnumeration platform = PlatformEnumeration.Unknown;
        private StackLayout layout;

        //For android, we need a hook into a view, we will use a custom 'VoiceButton' (see definition below)
        private VoiceButton androidVoiceButton;

        //For ios we will implement an interface
        private ISpeechToText iOSSpeechRecognitionInstnace;

        //These controls, if provided, will be updated to display the text that was recorded
        private Entry speechToTextEntry;
        private Label lblSpeecToTextEntry;

        private bool isIOSRecording = false;
       
        //Constructor
        public SpeechToText(StackLayout layout)
        {
            this.layout = layout;

            #if __ANDROID__
		        platform = PlatformEnumeration.Android;
            #endif
            #if __IOS__
                platform = PlatformEnumeration.IOS;
            #endif
        }

        public VoiceConvertedToTextDelegate VoiceConvertedToText_Event { get; set; }
        public NoMicrophoneIsAvailableDelegate NoMicrophoneIsAvailable_Notification { get; set; }

        //Bind a view (label, layout, image, etc), along with an entry control to speech to voice
        public void BindToNativeSpeechRecognistion(Entry entry, View clickableView)
        {
            this.speechToTextEntry = entry;
            SetupSpeechToText(clickableView);
        }

        public void BindToNativeSpeechRecognistion(Label entry, View clickableView)
        {
            this.lblSpeecToTextEntry = entry;
            SetupSpeechToText(clickableView);
        }

        private void SetupSpeechToText(View clickableControl) { 

            if (platform == PlatformEnumeration.Android)
            {
                SetupAndroidVoiceButton();
            }
            else if (platform == PlatformEnumeration.IOS)
            {
                SetUpIOSSpeechRecognizer();
            }

            //When 'clickable' is clicked, call this command logic
            var command = new Command(() =>
            {
                if (platform == PlatformEnumeration.Android)
                {
                    androidVoiceButton.LaunchNavtiveVoiceRecognitionIntent();
                }
                else
                {
                    if (isIOSRecording)
                    {
                        iOSSpeechRecognitionInstnace.Stop();
                    }
                    else
                    {
                        iOSSpeechRecognitionInstnace.Start();
                    }

                    isIOSRecording = !isIOSRecording;
                }
            });

            //Assign the command defined above to the clickableControl
            if (clickableControl.GetType().Name == "Button")
            {
                Button btn = (Button)clickableControl;
                btn.Command = command;
            }
            else
            {
                TapGestureRecognizer tap = new TapGestureRecognizer()
                {
                    Command = command
                };

                clickableControl.GestureRecognizers.Add(tap);
            }
        }

        private void SetupAndroidVoiceButton()
        {
            //the androidVoiceButton is an instance of VoiceButton (and it inherits from Button)
            //but we might not want to use an actual button, we might want to use a label,
            //or an image, or any other tap gesture away view as the clickable control to launch speech to text.

            //Create a VoiceButton, set it's height and width to 0 and color to tranparent
            //then add it to the layout so that we can hook into it when our clickableView is clicked (or tapped).
            androidVoiceButton = new VoiceButton()
            {
                BackgroundColor = Color.Transparent,
                HeightRequest = 0,
                WidthRequest = 0
            };

           
            layout.Children.Add(androidVoiceButton);

            androidVoiceButton.OnTextChanged += (s) => {
                if (s == androidVoiceButton.NO_MICROPHONE_AVAILABLE)
                {
                    NoMicrophoneIsAvailable_Notification?.Invoke();
                }

                if (speechToTextEntry != null)
                {
                    speechToTextEntry.Text = s;
                }

                if (lblSpeecToTextEntry != null)
                {
                    lblSpeecToTextEntry.Text = s;
                }

                VoiceConvertedToText_Event?.Invoke(s);
            };
        }

        private void SetUpIOSSpeechRecognizer()
        {
            iOSSpeechRecognitionInstnace = DependencyService.Get<ISpeechToText>();
            iOSSpeechRecognitionInstnace.textChanged += OnIOS_VoiceToTextChange;
        }

        //When speech is converted to text on IOS, this method will get called.
        private void OnIOS_VoiceToTextChange(object sender, EventArgsVoiceRecognition e)
        {
            if (speechToTextEntry != null)
            {
                speechToTextEntry.Text = e.Text;
            }

            if (lblSpeecToTextEntry != null)
            {
                lblSpeecToTextEntry.Text = e.Text;
            }

            if (e.IsFinal)
            {
                VoiceConvertedToText_Event(e.Text);
            }
        }
    }

    //Used by the ISpeechToText for iOS speech to text
    public class EventArgsVoiceRecognition : EventArgs
    {
        public EventArgsVoiceRecognition(string text, bool isFinal)
        {
            this.Text = text;
            this.IsFinal = isFinal;
        }
        public string Text { get; set; }
        public bool IsFinal { get; set; }

    }

    //The SpeechToText.cs file found in the .Droid project utialized this class
    //to hook into the voice to text
    public class VoiceButton : Button
    {
        public delegate void LaunchNavtiveVoiceRecognitionDelegate();

        public Action<string> OnTextChanged { get; set; }
        public LaunchNavtiveVoiceRecognitionDelegate LaunchNavtiveVoiceRecognitionIntent { get; set; }

        public string NO_MICROPHONE_AVAILABLE
        {
            get { return "~NO_MICROW_PHONE_IS_AVAILABLE~"; }
        }
    }
}

